using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using neo.BRLightREST;
using TCDF.Sinj;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.ESUtil;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace SINJ_PUSH_APP
{
    class Program
    {
        private FileInfo _file_error;
        private FileInfo _file_info;
        private StringBuilder _sb_error;
        private StringBuilder _sb_info;
        private DateTime _dtInicio;

        public Program()
        {
            _dtInicio = DateTime.Now;
            // Cria uma pasta de log por dia, separado por mes e por ano
            _file_error = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log" + Path.DirectorySeparatorChar.ToString() + _dtInicio.ToString("yyyy") + Path.DirectorySeparatorChar.ToString() + _dtInicio.ToString("MMMM") + Path.DirectorySeparatorChar.ToString() + _dtInicio.ToString("dd") + Path.DirectorySeparatorChar.ToString() + "Sinj_Push_App_ERROR_" + _dtInicio.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
            _file_info = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log" + Path.DirectorySeparatorChar.ToString() + _dtInicio.ToString("yyyy") + Path.DirectorySeparatorChar.ToString() + _dtInicio.ToString("MMMM") + Path.DirectorySeparatorChar.ToString() + _dtInicio.ToString("dd") + Path.DirectorySeparatorChar.ToString() + "Sinj_Push_App_INFO_" + _dtInicio.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");

            //_file_error = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log " + DateTime.Now.ToString("yyyy-MM-dd") + Path.DirectorySeparatorChar.ToString() + "Sinj_Push_App_ERROR_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
            //_file_info = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log " + DateTime.Now.ToString("yyyy-MM-dd") + Path.DirectorySeparatorChar.ToString() + "Sinj_Push_App_INFO_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".log");
            
            _sb_error = new StringBuilder();
            _sb_info = new StringBuilder();
            Console.Clear();
        }

        static void Main(string[] args)
        {
            var program = new Program();
            try
            {
                if (Convert.ToBoolean(Config.ValorChave("MonitorarPush", true)))
                {
                    program.MonitorarPush();
                }
            }
            catch (Exception ex)
            {
                var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                Console.WriteLine("Exception: " + mensagem);
                program._sb_error.AppendLine(DateTime.Now + ": " + mensagem);
            }
            program.Log();
        }

        public void MonitorarPush()
        {
            try
            {
                //MonitorarNormasAtualizadasPush();
                //MonitorarNormasNovasPush();
                MonitorarDiarioPush();
                this.Log();
            }
            catch
            {
            }
        }


        public void MonitorarNormasNovasPush()
        {
            Pesquisa pesquisa_norma = new Pesquisa();
            Pesquisa pesquisa_push = new Pesquisa();
            NotifiquemeOV notifiquemeOv = new NotifiquemeOV();
            NormaOV normaOv = new NormaOV();
            try
            {
                NotifiquemeRN notifiquemeRN = new NotifiquemeRN(false);
                NormaRN normaRn = new NormaRN();
                pesquisa_norma.literal = "st_nova=true";
                pesquisa_norma.limit = null;
                pesquisa_norma.select = new[] { "id_doc", "ch_norma", "nm_tipo_norma", "origens", "nr_norma", "dt_assinatura", "ds_ementa", "ch_tipo_norma", "indexacoes" };
                var results_normas = normaRn.Consultar(pesquisa_norma);
                this._sb_info.AppendLine(DateTime.Now + ": Total de normas ST_NOVA=true => " + results_normas.results.Count);

                //após a consulta atualiza o campo st_atualizada para false
                List<opMode<string>> opmode = new List<opMode<string>>();
                opmode.Add(new opMode<string> { args = new[] { "false" }, fn = null, mode = "update", path = "st_nova" });
                var result_opmode = normaRn.PathPut<string>(pesquisa_norma, opmode);
                this._sb_info.AppendLine(DateTime.Now + ": Atualizar campo ST_NOVA para false => " + result_opmode);

                foreach(var norma in results_normas.results){
                    var literal = "'"+norma.ch_tipo_norma+"'=any(ch_tipo_norma_criacao)";
                    foreach (var origem in norma.origens)
                    {
                        literal += " OR '" + origem.ch_orgao + "'=any(ch_orgao_criacao)";
                    }
                    foreach (var indexacao in norma.indexacoes)
                    {
                        foreach(var vocabulario in indexacao.vocabulario){
                            literal += " OR '" + vocabulario.ch_termo + "'=any(ch_termo_criacao)";
                        }
                    }

                    pesquisa_push.literal = "st_push AND (" + literal + ")";
                    pesquisa_push.limit = null;
                    var results_notifiqueme = notifiquemeRN.Consultar(pesquisa_push);
                    this._sb_info.AppendLine(DateTime.Now + ": Literal => " + literal);
                    this._sb_info.AppendLine(DateTime.Now + ": Total de usuários => " + results_notifiqueme.results.Count);

                    foreach (var usuario_push in results_notifiqueme.results)
                    {
                        this._sb_info.AppendLine(DateTime.Now + ": Verificando dados de monitoramento do usuário => " + usuario_push.email_usuario_push);
                        int quantidadeDeOrgaos = norma.origens.Count;
                        var ch_criacao_norma_monitorada = "";
                        foreach(var norma_monitorada in usuario_push.criacao_normas_monitoradas){
                            //Se a monitoração está desativada pula para a próxima.
                            if (!norma_monitorada.st_criacao) continue;

                            if(!string.IsNullOrEmpty(norma_monitorada.ch_tipo_norma_criacao) && !string.IsNullOrEmpty(norma_monitorada.ch_orgao_criacao) && !string.IsNullOrEmpty(norma_monitorada.ch_termo_criacao)){
                                if(norma_monitorada.primeiro_conector_criacao == "E" && norma_monitorada.segundo_conector_criacao == "E"){
                                    if(norma_monitorada.ch_tipo_norma_criacao == norma.ch_tipo_norma && norma.origens.Count<Orgao>(o=>o.ch_orgao == norma_monitorada.ch_orgao_criacao) > 0 && norma.indexacoes.Count<Indexacao>(i=>i.vocabulario.Count<Vocabulario>(v=>v.ch_termo == norma_monitorada.ch_termo_criacao) > 0) > 0){
                                        ch_criacao_norma_monitorada = norma_monitorada.ch_criacao_norma_monitorada;
                                        break;
                                    }
                                }
                                else if(norma_monitorada.primeiro_conector_criacao == "E" && norma_monitorada.segundo_conector_criacao == "OU"){
                                    if(norma_monitorada.ch_tipo_norma_criacao == norma.ch_tipo_norma && (norma.origens.Count<Orgao>(o=>o.ch_orgao == norma_monitorada.ch_orgao_criacao) > 0 || norma.indexacoes.Count<Indexacao>(i=>i.vocabulario.Count<Vocabulario>(v=>v.ch_termo == norma_monitorada.ch_termo_criacao) > 0) > 0)){
                                        ch_criacao_norma_monitorada = norma_monitorada.ch_criacao_norma_monitorada;
                                        break;
                                    }
                                }
                                else if(norma_monitorada.primeiro_conector_criacao == "OU" && norma_monitorada.segundo_conector_criacao == "E"){
                                    if(norma_monitorada.ch_tipo_norma_criacao == norma.ch_tipo_norma || (norma.origens.Count<Orgao>(o=>o.ch_orgao == norma_monitorada.ch_orgao_criacao) > 0 && norma.indexacoes.Count<Indexacao>(i=>i.vocabulario.Count<Vocabulario>(v=>v.ch_termo == norma_monitorada.ch_termo_criacao) > 0) > 0)){
                                        ch_criacao_norma_monitorada = norma_monitorada.ch_criacao_norma_monitorada;
                                        break;
                                    }
                                }
                                else{
                                    if(norma_monitorada.ch_tipo_norma_criacao == norma.ch_tipo_norma || norma.origens.Count<Orgao>(o=>o.ch_orgao == norma_monitorada.ch_orgao_criacao) > 0 || norma.indexacoes.Count<Indexacao>(i=>i.vocabulario.Count<Vocabulario>(v=>v.ch_termo == norma_monitorada.ch_termo_criacao) > 0) > 0){
                                        ch_criacao_norma_monitorada = norma_monitorada.ch_criacao_norma_monitorada;
                                        break;
                                    }
                                }
                            }
                            else if(!string.IsNullOrEmpty(norma_monitorada.ch_tipo_norma_criacao) && !string.IsNullOrEmpty(norma_monitorada.ch_orgao_criacao)){
                                if(norma_monitorada.primeiro_conector_criacao == "E"){
                                    if(norma_monitorada.ch_tipo_norma_criacao == norma.ch_tipo_norma && norma.origens.Count<Orgao>(o=>o.ch_orgao == norma_monitorada.ch_orgao_criacao) > 0){
                                        ch_criacao_norma_monitorada = norma_monitorada.ch_criacao_norma_monitorada;
                                        break;
                                    }
                                }
                                else{
                                    if(norma_monitorada.ch_tipo_norma_criacao == norma.ch_tipo_norma || norma.origens.Count<Orgao>(o=>o.ch_orgao == norma_monitorada.ch_orgao_criacao) > 0){
                                        ch_criacao_norma_monitorada = norma_monitorada.ch_criacao_norma_monitorada;
                                        break;
                                    }
                                }
                            }
                            else if(!string.IsNullOrEmpty(norma_monitorada.ch_orgao_criacao) && !string.IsNullOrEmpty(norma_monitorada.ch_termo_criacao)){
                                if(norma_monitorada.segundo_conector_criacao == "E"){
                                    if(norma.origens.Count<Orgao>(o=>o.ch_orgao == norma_monitorada.ch_orgao_criacao) > 0 && norma.indexacoes.Count<Indexacao>(i=>i.vocabulario.Count<Vocabulario>(v=>v.ch_termo == norma_monitorada.ch_termo_criacao) > 0) > 0){
                                        ch_criacao_norma_monitorada = norma_monitorada.ch_criacao_norma_monitorada;
                                        break;
                                    }
                                }
                                else{
                                    if(norma.origens.Count<Orgao>(o=>o.ch_orgao == norma_monitorada.ch_orgao_criacao) > 0 || norma.indexacoes.Count<Indexacao>(i=>i.vocabulario.Count<Vocabulario>(v=>v.ch_termo == norma_monitorada.ch_termo_criacao) > 0) > 0){
                                        ch_criacao_norma_monitorada = norma_monitorada.ch_criacao_norma_monitorada;
                                        break;
                                    }
                                }
                            }
                            else if(!string.IsNullOrEmpty(norma_monitorada.ch_tipo_norma_criacao) && !string.IsNullOrEmpty(norma_monitorada.ch_termo_criacao)){
                                if(norma_monitorada.primeiro_conector_criacao == "E" || norma_monitorada.segundo_conector_criacao == "E"){
                                    if(norma_monitorada.ch_tipo_norma_criacao == norma.ch_tipo_norma && norma.indexacoes.Count<Indexacao>(i=>i.vocabulario.Count<Vocabulario>(v=>v.ch_termo == norma_monitorada.ch_termo_criacao) > 0) > 0){
                                        ch_criacao_norma_monitorada = norma_monitorada.ch_criacao_norma_monitorada;
                                        break;
                                    }
                                }
                                else{
                                    if(norma_monitorada.ch_tipo_norma_criacao == norma.ch_tipo_norma || norma.indexacoes.Count<Indexacao>(i=>i.vocabulario.Count<Vocabulario>(v=>v.ch_termo == norma_monitorada.ch_termo_criacao) > 0) > 0){
                                        ch_criacao_norma_monitorada = norma_monitorada.ch_criacao_norma_monitorada;
                                        break;
                                    }
                                }
                            }
                            else if(!string.IsNullOrEmpty(norma_monitorada.ch_tipo_norma_criacao)){
                                if(norma_monitorada.ch_tipo_norma_criacao == norma.ch_tipo_norma){
                                    ch_criacao_norma_monitorada = norma_monitorada.ch_criacao_norma_monitorada;
                                    break;
                                }
                            }
                            else if(!string.IsNullOrEmpty(norma_monitorada.ch_orgao_criacao)){
                                if(norma.origens.Count<Orgao>(o=>o.ch_orgao == norma_monitorada.ch_orgao_criacao) > 0){
                                    ch_criacao_norma_monitorada = norma_monitorada.ch_criacao_norma_monitorada;
                                    break;
                                }
                            }
                            else if(!string.IsNullOrEmpty(norma_monitorada.ch_termo_criacao)){
                                if(norma.indexacoes.Count<Indexacao>(i=>i.vocabulario.Count<Vocabulario>(v=>v.ch_termo == norma_monitorada.ch_termo_criacao) > 0) > 0){
                                    ch_criacao_norma_monitorada = norma_monitorada.ch_criacao_norma_monitorada;
                                    break;
                                }
                            }
                        }

                        if(!string.IsNullOrEmpty(ch_criacao_norma_monitorada)){
                            var email = new EmailRN();
                            var display_name_remetente = "SINJ Notifica";
                            var destinatario = new[] { usuario_push.email_usuario_push };
                            var titulo = "Informações sobre o ato " + norma.nm_tipo_norma + " " + norma.nr_norma + " de " + norma.dt_assinatura + (quantidadeDeOrgaos > 0 ? " do órgão " + norma.origens[0].sg_orgao : "");
                            var html = true;

                            var _linkImagemEmailTopo = "" + Config.ValorChave("LinkSINJPadrao", true) + "/Imagens/topo_sinj.jpg";
                            var _linkImagemEmailRodape = "" + Config.ValorChave("LinkSINJPadrao", true) + "/Imagens/rodape_sinj.jpg";

                            var corpoEmail = "";
                            corpoEmail = corpoEmail + "<table width=\"100%\" style=\"border:1px\"> ";
                            corpoEmail = corpoEmail + "     <tr> <td>";
                            corpoEmail = corpoEmail + "<table width = \"600\" style=\"background:#ddffdc;\" align=\"center\" >";
                            corpoEmail = corpoEmail + "<tr>";
                            corpoEmail = corpoEmail + "<td>";
                            corpoEmail = corpoEmail + "<table width=\"600px\" align=\"center\">";
                            corpoEmail = corpoEmail + "<tr>";
                            corpoEmail = corpoEmail + "<td><a href=\"http://www.sinj.df.gov.br\" target=\"_blank\" title=\"http://www.sinj.df.gov.br\"><img src=" + _linkImagemEmailTopo + "></a></td>";
                            corpoEmail = corpoEmail + "</tr>";
                            corpoEmail = corpoEmail + "</table>";
                            corpoEmail = corpoEmail + "</td>";
                            corpoEmail = corpoEmail + "</tr>";
                            corpoEmail = corpoEmail + "<tr>";
                            corpoEmail = corpoEmail + "<td>";
                            corpoEmail = corpoEmail + "<table width=\"600px\" align=\"center\"><br/>";
                            corpoEmail = corpoEmail + "<HR SIZE=1 WIDTH=601 ALIGN=center>";
                            corpoEmail = corpoEmail + "<tr>";
                            corpoEmail = corpoEmail + "<td style=\"background-color: #B4E6CBs; text-align: left;\">";
                            corpoEmail = corpoEmail + "	<div style=\"margin-bottom: 3px; font-size: 12px; font-weight: bold; background-color:#B4E6CBs\">";
                            corpoEmail = corpoEmail + "		Um tipo de ato que você escolheu para monitorar foi criado.<br/>";
                            corpoEmail = corpoEmail + "		As informações sobre esse ato estão abaixo:";
                            corpoEmail = corpoEmail + "	</div>";

                            corpoEmail = corpoEmail + "	<div>";
                            corpoEmail = corpoEmail + "		Ato:";
                            corpoEmail = corpoEmail + "		<table cellspacing=\"0\" cellpadding=\"2\" rules=\"all\" border=\"1\" style=\"border-color:#A3A3A3;border-style:Solid;width:100%;border-collapse:collapse;font-size: 11px;\">";
                            corpoEmail = corpoEmail + "			<tbody>";
                            corpoEmail = corpoEmail + "			<tr class=\"textoCorVide\" align=\"left\" style=\"color:#323232;background-color:#B4E6CB;height:30px;\">";
                            corpoEmail = corpoEmail + "				<th scope=\"col\">Identificação</th>";
                            corpoEmail = corpoEmail + "				<th scope=\"col\">Ementa</th>";
                            corpoEmail = corpoEmail + "				<th scope=\"col\" style=\"width:80px;\">Link</th>";
                            corpoEmail = corpoEmail + "			</tr><tr align=\"left\" style=\"background-color:#F0F0F0;height:20px;\">";

                            corpoEmail = corpoEmail + "				<td valign=\"top\" style=\"width:100px;\">" + norma.nm_tipo_norma + " " + norma.nr_norma + " de " + norma.dt_assinatura;

                            if (quantidadeDeOrgaos > 0)
                            {
                                if (quantidadeDeOrgaos > 1)
                                {
                                    corpoEmail = corpoEmail + " dos órgãos: ";
                                    int i = 0;
                                    foreach (var orgao in norma.origens)
                                    {
                                        if (i == quantidadeDeOrgaos - 1)
                                            corpoEmail = corpoEmail + " e " + orgao.sg_orgao;
                                        else
                                            corpoEmail = corpoEmail + orgao.sg_orgao + ", ";
                                        i++;
                                    }
                                }
                                else
                                {
                                    corpoEmail = corpoEmail + " do órgão: " + norma.origens.First().sg_orgao;
                                }
                            }

                            corpoEmail = corpoEmail + "</td>";
                            corpoEmail = corpoEmail + "				<td valign=\"top\" style=\"width:200px;\">" + norma.ds_ementa + "</td>";
                            corpoEmail = corpoEmail + "				<td valign=\"top\" style=\"width:80px;\"> <a href=" + Config.ValorChave("LinkSINJ", true) + "/DetalhesDeNorma.aspx?id_norma=" + norma.ch_norma + ">Clique para ver o ato</a></td>";
                            corpoEmail = corpoEmail + "			</tr>";
                            corpoEmail = corpoEmail + "		</tbody>";
                            corpoEmail = corpoEmail + "		</table>";
                            corpoEmail = corpoEmail + "	</div>";
                            corpoEmail = corpoEmail + "		<div style=\"margin-bottom: 3px; font-size: 10px; font-weight: bold; background-color:#B4E6CBs\">";
                            corpoEmail = corpoEmail + "		<br/>";
                            corpoEmail = corpoEmail + "		<br/>";
                            corpoEmail = corpoEmail + "	</div>";
                            corpoEmail = corpoEmail + "	<div style=\"margin-bottom: 3px; font-size: 11px; font-weight: bold; background-color:#B4E6CBs\">";
                            corpoEmail = corpoEmail + "		<a href=" + Config.ValorChave("LinkSINJ", true) + "/DesativarNormaPush.aspx?email_usuario_push=" + usuario_push.email_usuario_push + "&" + "ch_criacao_norma_monitorada=" + ch_criacao_norma_monitorada + ">Não quero mais receber informações sobre a criação desses atos.</a>";
                            corpoEmail = corpoEmail + "	</div>";
                            corpoEmail = corpoEmail + "</td>";
                            corpoEmail = corpoEmail + "</tr>";
                            corpoEmail = corpoEmail + " </tr>";
                            corpoEmail = corpoEmail + " </table>";
                            corpoEmail = corpoEmail +
                                "<table width=\"600px\" style=\"background:#ddffdc;\" align=\"center\" > <br/>";
                            corpoEmail = corpoEmail + "<tr>";
                            corpoEmail = corpoEmail + "<td>";
                            corpoEmail = corpoEmail + "<HR SIZE=1 WIDTH=601 ALIGN=center>";
                            corpoEmail = corpoEmail + "<img src=" + _linkImagemEmailRodape + " width=\"600px\">";

                            corpoEmail = corpoEmail + "</td>";
                            corpoEmail = corpoEmail + "</tr>";
                            corpoEmail = corpoEmail + " </table >";
                            corpoEmail = corpoEmail + " </td>";
                            corpoEmail = corpoEmail + " </tr>";
                            corpoEmail = corpoEmail + " </table>";
                            corpoEmail = corpoEmail + "</td>";
                            corpoEmail = corpoEmail + "</tr>";
                            corpoEmail = corpoEmail + "</table>";

                            email.EnviaEmail(display_name_remetente, destinatario, titulo, html, corpoEmail);
                            var logEmail = new LogEmail();
                            logEmail.emails = destinatario;
                            logEmail.assunto = titulo;
                            logEmail.mensagem = corpoEmail;
                            TCDF.Sinj.Log.LogOperacao.gravar_operacao("APP.PUS.EMAIL", logEmail, "Aplicação Push", "SINJ_PUSH_APP");
                            this._sb_info.AppendLine(DateTime.Now + ": CRIACAO - Email enviado para " + destinatario[0] + " com o titulo " + titulo);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                Console.WriteLine("Exception: " + mensagem);
                this._sb_error.AppendLine(DateTime.Now + ": " + mensagem);
            }
        }

        public void MonitorarNormasAtualizadasPush()
        {
            Pesquisa pesquisa_norma = new Pesquisa();
            Pesquisa pesquisa_push = new Pesquisa();
            NotifiquemeOV notifiquemeOv = new NotifiquemeOV();
            NormaOV normaOv = new NormaOV();
            try
            {
                NormaRN normaRn = new NormaRN();
                pesquisa_norma.literal = "st_atualizada=true";
                pesquisa_norma.limit = null;
                pesquisa_norma.select = new[] { "id_doc", "ch_norma", "nm_tipo_norma", "origens", "nr_norma", "dt_assinatura", "ds_ementa" };
                var results_normas = normaRn.Consultar(pesquisa_norma);

                //após a consulta atualiza o campo st_atualizada para false
                List<opMode<string>> opmode = new List<opMode<string>>();
                opmode.Add(new opMode<string> { args = new[] { "false" }, fn = null, mode = "update", path = "st_atualizada" });
                var result_opmode = normaRn.PathPut<string>(pesquisa_norma, opmode);
                this._sb_info.AppendLine(DateTime.Now + ": Atualizar campo ST_ATUALIZADA para false => " + result_opmode);

                foreach (var resultado_norma in results_normas.results)
                {
                    normaOv = resultado_norma;
                    int quantidadeDeOrgaos = resultado_norma.origens.Count;
                    NotifiquemeRN notifiquemeRn = new NotifiquemeRN(false);
                    var query_user = "'" + resultado_norma.ch_norma + "'=any(ch_norma_monitorada) AND st_push=true";
                    pesquisa_push.literal = query_user;
                    pesquisa_push.limit = null;
                    var results_notifiqueme = notifiquemeRn.Consultar(pesquisa_push);
                    foreach (var usuario_push in results_notifiqueme.results)
                    {
                        notifiquemeOv = usuario_push;
                        if (!usuario_push.normas_monitoradas.Where(n => n.ch_norma_monitorada == resultado_norma.ch_norma).First().st_norma_monitorada)
                            continue;
                        var email = new EmailRN();
                        var display_name_remetente = "SINJ Notifica";
                        var destinatario = new[] { usuario_push.email_usuario_push };
                        var titulo = "Informações sobre o ato " + resultado_norma.nm_tipo_norma + " " + resultado_norma.nr_norma + " de " + resultado_norma.dt_assinatura + (quantidadeDeOrgaos > 0 ? " do órgão " + resultado_norma.origens[0].sg_orgao : "");
                        var html = true;

                        var _linkImagemEmailTopo = "" + Config.ValorChave("LinkSINJPadrao", true) + "/Imagens/topo_sinj.jpg";
                        var _linkImagemEmailRodape = "" + Config.ValorChave("LinkSINJPadrao", true) + "/Imagens/rodape_sinj.jpg";


                        var corpoEmail = "";
                        corpoEmail = corpoEmail + "<table width=\"100%\" style=\"border:1px\"> ";
                        corpoEmail = corpoEmail + "     <tr> <td>";
                        corpoEmail = corpoEmail + "<table width = \"600\" style=\"background:#ddffdc;\" align=\"center\" >";
                        corpoEmail = corpoEmail + "<tr>";
                        corpoEmail = corpoEmail + "<td>";
                        corpoEmail = corpoEmail + "<table width=\"600px\" align=\"center\">";
                        corpoEmail = corpoEmail + "<tr>";
                        corpoEmail = corpoEmail + "<td><a href=\"http://www.sinj.df.gov.br\" target=\"_blank\" title=\"http://www.sinj.df.gov.br\"><img src=" + _linkImagemEmailTopo + "></a></td>";
                        corpoEmail = corpoEmail + "</tr>";
                        corpoEmail = corpoEmail + "</table>";
                        corpoEmail = corpoEmail + "</td>";
                        corpoEmail = corpoEmail + "</tr>";
                        corpoEmail = corpoEmail + "<tr>";
                        corpoEmail = corpoEmail + "<td>";
                        corpoEmail = corpoEmail + "<table width=\"600px\" align=\"center\"><br/>";
                        corpoEmail = corpoEmail + "<HR SIZE=1 WIDTH=601 ALIGN=center>";
                        corpoEmail = corpoEmail + "<tr>";
                        corpoEmail = corpoEmail + "<td style=\"background-color: #B4E6CBs; text-align: left;\">";
                        corpoEmail = corpoEmail + "	<div style=\"margin-bottom: 3px; font-size: 12px; font-weight: bold; background-color:#B4E6CBs\">";
                        corpoEmail = corpoEmail + "		Um ato que você escolheu para monitorar sofreu alteração.<br/>";
                        corpoEmail = corpoEmail + "		As informações sobre esse ato estão abaixo:";
                        corpoEmail = corpoEmail + "	</div>";

                        corpoEmail = corpoEmail + "	<div>";
                        corpoEmail = corpoEmail + "		Ato:";
                        corpoEmail = corpoEmail + "		<table cellspacing=\"0\" cellpadding=\"2\" rules=\"all\" border=\"1\" style=\"border-color:#A3A3A3;border-style:Solid;width:100%;border-collapse:collapse;font-size: 11px;\">";
                        corpoEmail = corpoEmail + "			<tbody>";
                        corpoEmail = corpoEmail + "			<tr class=\"textoCorVide\" align=\"left\" style=\"color:#323232;background-color:#B4E6CB;height:30px;\">";
                        corpoEmail = corpoEmail + "				<th scope=\"col\">Identificação</th>";
                        corpoEmail = corpoEmail + "				<th scope=\"col\">Ementa</th>";
                        corpoEmail = corpoEmail + "				<th scope=\"col\" style=\"width:80px;\">Link</th>";
                        corpoEmail = corpoEmail + "			</tr><tr align=\"left\" style=\"background-color:#F0F0F0;height:20px;\">";

                        corpoEmail = corpoEmail + "				<td valign=\"top\" style=\"width:100px;\">" + resultado_norma.nm_tipo_norma + " " + resultado_norma.nr_norma + " de " + resultado_norma.dt_assinatura;

                        if (quantidadeDeOrgaos > 0)
                        {
                            if (quantidadeDeOrgaos > 1)
                            {
                                corpoEmail = corpoEmail + " dos órgãos: ";
                                int i = 0;
                                foreach (var orgao in resultado_norma.origens)
                                {
                                    if (i == quantidadeDeOrgaos - 1)
                                        corpoEmail = corpoEmail + " e " + orgao.sg_orgao;
                                    else
                                        corpoEmail = corpoEmail + orgao.sg_orgao + ", ";
                                    i++;
                                }
                            }
                            else
                            {
                                corpoEmail = corpoEmail + " do órgão: " + resultado_norma.origens[0].sg_orgao;
                            }
                        }

                        corpoEmail = corpoEmail + "</td>";
                        corpoEmail = corpoEmail + "				<td valign=\"top\" style=\"width:200px;\">" + resultado_norma.ds_ementa + "</td>";
                        corpoEmail = corpoEmail + "				<td valign=\"top\" style=\"width:80px;\"> <a href=" + Config.ValorChave("LinkSINJ", true) + "/DetalhesDeNorma.aspx?id_norma=" + resultado_norma.ch_norma + ">Clique para ver o ato</a></td>";
                        corpoEmail = corpoEmail + "			</tr>";
                        corpoEmail = corpoEmail + "		</tbody>";
                        corpoEmail = corpoEmail + "		</table>";
                        corpoEmail = corpoEmail + "	</div>";
                        corpoEmail = corpoEmail + "		<div style=\"margin-bottom: 3px; font-size: 10px; font-weight: bold; background-color:#B4E6CBs\">";
                        corpoEmail = corpoEmail + "		<br/>";
                        corpoEmail = corpoEmail + "		<br/>";
                        corpoEmail = corpoEmail + "	</div>";
                        corpoEmail = corpoEmail + "	<div style=\"margin-bottom: 3px; font-size: 11px; font-weight: bold; background-color:#B4E6CBs\">";
                        corpoEmail = corpoEmail + "		<a href=" + Config.ValorChave("LinkSINJ", true) + "/DesativarNormaPush.aspx?ch_norma_monitorada=" + resultado_norma.ch_norma + "&" + "email_usuario_push=" + usuario_push.email_usuario_push + ">Não quero mais receber informações sobre este ato. </a>";
                        corpoEmail = corpoEmail + "	</div>";
                        corpoEmail = corpoEmail + "</td>";
                        corpoEmail = corpoEmail + "</tr>";
                        corpoEmail = corpoEmail + " </tr>";
                        corpoEmail = corpoEmail + " </table>";
                        corpoEmail = corpoEmail + "<table width=\"600px\" style=\"background:#ddffdc;\" align=\"center\" > <br/>";
                        corpoEmail = corpoEmail + "<tr>";
                        corpoEmail = corpoEmail + "<td>";
                        corpoEmail = corpoEmail + "<HR SIZE=1 WIDTH=601 ALIGN=center>";
                        corpoEmail = corpoEmail + "<img src=" + _linkImagemEmailRodape + " width=\"600px\">";
                        corpoEmail = corpoEmail + "</td>";
                        corpoEmail = corpoEmail + "</tr>";
                        corpoEmail = corpoEmail + " </table >";
                        corpoEmail = corpoEmail + " </td>";
                        corpoEmail = corpoEmail + " </tr>";
                        corpoEmail = corpoEmail + " </table>";
                        corpoEmail = corpoEmail + "</td>";
                        corpoEmail = corpoEmail + "</tr>";
                        corpoEmail = corpoEmail + "</table>";

                        email.EnviaEmail(display_name_remetente, destinatario, titulo, html, corpoEmail);
                        var logEmail = new LogEmail();
                        logEmail.emails = destinatario;
                        logEmail.assunto = titulo;
                        logEmail.mensagem = corpoEmail;
                        LogOperacao.gravar_operacao("APP.PUS.EMAIL", logEmail, "Aplicação Push", "SINJ_PUSH_APP");
                        this._sb_info.AppendLine(DateTime.Now + ": ALTERACAO - Email enviado para " + destinatario[0] + " com o titulo " + titulo);
                    }
                }
            }
            catch (Exception ex)
            {
                var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                Console.WriteLine("Exception: " + mensagem);
                this._sb_error.AppendLine(DateTime.Now + ": " + mensagem);
            }
        }

        public void MonitorarDiarioPush()
        {
            Pesquisa pesquisaDiario = new Pesquisa();
            Pesquisa pesquisaPush = new Pesquisa();
            NotifiquemeOV notifiquemeOv = new NotifiquemeOV();
            DiarioOV diarioOv = new DiarioOV();
            try
            {
                NotifiquemeRN notifiquemeRN = new NotifiquemeRN(false);
                DiarioRN diarioRn = new DiarioRN();
                pesquisaDiario.literal = "st_novo AND dt_idx is not null";
                pesquisaDiario.limit = null;
                pesquisaDiario.select = new[] { "id_doc" };
                var resultsDiario = diarioRn.Consultar(pesquisaDiario);
                this._sb_info.AppendLine(DateTime.Now + ": Total de diários ST_NOVO=true => " + resultsDiario.results.Count);
                if (resultsDiario.results.Count > 0)
                {
                    var idsDiario = "";
                    string sFiltersId = "";
                    string sFilters = "";
                    var sQuery = "";
                    var corpoEmail = "";
                    var _linkImagemEmailTopo = "" + Config.ValorChave("LinkSINJPadrao", true) + "/Imagens/topo_sinj_large.jpg";
                    var url_es = new DocEs().GetUrlEs(util.BRLight.Util.GetVariavel("NmBaseDiario", true)) + "/_search";
                    foreach (var diario in resultsDiario.results)
                    {
                        idsDiario += (!string.IsNullOrEmpty(idsDiario) ? " OR " : "") + "id_doc=" + diario._metadata.id_doc;
                        sFiltersId += (!string.IsNullOrEmpty(sFiltersId) ? "," : "") + "{\"term\":{\"id_doc\":\""+diario._metadata.id_doc+"\"}}";
                    }
                    pesquisaDiario.literal = idsDiario;
                    //após a consulta atualiza o campo st_novo para false
                    List<opMode<string>> opmode = new List<opMode<string>>();
                    opmode.Add(new opMode<string> { args = new[] { "false" }, fn = null, mode = "update", path = "st_novo" });

                    var result_opmode = diarioRn.PathPut<string>(pesquisaDiario, opmode);
                    this._sb_info.AppendLine(DateTime.Now + ": Atualizar campo ST_NOVO para false => " + result_opmode);

                    pesquisaPush.literal = "st_push";
                    pesquisaPush.limit = null;
                    var resultsNotifiqueme = notifiquemeRN.Consultar(pesquisaPush);
                    this._sb_info.AppendLine(DateTime.Now + ": Total de usuários => " + resultsNotifiqueme.results.Count);

                    foreach (var usuarioPush in resultsNotifiqueme.results)
                    {
                        if(usuarioPush.termos_diarios_monitorados.Count > 0){
                            foreach(var termo_diario in usuarioPush.termos_diarios_monitorados){
                                if (termo_diario.st_termo_diario_monitorado)
                                {
                                    var textoConsultado = termo_diario.ds_termo_diario_monitorado.Replace("\"", "");
                                    if(textoConsultado.Contains<char>(' ')){
                                        if (termo_diario.in_exata_diario_monitorado)
                                        {
                                            textoConsultado = "\\\"" + textoConsultado + "\\\"";
                                        }
                                        else
                                        {
                                            textoConsultado = "\\\"" + textoConsultado + "\\\"~5";
                                        }
                                    }
                                    sFilters = ",\"filter\":{\"and\":[{\"or\":[" + sFiltersId + "]}";
                                    if (!string.IsNullOrEmpty(termo_diario.ch_tipo_fonte_diario_monitorado))
                                    {
                                        sFilters += ",{\"or\":[{\"term\":{\"ch_tipo_fonte\":\""+termo_diario.ch_tipo_fonte_diario_monitorado+"\"}}]}";
                                    }
                                    else{
                                        sFilters += ",{\"or\":[{\"term\":{\"ch_tipo_fonte\":\"1\"}}";
                                        sFilters += ",{\"term\":{\"ch_tipo_fonte\":\"4\"}}";
                                        sFilters += ",{\"term\":{\"ch_tipo_fonte\":\"11\"}}]}";
                                    }
                                    sFilters += "]}";
                                    sQuery = "{\"_source\":{\"exclude\":[\"*.filetext\"]},\"query\": {\"filtered\":{\"query\":{\"query_string\":{\"fields\":[\"arquivos.arquivo_diario.filetext\"],\"query\":\"" + textoConsultado + "\"}}" + sFilters + "}},\"highlight\":{\"pre_tags\":[\"<b>\"],\"post_tags\":[\"</b>\"],\"fields\":{\"arquivos.arquivo_diario.filetext\":{\"number_of_fragments\":8, \"fragment_size\":150}}}}";
                                    var diarios = new ES().PesquisarDocs<DiarioOV>(sQuery, url_es);
                                    if (diarios.hits.hits.Count > 0)
                                    {
                                        var email = new EmailRN();
                                        var display_name_remetente = "SINJ Notifica";
                                        var destinatario = new[] { usuarioPush.email_usuario_push };
                                        var titulo = (diarios.hits.hits.Count > 1 ? "Foram cadastrador " + diarios.hits.hits.Count + " diários foram cadastrados" : "Foi cadastrado um diário") + " no SINJ contendo o texto: " + termo_diario.ds_termo_diario_monitorado + ".";
                                        var html = true;


                                        corpoEmail = "";
                                        corpoEmail = corpoEmail + "<table width=\"100%\" style=\"border:1px\"> ";
                                        corpoEmail = corpoEmail + "    <tr>";
                                        corpoEmail = corpoEmail + "        <td>";
                                        corpoEmail = corpoEmail + "            <table width = \"800\" style=\"background:#ddffdc;\" align=\"center\" >";
                                        corpoEmail = corpoEmail + "                <tr>";
                                        corpoEmail = corpoEmail + "                    <td>";
                                        corpoEmail = corpoEmail + "                       <table width=\"800px\" align=\"center\">";
                                        corpoEmail = corpoEmail + "                           <tr>";
                                        corpoEmail = corpoEmail + "                               <td><a href=\"http://www.sinj.df.gov.br\" target=\"_blank\" title=\"http://www.sinj.df.gov.br\"><img src=" + _linkImagemEmailTopo + "></a></td>";
                                        corpoEmail = corpoEmail + "                           </tr>";
                                        corpoEmail = corpoEmail + "                       </table>";
                                        corpoEmail = corpoEmail + "                   </td>";
                                        corpoEmail = corpoEmail + "                </tr>";
                                        corpoEmail = corpoEmail + "                <tr>";
                                        corpoEmail = corpoEmail + "                    <td>";
                                        corpoEmail = corpoEmail + "                        <table width=\"800px\" align=\"center\"><br/>";
                                        corpoEmail = corpoEmail + "                            <HR SIZE=1 WIDTH=801 ALIGN=center>";
                                        corpoEmail = corpoEmail + "                            <tr>";
                                        corpoEmail = corpoEmail + "                                <td style=\"background-color: #B4E6CBs; text-align: left;\">";
                                        corpoEmail = corpoEmail + "                                    <div style=\"margin-bottom: 3px; font-size: 12px; font-weight: bold; background-color:#B4E6CBs\">";
                                        corpoEmail = corpoEmail + "	                                       " + titulo + "<br/>";
                                        corpoEmail = corpoEmail + "		                                   As informações sobre " + (diarios.hits.hits.Count > 1 ? diarios.hits.hits.Count + "esses diários" : "esse diário") + " estão abaixo:";
                                        corpoEmail = corpoEmail + "	                                   </div>";
                                        corpoEmail = corpoEmail + "	                                   <div>";
                                        corpoEmail = corpoEmail + "		                                   <table cellspacing=\"0\" cellpadding=\"2\" rules=\"all\" border=\"1\" style=\"border-color:#A3A3A3;border-style:Solid;width:100%;border-collapse:collapse;font-size: 11px;\">";
                                        corpoEmail = corpoEmail + "			                                   <tbody>";
                                        corpoEmail = corpoEmail + "			                                       <tr class=\"textoCorVide\" align=\"left\" style=\"color:#323232;background-color:#B4E6CB;height:30px;\">";
                                        corpoEmail = corpoEmail + "				                                       <th scope=\"col\"style=\"width:200px;\">Diário</th>";
                                        corpoEmail = corpoEmail + "				                                       <th scope=\"col\">Partes do texto encontrado</th>";
                                        corpoEmail = corpoEmail + "				                                       <th scope=\"col\" style=\"width:120px;\">Link</th>";
                                        corpoEmail = corpoEmail + "			                                       </tr>";
                                        foreach(var diario in diarios.hits.hits){
                                            corpoEmail = corpoEmail + "			                                   <tr align=\"left\" style=\"background-color:#F0F0F0;height:20px;\">";
                                            corpoEmail = corpoEmail + "				                                   <td valign=\"top\">" + diario._source.getDescricaoDiario() + "</td>";
                                            var aTexto = ((JContainer) diario.highlight)["arquivos.arquivo_diario.filetext"];
                                            corpoEmail = corpoEmail + "				                                   <td valign=\"top\">..." + string.Join("...<br/>...", aTexto) + "...</td>";
                                            var i = 0;
                                            corpoEmail = corpoEmail + "				                                   <td valign=\"top\">";
                                            foreach(var arquivo in diario._source.arquivos){
                                                corpoEmail = corpoEmail + "<a href=\"" + Config.ValorChave("LinkSINJ", true) + "/Diario/" + diario._source.ch_diario + "/" + arquivo.arquivo_diario.id_file + "/arq/" + i + "/" + arquivo.arquivo_diario.filename + "\">Clique para ver o diário"+(!string.IsNullOrEmpty(arquivo.ds_arquivo) ? arquivo.ds_arquivo : "")+"</a><br/>";
                                                i++;
                                            }
                                            corpoEmail = corpoEmail + "                                                </td>";
                                            corpoEmail = corpoEmail + "                                            </tr>";
                                        }
                                        corpoEmail = corpoEmail + "                                            </tbody>";
                                        corpoEmail = corpoEmail + "                                        </table>";
                                        corpoEmail = corpoEmail + "                                    </div><br/><br/>";
                                        corpoEmail = corpoEmail + "                                    <div style=\"margin-bottom: 3px; font-size: 11px; font-weight: bold; background-color:#B4E6CBs\">";
                                        corpoEmail = corpoEmail + "	                                       <a href=\"" + Config.ValorChave("LinkSINJ", true) + "/DesativarNormaPush.aspx?email_usuario_push=" + usuarioPush.email_usuario_push + "&" + "ch_termo_diario_monitorado=" + termo_diario.ch_termo_diario_monitorado + "\">Não quero mais receber informações sobre a criação de diários com esse texto.</a>";
                                        corpoEmail = corpoEmail + "	                                   </div>";
                                        corpoEmail = corpoEmail + "                                </td>";
                                        corpoEmail = corpoEmail + "                            </tr>";
                                        corpoEmail = corpoEmail + "                        </table>";
                                        corpoEmail = corpoEmail + "                    </td>";
                                        corpoEmail = corpoEmail + "                </tr>";
                                        corpoEmail = corpoEmail + "                <tr>";
                                        corpoEmail = corpoEmail + "                    <td>";
                                        corpoEmail = corpoEmail + "                        <table width=\"800px\" style=\"background:#ddffdc;\" align=\"center\" > <br/>";
                                        corpoEmail = corpoEmail + "                            <tr>";
                                        corpoEmail = corpoEmail + "                                <th>";
                                        corpoEmail = corpoEmail + "                                    <HR SIZE=1 WIDTH=801 ALIGN=center>";
                                        corpoEmail = corpoEmail + "                                    <a href=\"http://www.sinj.df.gov.br\">www.sinj.df.gov.br</a>";
                                        corpoEmail = corpoEmail + "                                </th>";
                                        corpoEmail = corpoEmail + "                            </tr>";
                                        corpoEmail = corpoEmail + "                        </table >";
                                        corpoEmail = corpoEmail + "                    </td>";
                                        corpoEmail = corpoEmail + "                </tr>";
                                        corpoEmail = corpoEmail + "            </table>";
                                        corpoEmail = corpoEmail + "        </td>";
                                        corpoEmail = corpoEmail + "    </tr>";
                                        corpoEmail = corpoEmail + "</table>";

                                        email.EnviaEmail(display_name_remetente, destinatario, titulo, html, corpoEmail);
                                        var logEmail = new LogEmail();
                                        logEmail.emails = destinatario;
                                        logEmail.assunto = titulo;
                                        logEmail.mensagem = corpoEmail;
                                        TCDF.Sinj.Log.LogOperacao.gravar_operacao("APP.PUS.EMAIL", logEmail, "Aplicação Push", "SINJ_PUSH_APP");
                                        this._sb_info.AppendLine(DateTime.Now + ": CRIACAO - Email enviado para " + destinatario[0] + " com o titulo " + titulo);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var mensagem = util.BRLight.Excecao.LerTodasMensagensDaExcecao(ex, false);
                Console.WriteLine("Exception: " + mensagem);
                this._sb_error.AppendLine(DateTime.Now + ": " + mensagem);
            }
        }

        private void Log()
        {
            if (!_file_error.Directory.Exists)
            {
                _file_error.Directory.Create();
            }
            var stream_error = _file_error.AppendText();
            stream_error.Write(_sb_error.ToString());
            stream_error.Flush();
            stream_error.Close();

            if (!_file_info.Directory.Exists)
            {
                _file_info.Directory.Create();
            }
            var stream_info = _file_info.AppendText();
            stream_info.Write(_sb_info.ToString());
            stream_info.Flush();
            stream_info.Close();
            _sb_error.Clear();
            _sb_info.Clear();
        }
    }
}

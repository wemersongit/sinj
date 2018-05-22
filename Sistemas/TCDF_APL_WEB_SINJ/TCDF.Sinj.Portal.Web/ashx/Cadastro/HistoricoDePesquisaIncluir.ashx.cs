using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;

namespace TCDF.Sinj.Portal.Web.ashx.Cadastro
{
    /// <summary>
    /// Summary description for HistoricoDePesquisaIncluir
    /// </summary>
    public class HistoricoDePesquisaIncluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var _chave = context.Request["chave"];
            var _tipo_pesquisa = context.Request["tipo_pesquisa"];
            //var _consulta = context.Request.QueryString.GetValues("consulta");
            var _sTotais = context.Request.QueryString.GetValues("total");
            HistoricoDePesquisaRN historicoDePesquisaRn = new HistoricoDePesquisaRN();
            try
            {
                //var consulta = string.Join("&", _consulta);
                //consulta = HttpUtility.UrlEncode(consulta);

                var pesquisa = new HistoricoDePesquisaOV();
                pesquisa.ch_usuario = _chave;
                //pesquisa.consulta = consulta;
                pesquisa.dt_historico = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                foreach(var sTotal in _sTotais)
                {
                    pesquisa.total.Add(JSON.Deserializa<TotalOV>(sTotal));
                }
                if (_tipo_pesquisa == "geral")
                {
                    SentencaPesquisaGeralOV pesquisaGeral = new SentencaPesquisaGeralOV();
                    pesquisaGeral.all = context.Request["all"];
                    historicoDePesquisaRn.MontarPesquisa(pesquisaGeral, pesquisa);
                }
                else if (_tipo_pesquisa == "norma")
                {
                    SentencaPesquisaDiretaNormaOV pesquisaDireta = new SentencaPesquisaDiretaNormaOV();
                    pesquisaDireta.all = context.Request["all"];
                    pesquisaDireta.ch_tipo_norma = context.Request["ch_tipo_norma"];
                    pesquisaDireta.nm_tipo_norma = context.Request["nm_tipo_norma"];
                    pesquisaDireta.nr_norma = context.Request["nr_norma"];
                    pesquisaDireta.norma_sem_numero = context.Request["norma_sem_numero"];
                    pesquisaDireta.ano_assinatura = context.Request["ano_assinatura"];
                    pesquisaDireta.ch_orgao = context.Request["ch_orgao"];
                    pesquisaDireta.nm_orgao = context.Request["sg_hierarquia_nm_vigencia"];
                    pesquisaDireta.origem_por = context.Request["origem_por"];
                    pesquisaDireta.nm_termos = context.Request.Params.GetValues("nm_termo");
                    pesquisaDireta.ch_termos = context.Request.Params.GetValues("ch_termo");
                    historicoDePesquisaRn.MontarPesquisa(pesquisaDireta, pesquisa);
                }
                else if (_tipo_pesquisa == "diario")
                {
                    SentencaPesquisaDiretaDiarioOV pesquisaDireta = new SentencaPesquisaDiretaDiarioOV();
                    pesquisaDireta.ds_norma = context.Request["ds_norma"];
                    pesquisaDireta.ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                    pesquisaDireta.nm_tipo_fonte = context.Request["nm_tipo_fonte"];
                    pesquisaDireta.ch_tipo_edicao = context.Request["ch_tipo_edicao"];
                    pesquisaDireta.nm_tipo_edicao = context.Request["nm_tipo_edicao"];
                    pesquisaDireta.nr_diario = context.Request["nr_diario"];
                    pesquisaDireta.secao_diario = context.Request.Params.GetValues("secao_diario");
                    pesquisaDireta.filetext = context.Request["filetext"];
                    pesquisaDireta.op_dt_assinatura = context.Request["op_dt_assinatura"];
                    pesquisaDireta.dt_assinatura = context.Request.Params.GetValues("dt_assinatura");
                    historicoDePesquisaRn.MontarPesquisa(pesquisaDireta, pesquisa);
                }
                else if (_tipo_pesquisa == "notifiqueme")
                {
                    SentencaPesquisaNotifiquemeDiarioOV pesquisaNotifiqueme = new SentencaPesquisaNotifiquemeDiarioOV();
                    pesquisaNotifiqueme.ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                    pesquisaNotifiqueme.nm_tipo_fonte = context.Request["nm_tipo_fonte"];
                    pesquisaNotifiqueme.filetext = context.Request["filetext"];
                    pesquisaNotifiqueme.in_exata = context.Request["in_exata"];
                    historicoDePesquisaRn.MontarPesquisa(pesquisaNotifiqueme, pesquisa);
                }
                else if (_tipo_pesquisa == "diretorio_diario")
                {
                    SentencaPesquisaDiretorioDiarioOV pesquisaDiretorio = new SentencaPesquisaDiretorioDiarioOV();
                    pesquisaDiretorio.ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                    pesquisaDiretorio.nm_tipo_fonte = context.Request["nm_tipo_fonte"];
                    pesquisaDiretorio.ano = context.Request["ano"];
                    pesquisaDiretorio.mes = context.Request["mes"];
                    historicoDePesquisaRn.MontarPesquisa(pesquisaDiretorio, pesquisa);
                }
                else if (_tipo_pesquisa == "texto_diario")
                {
                    SentencaPesquisaTextoDiarioOV pesquisaTexto = new SentencaPesquisaTextoDiarioOV();
                    pesquisaTexto.ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                    pesquisaTexto.nm_tipo_fonte = context.Request["nm_tipo_fonte"];
                    pesquisaTexto.filetext = context.Request["filetext"];
                    pesquisaTexto.intervalo = context.Request["intervalo"];
                    pesquisaTexto.dt_assinatura_inicio = context.Request["dt_assinatura_inicio"];
                    pesquisaTexto.dt_assinatura_termino = context.Request["dt_assinatura_termino"];
                    historicoDePesquisaRn.MontarPesquisa(pesquisaTexto, pesquisa);
                }
                else if (_tipo_pesquisa == "avancada")
                {
                    SentencaPesquisaAvancadaNormaOV pesquisaAvancada = new SentencaPesquisaAvancadaNormaOV();
                    pesquisaAvancada.ch_tipo_norma = context.Request.Params.GetValues("ch_tipo_norma");
                    pesquisaAvancada.argumentos = context.Request.Params.GetValues("argumento");
                    historicoDePesquisaRn.MontarPesquisa(pesquisaAvancada, pesquisa);
                }

                if (!Util.ehReplica())
                {
                    historicoDePesquisaRn.Incluir(pesquisa);
                }

                sRetorno = "{\"success_message\":\"Histórico incluído com sucesso\", \"pesquisa\":" + JSON.Serialize<HistoricoDePesquisaOV>(pesquisa) + "}";
            }
            catch (Exception ex)
            {
                var sErro = Excecao.LerTodasMensagensDaExcecao(ex, false);
                sRetorno = "{\"error_message\":\"" + sErro + "\"}";
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                LogErro.gravar_erro("HST.INC", erro, "visitante", "visitante");
            }
            context.Response.Write(sRetorno);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
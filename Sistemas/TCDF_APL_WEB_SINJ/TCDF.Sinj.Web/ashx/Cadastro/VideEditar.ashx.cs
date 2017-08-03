using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.Web.ashx.Arquivo;
using System.Text.RegularExpressions;
using neo.BRLightREST;
using TCDF.Sinj.Web.ashx.Exclusao;

namespace TCDF.Sinj.Web.ashx.Cadastro
{
    /// <summary>
    /// Summary description for VideEditar
    /// </summary>
    public class VideEditar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            NormaOV normaOv = new NormaOV();
            NormaOV normaAlteradoraOv = null;
            NormaOV normaAlteradaOv = null;
            Vide videAlteradorDesfazer = null;
            Vide videAlteradoDesfazer = null;
            var _id_doc = context.Request["id_doc"];
            ulong id_doc = 0;
            var _ch_vide = context.Request["ch_vide"];

            var _dt_controle_alteracao = context.Request["dt_controle_alteracao"];
            var _ch_tipo_relacao = context.Request["ch_tipo_relacao"];
            var _nm_tipo_relacao = context.Request["nm_tipo_relacao"];
            var _ds_texto_para_alterador = context.Request["ds_texto_para_alterador"];
            var _ds_texto_para_alterado = context.Request["ds_texto_para_alterado"];
            var ch_tipo_relacao_pos_verificacao = _ch_tipo_relacao;
            var nm_tipo_relacao_pos_verificacao = _nm_tipo_relacao;
            var ds_texto_para_alterador_pos_verificacao = _ds_texto_para_alterador;
            var ds_texto_para_alterado_pos_verificacao = _ds_texto_para_alterado;

            var _caput_norma_vide_alteradora = context.Request["caput_norma_vide_alteradora"];

            //var _artigo_norma_vide = context.Request["artigo_norma_vide"];
            //var _paragrafo_norma_vide = context.Request["paragrafo_norma_vide"];
            //var _inciso_norma_vide = context.Request["inciso_norma_vide"];
            //var _alinea_norma_vide = context.Request["alinea_norma_vide"];
            //var _item_norma_vide = context.Request["item_norma_vide"];
            //var _anexo_norma_vide = context.Request["anexo_norma_vide"];

            var _caput_norma_vide_alterada = context.Request["caput_norma_vide_alterada"];
            var _ds_caput_norma_alterada = context.Request["ds_caput_norma_alterada"];

            var _artigo_norma_vide_alterada = context.Request["artigo_norma_vide_alterada"];
            var _paragrafo_norma_vide_alterada = context.Request["paragrafo_norma_vide_alterada"];
            var _inciso_norma_vide_alterada = context.Request["inciso_norma_vide_alterada"];
            var _alinea_norma_vide_alterada = context.Request["alinea_norma_vide_alterada"];
            var _item_norma_vide_alterada = context.Request["item_norma_vide_alterada"];
            var _anexo_norma_vide_alterada = context.Request["anexo_norma_vide_alterada"];

            var _ds_comentario_vide = context.Request["ds_comentario_vide"];

            var _ch_tipo_norma_vide_fora_do_sistema = context.Request["ch_tipo_norma_vide_fora_do_sistema"];
            var _nm_tipo_norma_vide_fora_do_sistema = context.Request["nm_tipo_norma_vide_fora_do_sistema"];
            var _nr_norma_vide_fora_do_sistema = context.Request["nr_norma_vide_fora_do_sistema"];
            var _ch_tipo_fonte_vide_fora_do_sistema = context.Request["ch_tipo_fonte_vide_fora_do_sistema"];
            var _nm_tipo_fonte_vide_fora_do_sistema = context.Request["nm_tipo_fonte_vide_fora_do_sistema"];
            var _dt_publicacao_norma_vide_fora_do_sistema = context.Request["dt_publicacao_norma_vide_fora_do_sistema"];
            var _nr_pagina_publicacao_norma_vide_fora_do_sistema = context.Request["nr_pagina_publicacao_norma_vide_fora_do_sistema"];
            var _nr_coluna_publicacao_norma_vide_fora_do_sistema = context.Request["nr_coluna_publicacao_norma_vide_fora_do_sistema"];


            var _caput_texto_novo = context.Request.Form.GetValues("texto_novo");

            var action = AcoesDoUsuario.nor_edt;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                var dDt_controle_alteracao = Convert.ToDateTime(_dt_controle_alteracao);
                var normaRn = new NormaRN();
                if (!string.IsNullOrEmpty(_id_doc) && !string.IsNullOrEmpty(_ch_vide))
                {
                    if (!string.IsNullOrEmpty(_ch_tipo_relacao))
                    {
                        id_doc = ulong.Parse(_id_doc);
                        normaOv = normaRn.Doc(id_doc);

                        TipoDeRelacaoOV relacao = null;
                        //if (!string.IsNullOrEmpty(_artigo_norma_vide_alterada))
                        //{
                        //    relacao = normaRn.ObterRelacaoParcial(_ch_tipo_relacao);
                        //}
                        //else
                        //{
                        relacao = normaRn.ObterRelacao(_ch_tipo_relacao);
                        //}
                        if (relacao != null)
                        {
                            ch_tipo_relacao_pos_verificacao = relacao.ch_tipo_relacao;
                            nm_tipo_relacao_pos_verificacao = relacao.nm_tipo_relacao;
                            ds_texto_para_alterado_pos_verificacao = relacao.ds_texto_para_alterado;
                            ds_texto_para_alterador_pos_verificacao = relacao.ds_texto_para_alterador;
                        }

                        foreach (var vide in normaOv.vides)
                        {
                            if (vide.ch_vide == _ch_vide)
                            {
                                if (vide.in_norma_afetada)
                                {
                                    normaAlteradaOv = normaOv;
                                    normaAlteradoraOv = normaRn.Doc(vide.ch_norma_vide);
                                }
                                else
                                {
                                    normaAlteradoraOv = normaOv;
                                    if (!string.IsNullOrEmpty(vide.ch_norma_vide))
                                    {
                                        normaAlteradaOv = normaRn.Doc(vide.ch_norma_vide);
                                    }
                                }
                                break;
                            }
                        }
                        if (normaAlteradoraOv != null)
                        {
                            //Caput caput_norma_vide_alteradora = null;
                            //Caput caput_norma_vide_alterada = null;
                            //var adicionar_caput = false;
                            //var alterar_caput = false;
                            //var remover_caput = false;

                            //Vai comparar a data da ultima alteração com a data que o usuário abriu a página de editar vides e disparar uma exceção de risco de inconsistencia
                            if (normaAlteradoraOv.alteracoes.Count > 0)
                            {
                                var dDt_alteracao = Convert.ToDateTime(normaAlteradoraOv.alteracoes.Last<AlteracaoOV>().dt_alteracao);
                                var usuario = normaAlteradoraOv.alteracoes.Last<AlteracaoOV>().nm_login_usuario_alteracao;
                                if (dDt_controle_alteracao < dDt_alteracao)
                                {
                                    throw new RiskOfInconsistency("A norma alteradora foi modificada pelo usuário <b>" + usuario + "</b> às <b>" + dDt_alteracao + "</b> tornando a sua modificação inconsistente.<br/> É aconselhável atualizar a página e refazer as modificações ou forçar a alteração.<br/>Obs.: Clicar em 'Salvar mesmo assim' vai forçar a alteração e pode sobrescrever as modificações do usuário <b>" + usuario + "</b>.");
                                }
                            }

                            //Vai comparar a data da ultima alteração com a data que o usuário abriu a página de editar vides e disparar uma exceção de risco de inconsistencia
                            if (normaAlteradaOv != null && normaAlteradaOv.alteracoes.Count > 0)
                            {
                                var dDt_alteracao = Convert.ToDateTime(normaAlteradaOv.alteracoes.Last<AlteracaoOV>().dt_alteracao);
                                var usuario = normaAlteradaOv.alteracoes.Last<AlteracaoOV>().nm_login_usuario_alteracao;
                                if (dDt_controle_alteracao < dDt_alteracao)
                                {
                                    throw new RiskOfInconsistency("A norma alterada foi modificada pelo usuário <b>" + usuario + "</b> às <b>" + dDt_alteracao + "</b> tornando a sua modificação inconsistente.<br/> É aconselhável atualizar a página e refazer as modificações ou forçar a alteração.<br/>Obs.: Clicar em 'Salvar mesmo assim' vai forçar a alteração e pode sobrescrever as modificações do usuário <b>" + usuario + "</b>.");
                                }
                            }

                            foreach (var videAlterador in normaAlteradoraOv.vides)
                            {
                                if (videAlterador.ch_vide == _ch_vide)
                                {
                                    videAlteradorDesfazer = util.BRLight.objHelp.Clone<Vide>(videAlterador);
                                    if (normaAlteradaOv == null)
                                    {
                                        videAlterador.ch_tipo_norma_vide = _ch_tipo_norma_vide_fora_do_sistema;
                                        videAlterador.nm_tipo_norma_vide = _nm_tipo_norma_vide_fora_do_sistema;
                                        videAlterador.ch_tipo_fonte_norma_vide = _ch_tipo_fonte_vide_fora_do_sistema;
                                        videAlterador.nm_tipo_fonte_norma_vide = _nm_tipo_fonte_vide_fora_do_sistema;
                                        videAlterador.nr_norma_vide = _nr_norma_vide_fora_do_sistema;
                                        videAlterador.dt_publicacao_fonte_norma_vide = _dt_publicacao_norma_vide_fora_do_sistema;
                                        videAlterador.pagina_publicacao_norma_vide = _nr_pagina_publicacao_norma_vide_fora_do_sistema;
                                        videAlterador.coluna_publicacao_norma_vide = _nr_coluna_publicacao_norma_vide_fora_do_sistema;
                                    }

                                    videAlterador.ch_tipo_relacao = ch_tipo_relacao_pos_verificacao;
                                    videAlterador.nm_tipo_relacao = nm_tipo_relacao_pos_verificacao;
                                    videAlterador.ds_texto_relacao = ds_texto_para_alterado_pos_verificacao; //Exemplo: Revoga Totalmente

                                    ///o campo ds_texto_para_alterador_aux foi inserido no sistema após ele já estar em produção,
                                    ///para evitar problemas na edição destes vides,
                                    ///faço o preenchimento do campo caso o mesmo esteja vazio
                                    if (videAlterador.caput_norma_vide != null && !string.IsNullOrEmpty(videAlterador.caput_norma_vide.nm_relacao_aux) && string.IsNullOrEmpty(videAlterador.caput_norma_vide.ds_texto_para_alterador_aux))
                                    {
                                        videAlterador.caput_norma_vide.ds_texto_para_alterador_aux = relacao.ds_texto_para_alterador;
                                    }
                                    if (videAlterador.caput_norma_vide_outra != null && !string.IsNullOrEmpty(videAlterador.caput_norma_vide_outra.nm_relacao_aux) && string.IsNullOrEmpty(videAlterador.caput_norma_vide_outra.ds_texto_para_alterador_aux))
                                    {
                                        videAlterador.caput_norma_vide_outra.ds_texto_para_alterador_aux = relacao.ds_texto_para_alterador;
                                    }


                                    ///os procedimentos para adicionar, alterar e remover um dispositivo são bem distintos, na questão dos arquivos,
                                    ///então adotei essa verificação durante as atribuições dos valores para o tratamento com os arquivos serem realizados posteriormente,
                                    ///ou seja, após salvar as normas os arquivos terão seus textos alterados conforme as flags adicionar_caput, alterar_caput e remover_caput.
                                    if (!string.IsNullOrEmpty(_caput_norma_vide_alteradora))
                                    {
                                        videAlterador.caput_norma_vide = JSON.Deserializa<Caput>(_caput_norma_vide_alteradora);
                                        videAlterador.caput_norma_vide.nm_relacao_aux = nm_tipo_relacao_pos_verificacao.ToLower();
                                        videAlterador.caput_norma_vide.ds_texto_para_alterador_aux = ds_texto_para_alterador_pos_verificacao.ToLower();

                                        //caput_norma_vide_alteradora = JSON.Deserializa<Caput>(_caput_norma_vide_alteradora);
                                        /////se na norma não existia o dispositivo então setamos a flag adicionar_caput
                                        //if (vide_alterador.caput_norma_vide == null || vide_alterador.caput_norma_vide.caput == null)
                                        //{
                                        //    adicionar_caput = true;
                                        //}
                                        /////ou se na norma existia o dispositivo mas está diferente então setamos a flag alterar_caput
                                        //else if (caput_norma_vide_alteradora.caput[0] != vide_alterador.caput_norma_vide.caput[0] || caput_norma_vide_alteradora.link != vide_alterador.caput_norma_vide.link)
                                        //{
                                        //    caput_norma_vide_alteradora = vide_alterador.caput_norma_vide;
                                        //    alterar_caput = true;
                                        //}
                                        //vide_alterador.caput_norma_vide = JSON.Deserializa<Caput>(_caput_norma_vide_alteradora);
                                        //vide_alterador.caput_norma_vide.nm_relacao_aux = nm_tipo_relacao_pos_verificacao.ToLower();
                                        //vide_alterador.caput_norma_vide.ds_texto_para_alterador_aux = ds_texto_para_alterador_pos_verificacao.ToLower();
                                    }
                                    else
                                    {
                                        videAlterador.caput_norma_vide = new Caput();
                                        /////se na norma existia o dispositivo então setamos a flag remover_caput pois _caput_norma_vide_alteradora é null
                                        //if (vide_alterador.caput_norma_vide != null && vide_alterador.caput_norma_vide.caput != null && vide_alterador.caput_norma_vide.caput.Length > 0)
                                        //{
                                        //    caput_norma_vide_alteradora = vide_alterador.caput_norma_vide;
                                        //    remover_caput = true;
                                        //}
                                        //vide_alterador.caput_norma_vide = new Caput();
                                    }

                                    videAlterador.alinea_norma_vide_outra = _alinea_norma_vide_alterada;
                                    videAlterador.anexo_norma_vide_outra = _anexo_norma_vide_alterada;
                                    videAlterador.artigo_norma_vide_outra = _artigo_norma_vide_alterada;
                                    videAlterador.paragrafo_norma_vide_outra = _paragrafo_norma_vide_alterada;
                                    videAlterador.inciso_norma_vide_outra = _inciso_norma_vide_alterada;
                                    videAlterador.item_norma_vide_outra = _item_norma_vide_alterada;


                                    if (!string.IsNullOrEmpty(_caput_norma_vide_alterada))
                                    {
                                        videAlterador.caput_norma_vide_outra = JSON.Deserializa<Caput>(_caput_norma_vide_alterada);
                                        videAlterador.caput_norma_vide_outra.ds_caput = _ds_caput_norma_alterada;
                                        videAlterador.caput_norma_vide_outra.texto_novo = _caput_texto_novo;
                                        videAlterador.caput_norma_vide_outra.nm_relacao_aux = nm_tipo_relacao_pos_verificacao.ToLower();
                                        videAlterador.caput_norma_vide_outra.ds_texto_para_alterador_aux = ds_texto_para_alterador_pos_verificacao.ToLower();


                                        //caput_norma_vide_alterada = JSON.Deserializa<Caput>(_caput_norma_vide_alterada);
                                        //caput_norma_vide_alterada.texto_novo = _caput_texto_novo;
                                        /////Se o vide atual possui dispositivos então faz-se a comparação com os dispositivos do vide que está sendo salvo para
                                        /////definir se é ou não uma alteração
                                        //if (vide_alterador.caput_norma_vide_outra != null && vide_alterador.caput_norma_vide_outra.caput != null)
                                        //{
                                        //    ///Testa se o vide atual não possui a mesma quantidade de dispositivos que o vide que está sendo salvo
                                        //    ///caso não possua já seta a flag alterar_caput, caso contrário 
                                        //    if (caput_norma_vide_alterada.caput.Length != vide_alterador.caput_norma_vide_outra.caput.Length)
                                        //    {
                                        //        caput_norma_vide_alterada = vide_alterador.caput_norma_vide_outra;
                                        //        alterar_caput = true;
                                        //    }
                                        //    else
                                        //    {
                                        //        for (var i = 0; i < caput_norma_vide_alterada.caput.Length; i++)
                                        //        {
                                        //            ///verifica cada dispositivo comparando-os com os dispositivos que estão sendo salvos. Havendo alguma diferença seta a flag alterar_caput
                                        //            if (caput_norma_vide_alterada.caput[i] != vide_alterador.caput_norma_vide_outra.caput[i] || caput_norma_vide_alterada.texto_antigo[i] != vide_alterador.caput_norma_vide_outra.texto_antigo[i] || caput_norma_vide_alterada.texto_novo[i] != vide_alterador.caput_norma_vide_outra.texto_novo[i])
                                        //            {
                                        //                caput_norma_vide_alterada = vide_alterador.caput_norma_vide_outra;
                                        //                alterar_caput = true;
                                        //                break;
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                        /////Se o vide atual não possui dispositivos então deve-se setar a flag adicionar_caput
                                        //else
                                        //{
                                        //    adicionar_caput = true;
                                        //}
                                        /////atribui a vide_alterador.caput_norma_vide_outra desserializando _caput_norma_vide_alterada para preservar a instancia do objeto caput_norma_vide_alterada
                                        /////de forma a poder ser feita a comparação entre os dois objetos e desfazer as alterações anteriores dos dispositivos nos arquivos referenciados nesse vide
                                        //vide_alterador.caput_norma_vide_outra = JSON.Deserializa<Caput>(_caput_norma_vide_alterada);
                                        //vide_alterador.caput_norma_vide_outra.texto_novo = _caput_texto_novo;
                                        //vide_alterador.caput_norma_vide_outra.nm_relacao_aux = nm_tipo_relacao_pos_verificacao.ToLower();
                                        //vide_alterador.caput_norma_vide_outra.ds_texto_para_alterador_aux = ds_texto_para_alterador_pos_verificacao.ToLower();
                                    }
                                    else
                                    {
                                        videAlterador.caput_norma_vide_outra = new Caput();
                                        /////Se o vide atual possui dispositivos e o que está sendo salvo não possui, então deve-se desfazer as alterações nos arquivos das normas
                                        //if (vide_alterador.caput_norma_vide_outra != null && vide_alterador.caput_norma_vide_outra.caput != null && vide_alterador.caput_norma_vide_outra.caput.Length > 0)
                                        //{
                                        //    caput_norma_vide_alterada = vide_alterador.caput_norma_vide_outra;
                                        //    remover_caput = true;
                                        //}
                                        //vide_alterador.caput_norma_vide_outra = new Caput();
                                    }


                                    videAlterador.ds_comentario_vide = _ds_comentario_vide;
                                    normaAlteradoraOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                                    if (normaRn.Atualizar(normaAlteradoraOv._metadata.id_doc, normaAlteradoraOv))
                                    {
                                        if (normaAlteradaOv != null)
                                        {
                                            foreach (var videAlterado in normaAlteradaOv.vides)
                                            {
                                                if (videAlterado.ch_vide == _ch_vide)
                                                {
                                                    videAlteradoDesfazer = util.BRLight.objHelp.Clone<Vide>(videAlterado);
                                                    videAlterado.ch_tipo_relacao = ch_tipo_relacao_pos_verificacao;
                                                    videAlterado.nm_tipo_relacao = nm_tipo_relacao_pos_verificacao;
                                                    videAlterado.ds_texto_relacao = ds_texto_para_alterador_pos_verificacao; //Exemplo: Revogado Totalmente

                                                    videAlterado.alinea_norma_vide = _alinea_norma_vide_alterada;
                                                    videAlterado.anexo_norma_vide = _anexo_norma_vide_alterada;
                                                    videAlterado.artigo_norma_vide = _artigo_norma_vide_alterada;
                                                    videAlterado.paragrafo_norma_vide = _paragrafo_norma_vide_alterada;
                                                    videAlterado.inciso_norma_vide = _inciso_norma_vide_alterada;
                                                    videAlterado.item_norma_vide = _item_norma_vide_alterada;
                                                    videAlterado.caput_norma_vide = videAlterador.caput_norma_vide_outra;

                                                    videAlterado.caput_norma_vide_outra = videAlterador.caput_norma_vide;

                                                    videAlterado.ds_comentario_vide = _ds_comentario_vide;

                                                    var nm_situacao_anterior = normaAlteradaOv.nm_situacao;
                                                    if (!normaAlteradaOv.st_situacao_forcada)
                                                    {
                                                        var situacao = normaRn.ObterSituacao(normaAlteradaOv.vides);
                                                        normaAlteradaOv.ch_situacao = situacao.ch_situacao;
                                                        normaAlteradaOv.nm_situacao = situacao.nm_situacao;
                                                    }
                                                    normaAlteradaOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = normaAlteradoraOv.dt_cadastro, nm_login_usuario_alteracao = normaAlteradoraOv.nm_login_usuario_cadastro });
                                                    if (normaRn.Atualizar(normaAlteradaOv._metadata.id_doc, normaAlteradaOv))
                                                    {
                                                        sRetorno = "{\"id_doc_success\":" + id_doc + ", \"ch_norma\":\"" + normaAlteradoraOv.ch_norma + "\"}";
                                                        VerificarDispositivosDesfazerEAlterarOsTextosDasNormas(normaAlteradoraOv, normaAlteradaOv, videAlterador, videAlterado, videAlteradorDesfazer, videAlteradoDesfazer, nm_situacao_anterior);
                                                        //if(adicionar_caput){
                                                        //    new VideIncluir().VerificarDispositivosEAlterarOsTextosDasNormas(normaAlteradoraOv, normaAlteradaOv, vide_alterador, vide_alterada);

                                                            //if (vide_alterador.caput_norma_vide != null && vide_alterada.caput_norma_vide != null && vide_alterador.caput_norma_vide.caput.Length > 0 && vide_alterada.caput_norma_vide.caput.Length > 0)
                                                            //{
                                                            //    new VideIncluir().IncluirAlteracaoComDispositivosNosArquivosDasNormas(normaAlteradoraOv.ch_norma, normaAlteradaOv.ch_norma, vide_alterador.caput_norma_vide, vide_alterada.caput_norma_vide, _caput_texto_novo);
                                                            //}
                                                            //else if(vide_alterador.caput_norma_vide != null && vide_alterador.caput_norma_vide.caput.Length > 0)
                                                            //{
                                                            //    new VideIncluir().IncluirAlteracaoComDispositivosNosArquivosDasNormas(normaAlteradoraOv, normaAlteradaOv, vide_alterador.caput_norma_vide, ds_texto_para_alterador_pos_verificacao);
                                                            //}
                                                        //}
                                                        //else if (alterar_caput)
                                                        //{

                                                            //if (vide_alterador.caput_norma_vide != null && vide_alterada.caput_norma_vide != null && vide_alterador.caput_norma_vide.caput.Length > 0 && vide_alterada.caput_norma_vide.caput.Length > 0)
                                                            //{
                                                            //    AlterarCaputDosArquivos(normaAlteradoraOv, normaAlteradaOv, vide_alterador, vide_alterada, caput_norma_vide_alteradora, caput_norma_vide_alterada);
                                                            //}
                                                            //else if (vide_alterador.caput_norma_vide != null && vide_alterador.caput_norma_vide.caput.Length > 0)
                                                            //{
                                                            //    AlterarCaputDosArquivos(normaAlteradoraOv, normaAlteradaOv, vide_alterador, caput_norma_vide_alteradora);
                                                            //}
                                                        //}
                                                        //else if (remover_caput)
                                                        //{
                                                        //    if (caput_norma_vide_alteradora != null && caput_norma_vide_alterada != null && caput_norma_vide_alteradora.caput != null && caput_norma_vide_alteradora.caput.Length > 0 && caput_norma_vide_alterada.caput != null && caput_norma_vide_alterada.caput.Length > 0)
                                                        //    {
                                                        //        DesfazerCaputDosArquivos(normaAlteradoraOv, normaAlteradaOv, caput_norma_vide_alteradora, caput_norma_vide_alterada);
                                                        //    }
                                                        //    else if (caput_norma_vide_alteradora != null && caput_norma_vide_alteradora.caput != null && caput_norma_vide_alteradora.caput.Length > 0)
                                                        //    {
                                                        //        DesfazerCaputDosArquivos(normaAlteradoraOv, normaAlteradaOv, caput_norma_vide_alteradora, vide_alterador.ds_texto_relacao);
                                                        //    }
                                                        //}
                                                    }
                                                    else
                                                    {
                                                        throw new Exception("Erro ao atualizar Vide na norma alterada.");
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            sRetorno = "{\"id_doc_success\":" + id_doc + ", \"ch_norma\":\"" + normaAlteradoraOv.ch_norma + "\"}";
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Erro ao atualizar Vide.");
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new DocValidacaoException("Tipo de Relação não informado. Verifique se o Tipo de Relação está selecionado.");
                    }
                }
                else
                {
                    throw new DocValidacaoException("Erro na norma informada.");
                }
                var log_editar = new LogAlterar<NormaOV>
                {
                    registro = normaOv
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ",VIDE.EDT", log_editar, id_doc, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (FileNotFoundException ex)
            {
                sRetorno = "{\"id_doc_success\":" + id_doc + ", \"ch_norma\":\"" + normaAlteradoraOv.ch_norma + "\", \"alert_message\": \"" + ex.Message + "\"}";
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocDuplicateKeyException || ex is SessionExpiredException || ex is DocValidacaoException || ex is RiskOfInconsistency)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\", \"type_error\":\"" + ex.GetType().Name + "\", \"dt_controle_alteracao\":\"" + DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss") + "\"}";
                }
                else
                {
                    sRetorno = Excecao.LerTodasMensagensDaExcecao(ex, false);
                    context.Response.StatusCode = 500;
                }
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                if (sessao_usuario != null)
                {
                    LogErro.gravar_erro(Util.GetEnumDescription(action) + ",VIDE.EDT", erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
            }
            context.Response.Write(sRetorno);
            context.Response.End();
        }

        /// <summary>
        /// Remove as alteraçãos nos arquivos, verifica e executa as novas alterações.
        /// Estas alterações dependem dos dispositivos referenciados nos vides e da situação que o vide causou à norma.
        /// </summary>
        /// <param name="normaAlteradora"></param>
        /// <param name="normaAlterada"></param>
        /// <param name="videAlterador"></param>
        /// <param name="videAlterado"></param>
        /// <param name="videAlteradorDesfazer"></param>
        /// <param name="videAlteradoDesfazer"></param>
        private void VerificarDispositivosDesfazerEAlterarOsTextosDasNormas(NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlterador, Vide videAlterado, Vide videAlteradorDesfazer, Vide videAlteradoDesfazer, string nmSituacaoAnterior)
        {
            var dictionaryFiles = new VideExcluir().VerificarDispositivosEDesfazerAltercaoNosTextosDasNormas(normaAlteradora, normaAlterada, videAlteradorDesfazer, videAlteradoDesfazer, nmSituacaoAnterior);


            //Se possuir id_file_alterador a alteração implicou em um novo if_file para os arquivos, mesmo se não alterou os caputs podem estar utilizando id_file que não corresponde mais com o id_file da norma
            if (dictionaryFiles.ContainsKey("id_file_alterador"))
            {
                if (videAlterador.caput_norma_vide != null && !string.IsNullOrEmpty(videAlterador.caput_norma_vide.id_file))
                {
                    videAlterador.caput_norma_vide.id_file = dictionaryFiles["id_file_alterador"];
                }
                normaAlteradora.ar_atualizado.id_file = dictionaryFiles["id_file_alterador"];
            }
            else if(!string.IsNullOrEmpty(normaAlteradora.ar_atualizado.id_file))
            {
                if (videAlterador.caput_norma_vide != null && !string.IsNullOrEmpty(videAlterador.caput_norma_vide.id_file))
                {
                    videAlterador.caput_norma_vide.id_file = normaAlteradora.ar_atualizado.id_file;
                }
            }

            if (dictionaryFiles.ContainsKey("id_file_alterado"))
            {
                if (videAlterado.caput_norma_vide != null && !string.IsNullOrEmpty(videAlterado.caput_norma_vide.id_file))
                {
                    videAlterado.caput_norma_vide.id_file = dictionaryFiles["id_file_alterado"];
                }
                normaAlterada.ar_atualizado.id_file = dictionaryFiles["id_file_alterado"];
            }
            else if (!string.IsNullOrEmpty(normaAlterada.ar_atualizado.id_file))
            {
                if (videAlterado.caput_norma_vide != null && !string.IsNullOrEmpty(videAlterado.caput_norma_vide.id_file))
                {
                    videAlterado.caput_norma_vide.id_file = normaAlterada.ar_atualizado.id_file;
                }
            }
            
            new VideIncluir().VerificarDispositivosEAlterarOsTextosDasNormas(normaAlteradora, normaAlterada, videAlterador, videAlterado);

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
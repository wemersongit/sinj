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
            var _id_doc = context.Request["id_doc"];
            ulong id_doc = 0;
            var _ch_vide = context.Request["ch_vide"];

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
                var normaRn = new NormaRN();
                if (!string.IsNullOrEmpty(_id_doc) && !string.IsNullOrEmpty(_ch_vide))
                {
                    if (!string.IsNullOrEmpty(_ch_tipo_relacao))
                    {
                        id_doc = ulong.Parse(_id_doc);
                        normaOv = normaRn.Doc(id_doc);

                        TipoDeRelacaoOV relacao = null;
                        if (!string.IsNullOrEmpty(_artigo_norma_vide_alterada))
                        {
                            relacao = normaRn.ObterRelacaoParcial(_ch_tipo_relacao);
                        }
                        else
                        {
                            relacao = normaRn.ObterRelacao(_ch_tipo_relacao);
                        }
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
                            Caput caput_norma_vide_alteradora = null;
                            Caput caput_norma_vide_alterada = null;
                            var adicionar_caput = false;
                            var alterar_caput = false;
                            var remover_caput = false;
                            foreach (var vide_alterador in normaAlteradoraOv.vides)
                            {
                                if (vide_alterador.ch_vide == _ch_vide)
                                {
                                    if (normaAlteradaOv == null)
                                    {
                                        vide_alterador.ch_tipo_norma_vide = _ch_tipo_norma_vide_fora_do_sistema;
                                        vide_alterador.nm_tipo_norma_vide = _nm_tipo_norma_vide_fora_do_sistema;
                                        vide_alterador.ch_tipo_fonte_norma_vide = _ch_tipo_fonte_vide_fora_do_sistema;
                                        vide_alterador.nm_tipo_fonte_norma_vide = _nm_tipo_fonte_vide_fora_do_sistema;
                                        vide_alterador.nr_norma_vide = _nr_norma_vide_fora_do_sistema;
                                        vide_alterador.dt_publicacao_fonte_norma_vide = _dt_publicacao_norma_vide_fora_do_sistema;
                                        vide_alterador.pagina_publicacao_norma_vide = _nr_pagina_publicacao_norma_vide_fora_do_sistema;
                                        vide_alterador.coluna_publicacao_norma_vide = _nr_coluna_publicacao_norma_vide_fora_do_sistema;
                                    }

                                    vide_alterador.ch_tipo_relacao = ch_tipo_relacao_pos_verificacao;
                                    vide_alterador.nm_tipo_relacao = nm_tipo_relacao_pos_verificacao;
                                    vide_alterador.ds_texto_relacao = ds_texto_para_alterado_pos_verificacao; //Exemplo: Revoga Totalmente

                                    if (!string.IsNullOrEmpty(_caput_norma_vide_alteradora))
                                    {
                                        caput_norma_vide_alteradora = JSON.Deserializa<Caput>(_caput_norma_vide_alteradora);
                                        if(vide_alterador.caput_norma_vide.caput == null){
                                            adicionar_caput = true;
                                        }
                                        else if (caput_norma_vide_alteradora.caput[0] != vide_alterador.caput_norma_vide.caput[0] || caput_norma_vide_alteradora.link != vide_alterador.caput_norma_vide.link)
                                        {
                                            caput_norma_vide_alteradora = vide_alterador.caput_norma_vide;
                                            alterar_caput = true;
                                        }
                                        vide_alterador.caput_norma_vide = JSON.Deserializa<Caput>(_caput_norma_vide_alteradora);
                                        vide_alterador.caput_norma_vide.nm_relacao_aux = nm_tipo_relacao_pos_verificacao.ToLower();
                                    }
                                    else
                                    {
                                        if (vide_alterador.caput_norma_vide != null && vide_alterador.caput_norma_vide.caput != null && vide_alterador.caput_norma_vide.caput.Length > 0)
                                        {
                                            caput_norma_vide_alteradora = vide_alterador.caput_norma_vide;
                                            remover_caput = true;
                                        }
                                        vide_alterador.caput_norma_vide = new Caput();
                                    }

                                    vide_alterador.alinea_norma_vide_outra = _alinea_norma_vide_alterada;
                                    vide_alterador.anexo_norma_vide_outra = _anexo_norma_vide_alterada;
                                    vide_alterador.artigo_norma_vide_outra = _artigo_norma_vide_alterada;
                                    vide_alterador.paragrafo_norma_vide_outra = _paragrafo_norma_vide_alterada;
                                    vide_alterador.inciso_norma_vide_outra = _inciso_norma_vide_alterada;
                                    vide_alterador.item_norma_vide_outra = _item_norma_vide_alterada;

                                    if (!string.IsNullOrEmpty(_caput_norma_vide_alterada))
                                    {
                                        caput_norma_vide_alterada = JSON.Deserializa<Caput>(_caput_norma_vide_alterada);
                                        caput_norma_vide_alterada.texto_novo = _caput_texto_novo;
                                        if (vide_alterador.caput_norma_vide_outra != null && vide_alterador.caput_norma_vide_outra.caput != null)
                                        {
                                            if (caput_norma_vide_alterada.caput.Length != vide_alterador.caput_norma_vide_outra.caput.Length)
                                            {
                                                caput_norma_vide_alterada = vide_alterador.caput_norma_vide_outra;
                                                alterar_caput = true;
                                            }
                                            else
                                            {
                                                for (var i = 0; i < caput_norma_vide_alterada.caput.Length; i++)
                                                {
                                                    if (caput_norma_vide_alterada.caput[i] != vide_alterador.caput_norma_vide_outra.caput[i] || caput_norma_vide_alterada.texto_antigo[i] != vide_alterador.caput_norma_vide_outra.texto_antigo[i] || caput_norma_vide_alterada.texto_novo[i] != vide_alterador.caput_norma_vide_outra.texto_novo[i])
                                                    {
                                                        caput_norma_vide_alterada = vide_alterador.caput_norma_vide_outra;
                                                        alterar_caput = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            adicionar_caput = true;
                                        }
                                        vide_alterador.caput_norma_vide_outra = JSON.Deserializa<Caput>(_caput_norma_vide_alterada);
                                        vide_alterador.caput_norma_vide_outra.texto_novo = _caput_texto_novo;
                                        vide_alterador.caput_norma_vide_outra.nm_relacao_aux = nm_tipo_relacao_pos_verificacao.ToLower();
                                    }
                                    else
                                    {
                                        if (vide_alterador.caput_norma_vide_outra != null && vide_alterador.caput_norma_vide_outra.caput != null && vide_alterador.caput_norma_vide_outra.caput.Length > 0)
                                        {
                                            caput_norma_vide_alterada = vide_alterador.caput_norma_vide_outra;
                                            remover_caput = true;
                                        }
                                        vide_alterador.caput_norma_vide_outra = new Caput();
                                    }


                                    vide_alterador.ds_comentario_vide = _ds_comentario_vide;
                                    normaAlteradoraOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                                    if (normaRn.Atualizar(normaAlteradoraOv._metadata.id_doc, normaAlteradoraOv))
                                    {
                                        if (normaAlteradaOv != null)
                                        {
                                            foreach (var vide_alterada in normaAlteradaOv.vides)
                                            {
                                                if (vide_alterada.ch_vide == _ch_vide)
                                                {
                                                    
                                                    vide_alterada.ch_tipo_relacao = ch_tipo_relacao_pos_verificacao;
                                                    vide_alterada.nm_tipo_relacao = nm_tipo_relacao_pos_verificacao;
                                                    vide_alterada.ds_texto_relacao = ds_texto_para_alterador_pos_verificacao; //Exemplo: Revogado Totalmente

                                                    vide_alterada.alinea_norma_vide = _alinea_norma_vide_alterada;
                                                    vide_alterada.anexo_norma_vide = _anexo_norma_vide_alterada;
                                                    vide_alterada.artigo_norma_vide = _artigo_norma_vide_alterada;
                                                    vide_alterada.paragrafo_norma_vide = _paragrafo_norma_vide_alterada;
                                                    vide_alterada.inciso_norma_vide = _inciso_norma_vide_alterada;
                                                    vide_alterada.item_norma_vide = _item_norma_vide_alterada;
                                                    vide_alterada.caput_norma_vide = vide_alterador.caput_norma_vide_outra;

                                                    vide_alterada.caput_norma_vide_outra = vide_alterador.caput_norma_vide;

                                                    vide_alterada.ds_comentario_vide = _ds_comentario_vide;
                                                    var situacao = normaRn.ObterSituacao(normaAlteradaOv.vides);
                                                    normaAlteradaOv.ch_situacao = situacao.ch_situacao;
                                                    normaAlteradaOv.nm_situacao = situacao.nm_situacao;
                                                    normaAlteradaOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = normaAlteradoraOv.dt_cadastro, nm_login_usuario_alteracao = normaAlteradoraOv.nm_login_usuario_cadastro });
                                                    if (normaRn.Atualizar(normaAlteradaOv._metadata.id_doc, normaAlteradaOv))
                                                    {
                                                        sRetorno = "{\"id_doc_success\":" + id_doc + ", \"ch_norma\":\"" + normaAlteradoraOv.ch_norma + "\"}";
                                                        if(adicionar_caput){
                                                            if (vide_alterador.caput_norma_vide != null && vide_alterada.caput_norma_vide != null && vide_alterador.caput_norma_vide.caput.Length > 0 && vide_alterada.caput_norma_vide.caput.Length > 0)
                                                            {
                                                                new VideIncluir().IncluirCaputNosArquivos(normaAlteradoraOv.ch_norma, normaAlteradaOv.ch_norma, vide_alterador.caput_norma_vide, vide_alterada.caput_norma_vide, _caput_texto_novo, ds_texto_para_alterador_pos_verificacao);
                                                            }
                                                            else if(vide_alterador.caput_norma_vide != null && vide_alterador.caput_norma_vide.caput.Length > 0)
                                                            {
                                                                new VideIncluir().IncluirCaputNosArquivos(normaAlteradoraOv, normaAlteradaOv, vide_alterador.caput_norma_vide, ds_texto_para_alterador_pos_verificacao);
                                                            }
                                                        }
                                                        if (alterar_caput)
                                                        {
                                                            if (vide_alterador.caput_norma_vide != null && vide_alterada.caput_norma_vide != null && vide_alterador.caput_norma_vide.caput.Length > 0 && vide_alterada.caput_norma_vide.caput.Length > 0)
                                                            {
                                                                AlterarCaputDosArquivos(normaAlteradoraOv, normaAlteradaOv, vide_alterador, vide_alterada, caput_norma_vide_alteradora, caput_norma_vide_alterada);
                                                            }
                                                            else if (vide_alterador.caput_norma_vide != null && vide_alterador.caput_norma_vide.caput.Length > 0)
                                                            {
                                                                AlterarCaputDosArquivos(normaAlteradoraOv, normaAlteradaOv, vide_alterador, caput_norma_vide_alteradora);
                                                            }
                                                        }
                                                        if (remover_caput)
                                                        {
                                                            if (caput_norma_vide_alteradora != null && caput_norma_vide_alterada != null && caput_norma_vide_alteradora.caput != null && caput_norma_vide_alteradora.caput.Length > 0 && caput_norma_vide_alterada.caput != null && caput_norma_vide_alterada.caput.Length > 0)
                                                            {
                                                                DesfazerCaputDosArquivos(normaAlteradoraOv, normaAlteradaOv, caput_norma_vide_alteradora, caput_norma_vide_alterada);
                                                            }
                                                            else if (caput_norma_vide_alteradora != null && caput_norma_vide_alteradora.caput != null && caput_norma_vide_alteradora.caput.Length > 0)
                                                            {
                                                                DesfazerCaputDosArquivos(normaAlteradoraOv, normaAlteradaOv, caput_norma_vide_alteradora, vide_alterador.ds_texto_relacao);
                                                            }
                                                        }
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
            catch (Exception ex)
            {

                if (ex is PermissionException || ex is DocDuplicateKeyException || ex is SessionExpiredException || ex is DocValidacaoException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\"}";
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
        /// Altera os arquivos das normas, alteradora e alterada
        /// </summary>
        /// <param name="vide_alterador"></param>
        /// <param name="normaAlteradora"></param>
        /// <param name="normaAlterada"></param>
        /// <param name="caput_norma_vide_alteradora_desfazer">caput antigo que será desfeito caso seja diferente do atual</param>
        /// <param name="caput_norma_vide_alterada_desfazer"></param>
        /// <param name="_arquivo_norma_vide_alteradora"></param>
        /// <param name="_arquivo_norma_vide_alterada"></param>
        public void AlterarCaputDosArquivos(NormaOV normaAlteradora, NormaOV normaAlterada, Vide vide_alterador, Vide vide_alterado, Caput caput_norma_vide_alteradora_desfazer, Caput caput_norma_vide_alterada_desfazer)
        {
            var htmlFile = new HtmlFileEncoded();
            var pesquisa = new Pesquisa();
            var normaRn = new NormaRN();
            var upload = new UploadHtml();
            var listOp = new List<opMode<object>>();

            ///Verificando antes se é necessário atualizar o caput alterador. Às vezes a alteração foi somente no texto do caput da norma alterada
            if (vide_alterador.caput_norma_vide.caput[0] != caput_norma_vide_alteradora_desfazer.caput[0] ||
                vide_alterador.caput_norma_vide.link != caput_norma_vide_alteradora_desfazer.link ||
                vide_alterado.caput_norma_vide.caput[0] != caput_norma_vide_alterada_desfazer.caput[0])
            {
                var arquivo_norma_vide_alteradora = RemoverLinkNoTextoDaNormaAlteradora(normaAlteradora, caput_norma_vide_alteradora_desfazer, caput_norma_vide_alterada_desfazer);
                if (arquivo_norma_vide_alteradora != "")
                {
                    arquivo_norma_vide_alteradora = new VideIncluir().CriarLinkNoTextoDaNormaAlteradora(arquivo_norma_vide_alteradora, vide_alterador.caput_norma_vide, vide_alterador.caput_norma_vide_outra);

                    var retorno_file_alteradora = upload.AnexarHtml(arquivo_norma_vide_alteradora, normaAlteradora.ar_atualizado.filename, "sinj_norma");
                    if (retorno_file_alteradora.IndexOf("id_file") > -1)
                    {
                        listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retorno_file_alteradora } });
                        pesquisa.literal = "ch_norma='" + normaAlteradora.ch_norma + "'";
                        normaRn.PathPut<object>(pesquisa, listOp);
                    }
                }
            }
            if (vide_alterador.caput_norma_vide.caput[0] != caput_norma_vide_alteradora_desfazer.caput[0] ||
                vide_alterado.caput_norma_vide.caput[0] != caput_norma_vide_alterada_desfazer.caput[0])
            {
                listOp = new List<opMode<object>>();

                var arquivo_norma_vide_alterada = RemoverAlteracaoNoTextoDaNormaAlterada(normaAlterada, caput_norma_vide_alterada_desfazer, caput_norma_vide_alteradora_desfazer);
                if (arquivo_norma_vide_alterada != "")
                {
                    arquivo_norma_vide_alterada = new VideIncluir().AlterarTextoDaNormaAlterada(arquivo_norma_vide_alterada, vide_alterado.caput_norma_vide, vide_alterador.caput_norma_vide, vide_alterado.ds_texto_relacao.ToLower());
                    var retorno_file_alterada = upload.AnexarHtml(arquivo_norma_vide_alterada, normaAlterada.ar_atualizado.filename, "sinj_norma");
                    if (retorno_file_alterada.IndexOf("id_file") > -1)
                    {
                        listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retorno_file_alterada } });
                        pesquisa.literal = "ch_norma='" + normaAlterada.ch_norma + "'";
                        normaRn.PathPut<object>(pesquisa, listOp);
                    }
                }
            }
        }

        /// <summary>
        /// Altera os arquivos das normas, alteradora e alterada
        /// </summary>
        /// <param name="vide_alterador"></param>
        /// <param name="norma_alteradora"></param>
        /// <param name="norma_alterada"></param>
        /// <param name="caput_norma_vide_alteradora_desfazer">caput antigo que será desfeito caso seja diferente do atual</param>
        /// <param name="caput_norma_vide_alterada_desfazer"></param>
        /// <param name="_arquivo_norma_vide_alteradora"></param>
        /// <param name="_arquivo_norma_vide_alterada"></param>
        public void AlterarCaputDosArquivos(NormaOV norma_alteradora, NormaOV norma_alterada, Vide vide_alterador, Caput caput_norma_vide_alteradora_desfazer)
        {
            var htmlFile = new HtmlFileEncoded();
            var pesquisa = new Pesquisa();
            var normaRn = new NormaRN();
            var upload = new UploadHtml();
            var listOp = new List<opMode<object>>();

            var name_file_norma_revogada = norma_alterada.getNameFileArquivoVigente();

            ///Verificando antes se é necessário atualizar o caput alterador. Às vezes a alteração foi somente no texto do caput da norma alterada
            if (vide_alterador.caput_norma_vide.caput[0] != caput_norma_vide_alteradora_desfazer.caput[0] || vide_alterador.caput_norma_vide.link != caput_norma_vide_alteradora_desfazer.link)
            {

                var arquivo_norma_vide_alteradora = RemoverLinkNoTextoDaNormaAlteradora(norma_alteradora, caput_norma_vide_alteradora_desfazer, norma_alterada.ch_norma);

                if (arquivo_norma_vide_alteradora != "")
                {
                    arquivo_norma_vide_alteradora = new VideIncluir().CriarLinkNoTextoDaNormaAlteradora(arquivo_norma_vide_alteradora, vide_alterador.caput_norma_vide, norma_alterada.ch_norma, name_file_norma_revogada);

                    var retorno_file_alteradora = upload.AnexarHtml(arquivo_norma_vide_alteradora, norma_alteradora.ar_atualizado.filename, "sinj_norma");
                    if (retorno_file_alteradora.IndexOf("id_file") > -1)
                    {
                        listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retorno_file_alteradora } });
                        pesquisa.literal = "ch_norma='" + norma_alteradora.ch_norma + "'";
                        normaRn.PathPut<object>(pesquisa, listOp);
                    }
                }

                var aux_ds_texto_para_alterador = vide_alterador.ds_texto_relacao.ToLower();
                var aux_nm_situacao_alterada = norma_alterada.nm_situacao.ToLower();
                var if_file_norma_alterada = norma_alterada.getIdFileArquivoVigente();
                var name_file_norma_alterada = norma_alterada.getNameFileArquivoVigente();
                var arquivo_norma_vide_alterada = "";

                if ((aux_nm_situacao_alterada == "revogado" && aux_ds_texto_para_alterador == "revogado") ||
                (aux_nm_situacao_alterada == "anulado" && aux_ds_texto_para_alterador == "anulado") ||
                (aux_nm_situacao_alterada == "extinta" && aux_ds_texto_para_alterador == "extinta") ||
                (aux_nm_situacao_alterada == "inconstitucional" && aux_ds_texto_para_alterador == "declarado inconstitucional") ||
                (aux_nm_situacao_alterada == "cancelada" && aux_ds_texto_para_alterador == "cancelada") ||
                (aux_nm_situacao_alterada == "suspenso" && aux_ds_texto_para_alterador == "suspenso totalmente"))
                {
                    arquivo_norma_vide_alterada = RemoverAlteracaoNoTextoCompletoDaNormaAlterada(norma_alterada.ch_norma, if_file_norma_alterada, vide_alterador.ds_texto_relacao);
                    arquivo_norma_vide_alterada = new VideIncluir().AlterarTextoCompletoDaNormaAlterada(arquivo_norma_vide_alterada, norma_alteradora, vide_alterador.caput_norma_vide, vide_alterador.ds_texto_relacao);
                }
                else if (aux_ds_texto_para_alterador == "ratificado" ||
                    aux_ds_texto_para_alterador == "reeditado" ||
                    aux_ds_texto_para_alterador == "regulamentado" ||
                    aux_ds_texto_para_alterador == "prorrogado" ||
                    aux_ds_texto_para_alterador == "legislação correlata")
                {
                    arquivo_norma_vide_alterada = RemoverInformacaoNoTextoDaNormaAlterada(norma_alterada.ch_norma, if_file_norma_alterada, aux_ds_texto_para_alterador);
                    arquivo_norma_vide_alterada = new VideIncluir().AcrescentarInformacaoNoTextoDaNormaAlterada(arquivo_norma_vide_alterada, norma_alteradora, vide_alterador.caput_norma_vide, vide_alterador.ds_texto_relacao);
                }
                if (arquivo_norma_vide_alterada != "")
                {
                    var retorno_file_alterada = upload.AnexarHtml(arquivo_norma_vide_alterada, name_file_norma_alterada, "sinj_norma");

                    if (retorno_file_alterada.IndexOf("id_file") > -1)
                    {
                        pesquisa.literal = "ch_norma='" + norma_alterada.ch_norma + "'";
                        listOp = new List<opMode<object>>();
                        listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retorno_file_alterada } });
                        normaRn.PathPut<object>(pesquisa, listOp);
                    }
                }
            }
        }

        public void DesfazerCaputDosArquivos(NormaOV normaAlteradora, NormaOV normaAlterada, Caput caput_norma_vide_alteradora_desfazer, Caput caput_norma_vide_alterada_desfazer)
        {
            var pesquisa = new Pesquisa();
            var normaRn = new NormaRN();
            var upload = new UploadHtml();
            var listOp = new List<opMode<object>>();

            var arquivo_norma_vide_alteradora = RemoverLinkNoTextoDaNormaAlteradora(normaAlteradora, caput_norma_vide_alteradora_desfazer, caput_norma_vide_alterada_desfazer);
            if (arquivo_norma_vide_alteradora != "")
            {
                var retorno_file_alteradora = upload.AnexarHtml(arquivo_norma_vide_alteradora, normaAlteradora.ar_atualizado.filename, "sinj_norma");
                if (retorno_file_alteradora.IndexOf("id_file") > -1)
                {
                    listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retorno_file_alteradora } });
                    pesquisa.literal = "ch_norma='" + normaAlteradora.ch_norma + "'";
                    normaRn.PathPut<object>(pesquisa, listOp);
                }
            }

            var arquivo_norma_vide_alterada = RemoverAlteracaoNoTextoDaNormaAlterada(normaAlterada, caput_norma_vide_alterada_desfazer, caput_norma_vide_alteradora_desfazer);
            if (arquivo_norma_vide_alterada != "")
            {
                var retorno_file_alterada = upload.AnexarHtml(arquivo_norma_vide_alterada, normaAlterada.ar_atualizado.filename, "sinj_norma");
                if (retorno_file_alterada.IndexOf("id_file") > -1)
                {
                    listOp = new List<opMode<object>>();
                    listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retorno_file_alterada } });
                    pesquisa.literal = "ch_norma='" + normaAlterada.ch_norma + "'";
                    normaRn.PathPut<object>(pesquisa, listOp);
                }
            }
        }

        public void DesfazerCaputDosArquivos(NormaOV norma_alteradora, NormaOV norma_alterada, Caput caput_norma_vide_alteradora_desfazer, string _ds_texto_para_alterador)
        {
            var pesquisa = new Pesquisa();
            var normaRn = new NormaRN();
            var upload = new UploadHtml();
            var listOp = new List<opMode<object>>();

            var arquivo_norma_vide_alteradora = RemoverLinkNoTextoDaNormaAlteradora(norma_alteradora, caput_norma_vide_alteradora_desfazer, norma_alterada.ch_norma);

            if (arquivo_norma_vide_alteradora != "")
            {
                var retorno_file_alteradora = upload.AnexarHtml(arquivo_norma_vide_alteradora, norma_alteradora.ar_atualizado.filename, "sinj_norma");
                if (retorno_file_alteradora.IndexOf("id_file") > -1)
                {
                    listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retorno_file_alteradora } });
                    pesquisa.literal = "ch_norma='" + norma_alteradora.ch_norma + "'";
                    normaRn.PathPut<object>(pesquisa, listOp);
                }
            }

            var aux_ds_texto_para_alterador = _ds_texto_para_alterador.ToLower();
            var aux_nm_situacao_alterada = norma_alterada.nm_situacao.ToLower();
            var if_file_norma_alterada = norma_alterada.getIdFileArquivoVigente();
            var name_file_norma_alterada = norma_alterada.getNameFileArquivoVigente();
            var arquivo_norma_vide_alterada = "";

            if ((aux_nm_situacao_alterada == "revogado" && aux_ds_texto_para_alterador == "revogado") ||
                (aux_nm_situacao_alterada == "anulado" && aux_ds_texto_para_alterador == "anulado") ||
                (aux_nm_situacao_alterada == "extinta" && aux_ds_texto_para_alterador == "extinta") ||
                (aux_nm_situacao_alterada == "inconstitucional" && aux_ds_texto_para_alterador == "declarado inconstitucional") ||
                (aux_nm_situacao_alterada == "cancelada" && aux_ds_texto_para_alterador == "cancelada") ||
                (aux_nm_situacao_alterada == "suspenso" && aux_ds_texto_para_alterador == "suspenso totalmente"))
            {
                arquivo_norma_vide_alterada = RemoverAlteracaoNoTextoCompletoDaNormaAlterada(norma_alterada.ch_norma, if_file_norma_alterada, _ds_texto_para_alterador);
            }
            else if (aux_ds_texto_para_alterador == "ratificado" ||
                aux_ds_texto_para_alterador == "reeditado" ||
                aux_ds_texto_para_alterador == "regulamentado" ||
                aux_ds_texto_para_alterador == "prorrogado" ||
                aux_ds_texto_para_alterador == "legislação correlata")
            {
                arquivo_norma_vide_alterada = RemoverInformacaoNoTextoDaNormaAlterada(norma_alterada.ch_norma, if_file_norma_alterada, _ds_texto_para_alterador);
            }

            if (arquivo_norma_vide_alterada != "")
            {
                var retorno_file_alterada = upload.AnexarHtml(arquivo_norma_vide_alterada, name_file_norma_alterada, "sinj_norma");
                if (retorno_file_alterada.IndexOf("id_file") > -1)
                {
                    listOp = new List<opMode<object>>();
                    listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retorno_file_alterada } });
                    pesquisa.literal = "ch_norma='" + norma_alterada.ch_norma + "'";
                    normaRn.PathPut<object>(pesquisa, listOp);
                }
            }
        }

        public string RemoverLinkNoTextoDaNormaAlteradora(NormaOV norma_alteradora, Caput _caput_alteradora, Caput _caput_alterada)
        {
            var htmlFile = new HtmlFileEncoded();
            var arquivo_norma_vide_alteradora = htmlFile.GetHtmlFile(norma_alteradora.ar_atualizado.id_file, "sinj_norma", null);

            var pattern_caput = "";
            switch (_caput_alterada.nm_relacao_aux)
            {
                case "acrescimo":
                    pattern_caput = _caput_alterada.caput[0] + "_add_0";
                    break;
                case "renumeração":
                    pattern_caput = _caput_alterada.caput[0] + "_renum";
                    break;
                default:
                    pattern_caput = _caput_alterada.caput[0];
                    break;
            }
            var pattern = "(<p.+?linkname=\"" + _caput_alteradora.caput[0] + "\".*?>)(.*?)<a href=\"\\(_link_sistema_\\)Norma/" + _caput_alterada.ch_norma + '/' + _caput_alterada.filename + "#" + pattern_caput + "\" >" + UtilVides.EscapeCharsInToPattern(_caput_alteradora.link) + "</a>(.*?)</p>";
            var matches = Regex.Matches(arquivo_norma_vide_alteradora, pattern);
            if (matches.Count == 1)
            {
                var replacement = matches[0].Groups[1].Value + matches[0].Groups[2].Value + _caput_alteradora.link + matches[0].Groups[3].Value + "</p>";
                arquivo_norma_vide_alteradora = Regex.Replace(arquivo_norma_vide_alteradora, pattern, replacement);
            }
            return arquivo_norma_vide_alteradora;
        }

        public string RemoverLinkNoTextoDaNormaAlteradora(NormaOV norma_alteradora, Caput _caput_alteradora, string ch_norma_alterada)
        {
            var htmlFile = new HtmlFileEncoded();
            var arquivo_norma_vide_alteradora = htmlFile.GetHtmlFile(norma_alteradora.ar_atualizado.id_file, "sinj_norma", null);
            var pattern = "(<p.+?linkname=\"" + _caput_alteradora.caput[0] + "\".*?>)(.*?)<a href=\"\\(_link_sistema_\\)Norma/" + ch_norma_alterada + "/.+?\" >" + UtilVides.EscapeCharsInToPattern(_caput_alteradora.link) + "</a>(.*?)</p>";
            var matches = Regex.Matches(arquivo_norma_vide_alteradora, pattern);
            if (matches.Count == 1)
            {
                var replacement = matches[0].Groups[1].Value + matches[0].Groups[2].Value + _caput_alteradora.link + matches[0].Groups[3].Value + "</p>";
                arquivo_norma_vide_alteradora = Regex.Replace(arquivo_norma_vide_alteradora, pattern, replacement);
            }
            return arquivo_norma_vide_alteradora;
        }

        public string RemoverAlteracaoNoTextoDaNormaAlterada(NormaOV norma_alterada, Caput _caput_alterada, Caput _caput_alteradora)
        {
            var texto = "";
            if (_caput_alterada.caput.Length == _caput_alterada.texto_antigo.Length)
            {
                var htmlFile = new HtmlFileEncoded();
                texto = htmlFile.GetHtmlFile(norma_alterada.ar_atualizado.id_file, "sinj_norma", null);
                var pattern = "";
                var replacement = "";
                for (var i = 0; i < _caput_alterada.caput.Length; i++)
                {
                    switch (_caput_alterada.nm_relacao_aux)
                    {
                        case "acrescimo":
                            pattern = "<p.+?linkname=\"" + _caput_alterada.caput[i] + "_add_.+?\".*?>.*?<a class=\"link_vide\".*?>.+?</a></p>";
                            replacement = "";
                            break;
                        case "renumeração":
                            pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "_renum(\".*?>.*?<a.+?name=\")" + _caput_alterada.caput[i] + "_renum(\".*?></a>.*?)" + UtilVides.EscapeCharsInToPattern(_caput_alterada.texto_novo[i]) + "(.*?)<a class=\"link_vide\".*?>.+?</a></p>";
                            replacement = "$1" + _caput_alterada.caput[i] + "$2" + _caput_alterada.caput[i] + "$3" + _caput_alterada.texto_antigo[i] + "</p>";
                            break;
                        case "prorrogação":
                        case "ratificação":
                        case "regulamentação":
                        case "ressalva":
                        case "retificação":
                            pattern = "(<p.+?linkname=\"" + _caput_alterada.caput[i] + "\".*?<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>.*?) <a class=\"link_vide\".*?>.+?</a></p>";
                            replacement = "$1</p>";
                            break;
                        default:
                            pattern = "(<p.+?linkname=\")" + UtilVides.EscapeCharsInToPattern(_caput_alterada.caput[i]) + "_replaced(\".*?)replaced_by=\"" + _caput_alteradora.ch_norma + "\"(.*?)<s>(.*?)<a.+?name=\"" + _caput_alterada.caput[i] + "_replaced\".*?></a>(.*?)</s>(.*?)</p>\r\n<p.+?linkname=\"" + _caput_alterada.caput[i] + "\".*?>.*?" + UtilVides.EscapeCharsInToPattern(_caput_alterada.texto_novo[i]) + ".*?<a class=\"link_vide\".*?>.+?</a></p>";
                            replacement = "$1" + _caput_alterada.caput[i] + "$2$3$4<a name=\"" + _caput_alterada.caput[i] + "\"></a>" + _caput_alterada.texto_antigo[i] + "$6</p>";
                            break;
                    }
                    texto = Regex.Replace(texto, pattern, replacement);
                }
            }
            return texto;
        }

        public string RemoverAlteracaoNoTextoCompletoDaNormaAlterada(string ch_norma_revogadora, string id_file_norma_revogada, string _ds_texto_para_alterador)
        {
            var htmlFile = new HtmlFileEncoded();
            var texto = htmlFile.GetHtmlFile(id_file_norma_revogada, "sinj_norma", null);

            var pattern = "(?!<p.+replaced_by=.+>)(<p.+?>)<s>(.+?)</s></p>";
            var replacement = "$1$2</p>";
            texto = Regex.Replace(texto, pattern, replacement);

            pattern = "(<h1.+?epigrafe=.+?>.+?</h1>)\r\n<p.*?><a.+?/" + ch_norma_revogadora + "/.+?>\\(" + _ds_texto_para_alterador + ".+?\\)</a></p>";
            replacement = "$1";
            texto = Regex.Replace(texto, pattern, replacement);
            return texto;
        }

        public string RemoverInformacaoNoTextoDaNormaAlterada(string ch_norma_revogadora, string id_file_norma_revogada, string _ds_texto_para_alterador)
        {
            var htmlFile = new HtmlFileEncoded();
            var texto = htmlFile.GetHtmlFile(id_file_norma_revogada, "sinj_norma", null);
            var pattern = "(<h1.+?epigrafe=.+?>.+?</h1>)\r\n<p.*?><a.+?/" + ch_norma_revogadora + "/.+?>\\(" + _ds_texto_para_alterador + ".+?\\)</a></p>";
            if (_ds_texto_para_alterador.ToLower() == "legislação correlata"){
                pattern = "<p.*?><a.+?/" + ch_norma_revogadora + "/.+?>\\(" + _ds_texto_para_alterador + ".+?\\)</a></p>\r\n(<h1.+?epigrafe=.+?>.+?</h1>)";
            }
            var replacement = "$1";
            texto = Regex.Replace(texto, pattern, replacement);
            return texto;
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
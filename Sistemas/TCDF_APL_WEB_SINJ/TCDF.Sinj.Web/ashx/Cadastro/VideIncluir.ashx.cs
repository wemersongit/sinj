using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.RN;
using System.Text.RegularExpressions;
using TCDF.Sinj.Web.ashx.Arquivo;
using neo.BRLightREST;

namespace TCDF.Sinj.Web.ashx.Cadastro
{
    /// <summary>
    /// Summary description for VideIncluir
    /// </summary>
    public class VideIncluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            NormaOV normaAlteradoraOv = null;
            NormaOV normaAlteradaOv = null;
            var _id_doc = context.Request["id_doc"];
            ulong id_doc = 0;
            var _ch_norma_alteradora = context.Request["ch_norma_alteradora"];
            var _ch_norma_alterada = context.Request["ch_norma_alterada"];
            var _in_norma_fora_do_sistema = context.Request["in_norma_fora_do_sistema"];

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

                if (!string.IsNullOrEmpty(_ch_norma_alteradora))
                {
                    if (!string.IsNullOrEmpty(_ch_tipo_relacao))
                    {
                        normaAlteradoraOv = normaRn.Doc(_ch_norma_alteradora);
                        id_doc = normaAlteradoraOv._metadata.id_doc;

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


                        var vide_alterador = new Vide();
                        var ch_vide = Guid.NewGuid().ToString("N");
                        ///Esse vide será armazenado no documento da norma alteradora, logo devo referenciar qual a norma alterada.
                        ///Assim como também devo colocar aqui todas informações que devem aparecer nos detalhes do documento.
                        ///Exemplo: Revoga paragrafo tal da norma tal...
                        vide_alterador.ch_vide = ch_vide;
                        vide_alterador.in_relacao_de_acao = normaAlteradoraOv.st_acao;
                        vide_alterador.ch_norma_vide = _ch_norma_alterada;

                        vide_alterador.ch_tipo_relacao = ch_tipo_relacao_pos_verificacao;
                        vide_alterador.nm_tipo_relacao = nm_tipo_relacao_pos_verificacao;
                        vide_alterador.ds_texto_relacao = ds_texto_para_alterado_pos_verificacao; //Exemplo: Revoga Totalmente

                        vide_alterador.in_norma_afetada = false;
                        vide_alterador.in_relacao_de_acao = false;
                        if (_in_norma_fora_do_sistema == "1")
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
                        else if (!string.IsNullOrEmpty(_ch_norma_alterada))
                        {
                            normaAlteradaOv = normaRn.Doc(_ch_norma_alterada);
                            vide_alterador.ch_norma_vide = normaAlteradaOv.ch_norma;
                            vide_alterador.ch_tipo_norma_vide = normaAlteradaOv.ch_tipo_norma;
                            vide_alterador.nm_tipo_norma_vide = normaAlteradaOv.nm_tipo_norma;
                            vide_alterador.nr_norma_vide = normaAlteradaOv.nr_norma;
                            vide_alterador.dt_assinatura_norma_vide = normaAlteradaOv.dt_assinatura;
                            if (Convert.ToDateTime(normaAlteradaOv.dt_assinatura) > Convert.ToDateTime(normaAlteradoraOv.dt_assinatura))
                            {
                                // Essa regra nao se aplica caso a norma alteradora seja ADI.
                                if (normaAlteradoraOv.ch_tipo_norma != "11000000")
                                {
                                    throw new DocValidacaoException("Uma norma não pode possuir seu status alterado por uma norma anterior. Verifique as datas de assinatura das normas alteradora e alterada.");
                                }
                            }
                            if (normaAlteradoraOv.st_acao)
                            {
                                var tipo_norma_alterada = new TipoDeNormaRN().Doc(normaAlteradaOv.ch_tipo_norma);
                                if (!tipo_norma_alterada.in_questionavel)
                                {
                                    throw new DocValidacaoException("Uma ação não pode alterar uma norma que não é questionável.</br>Verifique no cadastro de Tipo de Norma se o Tipo '"+tipo_norma_alterada.nm_tipo_norma+"' está marcado como Questionável.");
                                }
                            }
                        }
                        else
                        {
                            throw new DocValidacaoException("Erro ao informar a norma alterada. Verifique se está selecionando corretamente a norma afetada pelo vide.");
                        }
                        //vide_alterador.alinea_norma_vide = _alinea_norma_vide;
                        //vide_alterador.anexo_norma_vide = _anexo_norma_vide;
                        //vide_alterador.artigo_norma_vide = _artigo_norma_vide;
                        //vide_alterador.paragrafo_norma_vide = _paragrafo_norma_vide;
                        //vide_alterador.inciso_norma_vide = _inciso_norma_vide;
                        //vide_alterador.item_norma_vide = _item_norma_vide;

                        vide_alterador.alinea_norma_vide_outra = _alinea_norma_vide_alterada;
                        vide_alterador.anexo_norma_vide_outra = _anexo_norma_vide_alterada;
                        vide_alterador.artigo_norma_vide_outra = _artigo_norma_vide_alterada;

                        if (!string.IsNullOrEmpty(_caput_norma_vide_alteradora))
                        {
                            vide_alterador.caput_norma_vide = JSON.Deserializa<Caput>(_caput_norma_vide_alteradora);
                            vide_alterador.caput_norma_vide.nm_relacao_aux = nm_tipo_relacao_pos_verificacao.ToLower();
                        }
                        if (!string.IsNullOrEmpty(_caput_norma_vide_alterada))
                        {
                            vide_alterador.caput_norma_vide_outra = JSON.Deserializa<Caput>(_caput_norma_vide_alterada);
                            vide_alterador.caput_norma_vide_outra.texto_novo = _caput_texto_novo;
                            vide_alterador.caput_norma_vide_outra.nm_relacao_aux = nm_tipo_relacao_pos_verificacao.ToLower();
                        }

                        vide_alterador.paragrafo_norma_vide_outra = _paragrafo_norma_vide_alterada;
                        vide_alterador.inciso_norma_vide_outra = _inciso_norma_vide_alterada;
                        vide_alterador.item_norma_vide_outra = _item_norma_vide_alterada;

                        vide_alterador.ds_comentario_vide = _ds_comentario_vide;

                        normaAlteradoraOv.vides.Add(vide_alterador);

                        var dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");

                        normaAlteradoraOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                        if (normaRn.Atualizar(normaAlteradoraOv._metadata.id_doc, normaAlteradoraOv))
                        {
                            if (normaAlteradaOv != null)
                            {
                                var vide_alterada = new Vide();
                                vide_alterada.ch_vide = ch_vide;
                                vide_alterada.in_relacao_de_acao = normaAlteradoraOv.st_acao;

                                vide_alterada.ch_tipo_relacao = ch_tipo_relacao_pos_verificacao;
                                vide_alterada.nm_tipo_relacao = nm_tipo_relacao_pos_verificacao;
                                vide_alterada.ds_texto_relacao = ds_texto_para_alterador_pos_verificacao; //Exemplo: Revogado Totalmente

                                vide_alterada.in_norma_afetada = true;
                                vide_alterada.ch_norma_vide = normaAlteradoraOv.ch_norma;
                                vide_alterada.ch_tipo_norma_vide = normaAlteradoraOv.ch_tipo_norma;
                                vide_alterada.nm_tipo_norma_vide = normaAlteradoraOv.nm_tipo_norma;
                                vide_alterada.nr_norma_vide = normaAlteradoraOv.nr_norma;
                                vide_alterada.dt_assinatura_norma_vide = normaAlteradoraOv.dt_assinatura;

                                vide_alterada.alinea_norma_vide = _alinea_norma_vide_alterada;
                                vide_alterada.anexo_norma_vide = _anexo_norma_vide_alterada;
                                vide_alterada.artigo_norma_vide = _artigo_norma_vide_alterada;
                                vide_alterada.paragrafo_norma_vide = _paragrafo_norma_vide_alterada;
                                vide_alterada.inciso_norma_vide = _inciso_norma_vide_alterada;
                                vide_alterada.item_norma_vide = _item_norma_vide_alterada;
                                vide_alterada.caput_norma_vide = vide_alterador.caput_norma_vide_outra;

                                //vide_alterada.alinea_norma_vide_outra = _alinea_norma_vide;
                                //vide_alterada.anexo_norma_vide_outra = _anexo_norma_vide;
                                //vide_alterada.artigo_norma_vide_outra = _artigo_norma_vide;
                                //vide_alterada.paragrafo_norma_vide_outra = _paragrafo_norma_vide;
                                //vide_alterada.inciso_norma_vide_outra = _inciso_norma_vide;
                                //vide_alterada.item_norma_vide_outra = _item_norma_vide;
                                vide_alterada.caput_norma_vide_outra = vide_alterador.caput_norma_vide;

                                vide_alterada.ds_comentario_vide = _ds_comentario_vide;

                                normaAlteradaOv.vides.Add(vide_alterada);
                                var situacao = normaRn.ObterSituacao(normaAlteradaOv.vides);
                                normaAlteradaOv.ch_situacao = situacao.ch_situacao;
                                normaAlteradaOv.nm_situacao = situacao.nm_situacao;
                                normaAlteradaOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                                if (normaRn.Atualizar(normaAlteradaOv._metadata.id_doc, normaAlteradaOv))
                                {
                                    sRetorno = "{\"id_doc_success\":" + id_doc + ", \"ch_norma\":\""+normaAlteradoraOv.ch_norma+"\"}";
                                    if (vide_alterador.caput_norma_vide != null && vide_alterada.caput_norma_vide != null && vide_alterador.caput_norma_vide.caput.Length > 0 && vide_alterada.caput_norma_vide.caput.Length > 0)
                                    {
                                        IncluirCaputNosArquivos(normaAlteradoraOv.ch_norma, normaAlteradaOv.ch_norma, vide_alterador.caput_norma_vide, vide_alterada.caput_norma_vide, _caput_texto_novo, ds_texto_para_alterador_pos_verificacao);
                                    }
                                    //else if (vide_alterador.caput_norma_vide != null && vide_alterador.caput_norma_vide.caput.Length > 0 && normaAlteradaOv.nm_situacao.ToLower() == "revogado" && nm_tipo_relacao_pos_verificacao.ToLower() == "revogação")
                                    else if (vide_alterador.caput_norma_vide != null && vide_alterador.caput_norma_vide.caput.Length > 0)
                                    {
                                        IncluirCaputNosArquivos(normaAlteradoraOv, normaAlteradaOv, vide_alterador.caput_norma_vide, ds_texto_para_alterador_pos_verificacao);
                                    }
                                }
                                else
                                {
                                    throw new Exception("Erro ao cadastrar Vide na norma alterada.");
                                }
                            }
                            else
                            {
                                sRetorno = "{\"id_doc_success\":" + id_doc + ", \"ch_norma\":\"" + normaAlteradoraOv.ch_norma + "\"}";
                            }
                        }
                        else
                        {
                            throw new Exception("Erro ao cadastrar Vide.");
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
                    registro = normaAlteradoraOv
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ",VIDE.INC", log_editar, id_doc, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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
                LogErro.gravar_erro(Util.GetEnumDescription(action) + ",VIDE.INC", erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            context.Response.Write(sRetorno);
            context.Response.End();

        }



        public void IncluirCaputNosArquivos(string ch_norma_alteradora, string ch_norma_alterada, Caput caput_norma_vide_alteradora, Caput caput_norma_vide_alterada, string[] _caput_texto_novo, string _ds_texto_para_alterador)
        {
            caput_norma_vide_alterada.texto_novo = _caput_texto_novo;
            var upload = new UploadHtml();
            var normaRn = new NormaRN();
            var pesquisa = new Pesquisa();

            var arquivo_norma_vide_alteradora = CriarLinkNoTextoDaNormaAlteradora(caput_norma_vide_alteradora, caput_norma_vide_alterada);

            var retorno_file_alteradora = upload.AnexarHtml(arquivo_norma_vide_alteradora, caput_norma_vide_alteradora.filename, "sinj_norma");

            if (retorno_file_alteradora.IndexOf("id_file") > -1)
            {
                pesquisa.literal = "ch_norma='" + ch_norma_alteradora + "'";
                var listOp = new List<opMode<object>>();
                listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retorno_file_alteradora } });
                normaRn.PathPut<object>(pesquisa, listOp);
            }

            var arquivo_norma_vide_alterada = AlterarTextoDaNormaAlterada(caput_norma_vide_alterada, caput_norma_vide_alteradora, _ds_texto_para_alterador.ToLower());

            var retorno_file_alterada = upload.AnexarHtml(arquivo_norma_vide_alterada, caput_norma_vide_alterada.filename, "sinj_norma");
            if (retorno_file_alterada.IndexOf("id_file") > -1)
            {
                pesquisa.literal = "ch_norma='" + ch_norma_alterada + "'";
                var listOp = new List<opMode<object>>();
                listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retorno_file_alterada } });
                normaRn.PathPut<object>(pesquisa, listOp);
            }
        }

        public void IncluirCaputNosArquivos(NormaOV norma_alteradora, NormaOV norma_alterada, Caput caput_norma_vide_alteradora, string _ds_texto_para_alterador)
        {
            var upload = new UploadHtml();
            var normaRn = new NormaRN();
            var pesquisa = new Pesquisa();

            var if_file_norma_alterada = norma_alterada.getIdFileArquivoVigente();
            var name_file_norma_alterada = norma_alterada.getNameFileArquivoVigente();

            if (!string.IsNullOrEmpty(if_file_norma_alterada))
            {
                var aux_ds_texto_para_alterador = _ds_texto_para_alterador.ToLower();
                var aux_nm_situacao_alterada = norma_alterada.nm_situacao.ToLower();

                var arquivo_norma_vide_alterada = "";

                if ((aux_nm_situacao_alterada == "revogado" && aux_ds_texto_para_alterador == "revogado") ||
                    (aux_nm_situacao_alterada == "anulado" && aux_ds_texto_para_alterador == "anulado") ||
                    (aux_nm_situacao_alterada == "extinta" && aux_ds_texto_para_alterador == "extinta") ||
                    (aux_nm_situacao_alterada == "inconstitucional" && aux_ds_texto_para_alterador == "declarado inconstitucional") ||
                    (aux_nm_situacao_alterada == "cancelada" && aux_ds_texto_para_alterador == "cancelada") ||
                    (aux_nm_situacao_alterada == "suspenso" && aux_ds_texto_para_alterador == "suspenso totalmente")){
                        arquivo_norma_vide_alterada = AlterarTextoCompletoDaNormaAlterada(norma_alteradora, if_file_norma_alterada, caput_norma_vide_alteradora, _ds_texto_para_alterador);
                    }
                else if (aux_ds_texto_para_alterador == "ratificado" ||
                    aux_ds_texto_para_alterador == "reeditado" ||
                    aux_ds_texto_para_alterador == "regulamentado" ||
                    aux_ds_texto_para_alterador == "prorrogado" ||
                    aux_ds_texto_para_alterador == "legislação correlata")
                {
                    arquivo_norma_vide_alterada = AcrescentarInformacaoNoTextoDaNormaAlterada(norma_alteradora, if_file_norma_alterada, caput_norma_vide_alteradora, _ds_texto_para_alterador);
                }


                if (!string.IsNullOrEmpty(arquivo_norma_vide_alterada))
                {
                    var retorno_file_alterada = upload.AnexarHtml(arquivo_norma_vide_alterada, name_file_norma_alterada, "sinj_norma");

                    if (retorno_file_alterada.IndexOf("id_file") > -1)
                    {
                        pesquisa.literal = "ch_norma='" + norma_alterada.ch_norma + "'";
                        var listOp = new List<opMode<object>>();
                        listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retorno_file_alterada } });
                        normaRn.PathPut<object>(pesquisa, listOp);

                        var arquivo_norma_vide_revogadora = CriarLinkNoTextoDaNormaAlteradora(caput_norma_vide_alteradora, norma_alterada.ch_norma, name_file_norma_alterada);

                        var retorno_file_alteradora = upload.AnexarHtml(arquivo_norma_vide_revogadora, caput_norma_vide_alteradora.filename, "sinj_norma");

                        if (retorno_file_alteradora.IndexOf("id_file") > -1)
                        {
                            pesquisa.literal = "ch_norma='" + norma_alteradora.ch_norma + "'";
                            listOp = new List<opMode<object>>();
                            listOp.Add(new opMode<object> { path = "ar_atualizado", mode = "update", args = new string[] { retorno_file_alteradora } });
                            normaRn.PathPut<object>(pesquisa, listOp);
                        }
                    }
                }
            }
            
        }

        public string CriarLinkNoTextoDaNormaAlteradora(Caput _caput_alteradora, Caput _caput_alterada)
        {
            var htmlFile = new HtmlFileEncoded();
            var texto = htmlFile.GetHtmlFile(_caput_alteradora.id_file, "sinj_norma", null);
            return CriarLinkNoTextoDaNormaAlteradora(texto, _caput_alteradora, _caput_alterada);
        }

        public string CriarLinkNoTextoDaNormaAlteradora(string texto, Caput _caput_alteradora, Caput _caput_alterada)
        {
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
            var pattern = "(<p.+?linkname=\"" + _caput_alteradora.caput[0] + "\".*?>)(.*?)" + UtilVides.EscapeCharsInToPattern(_caput_alteradora.link) + "(.*?)</p>";
            var replacement = "$1$2" + "<a href=\"(_link_sistema_)Norma/" + _caput_alterada.ch_norma + '/' + _caput_alterada.filename + "#" + pattern_caput + "\" >" + _caput_alteradora.link + "</a>" + "$3</p>";
            return Regex.Replace(texto, pattern, replacement);
        }

        public string CriarLinkNoTextoDaNormaAlteradora(Caput _caput_revogadora, string ch_norma_revogada, string name_file_norma_revogada)
        {
            var htmlFile = new HtmlFileEncoded();
            var texto = htmlFile.GetHtmlFile(_caput_revogadora.id_file, "sinj_norma", null);
            return CriarLinkNoTextoDaNormaAlteradora(texto, _caput_revogadora, ch_norma_revogada, name_file_norma_revogada);
        }

        public string CriarLinkNoTextoDaNormaAlteradora(string texto, Caput _caput_alteradora, string ch_norma_alterada, string name_file_norma_alterada)
        {
            var pattern = "(<p.+?linkname=\"" + _caput_alteradora.caput[0] + "\".*?>)(.*?)" + UtilVides.EscapeCharsInToPattern(_caput_alteradora.link) + "(.*?)</p>";
            var replacement = "$1$2" + "<a href=\"(_link_sistema_)Norma/" + ch_norma_alterada + '/' + name_file_norma_alterada + "\" >" + _caput_alteradora.link + "</a>" + "$3</p>";
            return Regex.Replace(texto, pattern, replacement);
        }

        public string AlterarTextoDaNormaAlterada(Caput _caput_alterada, Caput _caput_alteradora, string _ds_texto_para_alterador)
        {
            var texto = "";
            if (_caput_alterada.caput.Length == _caput_alterada.texto_antigo.Length)
            {
                var htmlFile = new HtmlFileEncoded();
                texto = htmlFile.GetHtmlFile(_caput_alterada.id_file, "sinj_norma", null);
                texto = AlterarTextoDaNormaAlterada(texto, _caput_alterada, _caput_alteradora, _ds_texto_para_alterador);
            }
            return texto;
        }

        public string AlterarTextoDaNormaAlterada(string texto, Caput _caput_alterada, Caput _caput_alteradora, string _ds_texto_para_alterador)
        {
            if (_caput_alterada.caput.Length == _caput_alterada.texto_antigo.Length)
            {
                var pattern = "";
                var replacement = "";
                for (var i = 0; i < _caput_alterada.caput.Length; i++)
                {
                    switch (_caput_alterada.nm_relacao_aux)
                    {
                        case "acrescimo":
                            pattern = "(<p.+?linkname=\")" + _caput_alterada.caput[i] + "(\".*?>)(.*?<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>.*?</p>)";
                            var texto_novo_splited = _caput_alterada.texto_novo[i].Split('\n');
                            replacement = "$1" + _caput_alterada.caput[i] + "$2$3";
                            for (var j = 0; j < texto_novo_splited.Length; j++)
                            {
                                replacement += "\r\n$1" + _caput_alterada.caput[i] + "_add_" + j + "$2<a name=\"" + _caput_alterada.caput[i] + "_add_" + j + "\"></a>" + texto_novo_splited[j] + " <a class=\"link_vide\" href=\"(_link_sistema_)Norma/" + _caput_alteradora.ch_norma + '/' + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\" >(" + gerarDescricaoDoTexto(texto_novo_splited[j]) + " " + _ds_texto_para_alterador + " pelo(a) " + _caput_alteradora.ds_norma + ")</a></p>";
                            }
                            break;
                        case "renumeração":
                            pattern = "(<p.+?linkname=\")" + _caput_alterada.caput[i] + "(\".*?)<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>.*?</p>";
                            replacement = "$1" + _caput_alterada.caput[i] + "_renum$2<a name=\"" + _caput_alterada.caput[i] + "_renum\"></a>" + _caput_alterada.texto_novo[i] + " <a class=\"link_vide\" href=\"(_link_sistema_)Norma/" + _caput_alteradora.ch_norma + '/' + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\" >(" + gerarDescricaoDoCaput(_caput_alterada.caput[i]) + " " + _ds_texto_para_alterador + " pelo(a) " + _caput_alteradora.ds_norma + ")</a></p>";
                            break;
                        case "prorrogação":
                        case "ratificação":
                        case "regulamentação":
                        case "ressalva":
                        case "retificação":
                            pattern = "(<p.+?linkname=\"" + _caput_alterada.caput[i] + "\".*?<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>.*?)</p>";
                            replacement = "$1 <a class=\"link_vide\" href=\"(_link_sistema_)Norma/" + _caput_alteradora.ch_norma + '/' + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\" >(" + gerarDescricaoDoCaput(_caput_alterada.caput[i]) + " " + _ds_texto_para_alterador + " pelo(a) " + _caput_alteradora.ds_norma + ")</a></p>";
                            break;
                        default:
                            pattern = "(<p.+?linkname=\")" + _caput_alterada.caput[i] + "(\".*?>)(.*?)(<a.+?name=\"" + _caput_alterada.caput[i] + "\".*?></a>)(.*?)</p>";
                            replacement = "$1" + _caput_alterada.caput[i] + "_replaced\" replaced_by=\"" + _caput_alteradora.ch_norma + "$2<s>$3<a name=\"" + _caput_alterada.caput[i] + "_replaced\"></a>$5</s></p>\r\n$1" + _caput_alterada.caput[i] + "$2$3$4" + _caput_alterada.texto_novo[i] + " <a class=\"link_vide\" href=\"(_link_sistema_)Norma/" + _caput_alteradora.ch_norma + '/' + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\" >(" + gerarDescricaoDoCaput(_caput_alterada.caput[i]) + " " + _ds_texto_para_alterador + " pelo(a) " + _caput_alteradora.ds_norma + ")</a></p>";
                            break;
                    }
                    texto = Regex.Replace(texto, pattern, replacement);
                }
            }
            return texto;
        }

        public string AlterarTextoCompletoDaNormaAlterada(NormaOV norma_alteradora, string id_file_norma_alterada, Caput _caput_alteradora, string _ds_texto_para_alterador)
        {
            var htmlFile = new HtmlFileEncoded();
            var texto = htmlFile.GetHtmlFile(id_file_norma_alterada, "sinj_norma", null);
            return AlterarTextoCompletoDaNormaAlterada(texto, norma_alteradora, _caput_alteradora, _ds_texto_para_alterador);
        }

        public string AlterarTextoCompletoDaNormaAlterada(string texto, NormaOV norma_alteradora, Caput _caput_alteradora, string _ds_texto_para_alterador)
        {
            var htmlFile = new HtmlFileEncoded();
            var pattern = "(?!<p.+replaced_by=.+>)(<p.+?>)(.+?)</p>";
            var replacement = "$1<s>$2</s></p>";
            texto = Regex.Replace(texto, pattern, replacement);

            pattern = "(<h1.+?epigrafe=.+?>.+?</h1>)";
            replacement = "$1\r\n<p style=\"text-align:center;\"><a href=\"(_link_sistema_)Norma/" + norma_alteradora.ch_norma + "/" + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\" >(" + _ds_texto_para_alterador + " pelo(a) " + _caput_alteradora.ds_norma + ")</a></p>";
            texto = Regex.Replace(texto, pattern, replacement);

            return texto;
        }

        public string AcrescentarInformacaoNoTextoDaNormaAlterada(NormaOV norma_alteradora, string id_file_norma_alterada, Caput _caput_alteradora, string _ds_texto_para_alterador)
        {
            var htmlFile = new HtmlFileEncoded();
            var texto = htmlFile.GetHtmlFile(id_file_norma_alterada, "sinj_norma", null);
            return AcrescentarInformacaoNoTextoDaNormaAlterada(texto, norma_alteradora, _caput_alteradora, _ds_texto_para_alterador);
        }

        public string AcrescentarInformacaoNoTextoDaNormaAlterada(string texto, NormaOV norma_alteradora, Caput _caput_alteradora, string _ds_texto_para_alterador)
        {
            var htmlFile = new HtmlFileEncoded();
            var pattern = "(<h1.+?epigrafe=.+?>.+?</h1>)";
            var replacement = "$1\r\n<p><a href=\"(_link_sistema_)Norma/" + norma_alteradora.ch_norma + "/" + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\" >(" + _ds_texto_para_alterador + " pelo(a) " + _caput_alteradora.ds_norma + ")</a></p>";
            if (_ds_texto_para_alterador.ToLower() == "legislação correlata")
            {
                replacement = "<p><a href=\"(_link_sistema_)Norma/" + norma_alteradora.ch_norma + "/" + _caput_alteradora.filename + "#" + _caput_alteradora.caput[0] + "\" >" + _ds_texto_para_alterador + " - " + _caput_alteradora.ds_norma + "</a></p>\r\n$1";
            }
            texto = Regex.Replace(texto, pattern, replacement);
            return texto;
        }

        public string gerarDescricaoDoCaput(string _caput)
        {
            var caput_splited = _caput.Split('_');
            var last_caput = caput_splited.Last();
            if (last_caput.IndexOf("tit") == 0)
            {
                _caput = "Título";
            }
            else if (last_caput.IndexOf("cap") == 0)
            {
                _caput = "Capítulo";
            }
            else if (last_caput.IndexOf("art") == 0)
            {
                _caput = "Artigo";
            }
            else if (last_caput.IndexOf("par") == 0)
            {
                _caput = "Parágrafo";
            }
            else if (last_caput.IndexOf("inc") == 0)
            {
                _caput = "Inciso";
            }
            else if (last_caput.IndexOf("ltr") == 0)
            {
                _caput = "Letra";
            }
            else if (last_caput.IndexOf("aln") == 0)
            {
                _caput = "Alínea";
            }
            return _caput;
        }

        public string gerarDescricaoDoTexto(string texto)
        {
            var caput = texto.Substring(0, texto.Trim().IndexOf(' ')).ToUpper();
            if (caput == "TITULO" || caput == "TÍTULO")
            {
                caput = "Parágrafo";
            }
            else if (caput == "CAPITULO" || caput == "CAPÍTULO")
            {
                caput = "Capítulo";
            }
            else if (caput == "ART" || caput == "ART.")
            {
                caput = "Artigo";
            }
            else if (caput == "PARAGRAFO" || caput == "PARÁGRAFO" || caput == "§")
            {
                caput = "Parágrafo";
            }
            else if (ehInciso(caput))
            {
                caput = "Inciso";
            }
            else if (caput.LastIndexOf(')') > caput.Length)
            {
                caput = "Letra";
            }
            else if (ehAlinea(caput))
            {
                caput = "Alínea";
            }
            return caput;
        }

        public bool ehInciso(string termo)
        {
            char[] chars = new char[] { 'I', 'V', 'X', 'L', 'C', 'D', 'M' };
            for (var i = 0; i < termo.Length; i++)
            {
                if (chars.Count<char>(c => c == termo[i]) < 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool ehAlinea(string termo)
        {
            termo = termo.ToLower();
            char[] chars = new char[] { 'i', 'v', 'x', 'l', 'c', 'd', 'm' };
            for (var i = 0; i < termo.Length; i++)
            {
                if (chars.Count<char>(c => c == termo[i]) < 0)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class UtilVides
    {
        public static string EscapeCharsInToPattern(string text)
        {
            var aChars = new string[] { "(", ")", "$" };
            foreach(var aChar in aChars){
                text = text.Replace(aChar, "\\" + aChar);
            }
            return text;
        }
    }
}
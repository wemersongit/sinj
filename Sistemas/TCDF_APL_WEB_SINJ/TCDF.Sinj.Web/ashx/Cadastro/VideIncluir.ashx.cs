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
using System.Globalization;

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
            var _dt_controle_alteracao = context.Request["dt_controle_alteracao"];
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
                var normaRn = new NormaRN();
                var dDt_controle_alteracao = Convert.ToDateTime(_dt_controle_alteracao);

                if (!string.IsNullOrEmpty(_ch_norma_alteradora))
                {
                    if (!string.IsNullOrEmpty(_ch_tipo_relacao))
                    {
                        normaAlteradoraOv = normaRn.Doc(_ch_norma_alteradora);
                        id_doc = normaAlteradoraOv._metadata.id_doc;

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
                            //Vai comparar a data da ultima alteração com a data que o usuário abriu a página de editar vides e disparar uma exceção de risco de inconsistencia
                            if (normaAlteradaOv.alteracoes.Count > 0)
                            {
                                var dDt_alteracao = Convert.ToDateTime(normaAlteradaOv.alteracoes.Last<AlteracaoOV>().dt_alteracao);
                                var usuario = normaAlteradaOv.alteracoes.Last<AlteracaoOV>().nm_login_usuario_alteracao;
                                if (dDt_controle_alteracao < dDt_alteracao)
                                {
                                    throw new RiskOfInconsistency("A norma alterada foi modificada pelo usuário <b>" + usuario + "</b> às <b>" + dDt_alteracao + "</b> tornando a sua modificação inconsistente.<br/> É aconselhável atualizar a página e refazer as modificações ou forçar a alteração.<br/>Obs.: Clicar em 'Salvar mesmo assim' vai forçar a alteração e pode sobrescrever as modificações do usuário <b>" + usuario + "</b>.");
                                }
                            }
                            vide_alterador.ch_norma_vide = normaAlteradaOv.ch_norma;
                            vide_alterador.ch_tipo_norma_vide = normaAlteradaOv.ch_tipo_norma;
                            vide_alterador.nm_tipo_norma_vide = normaAlteradaOv.nm_tipo_norma;
                            vide_alterador.nr_norma_vide = normaAlteradaOv.nr_norma;
                            if (vide_alterador.nr_norma_vide == "0")
                            {
                                vide_alterador.nr_norma_vide = "";
                            }
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
                                    throw new DocValidacaoException("Uma ação não pode alterar uma norma que não é questionável.</br>Verifique no cadastro de Tipo de Norma se o Tipo '" + tipo_norma_alterada.nm_tipo_norma + "' está marcado como Questionável.");
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
                            vide_alterador.caput_norma_vide.ds_texto_para_alterador_aux = ds_texto_para_alterador_pos_verificacao.ToLower();
                        }
                        if (!string.IsNullOrEmpty(_caput_norma_vide_alterada))
                        {
                            vide_alterador.caput_norma_vide_outra = JSON.Deserializa<Caput>(_caput_norma_vide_alterada);
                            vide_alterador.caput_norma_vide_outra.ds_caput = _ds_caput_norma_alterada;
                            vide_alterador.caput_norma_vide_outra.texto_novo = _caput_texto_novo;
                            vide_alterador.caput_norma_vide_outra.nm_relacao_aux = nm_tipo_relacao_pos_verificacao.ToLower();
                            vide_alterador.caput_norma_vide_outra.ds_texto_para_alterador_aux = ds_texto_para_alterador_pos_verificacao.ToLower();
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
                                if (vide_alterada.nr_norma_vide == "0")
                                {
                                    vide_alterada.nr_norma_vide = "";
                                }
                                vide_alterada.dt_assinatura_norma_vide = normaAlteradoraOv.dt_assinatura;
                                if (normaAlteradoraOv.st_vacatio_legis && !string.IsNullOrEmpty(normaAlteradoraOv.dt_inicio_vigencia)){
                                    vide_alterada.dt_inicio_vigencia_norma_vide = normaAlteradoraOv.dt_inicio_vigencia;
                                }

                                vide_alterada.alinea_norma_vide = _alinea_norma_vide_alterada;
                                vide_alterada.anexo_norma_vide = _anexo_norma_vide_alterada;
                                vide_alterada.artigo_norma_vide = _artigo_norma_vide_alterada;
                                vide_alterada.paragrafo_norma_vide = _paragrafo_norma_vide_alterada;
                                vide_alterada.inciso_norma_vide = _inciso_norma_vide_alterada;
                                vide_alterada.item_norma_vide = _item_norma_vide_alterada;
                                vide_alterada.caput_norma_vide = vide_alterador.caput_norma_vide_outra;

                                vide_alterada.caput_norma_vide_outra = vide_alterador.caput_norma_vide;

                                vide_alterada.ds_comentario_vide = _ds_comentario_vide;

                                normaAlteradaOv.vides.Add(vide_alterada);
                                if (!normaAlteradaOv.st_situacao_forcada)
                                {
                                    var situacaoRn = new SituacaoRN();
                                    var situacaoOv = new SituacaoOV { ch_situacao = normaAlteradaOv.ch_situacao, nm_situacao = normaAlteradaOv.nm_situacao };
                                    var situacao = normaRn.ObterSituacao(normaAlteradaOv.vides);
									//Cod inserido para trocar o nome da situação caso for revigorado ele voltara a ser "sem revogação expressa"
                                    situacaoOv = situacaoRn.Doc("semrevogacaoexpressa");

                                    if (_nm_tipo_relacao == "REVIGORAÇÃO")
                                    {
                                        normaAlteradaOv.ch_situacao = situacao.ch_situacao;
                                        normaAlteradaOv.nm_situacao = situacaoOv.nm_situacao;
                                    } else {
                                        normaAlteradaOv.nm_situacao = situacao.nm_situacao;
                                    }
                                }
                                normaAlteradaOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                                if (normaRn.Atualizar(normaAlteradaOv._metadata.id_doc, normaAlteradaOv))
                                {
                                    sRetorno = "{\"id_doc_success\":" + id_doc + ", \"ch_norma\":\"" + normaAlteradoraOv.ch_norma + "\", \"dt_controle_alteracao\":\"" + DateTime.Now.AddSeconds(1).ToString("dd'/'MM'/'yyyy HH:mm:ss") + "\"}";
                                    if (!normaAlteradoraOv.st_vacatio_legis || string.IsNullOrEmpty(normaAlteradoraOv.dt_inicio_vigencia) || Convert.ToDateTime(normaAlteradoraOv.dt_inicio_vigencia, new CultureInfo("pt-BR")) <= DateTime.Now)
                                    {
                                        normaRn.VerificarDispositivosESalvarOsTextosAntigosDasNormas(normaAlteradoraOv, normaAlteradaOv, vide_alterador, vide_alterada, sessao_usuario.nm_login_usuario);
                                        normaRn.VerificarDispositivosEAlterarOsTextosDasNormas(normaAlteradoraOv, normaAlteradaOv, vide_alterador, vide_alterada);
                                    }
                                }
                                else
                                {
                                    throw new Exception("Erro ao cadastrar Vide na norma alterada.");
                                }
                            }
                            else
                            {
                                sRetorno = "{\"id_doc_success\":" + id_doc + ", \"ch_norma\":\"" + normaAlteradoraOv.ch_norma + "\", \"dt_controle_alteracao\":\"" + DateTime.Now.AddSeconds(1).ToString("dd'/'MM'/'yyyy HH:mm:ss") + "\"}";
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
            catch (FileNotFoundException ex)
            {
                sRetorno = "{\"id_doc_success\":" + id_doc + ", \"ch_norma\":\"" + normaAlteradoraOv.ch_norma + "\", \"alert_message\": \"" + ex.Message + "\"}";
            }
            catch (Exception ex)
            {

                if (ex is PermissionException || ex is DocDuplicateKeyException || ex is SessionExpiredException || ex is DocValidacaoException || ex is RiskOfInconsistency)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\", \"type_error\":\"" + ex.GetType().Name + "\", \"dt_controle_alteracao\":\"" + DateTime.Now.AddSeconds(1).ToString("dd'/'MM'/'yyyy HH:mm:ss") + "\"}";
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    
}

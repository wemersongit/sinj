using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.Web.ashx.Cadastro;

namespace TCDF.Sinj.Web.ashx.Exclusao
{
    /// <summary>
    /// Summary description for VideExcluir
    /// </summary>
    public class VideExcluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            NormaOV normaOv = new NormaOV();
            NormaOV normaAlteradoraOv = null;
            NormaOV normaAlteradaOv = null;
            var _id_doc = context.Request["id_doc"];
            var _ch_vide = context.Request["ch_vide"];
            ulong id_doc = 0;
            var action = AcoesDoUsuario.nor_edt;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                var normaRn = new NormaRN();
                if (!string.IsNullOrEmpty(_id_doc) && !string.IsNullOrEmpty(_ch_vide))
                {
                    id_doc = ulong.Parse(_id_doc);
                    normaOv = normaRn.Doc(id_doc);
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
                        var enum_caput_alterador = normaAlteradoraOv.vides.Where(v => v.ch_vide == _ch_vide);
                        var enum_caput_alterada = normaAlteradaOv.vides.Where(v => v.ch_vide == _ch_vide);

                        var caput_alterador = new Caput();
                        var caput_alterada = new Caput(); 
                        var nm_tipo_relacao = "";
                        if (enum_caput_alterador.Count() > 0 && enum_caput_alterada.Count() > 0){
                            caput_alterador = enum_caput_alterador.Select(v => v.caput_norma_vide).First();
                            caput_alterada = enum_caput_alterada.Select(v => v.caput_norma_vide).First();
                            nm_tipo_relacao = enum_caput_alterador.Select(v => v.nm_tipo_relacao).First();;
                        }

                        if (normaAlteradoraOv.vides.RemoveAll(vd => vd.ch_vide == _ch_vide) > 0)
                        {
                            normaAlteradoraOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                            if (normaRn.Atualizar(normaAlteradoraOv._metadata.id_doc, normaAlteradoraOv))
                            {
                                if (normaAlteradaOv != null)
                                {
                                    if (normaAlteradaOv.vides.RemoveAll(vd => vd.ch_vide == _ch_vide) > 0)
                                    {
                                        var situacao = normaRn.ObterSituacao(normaAlteradaOv.vides);
                                        var old_nm_situacao = normaAlteradaOv.nm_situacao;
                                        normaAlteradaOv.ch_situacao = situacao.ch_situacao;
                                        normaAlteradaOv.nm_situacao = situacao.nm_situacao;

                                        normaAlteradaOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = normaAlteradoraOv.dt_cadastro, nm_login_usuario_alteracao = normaAlteradoraOv.nm_login_usuario_cadastro });
                                        if (normaRn.Atualizar(normaAlteradaOv._metadata.id_doc, normaAlteradaOv))
                                        {
                                            sRetorno = "{\"id_doc_success\":" + id_doc + "}";

                                            if (caput_alterador != null && caput_alterador.caput != null && caput_alterador.caput.Length > 0 && caput_alterada != null && caput_alterada.caput != null && caput_alterada.caput.Length > 0)
                                            {
                                                new VideEditar().DesfazerCaputDosArquivos(normaAlteradoraOv, normaAlteradaOv, caput_alterador, caput_alterada);
                                            }
                                            else if (caput_alterador != null && caput_alterador.caput != null && caput_alterador.caput.Length > 0 && nm_tipo_relacao.ToLower() == "revogação" && old_nm_situacao.ToLower() == "revogado" && normaAlteradaOv.nm_situacao != old_nm_situacao)
                                            {
                                                new VideEditar().DesfazerRevogacaoDosArquivos(normaAlteradoraOv, normaAlteradaOv, caput_alterador);
                                            }
                                        }
                                        else
                                        {
                                            throw new Exception("Erro ao excluir Vide na norma alterada.");
                                        }
                                    }
                                }
                                else
                                {
                                    sRetorno = "{\"id_doc_success\":" + id_doc + "}";
                                }
                            }
                            else
                            {
                                throw new Exception("Erro ao excluir Vide.");
                            }
                        }
                        foreach (var vide_alteradora in normaAlteradoraOv.vides)
                        {
                            if (vide_alteradora.ch_vide == _ch_vide)
                            {
                                normaAlteradoraOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                                
                                break;
                            }
                        }
                    }
                    var log_editar = new LogAlterar<NormaOV>
                    {
                        id_doc = id_doc
                    };
                    LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ",VIDE.EXC", log_editar, id_doc, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocDependenciesException || ex is SessionExpiredException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\", \"id_doc_error\":" + _id_doc + "}";
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
                    LogErro.gravar_erro(Util.GetEnumDescription(action) + ",VIDE.EXC", erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
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
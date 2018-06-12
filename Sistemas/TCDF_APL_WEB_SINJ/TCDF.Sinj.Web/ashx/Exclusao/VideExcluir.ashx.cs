using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.Web.ashx.Cadastro;
using TCDF.Sinj.Web.ashx.Arquivo;
using neo.BRLightREST;
using System.Text.RegularExpressions;

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
            var dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
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
                                try
                                {

                                    normaAlteradoraOv = normaRn.Doc(vide.ch_norma_vide);
                                }
                                catch (DocNotFoundException)
                                {

                                }
                            }
                            else
                            {
                                normaAlteradoraOv = normaOv;
                                if (!string.IsNullOrEmpty(vide.ch_norma_vide))
                                {
                                    try
                                    {
                                        normaAlteradaOv = normaRn.Doc(vide.ch_norma_vide);
                                    }
                                    catch (DocNotFoundException)
                                    {

                                    }
                                }
                            }
                            break;
                        }
                    }
                    if (normaAlteradoraOv != null && normaAlteradoraOv.vides.Count > 0)
                    {
                        var iEnumVideAlteradorDesfazer = normaAlteradoraOv.vides.Where(v => v.ch_vide == _ch_vide);
                        Vide videAlteradorDesfazer = null;
                        if (iEnumVideAlteradorDesfazer.Count() > 0)
                        {
                            videAlteradorDesfazer = iEnumVideAlteradorDesfazer.First();
                        }
                        if (normaAlteradoraOv.vides.RemoveAll(vd => vd.ch_vide == _ch_vide) > 0 && videAlteradorDesfazer != null)
                        {
                            normaAlteradoraOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                            if (normaRn.Atualizar(normaAlteradoraOv._metadata.id_doc, normaAlteradoraOv))
                            {
                                if (normaAlteradaOv != null && normaAlteradaOv.vides.Count > 0)
                                {

                                    var iEnumVideAlteradoDesfazer = normaAlteradaOv.vides.Where(v => v.ch_vide == _ch_vide);
                                    Vide videAlteradoDesfazer = null;
                                    if (iEnumVideAlteradoDesfazer.Count() > 0)
                                    {
                                        videAlteradoDesfazer = iEnumVideAlteradoDesfazer.First();
                                    }
                                    if (normaAlteradaOv.vides.RemoveAll(vd => vd.ch_vide == _ch_vide) > 0 && videAlteradoDesfazer != null)
                                    {
                                        var situacao = normaRn.ObterSituacao(normaAlteradaOv.vides);
                                        var nmSituacaoAnterior = normaAlteradaOv.nm_situacao;
                                        normaAlteradaOv.ch_situacao = situacao.ch_situacao;
                                        normaAlteradaOv.nm_situacao = situacao.nm_situacao;

                                        normaAlteradaOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                                        if (normaRn.Atualizar(normaAlteradaOv._metadata.id_doc, normaAlteradaOv))
                                        {
                                            sRetorno = "{\"id_doc_success\":" + id_doc + "}";
                                            normaRn.VerificarDispositivosEDesfazerAltercaoNosTextosDasNormas(normaAlteradoraOv, normaAlteradaOv, videAlteradorDesfazer, videAlteradoDesfazer, nmSituacaoAnterior, sessao_usuario.nm_login_usuario);
                                        }
                                        else
                                        {
                                            throw new Exception("Erro ao excluir Vide na norma alterada.");
                                        }
                                    }
                                }
                                else
                                {
                                    sRetorno = "{\"id_doc_success\":" + normaAlteradoraOv._metadata.id_doc + ", \"alert_message\": \"O vide foi excluído mas a norma alterada não foi encontrada ou não possui vides. Se há alteração de vide no arquivo da norma, a mesma deve ser desfeita manualmente.\"}";
                                }
                            }
                            else
                            {
                                throw new Exception("Erro ao excluir Vide da norma alteradora e consequentemente o vido da norma alterada não foi excluído também.");
                            }
                        }
                    }
                    else if (normaAlteradaOv != null && normaAlteradaOv.vides.Count > 0)
                    {
                        if (normaAlteradaOv.vides.RemoveAll(vd => vd.ch_vide == _ch_vide) > 0)
                        {
                            var nmSituacaoAnterior = normaAlteradaOv.nm_situacao;
                            if (!normaAlteradaOv.st_situacao_forcada)
                            {
                                var situacao = normaRn.ObterSituacao(normaAlteradaOv.vides);
                                normaAlteradaOv.ch_situacao = situacao.ch_situacao;
                                normaAlteradaOv.nm_situacao = situacao.nm_situacao;
                            }

                            normaAlteradaOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = dt_alteracao, nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                            if (normaRn.Atualizar(normaAlteradaOv._metadata.id_doc, normaAlteradaOv))
                            {
                                sRetorno = "{\"id_doc_success\":" + normaAlteradaOv._metadata.id_doc + ", \"alert_message\": \"O vide foi excluído mas a norma alteradora não foi encontrada ou não possui vides. Se há alteração de vide no arquivo da norma, a mesma deve ser desfeita manualmente.\"}";
                            }
                            else
                            {
                                throw new Exception("Erro ao excluir Vide na norma alterada.");
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
            catch (FileNotFoundException ex)
            {
                sRetorno = "{\"id_doc_success\":" + id_doc + ", \"alert_message\": \"" + ex.Message + "\"}";
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
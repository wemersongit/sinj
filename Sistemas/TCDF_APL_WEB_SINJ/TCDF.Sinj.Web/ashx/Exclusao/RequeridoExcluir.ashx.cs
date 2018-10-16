using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Exclusao
{
    /// <summary>
    /// Summary description for RequeridoExcluir
    /// </summary>
    public class RequeridoExcluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var _id_doc = context.Request["id_doc"];
            var _justificativa = context.Request["justificativa"];
            ExcluidoOV excluidoOv = null;
            ulong id_doc = 0;
            var action = AcoesDoUsuario.rqi_exc;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (ulong.TryParse(_id_doc, out id_doc))
                {
                    sessao_usuario = Util.ValidarSessao();
                    Util.ValidarUsuario(sessao_usuario, action);
                    if (string.IsNullOrEmpty(_justificativa))
                    {
                        throw new Exception("Obrigatório justificar.");
                    }
                    var requeridoOv = new RequeridoRN().JsonReg(id_doc);
                    if (new RequeridoRN().Excluir(id_doc))
                    {
                        sRetorno = "{\"excluido\":true, \"id_doc_success\":" + id_doc + "}";
                        try
                        {
                            excluidoOv = new ExcluidoOV();
                            excluidoOv.id_doc_excluido = id_doc;
                            excluidoOv.json_doc_excluido = requeridoOv;
                            excluidoOv.nm_base_excluido = Config.ValorChave("NmBaseRequerido", true);
                            excluidoOv.nm_login_usuario_exclusao = sessao_usuario.nm_login_usuario;
                            excluidoOv.dt_exclusao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                            var sRetornoExcluido = new ExcluidoRN().Incluir(excluidoOv);
                            if (sRetornoExcluido < 0)
                            {
                                throw new Exception("Erro ao salvar na base de excluídos.");
                            }
                        }
                        catch (Exception ex)
                        {
                            var erro = new ErroRequest
                            {
                                Pagina = context.Request.Path,
                                RequestQueryString = context.Request.QueryString,
                                MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                                StackTrace = ex.StackTrace
                            };
                            if (sessao_usuario != null)
                            {
                                LogErro.gravar_erro(Util.GetEnumDescription(action), erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                            }
                        }
                        var log_excluir = new LogExcluir
                        {
                            id_doc = id_doc,
                            nm_base = "requerido"
                        };
                        LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_excluir, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                    }
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
                    LogErro.gravar_erro(Util.GetEnumDescription(action), erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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

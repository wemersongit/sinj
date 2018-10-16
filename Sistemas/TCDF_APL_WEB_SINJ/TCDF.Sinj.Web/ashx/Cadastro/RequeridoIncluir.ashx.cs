using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Cadastro
{
    /// <summary>
    /// Summary description for RequeridoIncluir
    /// </summary>
    public class RequeridoIncluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno;
            RequeridoOV requeridoOv = null;
            var action = AcoesDoUsuario.rqi_inc;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                var _nm_requerido = context.Request["nm_requerido"];
                var _ds_requerido = context.Request["ds_requerido"];
                requeridoOv = new RequeridoOV();

                requeridoOv.nm_requerido = _nm_requerido;
                requeridoOv.ds_requerido = _ds_requerido;

                requeridoOv.nm_login_usuario_cadastro = sessao_usuario.nm_login_usuario;
                requeridoOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                var id_doc = new RequeridoRN().Incluir(requeridoOv);
                if (id_doc > 0)
                {
                    sRetorno = "{\"id_doc_success\":" + id_doc + "}";
                }
                else
                {
                    throw new Exception("Erro ao incluir registro.");
                }
                var log_incluir = new LogIncluir<RequeridoOV>
                {
                    registro = requeridoOv
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_incluir, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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

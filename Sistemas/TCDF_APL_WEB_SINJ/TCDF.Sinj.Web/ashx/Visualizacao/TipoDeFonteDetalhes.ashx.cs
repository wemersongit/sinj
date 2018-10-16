using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.OV;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Visualizacao
{
    /// <summary>
    /// Summary description for TipoDeFonteDetalhes
    /// </summary>
    public class TipoDeFonteDetalhes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var _id_doc = context.Request["id_doc"];
            var _ch_tipo_fonte = context.Request["ch_tipo_fonte"];
            
            ulong id_doc = 0;
            var tipoDeFonteRn = new TipoDeFonteRN();
            TipoDeFonteOV tipoDeFonteOv = null;
            var action = AcoesDoUsuario.tdf_vis;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                if (ulong.TryParse(_id_doc, out id_doc))
                {
                    tipoDeFonteOv = tipoDeFonteRn.Doc(id_doc);
                }
                else if (!string.IsNullOrEmpty(_ch_tipo_fonte))
                {
                    Util.rejeitarInject(_ch_tipo_fonte);
                    tipoDeFonteOv = tipoDeFonteRn.Doc(_ch_tipo_fonte);
                }
                else
                {
                    throw new ParametroInvalidoException("Não foi passado parametro para a busca.");
                }
                if (tipoDeFonteOv != null)
                {
                    sRetorno = JSON.Serialize<TipoDeFonteOV>(tipoDeFonteOv);
                }
                else
                {
                    sRetorno = "{\"error_message\":\"registro não encontrado.\"}";
                }
                var log_visualizar = new LogVisualizar
                {
                    id_doc = id_doc,
                    ch_doc = _ch_tipo_fonte
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_visualizar, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocNotFoundException || ex is SessionExpiredException)
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
            context.Response.ContentType = "application/json";
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

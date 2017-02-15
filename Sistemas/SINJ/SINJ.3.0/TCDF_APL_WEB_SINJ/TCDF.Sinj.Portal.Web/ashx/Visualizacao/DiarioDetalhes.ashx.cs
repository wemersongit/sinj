using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Portal.Web.ashx.Visualizacao
{
    /// <summary>
    /// Summary description for DiarioDetalhes
    /// </summary>
    public class DiarioDetalhes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            var sRetorno = "";
            var _id_doc = context.Request["id_doc"];
            var _ch_diario = context.Request["ch_diario"];
            ulong id_doc = 0;
            var diarioRn = new DiarioRN();
            DiarioOV diarioOv = null;
            var action = "PORTAL_DIO.VIS";
            try
            {
                if (ulong.TryParse(_id_doc, out id_doc))
                {
                    diarioOv = diarioRn.Doc(id_doc);
                }
                else if (!string.IsNullOrEmpty(_ch_diario))
                {
                    diarioOv = diarioRn.Doc(_ch_diario);
                }
                else
                {
                    throw new ParametroInvalidoException("Não foi passado parametro para a busca.");
                }
                if (diarioOv != null)
                {
                    sRetorno = JSON.Serialize<DiarioOV>(diarioOv);
                }
                else
                {
                    sRetorno = "{\"error_message\":\"registro não encontrado.\"}";
                }
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocNotFoundException || ex is SessionExpiredException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\", \"id_doc_error\":\"" + _id_doc + "\"}";
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
                LogErro.gravar_erro(action, erro, "visitante", "visitante");
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
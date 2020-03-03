using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Reindexacao
{
    /// <summary>
    /// Summary description for CreateType
    /// </summary>
    public class CreateType : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            try
            {
                var _type = context.Request["type"];
                var _mapping = context.Request["mapping"];
                if (!string.IsNullOrEmpty(_type))
                {
                    if (!string.IsNullOrEmpty(_mapping) && _type.IndexOf("/_mapping") == -1)
                    {
                        _type += "/_mapping";
                    }
                    var retorno_es = new REST(_type, HttpVerb.POST, _mapping).GetResponse();
                    if (retorno_es == "{\"acknowledged\":true}")
                    {
                        sRetorno = "{\"success_message\":\"Type criado com sucesso.\"}";
                    }
                    else
                    {
                        sRetorno = retorno_es;
                    }
                }
            }
            catch (Exception ex)
            {
                sRetorno = "{\"error_message\":\"" + Excecao.LerTodasMensagensDaExcecao(ex, false) + "\"}";
                context.Response.StatusCode = 500;
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

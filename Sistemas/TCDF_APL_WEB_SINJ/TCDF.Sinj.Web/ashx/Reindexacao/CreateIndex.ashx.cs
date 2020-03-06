using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Reindexacao
{
    /// <summary>
    /// Summary description for CreateIndex
    /// </summary>
    public class CreateIndex : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            try
            {
                var _url_index_novo = context.Request["index"];
                var _settings = context.Request["settings"];
                if (!string.IsNullOrEmpty(_url_index_novo))
                {
                    var retorno_es = new REST(_url_index_novo, HttpVerb.POST, _settings).GetResponse();
                    if (retorno_es == "{\"acknowledged\":true}")
                    {
                        sRetorno = "{\"success_message\":\"Index criado com sucesso.\"}";
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

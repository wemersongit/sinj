using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Reindexacao
{
    /// <summary>
    /// Summary description for FollowIndexing
    /// </summary>
    public class FollowIndexing : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            try
            {
                var _url_es_antigo = context.Request["url_es_antigo"];
                var _url_es_novo = context.Request["url_es_novo"];
                if (!string.IsNullOrEmpty(_url_es_antigo) && !string.IsNullOrEmpty(_url_es_novo))
                {

                    var retorno_es_antigo = new REST(_url_es_antigo + "/_count", HttpVerb.GET, "").GetResponse();
                    var retorno_es_novo = new REST(_url_es_novo + "/_count", HttpVerb.GET, "").GetResponse();
                    sRetorno = "{\n\"" + _url_es_antigo + "\":"+retorno_es_antigo+",\n\""+_url_es_novo+"\":"+retorno_es_novo+"\n}";
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

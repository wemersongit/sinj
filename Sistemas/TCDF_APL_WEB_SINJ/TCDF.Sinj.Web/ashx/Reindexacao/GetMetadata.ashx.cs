using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Reindexacao
{
    /// <summary>
    /// Summary description for GetMetadata
    /// </summary>
    public class GetMetadata : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            try
            {
                var _nm_base = context.Request["nm_base_metadata"];
                if (!string.IsNullOrEmpty(_nm_base))
                {
                    sRetorno = new REST(Config.ValorChave("URLBaseREST", true) + "/" + _nm_base + "/metadata", HttpVerb.GET, "").GetResponse();
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

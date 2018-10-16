using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Reindexacao
{
    /// <summary>
    /// Summary description for ResetRest
    /// </summary>
    public class ResetRest : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            List<string> urls_erro = new List<string>();
            int urls_sucesso = 0;
            try
            {
                var _url_rest = context.Request["url_rest"];
                if (string.IsNullOrEmpty(_url_rest))
                {
                    _url_rest = Config.ValorChave("URLBaseREST", true);
                }
                var url_rest_splited = _url_rest.Split(',');
                foreach(var url_rest in url_rest_splited){
                    if (new REST(url_rest + "/_command/reset", HttpVerb.POST, "").GetResponse() == "OK")
                    {
                        urls_sucesso++;
                    }
                    else
                    {
                        urls_erro.Add(url_rest);
                    }
                }
                sRetorno = "{\"success_message\":\"Quantidade de RESTs resetados: " + urls_sucesso + ". RESTs que deram erro: " + JSON.Serialize<List<string>>(urls_erro) + "\"}";

            }
            catch (Exception ex)
            {
                sRetorno = "{\"error_message\":\"" + Excecao.LerTodasMensagensDaExcecao(ex, false) + ".<br/>\"Quantidade de RESTs resetados: " + urls_sucesso + ".<br/> RESTs que deram erro: " + JSON.Serialize<List<string>>(urls_erro) + "\". }";
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using util.BRLight;
using Newtonsoft.Json;

namespace TCDF.Sinj.Web.ashx.Reindexacao
{
    /// <summary>
    /// Summary description for AlterIdxExpUrl
    /// </summary>
    public class AlterIdxExpUrl : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            try
            {
                var _nm_base = context.Request["nm_base"];
                var _idx_exp_url = context.Request["idx_exp_url"];
                if (!string.IsNullOrEmpty(_nm_base))
                {
                    var json_base = new REST(Config.ValorChave("URLBaseREST", true) + "/" + _nm_base, HttpVerb.GET, "").GetResponse();
                    var base_ov = JsonConvert.DeserializeObject<BaseRest>(json_base);
                    base_ov.metadata.idx_exp_url = _idx_exp_url;

                    var retorno_rest = new REST(Config.ValorChave("URLBaseREST", true) + "/" + _nm_base, HttpVerb.PUT, new Dictionary<string, object> { { "json_base", JsonConvert.SerializeObject(base_ov) } }).GetResponse();
                    if (retorno_rest == "UPDATED")
                    {
                        sRetorno = "{\"success_message\":\"idx_exp_url alterado com sucesso.\"}";
                    }
                    else
                    {
                        sRetorno = retorno_rest;
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

        private class BaseRest
        {
            public object[] content { get; set; }
            public Metadata metadata { get; set; }
        }

        private class Metadata
        {
            public bool idx_exp { get; set; }
            public string password { get; set; }
            public string description { get; set; }
            public string color { get; set; }
            public long file_ext_time { get; set; }
            public string dt_base { get; set; }
            public string idx_exp_url { get; set; }
            public bool file_ext { get; set; }
            public long idx_exp_time { get; set; }
            public object model { get; set; }
            public ulong id_base { get; set; }
            public string name { get; set; }
        }
    }
}

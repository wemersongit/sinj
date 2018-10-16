using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using util.BRLight;
using neo.BRLightREST;

namespace TCDF.Sinj.Web.ashx.Autocomplete
{
    /// <summary>
    /// Summary description for ProcuradorAutocomplete
    /// </summary>
    public class ProcuradorAutocomplete : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";

            var _texto = context.Request["texto"];
            var _offset = context.Request["offset"];
            var _limit = context.Request["limit"];

            var query = new Pesquisa();
            string sQuery = "";

            if (_limit != "-1" && !string.IsNullOrEmpty(_limit))
            {
                query.limit = _limit;
                query.offset = _offset;
            }
            if (!string.IsNullOrEmpty(_texto))
            {
                if (_texto != "...")
                {
                    sQuery = "Upper(nm_procurador) like'%" + _texto.ToUpper() + "%'";
                }
            }

            query.literal = sQuery;
            query.order_by.asc = new[] { "nm_procurador" };
            context.Response.Clear();

            try
            {
                sRetorno = new ProcuradorRN().JsonReg(query);
            }
            catch (Exception Ex)
            {
                var retorno = new
                {
                    responseText = Excecao.LerInnerException(Ex, true),
                    statusText = "Erro interno do Servidor!!!",
                    status = 500,
                    url = context.Request.Url.PathAndQuery.ToString(),
                    ErroCallBack = true
                };

                sRetorno = JSON.Serialize<object>(retorno);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Autocomplete
{
    /// <summary>
    /// Summary description for NotifiquemeAutocomplete
    /// </summary>
    public class NotifiquemeAutocomplete : IHttpHandler
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
            else
            {
                query.limit = "30";
            }
            if (!string.IsNullOrEmpty(_texto) && _texto != "...")
            {
                sQuery = "Upper(nm_usuario_push) like'%" + _texto.ToUpper() + "%' OR  Upper(email_usuario_push) like'%" + _texto.ToUpper() + "%'";
            }

            query.literal = sQuery;
            query.order_by.asc = new[] { "nm_usuario_push" };
            context.Response.Clear();

            try
            {
                sRetorno = new NotifiquemeRN().JsonReg(query);
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

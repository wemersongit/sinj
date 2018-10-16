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
    /// Summary description for TipoDePublicacaoAutocomplete
    /// </summary>
    public class TipoDePublicacaoAutocomplete : IHttpHandler
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
            if (!string.IsNullOrEmpty(_texto) && _texto != "...")
            {
                sQuery = "Upper(nm_tipo_publicacao) like'%" + _texto.ToUpper() + "%'";
            }

            query.literal = sQuery;
            query.order_by.asc = new[] { "nm_tipo_publicacao" };
            context.Response.Clear();

            try
            {
                sRetorno = new TipoDePublicacaoRN().JsonReg(query);
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

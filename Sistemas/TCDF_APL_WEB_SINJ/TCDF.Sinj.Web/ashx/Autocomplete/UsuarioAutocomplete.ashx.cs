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
    /// Summary description for UsuarioAutocomplete
    /// </summary>
    public class UsuarioAutocomplete : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";

            var _texto = context.Request["texto"];
            var _offset = context.Request["offset"];
            var _limit = context.Request["limit"];
            var _inativo = context.Request["inativo"];

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
                if (!string.IsNullOrEmpty(_inativo) && _inativo == "true")
                {
                    sQuery = "(Upper(nm_login_usuario) like'%" + _texto.ToUpper() + "%' or Upper(nm_usuario) like'%" + _texto.ToUpper() + "%') and st_usuario=false";
                }
                else
                {
                    sQuery = "(Upper(nm_login_usuario) like'%" + _texto.ToUpper() + "%' or Upper(nm_usuario) like'%" + _texto.ToUpper() + "%') and st_usuario=true";
                }
            }

            query.literal = sQuery;
            query.order_by.asc = new[] { "nm_usuario" };
            context.Response.Clear();

            try
            {
                sRetorno = new UsuarioRN().JsonReg(query);
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

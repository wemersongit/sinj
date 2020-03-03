using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Portal.Web.ashx.Autocomplete
{
    /// <summary>
    /// Summary description for OrgaoAutocomplete
    /// </summary>
    public class OrgaoAutocomplete : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno;


            var _texto = context.Request["texto"];
            var _offset = context.Request["offset"];
            var _limit = context.Request["limit"];
            var _chaves = context.Request["chaves"];
            var _st_orgao = context.Request["st_orgao"];
            var st_orgao = false;

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
            if (!string.IsNullOrEmpty(_texto))
            {
                if (_texto != "...")
                {
                    sQuery = "Upper(nm_orgao) like'%" + _texto.ToUpper() + "%' or Upper(sg_orgao) like'%" + _texto.ToUpper() + "%'";
                }
            }
            if(!string.IsNullOrEmpty(_st_orgao) && bool.TryParse(_st_orgao, out st_orgao)){
                sQuery += (sQuery != "" ? " and " : "") + "st_orgao=" + _st_orgao;
            }
            if (!string.IsNullOrEmpty(_chaves))
            {
                var sQueryChaves = "";
                var chaves = _chaves.Split(',');
                foreach (var chave in chaves)
                {
                    sQueryChaves += (sQueryChaves != "" ? " OR " : "") + "ch_orgao='" + chave + "'";
                }
                sQuery += (sQuery != "" ? " AND " : "") + "(" + sQueryChaves + ")";
            }

            query.literal = sQuery;
            query.order_by.asc = new[] { "nm_orgao" };
            context.Response.Clear();

            try
            {
                var json_reg = new OrgaoRN().JsonReg(query);
                var results = JSON.Deserializa<Results<OrgaoDetalhado>>(json_reg);
                sRetorno = JSON.Serialize<Results<OrgaoDetalhado>>(results);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;

namespace TCDF.Sinj.Portal.Web.ashx.Autocomplete
{
    /// <summary>
    /// Summary description for TipoDeNormaAutocomplete
    /// </summary>
    public class TipoDeNormaAutocomplete : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";

            var _texto = context.Request["texto"];
            var _offset = context.Request["offset"];
            var _limit = context.Request["limit"];
            var _chaves = context.Request["chaves"];

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
                    sQuery = "Upper(nm_tipo_norma) like'%" + _texto.ToUpper() + "%'";
                }
                sQuery += (sQuery != "" ? " AND " : "") + "nm_tipo_norma!='ADO' AND nm_tipo_norma!='AIL' AND nm_tipo_norma!='ADPF' AND nm_tipo_norma!='ADC'";
            }
            if(!string.IsNullOrEmpty(_chaves)){
                var sQueryChaves = "";
                var chaves = _chaves.Split(',');
                foreach (var chave in chaves)
                {
                    sQueryChaves += (sQueryChaves != "" ? " OR " : "") + "ch_tipo_norma='" + chave + "'";
                }
                sQuery += (sQuery != "" ? " AND " : "") + "(" + sQueryChaves + ")";
            }

            query.literal = sQuery;
            query.order_by.asc = new[] { "nm_tipo_norma" };
            context.Response.Clear();

            try
            {
                var sResults = new TipoDeNormaRN().JsonReg(query);
                var oResult = JSON.Deserializa<Results<TipoDeNorma>>(sResults);
                sRetorno = JSON.Serialize<Results<TipoDeNorma>>(oResult);
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

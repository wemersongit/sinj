using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using util.BRLight;

namespace TCDF.Sinj.Portal.Web.ashx.Autocomplete
{
    /// <summary>
    /// Summary description for TipoDeFonteAutocomplete
    /// </summary>
    public class TipoDeFonteAutocomplete : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";

            var _texto = context.Request["texto"];
            var _offset = context.Request["offset"];
            var _limit = context.Request["limit"];
            var _chaves = context.Request["chaves"];
            //Filtrar pelos que estão sendo usados pesquisa no es usando aggregation
            var _filtro_especial = context.Request["filtro_especial"];
            context.Response.Clear();

            try
            {
                sRetorno = AutocompleteTipoDeFonte(_texto, _offset, _limit, _chaves, _filtro_especial);
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

        public string AutocompleteTipoDeFonte(string _texto, string _offset, string _limit, string _chaves, string _filtro_especial)
        {
            var query = new Pesquisa();
            query.limit = null;
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
                    sQuery = "Upper(nm_tipo_fonte) like'%" + _texto.ToUpper() + "%'";
                }
            }
            if (!string.IsNullOrEmpty(_chaves))
            {
                var sQueryChaves = "";
                var chaves = _chaves.Split(',');
                foreach (var chave in chaves)
                {
                    sQueryChaves += (sQueryChaves != "" ? " OR " : "") + "ch_tipo_fonte='" + chave + "'";
                }
                sQuery += (sQuery != "" ? " AND " : "") + "(" + sQueryChaves + ")";
            }

            if(_filtro_especial == "usados"){

            }

            query.literal = sQuery;
            query.order_by.asc = new[] { "nm_tipo_fonte" };

            return new TipoDeFonteRN().JsonReg(query);
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

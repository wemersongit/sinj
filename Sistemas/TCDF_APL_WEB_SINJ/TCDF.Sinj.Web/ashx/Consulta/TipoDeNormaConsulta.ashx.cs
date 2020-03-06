using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Web.ashx.Consulta
{
    /// <summary>
    /// Summary description for TipoDeNormaConsulta
    /// </summary>
    public class TipoDeNormaConsulta : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";

            var query = new Pesquisa();
            query.limit = null;
            query.select = new string[] { "ch_tipo_norma", "nm_tipo_norma" };
            query.order_by.asc = new[] { "nm_tipo_norma" };
            context.Response.Clear();

            try
            {
                var sResult = new TipoDeNormaRN().JsonReg(query);
                var oResult = Newtonsoft.Json.JsonConvert.DeserializeObject<Results<TipoDeNorma>>(sResult);
                sRetorno = Newtonsoft.Json.JsonConvert.SerializeObject(oResult);
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

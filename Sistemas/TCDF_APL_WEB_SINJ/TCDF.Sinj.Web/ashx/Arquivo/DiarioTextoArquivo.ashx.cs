using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Arquivo
{
    /// <summary>
    /// Summary description for DiarioTextoArquivo
    /// </summary>
    public class DiarioTextoArquivo : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var _id_file = context.Request["id_file"];
            var sRetorno = "";
            try
            {
                if (!string.IsNullOrEmpty(_id_file))
                {

                    Util.rejeitarInject(_id_file);
                    var json_doc = new DiarioRN().GetDoc(_id_file);

                    if (json_doc.IndexOf("\"status\": 500") > -1)
                    {
                        throw new Exception("Erro ao obter texto do arquivo.");
                    }
                    sRetorno = json_doc;
                }
            }
            catch (Exception Ex)
            {
                context.Response.Clear();
                sRetorno = "{\"error_message\":\"" + util.BRLight.Excecao.LerInnerException(Ex, true) + "\"}";
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

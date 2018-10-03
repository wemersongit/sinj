using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;

namespace TCDF.Sinj.Web.ashx.Arquivo
{
    /// <summary>
    /// Summary description for NormaTextoArquivo
    /// </summary>
    public class NormaTextoArquivo : IHttpHandler
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
                    var json_doc = new NormaRN().GetDoc(_id_file);

                    if (json_doc.IndexOf("\"status\": 500") > -1)
                    {
                        throw new Exception("Erro ao obter texto do arquivo.");
                    }

                    if (json_doc.IndexOf("\"filetext\": null") > -1)
                    {
                        throw new Exception("O texto do arquivo não foi extraído. Tente mais tarde ou contate o administrador.");
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
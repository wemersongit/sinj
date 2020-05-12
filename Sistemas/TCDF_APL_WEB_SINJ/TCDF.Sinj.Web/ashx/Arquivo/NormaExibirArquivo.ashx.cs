using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.OV;
using System.Text;

namespace TCDF.Sinj.Web.ashx.Arquivo
{
    /// <summary>
    /// Summary description for NormaExibirArquivo
    /// </summary>
    public class NormaExibirArquivo : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var _id_file = context.Request["id_file"];
            var sRetorno = "";
            try
            {
                if (!string.IsNullOrEmpty(_id_file))
                {
                    var normaRn = new NormaRN();
                    Util.rejeitarInject(_id_file);
                    var json_doc = normaRn.GetDoc(_id_file);
                    if (json_doc.IndexOf("\"status\": 500") > -1)
                    {
                        throw new Exception("Erro ao obter arquivo.");
                    }
                    else if (json_doc.IndexOf("\"status\": 404") > -1)
                    {
                        throw new Exception("O arquivo não foi encontrado.");
                    }
                    else if (!string.IsNullOrEmpty(json_doc))
                    {
                        var doc = JSON.Deserializa<ArquivoFullOV>(json_doc);
                        var arquivo = normaRn.Download(_id_file);
                        var sArquivo = Util.FileBytesInUTF8String(arquivo);
                        sArquivo = HttpUtility.UrlEncode(
                                    sArquivo, 
                                    System.Text.Encoding.Default
                                ).Replace("+","%20");
                        sRetorno = "{\"filename\":\"" + doc.filename + "\",\"fileencoded\":\"" + sArquivo + "\"}";
                    }
                    else
                    {
                        throw new Exception("Arquivo não encontrado.");
                    }
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

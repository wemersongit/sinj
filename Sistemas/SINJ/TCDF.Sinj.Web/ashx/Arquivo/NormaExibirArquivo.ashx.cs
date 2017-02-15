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
                        var sArquivo = Encoding.UTF8.GetString(arquivo);
                        if (sArquivo.IndexOf("charset=windows-1252") > -1)
                        {
                            Encoding wind1252 = Encoding.GetEncoding(1252);
                            Encoding utf8 = Encoding.UTF8;
                            byte[] utfBytes = Encoding.Convert(wind1252, utf8, arquivo);
                            sArquivo = utf8.GetString(utfBytes);
                        }
                        sArquivo = HttpUtility.UrlEncode(sArquivo, System.Text.Encoding.Default).Replace("+","%20");
                        //if (sArquivo.Length > 20000)
                        //{
                        //    StringBuilder sb = new StringBuilder();
                        //    int loops = sArquivo.Length / 20000;
                        //    for (int i = 0; i <= loops; i++)
                        //    {
                        //        if (i < loops)
                        //        {
                        //            sb.Append(System.Uri.EscapeDataString(sArquivo.Substring(20000 * i, 20000)));
                        //        }
                        //        else
                        //        {
                        //            sb.Append(System.Uri.EscapeDataString(sArquivo.Substring(20000 * i)));
                        //        }
                        //    }
                        //    sArquivo = sb.ToString();
                        //}
                        //else
                        //{
                        //    sArquivo = System.Uri.EscapeUriString(sArquivo);
                        //}
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
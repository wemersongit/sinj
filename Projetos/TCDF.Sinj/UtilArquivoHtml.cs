using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;
using System.Text.RegularExpressions;
using System.Web;
using util.BRLight;

namespace TCDF.Sinj
{
    public class UtilArquivoHtml
    {
        public string GetHtmlFile(string _id_file, string _nm_base, File docOv)
        {
            var sArquivo = "";
            var docRn = new Doc(_nm_base);
            if (docOv == null)
            {
                docOv = docRn.doc(_id_file);
            }

            if (docOv.id_file != null && docOv.mimetype == "text/html")
            {
                var file = docRn.download(_id_file);
                if (file != null && file.Length > 0)
                {
                    //Encoding wind1252 = Encoding.GetEncoding(1252);
                    //Encoding utf8 = Encoding.UTF8;
                    //byte[] wind1252Bytes = file;
                    //byte[] utfBytes = Encoding.Convert(wind1252, utf8, wind1252Bytes);
                    //sArquivo = utf8.GetString(utfBytes);



                    // Encoding file_encoding = Util.DetectEncoding(file);
                    // Encoding utf8 = Encoding.UTF8;
                    // byte[] file_encoding_bytes = file;
                    // byte[] utfBytes = Encoding.Convert(file_encoding, utf8, file_encoding_bytes);
                    // sArquivo = utf8.GetString(utfBytes);
                    sArquivo = Util.FileBytesInUTF8String(file);


                    // sArquivo = Encoding.UTF8.GetString(file);
                    //o editor de html (ckeditor) coloca o title dento do body autocomaticamente, então as tags e retorno só conteúdo do body, 
                    sArquivo = Regex.Replace(sArquivo, "<html>.*<body>|</body></html>", String.Empty);
                    sArquivo = HttpUtility.HtmlDecode(sArquivo);
                }
                else
                {
                    throw new FileNotFoundException("Arquivo não encontrado.");
                }
            }
            else
            {
                throw new FileNotFoundException("Arquivo não encontrado.");
            }
            return sArquivo;
        }

        public string AnexarHtml(string _arquivo_text, string _filename, string _nm_base)
        {
            string sRetorno = "";

            if (_filename.IndexOf(".htm") < 0 || _filename.IndexOf(".html") < 0)
            {
                _filename += ".html";
            }

            if (_arquivo_text.IndexOf("<head>") < 0)
            {
                _arquivo_text = "<html><head><title>" + _filename.Replace(".html", "") + "</title></head><body>" + _arquivo_text + "</body></html>";
            }

            var arquivo_bytes = System.Text.UnicodeEncoding.UTF8.GetBytes(_arquivo_text);


            var fileParameter = new FileParameter(arquivo_bytes, _filename, "text/html");

            try
            {
                var doc = new Doc(_nm_base);
                var dicionario = new Dictionary<string, object>();
                dicionario.Add("file", fileParameter);
                sRetorno = doc.incluir(dicionario);
                
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("Não foi possível anexar o arquivo", ex);
            }

            return sRetorno;
        }
    }
}

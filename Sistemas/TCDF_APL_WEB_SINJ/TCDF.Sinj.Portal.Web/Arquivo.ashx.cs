using System;
using System.Web;
using System.Web.UI;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.OV;
using System.Text;


namespace TCDF.Sinj.Portal.Web
{

	public class Arquivo : System.Web.IHttpHandler
	{

		public void ProcessRequest (HttpContext context)
		{
			var _id_norma = context.Request ["id_norma_consolidado"];
			NormaOV normaOv = null;

			try
			{
				if (!string.IsNullOrEmpty(_id_norma))
                {
                    Util.rejeitarInject(_id_norma);
					normaOv = new NormaRN().Doc(_id_norma);
					var documento = new NormaRN().Download(normaOv.ar_atualizado.id_file);

					if (documento != null && documento.Length > 0)
					{

//                        Encoding wind1252 = Encoding.GetEncoding(1252);
//                        Encoding utf8 = Encoding.UTF8;
//                        byte[] wind1252Bytes = documento;
//                        byte[] utfBytes = Encoding.Convert(wind1252, utf8, wind1252Bytes);
						//                        var documentoUtf8 = utf8.GetString(utfBytes);
						context.Response.Clear();
						context.Response.ClearHeaders();
						context.Response.ClearContent();
						context.Response.Charset = "ISO-8859-1";
						context.Response.ContentType = normaOv.ar_atualizado.mimetype;
						context.Response.AppendHeader("Content-Length", documento.Length.ToString());
						context.Response.AppendHeader("Content-Disposition", "inline; filename=\"" + normaOv.ar_atualizado.filename + "\"");
						context.Response.BinaryWrite(documento);
						context.Response.Flush();
                    }
                    else
                    {
                        documento = new NormaRN().Download(normaOv.fontes[0].ar_fonte.id_file);
                        if (documento != null && documento.Length > 0)
                        {
                            context.Response.Clear();
                            context.Response.ContentType = normaOv.ar_atualizado.mimetype;
                            context.Response.AppendHeader("Content-Length", documento.Length.ToString());
                            context.Response.AppendHeader("Content-Disposition", "inline; filename=\"" + normaOv.ar_atualizado.filename + "\"");
                            context.Response.BinaryWrite(documento);
                            context.Response.Flush();
                        }
                        else
                        {
                            throw new Exception("Arquivo não encontrado.");
                        }
                    }
				}
            }
            catch (ParamDangerousException)
            {
                context.Response.Clear();
                context.Response.Write("<html><head></head><body><div style=\"color:#990000; width:500px; margin:auto; text-align:center;\">Erro ao obter arquivo.<br/><br/>Nossa equipe resolverá o problema, você pode tentar mais tarde ou entrar em contato conosco.</div></body></html>");
            }
			catch (Exception Ex)
			{
				context.Response.Clear();
                context.Response.Write("<html><head></head><body><div style=\"color:#990000; width:500px; margin:auto; text-align:center;\">" + util.BRLight.Excecao.LerInnerException(Ex, true) + "<br/><br/>Nossa equipe resolverá o problema, você pode tentar mais tarde ou entrar em contato conosco.</div></body></html>");
			}
			context.Response.End();
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}


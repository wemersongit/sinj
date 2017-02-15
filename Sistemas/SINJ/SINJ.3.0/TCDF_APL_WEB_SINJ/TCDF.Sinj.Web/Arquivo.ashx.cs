using System;
using System.Web;
using System.Web.UI;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.OV;


namespace TCDF.Sinj.Web
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
					normaOv = new NormaRN().Doc(_id_norma);
					var documento = new NormaRN().Download(normaOv.ar_atualizado.id_file);
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
			catch (Exception Ex)
			{
				context.Response.Clear();
                context.Response.Write("<html><head></head><body><div style=\"color:#990000; width:500px; margin:auto; text-align:center;\">" + util.BRLight.Excecao.LerInnerException(Ex, true) + "<br/><br/>Nossa equipe resolverá o problema, você pode tentar mais tarde ou entrar em contato conosco.</div></body><html>");
			}
		}

		public bool IsReusable {
			get {
				return false;
			}
		}
	}
}
	

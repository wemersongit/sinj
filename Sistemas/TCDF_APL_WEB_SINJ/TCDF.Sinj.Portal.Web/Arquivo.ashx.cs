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
                    var nm_file = "";

                    if (!string.IsNullOrEmpty(normaOv.ar_atualizado.id_file))
                    {
                        nm_file = normaOv.ar_atualizado.filename;
                    }
                    else
                    {
                        foreach (var fonte in normaOv.fontes)
                        {
                            if (!string.IsNullOrEmpty(fonte.ar_fonte.filename))
                            {
                                nm_file = fonte.ar_fonte.filename;
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(nm_file))
                    {
                        //Redireciona para nova página de downloads da norma
                        context.Response.Redirect("./Norma/" + _id_norma + "/" + nm_file, true);
                    }
                    else
                    {
                        throw new Exception("Arquivo não encontrado.");
                    }
                }
                else
                {
                    throw new Exception("Arquivo não encontrado.");
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


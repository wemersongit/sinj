using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using TCDF.Sinj.Log;
using util.BRLight;
using System.Web.Routing;

namespace TCDF.Sinj.Portal.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterCustomRoutes(RouteTable.Routes);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                var context = HttpContext.Current;
                var exception = HttpContext.Current.Server.GetLastError();

                if (exception != null)
                {
                    var httpException = (HttpException)exception;
                    var errorCode = httpException.GetHttpCode();

                    if (errorCode.ToString() != "404")
                    {
                        var _erro = new ErroNET
                        {
                            Code = errorCode.ToString(),
                            Mensagem = exception.Message.ToString(),
                            Trace = exception.StackTrace.ToString(),
                            Source = exception.Source.ToString(),
                            Pagina = context.Request.Url.ToString()
                        };

                        LogErro.gravar_erro(".NET", _erro, "", "");
                    }

                    if (errorCode.ToString() == "404")
                    {
                        Response.Clear();
                        Server.ClearError();
                        Response.Redirect(Config.ValorChave("Padrao", true) + "/404.html", false);
                    }
                }

                Response.Clear();
                Server.ClearError();

            }
            catch
            {
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        private void RegisterCustomRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("Default", "Default", "~/Default.aspx");
            routes.MapPageRoute("Pesquisas", "Pesquisas.aspx", "~/Default.aspx");

            routes.MapPageRoute("Captcha", "Captcha", "~/captcha.aspx", false, new RouteValueDictionary(), new RouteValueDictionary { { "keywords", ".*" } });
            routes.MapPageRoute("FaleConosco", "FaleConosco", "~/FaleConosco.aspx");
            routes.MapPageRoute("HistoricoDePesquisa", "HistoricoDePesquisa", "~/HistoricoDePesquisa.aspx");
            routes.MapPageRoute("Notifiqueme", "Notifiqueme", "~/Notifiqueme.aspx");
            routes.MapPageRoute("Favoritos", "Favoritos", "~/Favoritos.aspx");

            routes.MapPageRoute("ResultadoDePesquisa", "ResultadoDePesquisa", "~/ResultadoDePesquisa.aspx");

            routes.MapPageRoute("Download", "Download/{*keywords}", "~/Download.aspx", false, new RouteValueDictionary(), new RouteValueDictionary { { "keywords", ".*" } });
            routes.MapPageRoute("Norma", "Norma/{*keywords}", "~/Norma.aspx", false, new RouteValueDictionary(), new RouteValueDictionary { { "keywords", ".*" } });
            routes.MapPageRoute("Diario", "Diario/{*keywords}", "~/Diario.aspx", false, new RouteValueDictionary(), new RouteValueDictionary { { "keywords", ".*" } });
        }
    }
}
using System;
using System.Web;
using System.Web.SessionState;
using System.Threading;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;
using System.Linq;
using TCDF.Sinj.Log;
using System.Web.Routing;

namespace TCDF.Sinj.Web
{
    public class Global : HttpApplication, IRequiresSessionState
    {
		void Application_Start(object sender, EventArgs e)
        {
            RegisterCustomRoutes(RouteTable.Routes);
		    new Thread(ClearLog).Start();
		}

        public void ClearLog()
        {
            try
            {
                // data de hoje menos(-) dez dias....
                var _dt = DateTime.Now.AddDays(-100).ToString("dd'/'MM'/'yyyy HH:mm:ss");
                var query = new Pesquisa { literal = string.Format("CAST(dt_log_erro AS DATE) < '{0}'", _dt) };

                var olog_erro = new Reg("sinj_log_erro");
                olog_erro.excluir(query);
                olog_erro = null;
            }
            catch
            {

            }
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

        private void RegisterCustomRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("Default", "Default", "~/Default.aspx");
            routes.MapPageRoute("Pesquisas", "Pesquisas.aspx", "~/Default.aspx");


            routes.MapPageRoute("FaleConosco", "FaleConosco", "~/FaleConosco.aspx");
            routes.MapPageRoute("Contatos", "Contatos", "~/Contatos.aspx");
            routes.MapPageRoute("HistoricoDePesquisa", "HistoricoDePesquisa", "~/HistoricoDePesquisa.aspx");
            routes.MapPageRoute("Notifiqueme", "Notifiqueme", "~/Notifiqueme.aspx");
            routes.MapPageRoute("Favoritos", "Favoritos", "~/Favoritos.aspx");
            
            routes.MapPageRoute("ResultadoDePesquisaPublicacao", "ResultadoDePesquisaPublicacao", "~/ResultadoDePesquisaPublicacao.aspx");

            routes.MapPageRoute("ResultadoDePesquisa", "ResultadoDePesquisa", "~/ResultadoDePesquisa.aspx");

            //routes.MapPageRoute("dwn", "dwn/{*keywords}", "~/dwn.aspx", false, new RouteValueDictionary(), new RouteValueDictionary { { "keywords", ".*" } });
            routes.MapPageRoute("Download", "Download/{*keywords}", "~/Download.aspx", false, new RouteValueDictionary(), new RouteValueDictionary { { "keywords", ".*" } });
            routes.MapPageRoute("Norma", "Norma/{*keywords}", "~/Norma.aspx", false, new RouteValueDictionary(), new RouteValueDictionary { { "keywords", ".*" } });
            routes.MapPageRoute("Diario", "Diario/{*keywords}", "~/Diario.aspx", false, new RouteValueDictionary(), new RouteValueDictionary { { "keywords", ".*" } });

            //routes.MapPageRoute("Htmltopdf", "Htmltopdf/{*keywords}", "~/Htmltopdf.aspx", false, new RouteValueDictionary(), new RouteValueDictionary { { "keywords", ".*" } });
        }
    }
}

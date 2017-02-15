using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web
{
    public partial class ResultadoDePesquisaOrgao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Util.ValidarAcessoNasPaginas(base.Page, AcoesDoUsuario.org_pes);
            //try
            //{
            //    Util.ValidarUsuario(action);
            //}
            //catch (SessionExpiredException)
            //{
            //    Response.Redirect("~/Login.aspx?cd=1");
            //}
            //catch (PermissionException)
            //{
            //    Response.Redirect("~/Erro.aspx?cd=0");
            //}
            //catch (Exception ex)
            //{
            //    Response.Redirect("~/Erro.aspx");
            //    var erro = new ErroRequest
            //    {
            //        Pagina = Request.Path,
            //        RequestQueryString = Request.QueryString,
            //        MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
            //        StackTrace = ex.StackTrace
            //    };
            //    LogErro.gravar_erro(Util.GetEnumDescription(action), erro);
            //}
        }
    }
}
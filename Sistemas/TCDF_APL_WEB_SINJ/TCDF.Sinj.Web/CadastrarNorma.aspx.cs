using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.Log;
using util.BRLight;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;

namespace TCDF.Sinj.Web
{
    public partial class CadastrarNorma : System.Web.UI.Page
    {
        protected bool isAdmin;
        protected string situacoes;
        protected void Page_Load(object sender, EventArgs e)
        {
            var action = AcoesDoUsuario.nor_inc;
            try
            {
                Util.ValidarUsuario(Sinj.oSessaoUsuario, action);
                isAdmin = Util.IsSuperAdmin(Sinj.oSessaoUsuario);
                if (isAdmin)
                {
                    situacoes = Util.GetSituacoes();
                }

            }
            catch (SessionExpiredException)
            {
                Response.Redirect("./Login.aspx?cd=1",true);
            }
            catch (PermissionException)
            {
                Response.Redirect("./PesquisarNorma.aspx",true);
            }
            catch (Exception ex)
            {
                Response.Redirect("./Erro.aspx",true);
                var erro = new ErroRequest
                {
                    Pagina = Request.Path,
                    RequestQueryString = Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                if (Sinj.oSessaoUsuario != null)
                {
                    LogErro.gravar_erro(Util.GetEnumDescription(action), erro, Sinj.oSessaoUsuario.nm_usuario, Sinj.oSessaoUsuario.nm_login_usuario);
                }
            }
        }
    }
}
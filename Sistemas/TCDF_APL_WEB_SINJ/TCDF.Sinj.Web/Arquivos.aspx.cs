using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.OV;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Web
{
    public partial class Arquivos : System.Web.UI.Page
    {
        protected string _diretorio { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            var action = AcoesDoUsuario.arq_pro;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
            }
            catch (SessionExpiredException)
            {
                Response.Redirect("./Login.aspx?cd=1", true);
            }
            catch (Exception ex)
            {
                Response.Redirect("./Erro.aspx", true);
                var erro = new ErroRequest
                {
                    Pagina = Request.Path,
                    RequestQueryString = Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                if (sessao_usuario != null)
                {
                    LogErro.gravar_erro(Util.GetEnumDescription(action), erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
            }

            _diretorio = Request["diretorio"];
            if(string.IsNullOrEmpty(_diretorio)){
                _diretorio = "arquivos_orgao_cadastrador";
            }
        }
    }
}

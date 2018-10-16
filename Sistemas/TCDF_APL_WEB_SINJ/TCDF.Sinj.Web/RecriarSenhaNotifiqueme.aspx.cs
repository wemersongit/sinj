using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using neo.BRLightREST;
using util.BRLight;
using System.Threading;

namespace TCDF.Sinj.Web
{
    public partial class RecriarSenhaNotifiqueme : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var _ch_token = Request["recriar"];
            var resultado_acao = Request["ra"];
            if (!string.IsNullOrEmpty(_ch_token))
            {
                var ch_token = _ch_token.Substring(_ch_token.IndexOf('A')+1);
                var token = new Token().Doc(ch_token);
                if (token != null)
                {
                    var itime = Convert.ToInt32(Config.ValorChave("IntMillisecondsTokenRecriarSenhaPush"));
                    if (Convert.ToDateTime(token._metadata.dt_doc).AddMilliseconds(itime) >= DateTime.Now)
                    {
                        div_form_nova_senha.Visible = true;
                        div_form_recriar_senha.Visible = false;
                    }
                    else
                    {
                        div_form_nova_senha.Visible = false;
                        div_form_recriar_senha.Visible = true;
                        div_info_recriar_senha.InnerHtml = "<span class='alert'>A solicitação para recriar senha expirou.</span><br/>Informe novamente seu email para confirmação.";
                    }
                }
                else
                {
                    div_form_recriar_senha.Visible = true;
                    div_form_nova_senha.Visible = false;
                }
            }
            else if (!string.IsNullOrEmpty(resultado_acao))
            {
                div_form_recriar_senha.Visible = false;
                div_form_nova_senha.Visible = true;
                div_form_nova_senha.InnerHtml = "<span class='success'>Enviamos um link para seu e-mail.</span>";
            }
            else
            {
                div_form_recriar_senha.Visible = true;
                div_form_nova_senha.Visible = false;
            }
            try
            {
                new Token().Delete("sinj", "recriar_senha_push", Convert.ToInt32(Config.ValorChave("IntMillisecondsTokenRecriarSenhaPush")));
            }
            catch (Exception ex)
            {
                new Log.Erro { MensagemDaExcecao = "Erro ao deletar tokens recriar_senha_push. " + Excecao.LerTodasMensagensDaExcecao(ex, false), StackTrace = ex.StackTrace };
            }
        }
    }
}

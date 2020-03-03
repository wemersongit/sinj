using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Email
{
    /// <summary>
    /// Summary description for FaleConoscoEnviarEmail
    /// </summary>
    public class FaleConoscoEnviarEmail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";


            var mensagem = new FaleConoscoMensagemResposta();
            mensagem.ds_assunto_resposta = context.Request["assunto"];
            mensagem.ds_msg_resposta = context.Request["mensagem"];

            var _ch_chamado = context.Request["ch_chamado"];

            var sAction = "FLC.EMAIL";
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                var faleConoscoRn = new FaleConoscoRN();

                Util.rejeitarInject(_ch_chamado);
                var faleConosco = faleConoscoRn.Doc(_ch_chamado);

                var emails = new string[] { faleConosco.ds_email };

                var link = util.BRLight.Util.GetVariavel("URLSinjPortal", true) + "/?ch_chamado_fale_conosco=" + faleConosco.ch_chamado + "&ds_email_fale_conosco=" + faleConosco.ds_email + "&nm_user_fale_conosco=" + faleConosco.nm_user + "&ds_assunto_fale_conosco=" + faleConosco.ds_assunto;

                var sHtml = "<br/><br/>Para responder essa mensagem, <a target='_blank' href='" + link + "'>clique aqui</a>";
                var message = faleConosco.mensagens;
                if (faleConosco.mensagens.Count > 0)
                {
                    sHtml += "<br/><br/><h3>Mensagens anteriores</h3>";
                    foreach (var msg in faleConosco.mensagens)
                    {
                        if (!string.IsNullOrEmpty(msg.nm_login_usuario_resposta))
                        {
                            sHtml += "<br/>Em <b>" + msg.dt_resposta + "</b>, <b>" + msg.nm_usuario_resposta + "</b> escreveu:";
                            sHtml += "<br/>" + msg.ds_msg_resposta;
                            sHtml += "<br/>";
                        }
                        else
                        {
                            sHtml += "<br/>Em <b>" + msg.dt_resposta + "</b>, <b>" + faleConosco.nm_user + "</b> escreveu:";
                            sHtml += "<br/>" + msg.ds_msg_resposta;
                            sHtml += "<br/>";
                        }
                    }
                }


                sRetorno = new EnviarEmail().EnviarEmails(emails, mensagem.ds_assunto_resposta, true, mensagem.ds_msg_resposta + sHtml);

                mensagem.dt_resposta = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                mensagem.nm_login_usuario_resposta = sessao_usuario.nm_login_usuario;
                mensagem.nm_usuario_resposta = sessao_usuario.nm_usuario;

                faleConosco.mensagens.Add(mensagem);

                faleConoscoRn.Atualizar(faleConosco._metadata.id_doc, faleConosco);

            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocValidacaoException || ex is SessionExpiredException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\"}";
                }
                else
                {
                    sRetorno = Excecao.LerTodasMensagensDaExcecao(ex, false);
                    context.Response.StatusCode = 500;
                }
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                if (sessao_usuario != null)
                {
                    LogErro.gravar_erro(sAction, erro, sessao_usuario.nm_usuario, sessao_usuario.email_usuario);
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(sRetorno);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

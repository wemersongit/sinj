using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace util.BRLight
{
    public class ParamsEmails
    {

        /// <summary>
        /// Endereço do servidor de e-mail
        /// </summary>
        public string servidor;
        /// <summary>
        /// Porta do servidor de e-mail
        /// </summary>
        public int porta;
        /// <summary>
        /// Se possui autenticação SSL
        /// </summary>
        public bool bSsl;
        /// <summary>
        /// Se possui credenciais
        /// </summary>
        public bool bCredenciais;
        /// <summary>
        /// Senha da conta de e-mail
        /// </summary>
        public string senha;
        
        /// <summary>
        /// E-mail do remetente
        /// </summary>
        public string ds_email_remetente;
        /// <summary>
        /// Nome do remetente
        /// </summary>
        public string nm_remetente;
        /// <summary>
        /// E-mails dos destinatários
        /// </summary>
        public string[] destinatarios;
        /// <summary>
        /// Titulo do assunto
        /// </summary>
        public string assunto;
        /// <summary>
        /// Corpo da mensagem do e-mail
        /// </summary>
        public string corpo;
        /// <summary>
        /// Se possui html na mensagem
        /// </summary>
        public bool bHtml;
    }

    public class Emails
    {
        public void EnviarEmail(ParamsEmails paramsEmail)
        {
            MailAddress from = new MailAddress(paramsEmail.ds_email_remetente, paramsEmail.nm_remetente);
            MailMessage mensagem = new MailMessage();
            mensagem.From = from;
            foreach (var destinario in paramsEmail.destinatarios)
            {
                mensagem.Bcc.Add(new MailAddress(destinario));
            }
            mensagem.Subject = paramsEmail.assunto;
            mensagem.IsBodyHtml = paramsEmail.bHtml;
            mensagem.Body = paramsEmail.corpo;

            SmtpClient smtp = new SmtpClient(paramsEmail.servidor, paramsEmail.porta);
            if (paramsEmail.bCredenciais)
            {
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(paramsEmail.ds_email_remetente, paramsEmail.senha);
            }
            smtp.EnableSsl = Convert.ToBoolean(paramsEmail.bSsl);
            try
            {
                smtp.Send(mensagem);
            }
            catch (SmtpException)
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                smtp.Send(mensagem);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;
using System.Net.Mail; 
using System.Runtime.CompilerServices; 
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;

namespace TCDF.Sinj
{
	public class EmailRN
	{
		public EmailRN ()
		{
		}

		public void EnviaEmail(string destinatario, string nova_senha)
		{
			MailAddress from = new MailAddress ("monitoracao.sinj@gmail.com", "Recuperação de Senha SINJ");
			MailAddress to = new MailAddress (destinatario);
			MailMessage mensagem = new MailMessage (from, to);
			mensagem.Subject = "Notifique-me: Recuperação de Senha";
			mensagem.IsBodyHtml = true;
			mensagem.Body = "Uma nova senha foi gerada. <br/> Faça seu login usando a senha: " + nova_senha;
			
			SmtpClient smtp = new SmtpClient (Config.ValorChave("NotifyEmailServer",true), Convert.ToInt32(Config.ValorChave("NotifyEmailPort", true)));
			if (Config.ValorChave("NotifyEmailCredentials", true) == "true"){
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(Config.ValorChave("NotifyEmailAccount", true), Config.ValorChave("NotifyEmailPassword", true));
			}
			smtp.EnableSsl = Convert.ToBoolean(Config.ValorChave("NotifyEmailSsl", true));

            try
            {
                smtp.Send(mensagem);
            }
            catch (SmtpException)
            {
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors){ return true; };
                smtp.Send(mensagem);
            }
		}

		public void EnviaEmail(string display_name_remetente, string[] destinatarios, string titulo, bool html, string corpo)
		{
            MailAddress from = new MailAddress(Config.ValorChave("NotifyEmailAccount", true).ToString(), display_name_remetente);
			MailMessage mensagem = new MailMessage ();
			mensagem.From = from;
			foreach (var destinario in destinatarios){
                mensagem.Bcc.Add(new MailAddress (destinario));
			}
			mensagem.Subject = titulo;
			mensagem.IsBodyHtml = html;
			mensagem.Body = corpo;

			SmtpClient smtp = new SmtpClient (Config.ValorChave("NotifyEmailServer",true), Convert.ToInt32(Config.ValorChave("NotifyEmailPort", true)));
			if (Config.ValorChave("NotifyEmailCredentials", true) == "true")
			{
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(Config.ValorChave("NotifyEmailAccount", true), Config.ValorChave("NotifyEmailPassword", true));
			}
			smtp.EnableSsl = Convert.ToBoolean(Config.ValorChave("NotifyEmailSsl", true));
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


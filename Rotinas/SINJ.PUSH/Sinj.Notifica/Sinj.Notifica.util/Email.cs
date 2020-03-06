using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Sinj.Notifica.util
{
    public class Email
    {

        /// <summary>
        /// Envia E-mails através de um servidor SMTP
        /// </summary>
        /// <param name="mensagem">corpo do e-mail</param>
        /// <param name="assunto">assunto do e-mail</param>
        /// <param name="emailRemetente">e-mail usado para o envio da mensagem</param>
        /// <param name="nomeRemetente">nome que será exibido ao receptor do e-mail</param>
        /// <param name="emailDestinatario">e-mail do destinatário</param>
        /// <param name="servidorSmtp">um domínio de servidor SMTP</param>
        /// <param name="portaDeEmail">uma porta usada para a comunicação SMTP</param>
        /// <param name="senha">a senha da conta de e-mail usada para o envio do e-mail</param>
        /// <param name="ssl">Indica o uso, ou não, de conexão SSL</param>
        /// <param name="credenciais">Indica o uso, ou não, de autenticação</param>
        public static void EnviaEmailSmtp(string mensagem, string assunto, string emailRemetente, string nomeRemetente, string emailDestinatario, string servidorSmtp, int portaDeEmail, string senha, bool ssl, bool credenciais)
        {
            try
            {
                MailAddress from = new MailAddress(emailRemetente, nomeRemetente);
                MailAddress to = new MailAddress(emailDestinatario);
                MailMessage msg = new MailMessage(from, to)
                                      {
                                          Subject = assunto,
                                          SubjectEncoding = Encoding.GetEncoding("ISO-8859-1"),
                                          Body = mensagem,
                                          BodyEncoding = Encoding.GetEncoding("ISO-8859-1"),
                                          IsBodyHtml = true,
                                          Priority = MailPriority.High
                                      };

                SmtpClient smtp = new SmtpClient(servidorSmtp, portaDeEmail);
                smtp.EnableSsl = ssl;
                if(credenciais)
                {
                    smtp.Credentials = new NetworkCredential(emailRemetente, senha);
                }
                try
                {
                    smtp.Send(msg);
                }
                catch (SmtpFailedRecipientException ex)
                {
                    throw new Exception("Houve uma falha no Recipiente SMTP.", ex);
                }
                catch (SmtpException ex)
                {
                    throw new Exception("Houve um erro SMTP.", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("Houve um erro em smtp.Send(msg);", ex);
                }
                finally
                {
                    msg.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Houve um erro em SendSsl.", ex);
            }
        }
    }
}

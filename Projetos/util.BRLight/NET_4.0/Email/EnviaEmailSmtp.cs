using System;
using System.Net.Mail;

namespace util.BRLight.Email
{
    /// <summary>
    /// Classe respons�vel por enviar e-mails via SMTP.
    /// </summary>
    public sealed class EnviaEmailSmtp : EnviaEmail
    {
        /// <summary>
        /// Endere�o do servidor SMTP.
        /// </summary>
        public string Smtp { get; set; }

        /// <summary>
        /// Construtor padr�o da classe EnviaEmailSmtp.
        /// </summary>
        public EnviaEmailSmtp() { }

        /// <summary>
        /// Construtor da classe EnviaEmailSmtp.
        /// </summary>
        /// <param name="smtp">Endere�o do servidor SMTP.</param>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        public EnviaEmailSmtp(string smtp)
        {
            // Verifica se o endere�o do servidor SMTP foi informado.
            if (string.IsNullOrEmpty(smtp))
                throw new ArgumentException("O endere�o do servidor SMTP n�o pode ser nulo ou vazio.", "smtp");

            Smtp = smtp;
        }

        /// <summary>
        /// M�todo para envio de e-mails complexos (e-mails que contenham anexos, c�pia, c�pia oculta, m�ltiplos destinat�rios, etc).
        /// </summary>
        /// <param name="email">Email a ser enviado.</param>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">System.ArgumentOutOfRangeException</exception>
        /// <exception cref="System.FormatException">System.FormatException</exception>
        /// <exception cref="System.InvalidOperationException">System.InvalidOperationException</exception>
        /// <exception cref="System.ObjectDisposedException">System.ObjectDisposedException</exception>
        /// <exception cref="System.Net.Mail.SmtpException">System.Net.Mail.SmtpException</exception>
        /// <exception cref="System.Net.Mail.SmtpFailedRecipientsException">System.Net.Mail.SmtpFailedRecipientsException</exception>
        public override void Enviar(Email email)
        {
            // Verifica se um objeto nulo foi passado como par�metro.
            if (email == null)
                throw new ArgumentNullException("email");

            // Valida os par�metros obrigat�rios do e-mail.
            if (email.Destinatarios.Length == 0)
                throw new ArgumentException("O e-mail do destinat�rio n�o foi informado.", "email");

            if (string.IsNullOrEmpty(email.EmailRemetente))
                throw new ArgumentException("O e-mail n�o possui remetente.", "email");

            // Cria o objeto da mensagem de e-mail.
            MailMessage mensagemEmail = new MailMessage();

            // Carrega os arquivos anexos ao e-mail.
            for (int i = 0; i < email.Anexos.Length; i++)
            {
                mensagemEmail.Attachments.Add(new Attachment(email.Anexos[i]));
            }

            // Separador utilizado quando um e-mail � enviado para mais de um destinat�rio.
            const string SEPARADOR_EMAILS = ",";

            if (email.Destinatarios.Length > 0)
            {
                // Adiciona os destinat�rios que receber�o o e-mail.
                mensagemEmail.To.Add(base.FormatarListaDestinatarios(email.Destinatarios, SEPARADOR_EMAILS));
            }

            if (email.Cc.Length > 0)
            {
                // Adiciona os destinat�rios que receber�o a c�pia do e-mail.
                mensagemEmail.CC.Add(base.FormatarListaDestinatarios(email.Cc, SEPARADOR_EMAILS));
            }

            if (email.Cco.Length > 0)
            {
                // Adiciona os destinat�rios que receber�o a c�pia oculta do e-mail.
                mensagemEmail.Bcc.Add(base.FormatarListaDestinatarios(email.Cco, SEPARADOR_EMAILS));
            }

            // Define a mensagem do e-mail.
            mensagemEmail.Body = email.Mensagem;

            // Define se o corpo da mensagem cont�m formata��o HTML, para que o conte�do do e-mail 
            // seja visualizado corretamente pelos destinat�rios.
            mensagemEmail.IsBodyHtml = email.Html;

            // Define a prioridade do e-mail.
            mensagemEmail.Priority = email.Prioridade;

            // Define o e-mail e o nome de exibi��o do remetente.
            mensagemEmail.From = new MailAddress(email.EmailRemetente);

            // Define o assunto do e-mail.
            mensagemEmail.Subject = email.Titulo;

            if (!string.IsNullOrEmpty(email.EmailResposta))
            {
                // Define o e-mail para o qual a mensagem a ser enviada deve ser respondida.
                mensagemEmail.ReplyToList.Add(new MailAddress(email.EmailResposta));
            }
            
            try
            {
                // Envia o e-mail.
                this.EnviarEmail(ref mensagemEmail);
            }
            finally
            {
                // Libera os recursos utilizados pelo m�todo.
                mensagemEmail.Dispose();
                mensagemEmail = null;
            }
        }

        /// <summary>
        /// M�todo para envio de e-mails simples (formul�rios de contato, por exemplo).
        /// </summary>
        /// <param name="emailRemetente">E-mail do remetente.</param>
        /// <param name="emailDestinatario">E-mail do destinat�rio.</param>
        /// <param name="assunto">Assunto do e-mail.</param>
        /// <param name="mensagem">Mensagem do e-mail.</param>
        /// <param name="html">Habilita formata��o HTML na mensagem do e-mail.</param>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <exception cref="System.InvalidOperationException">System.InvalidOperationException</exception>
        /// <exception cref="System.ObjectDisposedException">System.ObjectDisposedException</exception>
        /// <exception cref="System.Net.Mail.SmtpException">System.Net.Mail.SmtpException</exception>
        /// <exception cref="System.Net.Mail.SmtpFailedRecipientsException">System.Net.Mail.SmtpFailedRecipientsException</exception>
        public override void Enviar(string emailRemetente, string emailDestinatario, string assunto, string mensagem, bool html)
        {
            // Valida os par�metros obrigat�rios do e-mail.
            if (string.IsNullOrEmpty(emailDestinatario))
                throw new ArgumentException("O e-mail do destinat�rio n�o foi informado.", "emailDestinatario");

            if (string.IsNullOrEmpty(emailRemetente))
                throw new ArgumentException("O e-mail n�o possui remetente.", "emailRemetente");

            // Cria o objeto da mensagem de e-mail.
            MailMessage mensagemEmail = new MailMessage();

            // Define a mensagem do e-mail.
            mensagemEmail.Body = mensagem;

            // Define o e-mail do remetente.
            mensagemEmail.From = new MailAddress(emailRemetente);

            // Define se o corpo da mensagem cont�m formata��o HTML, para que o conte�do do e-mail 
            // seja visualizado corretamente pelos destinat�rios.
            mensagemEmail.IsBodyHtml = html;

            // Define o assunto do e-mail.
            mensagemEmail.Subject = assunto;

            // Define o destinat�rio do e-mail.
            mensagemEmail.To.Add(emailDestinatario);

            try
            {
                // Envia o e-mail.
                this.EnviarEmail(ref mensagemEmail);
            }
            finally
            {
                // Libera os recursos utilizados pelo m�todo.
                mensagemEmail.Dispose();
                mensagemEmail = null;
            }
        }

        /// <summary>
        /// Envia o e-mail para o servidor SMTP.
        /// </summary>
        /// <param name="email">Email a ser enviado.</param>
        /// <exception cref="System.InvalidOperationException">System.InvalidOperationException</exception>
        /// <exception cref="System.ObjectDisposedException">System.ObjectDisposedException</exception>
        /// <exception cref="System.Net.Mail.SmtpException">System.Net.Mail.SmtpException</exception>
        /// <exception cref="System.Net.Mail.SmtpFailedRecipientsException">System.Net.Mail.SmtpFailedRecipientsException</exception>
        private void EnviarEmail(ref MailMessage email)
        {
            // Antes de tentar enviar o e-mail, verifica se o endere�o do servidor SMTP foi informado.
            if (string.IsNullOrEmpty(this.Smtp))
                throw new InvalidOperationException("O endere�o do servidor SMTP n�o foi informado.");

            // Cria e configura objeto SmtpClient.
            SmtpClient enviaEmailSMTP = new SmtpClient();
            enviaEmailSMTP.Host = this.Smtp;

            try
            {
                // Envia o e-mail.
                enviaEmailSMTP.Send(email);
            }
            finally
            {
                // Libera os recursos do sistema.
                enviaEmailSMTP = null;
            }
        }
    }
}
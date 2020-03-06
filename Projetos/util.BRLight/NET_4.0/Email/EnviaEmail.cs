using System.Text;

namespace util.BRLight.Email
{
    /// <summary>
    /// Classe base para envio de e-mails.
    /// </summary>
    public abstract class EnviaEmail
    {
        /// <summary>
        /// Método para envio de e-mails complexos (e-mails que contenham anexos, cópia, cópia oculta, múltiplos destinatários, etc).
        /// </summary>
        /// <param name="email">Email a ser enviado.</param>
        public abstract void Enviar(Email email);

        /// <summary>
        /// Método para envio de e-mails simples (formulários de contato, por exemplo).
        /// </summary>
        /// <param name="emailRemetente">E-mail do remetente.</param>
        /// <param name="emailDestinatario">E-mail do destinatário.</param>
        /// <param name="assunto">Assunto do e-mail.</param>
        /// <param name="mensagem">Mensagem do e-mail.</param>
        /// <param name="html">Habilita formatação HTML na mensagem do e-mail.</param>
        public abstract void Enviar(string emailRemetente, string emailDestinatario, string assunto, string mensagem, bool html);

        /// <summary>
        /// Formata uma lista de e-mails de destinatários para um formato aceitável 
        /// para utilização nas classes de envio de e-mail.
        /// </summary>
        /// <param name="emailDestinatarios">Lista com e-mails de destinatários.</param>
        /// <param name="separador">Caractere separador dos e-mails na lista.</param>
        /// <returns>
        /// Lista de e-mails de destinatários em um formato aceitável 
        /// para utilização nas classes de envio de e-mail.
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">System.ArgumentOutOfRangeException</exception>
        protected string FormatarListaDestinatarios(string[] emailDestinatarios, string separador)
        {
            // Lista com os e-mails de destinatários em um formato aceitável 
            // para utilização nas classes de envio de e-mail.
            StringBuilder listaEmailDestinatarios = new StringBuilder();

            // Para cada e-mail...
            for (int i = 0; i < emailDestinatarios.Length; i++)
            {
                // adiciona na nova lista...
                listaEmailDestinatarios.Append(emailDestinatarios[i]);
                // e adiciona o separador correto entre os e-mails.
                listaEmailDestinatarios.Append(separador);
            }

            // Retorna a lista de destinatários formatada corretamente, 
            // com o separador utilizado nas classes de envio de e-mail.
            return listaEmailDestinatarios.ToString();
        }
    }
}
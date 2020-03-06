using System;
using System.Net.Mail;
using System.Collections.Generic;

namespace util.BRLight.Email
{
    /// <summary>
    /// Classe que representa a mensagem de e-mail a ser enviada.
    /// </summary>
    public class Email : IDisposable
    {
        /// <summary>
        /// Lista de arquivos anexos da mensagem de e-mail.
        /// </summary>
        private List<string> anexos = new List<string>();
        
        /// <summary>
        /// Lista de e-mails dos destinat�rios.
        /// </summary>
        private List<string> destinatarios = new List<string>();
        
        /// <summary>
        /// Lista de e-mails dos destinat�rios que receber�o a c�pia do e-mail.
        /// </summary>
        private List<string> cc = new List<string>();

        /// <summary>
        /// Lista de e-mails dos destinat�rios que receber�o a c�pia oculta do e-mail.
        /// </summary>
        private List<string> cco = new List<string>();

        #region Propriedades

        /// <summary>
        /// E-mail do(s) destinat�rio(s).
        /// </summary>
        public string[] Destinatarios
        {
            get { return this.destinatarios.ToArray(); }
        }

        /// <summary>
        /// E-mail dos destinat�rio(s) que receber�(�o) a c�pia do e-mail.
        /// </summary>
        public string[] Cc
        {
            get { return this.cc.ToArray(); }
        }

        /// <summary>
        /// E-mail dos destinat�rio(s) que receber�(�o) a c�pia oculta do e-mail.
        /// </summary>
        public string[] Cco
        {
            get { return this.cco.ToArray(); }
        }

        /// <summary>
        /// Arquivos anexados ao e-mail.
        /// </summary>
        public string[] Anexos 
        {
            get { return this.anexos.ToArray(); }
        }

        /// <summary>
        /// T�tulo do e-mail.
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// Mensagem do e-mail.
        /// </summary>
        public string Mensagem { get; set; }

        /// <summary>
        /// E-mail do remetente.
        /// </summary>
        public string EmailRemetente { get; set; }

        /// <summary>
        /// Habilita formata��o HTML na mensagem do e-mail.
        /// </summary>
        public bool Html { get; set; }

        /// <summary>
        /// Prioridade de envio do e-mail.
        /// </summary>
        public MailPriority Prioridade { get; set; }

        /// <summary>
        /// E-mail para o qual a mensagem deve ser respondida.
        /// </summary>
        public string EmailResposta { get; set; }

        #endregion

        /* 
         * A classe List<> permite que valores nulos sejam adicionados � cole��o, 
         * podendo causar erro mais adiante quando o e-mail for enviado.
         * As classes de envio de e-mail n�o aceitam destinat�rios ou anexos com o valor vazio ou nulo.
         * Por isso, este poss�vel erro deve ser tratado e evitado com m�todos que encapsulam a lista 
         * e fazem o tratamento dos dados que s�o adicionados � lista.
         */

        /// <summary>
        /// Adiciona arquivo em anexo para a lista de arquivos em anexo j� existentes para o atual e-mail.
        /// </summary>
        /// <param name="caminhoArquivoAnexo">Diret�rio do arquivo anexo, incluindo nome do arquivo e extens�o.</param>
        public void AdicionarAnexo(string caminhoArquivoAnexo)
        {
            if (!string.IsNullOrEmpty(caminhoArquivoAnexo))
                this.anexos.Add(caminhoArquivoAnexo);
        }

        /// <summary>
        /// Adiciona um destinat�rio que receber� o e-mail.
        /// </summary>
        /// <param name="emailDestinatario">E-mail do destinat�rio que receber� o e-mail.</param>
        public void AdicionarDestinatario(string emailDestinatario)
        {
            if (!string.IsNullOrEmpty(emailDestinatario))
                this.destinatarios.Add(emailDestinatario);
        }

        /// <summary>
        /// Adiciona um destinat�rio que receber� a c�pia do e-mail.
        /// </summary>
        /// <param name="emailDestinatarioCc">E-mail do destinat�rio que receber� a c�pia do e-mail.</param>
        public void AdicionarCc(string emailDestinatarioCc)
        {
            if (!string.IsNullOrEmpty(emailDestinatarioCc))
                this.cc.Add(emailDestinatarioCc);
        }

        /// <summary>
        /// Adiciona um destinat�rio que receber� a c�pia oculta do e-mail.
        /// </summary>
        /// <param name="emailDestinatarioCco">E-mail do destinat�rio que receber� a c�pia oculta do e-mail.</param>
        public void AdicionarCCo(string emailDestinatarioCco)
        {
            if (!string.IsNullOrEmpty(emailDestinatarioCco))
                this.cco.Add(emailDestinatarioCco);
        }

        #region Implementa��o da interface IDisposable.

        /// <summary>
        /// Limpa todos os recursos alocados pela classe.
        /// </summary>
        public void Dispose()
        {
            this.anexos.Clear();
            this.anexos = null;
            this.cc.Clear();
            this.cc = null;
            this.cco.Clear();
            this.cco = null;
            this.destinatarios.Clear();
            this.destinatarios = null;
            this.EmailRemetente = null;
            this.Mensagem = null;
            this.Titulo = null;
        }

        #endregion
    }
}
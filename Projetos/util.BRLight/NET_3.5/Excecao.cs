using System;
using System.Text;

namespace util.BRLight {
    public static class Excecao
    {
        /// <summary>
        /// Lê a mensagem de exceção da última innerException
        /// </summary>
        /// <param name="ex">exceção</param>
        /// <param name="retiraCaracteresEspeciais"> boleano pra retirada dos caracteres especiais</param>
        /// <returns>mensagem de exceção caso exista</returns>
        public static string LerInnerException(Exception ex, bool retiraCaracteresEspeciais)
        {
            var msgInner = string.Empty;
            if (ex != null)
                msgInner = ex.InnerException != null ? LerInnerException(ex.InnerException, retiraCaracteresEspeciais) : ex.Message;
            if (retiraCaracteresEspeciais)
                msgInner = f_retiraCaracteresEspeciais(msgInner);
            return msgInner;
        }

        private static string LerMensagensDaExcecaoRecursivamente(int iExcecao, Exception ex, bool retiraCaracteresEspeciais)
        {
            var msg = string.Empty;
            if (ex != null)
            {
                msg = iExcecao + ". " + ex.Message;
                msg += LerMensagensDaExcecaoRecursivamente(iExcecao++, ex.InnerException, retiraCaracteresEspeciais);
                if (retiraCaracteresEspeciais)
                    msg = f_retiraCaracteresEspeciais(msg);
            }
            return msg;
        }

        public static string LerTodasMensagensDaExcecao(Exception ex, bool retiraCaracteresEspeciais)
        {
            var msg = string.Empty;
            if (ex != null)
            {
                msg = LerMensagensDaExcecaoRecursivamente(1, ex, retiraCaracteresEspeciais);
            }
            return msg;
        }

        public static string f_retiraCaracteresEspeciais(string text) {
                string[] cEspeciais = { "#39", "---", "--", "'", "#", "\r\n", "\n", "\r", "\t", Environment.NewLine };
                for (int q = 0; q < cEspeciais.Length; q++) {
                    text = text.Replace(cEspeciais[q], " -");
                }
 
           return text;
        }

        public static string LerException(Exception ex) {
            var logErro = new StringBuilder();

            logErro.AppendLine("-- Lista de Erros Encontrados --");
            logErro.AppendLine();

            while (ex != null) {
                logErro.Append("- Erro: ");
                logErro.AppendLine(ex.Message);

                logErro.Append("- Tipo do erro: ");
                logErro.AppendLine(ex.GetType().FullName);

                logErro.AppendLine("- StackTrace: ");
                logErro.AppendLine(ex.StackTrace);

                logErro.AppendLine();
                logErro.AppendLine();
                ex = ex.InnerException;
                logErro.AppendLine("-- Exceção anterior --");
            }
            return logErro.ToString();
        }
    }

    public class ParametroInvalidoException : Exception
    {
        /// <summary>
        /// Dispara a exceção de parâmetro inválido
        /// </summary>
        /// <param name="nmParametro">Nome do parâmetro inválido</param>
        public ParametroInvalidoException(string nmParametro)
            : base(string.Format("Parâmetro {0} inválido.", nmParametro))
        {
        }


        public ParametroInvalidoException(string nmParametro, Exception ex)
            : base(string.Format("Parâmetro {0} inválido.", nmParametro), ex)
        {
        }

        /// <summary>
        /// Dispara a exceção de parâmetro inválido
        /// </summary>
        public ParametroInvalidoException()
            : base(string.Format("Parâmetro(s) inválido(s)."))
        {

        }
    }

    /// <summary>
    /// Dispara a exceção de qualquer falha que o sistema sofreu
    /// </summary>
    public class FalhaOperacaoException : Exception
    {

        public FalhaOperacaoException(Exception ex)
            : base("Falha na operação", ex)
        {
        }

        public FalhaOperacaoException(string msg, Exception ex)
            : base(msg, ex)
        {
        }
    }
    
}

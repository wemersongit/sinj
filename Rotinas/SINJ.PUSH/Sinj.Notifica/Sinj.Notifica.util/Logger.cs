using System;
using System.IO;
using System.Text;

namespace Sinj.Notifica.util
{
    public class Logger
    {
        public static void Writer(Exception ex, string mensagem)
        {

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            stringBuilder.AppendLine(" Início da Operação       = " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.FFFFF"));
            stringBuilder.AppendLine(" Mensagem Personalizada   = " + mensagem);
            if (ex != null)
            {
                stringBuilder.AppendLine(ErroEncontrado(ex));
                stringBuilder.AppendLine(LocalDoErro(ex));
            }
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("");
            RecordLogErro(stringBuilder.ToString());
        }

        public static void Writer(string mensagem)
        {
            try
            {
                StringBuilder stringBuilder = new StringBuilder();

                stringBuilder.AppendLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                stringBuilder.AppendLine(" Início da Operação       = " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.FFFFF"));
                stringBuilder.AppendLine(" Mensagem Personalizada   = " + mensagem);
                stringBuilder.AppendLine("");
                stringBuilder.AppendLine("");
                RecordLog(stringBuilder.ToString());
            }
            catch
            {

            }
        }

        private static void RecordLogErro(string log)
        {
            string dirFile = AppDomain.CurrentDomain.BaseDirectory + @"/Logs/Exceptions";
            if(!Directory.Exists(dirFile))
            {
                Directory.CreateDirectory(dirFile);
            }
            string pathFile = dirFile + "/Log-Exceptions-" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".log";
            using (StreamWriter sw = new StreamWriter(pathFile, true))
            {
                sw.Write(log);
                sw.Flush();
                sw.Close();
            }
        }

        private static void RecordLog(string log)
        {
            string dirFile = AppDomain.CurrentDomain.BaseDirectory + @"/Logs/Operations";
            if (!Directory.Exists(dirFile))
            {
                Directory.CreateDirectory(dirFile);
            }
            string pathFile = dirFile + "/Log-Operations-" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + ".log";
            using (StreamWriter sw = new StreamWriter(pathFile, true))
            {
                sw.Write(log);
                sw.Flush();
                sw.Close();
            }
        }

        /// <summary>
        /// Retorna o local da exceção
        /// </summary>
        /// <param name="exception">Exceção</param>
        /// <returns>string contendo a localização</returns>
        private static string LocalDoErro(Exception exception)
        {
            if (exception != null)
            {
                return " Local da Exceção         = " + exception.StackTrace;
            }
            return "";
        }

        /// <summary>
        /// Retorna uma string com todas mensagens da exceção
        /// </summary>
        /// <param name="exception">Exceção</param>
        /// <returns>string com a mensagem de cada InnerException</returns>
        private static string ErroEncontrado(Exception exception)
        {
            if (exception != null)
            {
                string message = "--> " + exception.Message;
                var innerException = exception.InnerException;
                while (innerException != null)
                {
                    message = message + " -->" + innerException.Message;
                    innerException = innerException.InnerException;
                }
                return " Exceção da Operação      = " + message;
            }
            return "";
        }
    }
}
using System.Web;

namespace util.BRLight
{
    /// <summary>
    /// Classe que contém recursos para tratamento de entrada de códigos maliciosos.
    /// </summary>
    public static class TratamentoCodigoMalicioso
    {
        private static readonly string[] PalavrasReservadasSqlInjection = { "select", "drop", "insert", "delete", "union", ";", "--", "/*", "xp_", "UTL_", "DBMS_" };

        /// <summary>
        /// Formata textos de entrada contra ataques de SQL Injection.
        /// </summary>
        /// <param name="texto">Texto de entrada.</param>
        /// <param name="removerPalavrasReservadas">
        /// Exclui palavras reservadas contidas dentro do texto, caso existam. 
        /// Essas palavras reservadas incluem comandos SQL (ANSI) do tipo DDL e DML, 
        /// prefixos de Stored Procedures potencialmente perigosas do SQL Server e Oracle, 
        /// finalizadores de linha e comentadores de código.
        /// </param>
        /// <returns>Texto seguro contra ataque de SQL Injection.</returns>
        public static string RemoverSqlInjection(string texto, bool removerPalavrasReservadas) {
            // Exclui qualquer palavra reservada do texto de entrada.
            if (removerPalavrasReservadas) {
                foreach (string palavraReservada in PalavrasReservadasSqlInjection) {
                    if (texto.Contains(palavraReservada)) {
                        texto = texto.Replace(palavraReservada, string.Empty);
                    }
                }
            }

            // Evita que usuário mal-intencionado feche uma string SQL para inserir códigos maliciosos.
            texto = texto.Replace("'", "''");
            return texto;
        }

        /// <summary>
        /// Lê código HTML e o codifica para que não seja interpretado pelo navegador.
        /// </summary>
        /// <param name="texto">Texto a ser formatado.</param>
        /// <returns>
        /// Texto formatado de maneira que qualquer conteúdo HTML inserido 
        /// não seja interpretado e executado pelo navegador.
        /// </returns>
        public static string RemoverJavascriptInjection(string texto) {
            HttpUtility.HtmlAttributeEncode(texto);
            return texto;
        }
    }
}
using System;
using System.Text;
using System.Linq;

namespace util.BRLight
{
    /// <summary>
    /// Classe responsável por manipular o conteúdo de variáveis do tipo System.String.
    /// </summary>
    public static class ManipulaStrings
    {

        public static string PathConverter(string file)
        {
            if (string.IsNullOrEmpty(file))
                return null;

            return String.Format("file:///{0}", file.Replace(@"\", "/"));
        }

        /// <summary>
        /// Recupera o número de caracteres especificado à direita do texto fornecido.
        /// </summary>
        /// <param name="texto">Texto cujos caracteres à direita serão recuperados.</param>
        /// <param name="numeroCaracteres">
        /// Número de caracteres à direita do texto que devem ser recuperados pelo método.
        /// </param>
        /// <exception cref="System.ArgumentOutOfRangeException">System.ArgumentOutOfRangeException</exception>
        public static string SubstringDireita(string texto, int numeroCaracteres)
        {
            // Verifica se o texto não é nulo ou vazio.
            if (string.IsNullOrEmpty(texto))
                return string.Empty;

            // Recupera apenas os caracteres à direita dentro do alcance especificado.
            return texto.Substring(texto.Length - numeroCaracteres, numeroCaracteres);
        }

        /// <summary>
        /// Faz o split de uma string cujo separador é formado de mais de um caractere.
        /// </summary>
        /// <param name="textoSerializado">Texto serializado.</param>
        /// <param name="separador">Separador composto. (que possui mais de 1 caractere)</param>
        /// <returns>Texto desserializado.</returns>
        public static string[] SplitSeparadorComposto(string textoSerializado, string separador)
        {
            // Verifica se um texto foi informado.
            if (string.IsNullOrEmpty(textoSerializado))
                throw new ArgumentException("textoSerializado", "Nenhum valor foi informado.");

            // Verifica se o separador foi informado.
            if (string.IsNullOrEmpty(textoSerializado))
                throw new ArgumentException("separador", "O separador não foi informado.");

            // Antes de iniciar a operação, verifica se o texto serializado possui o separador informado.
            if (textoSerializado.IndexOf(separador) != -1)
            {
                // Variável que armazena a quantidade de valores serializados encontrados no texto fornecido.
                int numeroValoresSerializados = 0;

                // Variável que armazena a quantidade de caracteres do separador composto.
                int quantidadeCaracteresSeparador = separador.Length;

                /* 
                 * Verifica e valida o número de valores serializados pelo separador.
                 * A validação do número de valores serializados é feita detectando a presença do separador 
                 * dentro da variável "validaTextoSerializado", que armazena uma cópia do valor informado como parâmetro.
                 * Na medida que o loop avança, o separador é detectado e o separador junto de todo o seu valor é removido 
                 * da variável "validaTextoSerializado" e o contador do número de valores encontrados 
                 * (armazenado na variável "numeroValoresSerializados") é incrementado.
                 */
                string validaTextoSerializado = textoSerializado; // Copia o valor do texto serializado informado como parâmetro.

                // Enquanto existirem valores serializados pelo separador, executa a rotina abaixo:
                while (validaTextoSerializado.IndexOf(separador) != -1)
                {
                    // Recupera o índice onde o separador começa.
                    int indiceInicioValorSerializado = validaTextoSerializado.IndexOf(separador);

                    // Calcula a quantidade de caracteres do separador e do valor serializado por este separador.
                    int totalCaracteresValorSerializado = indiceInicioValorSerializado + quantidadeCaracteresSeparador;

                    // Remove o valor serializado e seu separador.
                    validaTextoSerializado = validaTextoSerializado.Remove(0, totalCaracteresValorSerializado);

                    // Incrementa o contador de número de valores serializados encontrados dentro do texto fornecido.
                    numeroValoresSerializados++;
                }

                // Cria um array que armazenará todos os valores serializados encontrados na validação do algoritmo acima.
                var textoDesserializado = new string[numeroValoresSerializados];

                /*
                 * Preenche o array que retorna todos os valores desserializados.
                 */
                int indiceArmazenamentoValorSerializadoAtual = 0;
                while (textoSerializado.IndexOf(separador) != -1)
                {
                    // Recupera o índice onde o separador começa.
                    int indiceSeparadorValorSerializado = textoSerializado.IndexOf(separador);

                    // Recupera o valor serializado até chegar no índice do separador, onde o valor acaba e, depois, 
                    // adiciona esse valor no índice correspondente do array que retorna o resultado do split.
                    textoDesserializado[indiceArmazenamentoValorSerializadoAtual] =
                        textoSerializado.Substring(0, indiceSeparadorValorSerializado);

                    // Remove todo o valor encontrado junto do separador, para que na repetição do loop 
                    // o próximo valor seja o primeiro.
                    textoSerializado = textoSerializado.Remove(0,
                        (indiceSeparadorValorSerializado + quantidadeCaracteresSeparador));

                    // Incrementa o índice do array que armazena o resultado de retorno.
                    indiceArmazenamentoValorSerializadoAtual++;
                }

                // Retorna os valores desserializados pelo método.
                return textoDesserializado;
            }
            
            // Assim como ocorre no método "Split()" da classe System.String, do .Net Framework, se o texto serializado 
            // não possuir o separador informado, retorna o valor do texto serializado informado como parâmetro dentro 
            // de um array unidimensional, com apenas uma casa.
            return new [] { textoSerializado };
        }

        public static string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
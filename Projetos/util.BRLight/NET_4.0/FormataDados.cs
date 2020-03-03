using System;

namespace util.BRLight
{
    /// <summary>
    /// Classe responsável por formatar dados cadastrais.
    /// </summary>
    public static class FormataDados
    {
        /// <summary>
        /// Adiciona os separadores a um número de CPF.
        /// </summary>
        /// <param name="numeroCPF">Número de CPF sem separadores (apenas os números).</param>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">System.ArgumentOutOfRangeException</exception>
        public static void FormatarCPF(ref string numeroCPF)
        {
            // Verifica se o parâmetro veio nulo ou vazio, antes de iniciar a operação.
            if (string.IsNullOrEmpty(numeroCPF))
                throw new ArgumentNullException("numeroCPF", "O parâmetro é nulo ou está vazio.");

            // Valida a quantidade de dígitos do número de CPF informado.
            if (numeroCPF.Length == 11) {
                numeroCPF = string.Format("{0}.{1}.{2}-{3}", numeroCPF.Substring(0, 3),
                    numeroCPF.Substring(3, 3), numeroCPF.Substring(6, 3), numeroCPF.Substring(9, 2));
            } else {
                throw new ArgumentOutOfRangeException("numeroCPF", "O número de CPF não possui 11 dígitos.");
            }
        }

        /// <summary>
        /// Adiciona os separadores a um número de CNPJ.
        /// </summary>
        /// <param name="numeroCNPJ">Número de CNPJ sem separadores (apenas os números).</param>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">System.ArgumentOutOfRangeException</exception>
        public static void FormatarCNPJ(ref string numeroCNPJ)
        {
            // Verifica se o parâmetro veio nulo ou vazio, antes de iniciar a operação.
            if (string.IsNullOrEmpty(numeroCNPJ))
                throw new ArgumentNullException("numeroCNPJ");

            // Valida a quantidade de dígitos do número de CNPJ informado.
            if (numeroCNPJ.Length == 14) {
                numeroCNPJ = string.Format("{0}.{1}.{2}/{3}-{4}", numeroCNPJ.Substring(0, 2), numeroCNPJ.Substring(2, 3),
                    numeroCNPJ.Substring(5, 3), numeroCNPJ.Substring(8, 4), numeroCNPJ.Substring(12, 2));
            } else {
                throw new ArgumentOutOfRangeException("numeroCNPJ", "O número de CNPJ não possui 14 dígitos.");
            }
        }

        /// <summary>
        /// Remove os separadores de um CPF.
        /// </summary>
        /// <param name="numeroCPF">Número de CPF.</param>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        public static void RemoverFormatacaoCPF(ref string numeroCPF) {
            // Verifica se o parâmetro veio nulo ou vazio, antes de iniciar a operação.
            if (string.IsNullOrEmpty(numeroCPF))
                throw new ArgumentNullException("numeroCPF");

            numeroCPF = numeroCPF.Replace(".", "").Replace("-", "");
        }

        /// <summary>
        /// Remove os separadores de um CNPJ.
        /// </summary>
        /// <param name="numeroCNPJ">Número de CNPJ.</param>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        public static void RemoverFormatacaoCNPJ(ref string numeroCNPJ) {
            // Verifica se o parâmetro veio nulo ou vazio, antes de iniciar a operação.
            if (string.IsNullOrEmpty(numeroCNPJ))
                throw new ArgumentNullException("numeroCNPJ");

            numeroCNPJ = numeroCNPJ.Replace(".", "").Replace("/", "").Replace("-", "");
        }

        /// <summary>
        /// Formata uma data para o formato dd/mm/aaaa.
        /// </summary>
        /// <param name="data">Data a ser formatada.</param>
        /// <returns>
        /// Data no formato dd/mm/aaaa. 
        /// Caso a data informada como parâmetro de entrada seja inválida é retornada uma string vazia.
        /// </returns>
        /// <exception cref="System.FormatException">System.FormatException</exception>
        public static string FormatarDataDDmmAAAA(DateTime data) {
            return data.ToString("dd/MM/yyyy"); // Retorna a data formatada.
        }

        /// <summary>
        /// Formata uma data para o formato dd/mm/aaaa.
        /// </summary>
        /// <param name="data">Data a ser formatada.</param>
        /// <returns>
        /// Data no formato dd/mm/aaaa. 
        /// Caso a data informada como parâmetro de entrada seja inválida é retornada uma string vazia.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.FormatException">System.FormatException</exception>
        public static string FormatarDataDDmmAAAA(string data) {
            // Verifica se o parâmetro veio nulo ou vazio, antes de iniciar a operação.
            if (string.IsNullOrEmpty(data))
                throw new ArgumentNullException("data", "Nenhuma data foi informada.");

            try {
                DateTime dataValida = DateTime.Parse(data);
                return FormataDados.FormatarDataDDmmAAAA(dataValida);
	        } catch {
		        throw;
	        }
        }

        public static string FormataNUP(string valor)
        {
            var str = string.Empty;

            if (valor.Length == 17) {
                for (var i = 0; i < valor.Length; i++) {
                    if (int.Parse(valor[i].ToString()) >= 0 && int.Parse(valor[i].ToString()) <= 9 && valor[i] != ' ') str += valor[i];
                    if (str.Length == 5) str += ".";
                    if (str.Length == 12) str += "/";
                    if (str.Length > 13)
                        if ((str[13] != '2' && str.Length == 15) || (str[13] == '2' && str.Length == 17)) str += "-";
                }
            }
            else
                return valor;

            return str;
        }

        public static string FormataCpfCnpj(string valor)
        {
            var str = string.Empty;
            valor = valor.Trim();
            if (valor.Length == 11) {
                foreach (var t in valor) {
                    if (str.Length == 3 || str.Length == 7) str += ".";
                    if (str.Length == 11) str += "-";
                    str += t;
                }
            } else {
                str = valor;
            }

            return str;
        }

    }
}
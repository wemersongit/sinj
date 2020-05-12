using System;
using System.Net.Mail;

namespace util.BRLight
{
    /// <summary>
    /// Classe responsável por realizar a validação de dados via servidor.
    /// </summary>
    public static class ValidaDados
    {
        /// <summary>
        /// Valida números que utilzam o algoritmo de controle Módulo 11.
        /// </summary>
        /// <param name="numero">Número a ser validado pelo algoritmo de controle Módulo 11.</param>
        /// <param name="quantidadeDigitos">Quantidade de dígitos verificadores do número.</param>
        /// <returns>Número informado com o(s) dígito(s) verificador(es).</returns>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.FormatException">System.FormatException</exception>
        /// <exception cref="System.OverflowException">System.OverflowException</exception>
        public static string ValidarDigitoModulo11(string numero, int quantidadeDigitos)
        {
            // Verifica se o número não é vazio ou nulo.
            if (string.IsNullOrEmpty(numero))
                throw new ArgumentNullException("numero", "Nenhum número foi informado.");

            var digitos = string.Empty;
            var numeroModulo11 = numero + digitos;

            for (var i = 0; i < quantidadeDigitos; i++){
                numeroModulo11 = numero + digitos;
                var soma = 0;
                var peso = 2;
                
                for (var j = numeroModulo11.Length - 1; j >= 0; j--) {
                    soma += Convert.ToInt32(numeroModulo11[j]) * peso;
                    peso++;
                }

                var iResto = (soma) % 11;
                digitos = (11 - iResto).ToString();
                if (int.Parse(digitos) >= 10)
                    digitos = (int.Parse(digitos) - 10).ToString();
            }

            return numeroModulo11 + digitos;
        }

        /// <summary>
        /// Valida um número PIS.
        /// </summary>
        /// <param name="pis">Número PIS.</param>
        /// <returns>True para número PIS válido.</returns>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <exception cref="System.FormatException">System.FormatException</exception>
        /// <exception cref="System.OverflowException">System.OverflowException</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">System.ArgumentOutOfRangeException</exception>
        public static bool ValidarPis(string pis)
        {
            // Verifica se o parâmetro não é vazio ou nulo.
            if (string.IsNullOrEmpty(pis))
                throw new ArgumentNullException("pis", "Nenhum número PIS foi informado.");

            // Valida o número PIS.
            int[] multiplicador = new int[10] { 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;

            pis = pis.Trim();
            pis = pis.Replace("-", "").Replace(".", "").PadLeft(11, '0');

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(pis[i].ToString()) * multiplicador[i];

            resto = soma % 11;

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            return pis.EndsWith(resto.ToString());
        }

        /// <summary>
        /// Valida um número de CPF.
        /// </summary> 
        /// <param name="cpf">Número de CPF com ou sem máscara.</param>
        /// <returns>True para CPF válido ou False para CPF inválido.</returns> 
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">System.ArgumentOutOfRangeException</exception>
        /// <exception cref="System.FormatException">System.FormatException</exception>
        /// <exception cref="System.OverflowException">System.OverflowException</exception>
        public static bool ValidarCPF(string cpf)
        {
            // Verifica se o parâmetro não é vazio ou nulo.
            if (string.IsNullOrEmpty(cpf))
                throw new ArgumentNullException("cpf", "Nenhum número de CPF foi informado.");

            // Remove os espaços em branco e a máscara que o número de CPF informado pode conter.
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");

            // A primeira validação é da quantidade de dígitos do número.
            // Caso possua menos de 11 dígitos não é um número de CPF válido.
            if (cpf.Length != 11)
                return false;

            // Valida se existem apenas números no CPF tentando converter o CPF informado
            // em um tipo de dado Long.
            long numeroCPF;

            // Caso o resultado seja verdadeiro, valida os números do CPF informado.
            if ( long.TryParse(cpf, out numeroCPF) )
            {
                // Caso o número de CPF possua 11 dígitos, valida o número de acordo com a lógica 
                // do dígito verificador módulo 11: http://pt.wikipedia.org/wiki/D%C3%ADgito_verificador.
                var multiplicador1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                var multiplicador2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                var tempCpf = string.Empty;
                var digito = string.Empty;
                int soma = 0;
                int resto = 0;

                tempCpf = cpf.Substring(0, 9);
                soma = 0;
                for (int i = 0; i < 9; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

                resto = soma % 11;
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = resto.ToString();

                tempCpf = tempCpf + digito;

                soma = 0;
                for (int i = 0; i < 10; i++)
                    soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

                resto = soma % 11;

                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito = digito + resto.ToString();

                return cpf.EndsWith(digito);
            }
            else
            {
                // Caso não contenha apenas números (a conversão não é possível), o número de CPF 
                // é retornado como inválido.
                return false;
            }
        }

        /// <summary>
        /// Valida um número de CNPJ.
        /// </summary> 
        /// <param name="cnpj">Número de CNPJ com ou sem máscara.</param>
        /// <returns>True para CNPJ válido ou False para CNPJ inválido.</returns> 
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">System.ArgumentOutOfRangeException</exception>
        /// <exception cref="System.FormatException">System.FormatException</exception>
        /// <exception cref="System.OverflowException">System.OverflowException</exception>
        public static bool ValidaCNPJ(string cnpj)
        {
            // Verifica se o parâmetro não é vazio ou nulo.
            if (string.IsNullOrEmpty(cnpj))
                throw new ArgumentNullException("cnpj", "Nenhum número de CNPJ foi informado.");

            // Remove os espaços em branco e a máscara que o número de CNPJ informado pode conter.
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");

            // A primeira validação é da quantidade de dígitos do número.
            // Caso possua menos de 14 dígitos não é um número de CNPJ válido.
            if (cnpj.Length != 14)
                return false;

            // Caso o número de CPF possua 11 dígitos, valida o número de acordo com a lógica 
            // do dígito verificador módulo 11: http://pt.wikipedia.org/wiki/D%C3%ADgito_verificador.
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            string digito = string.Empty;
            string tempCnpj = string.Empty;
            int soma = 0;
            int resto = 0;

            tempCnpj = cnpj.Substring(0, 12);

            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            resto = (soma % 11);

            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = resto.ToString();

            tempCnpj = tempCnpj + digito;
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            return cnpj.EndsWith(digito);
        }

        /// <summary>
        /// Validação de NUP
        /// </summary>
        /// <param name="nr_nup">Número da NUP com ou sem mascara.</param>
        /// <returns></returns>
        public static bool ValidaNup(string nr_nup)
        {
            if (!string.IsNullOrEmpty(nr_nup))
            {
                if (nr_nup.Length > 0)
                {
                    if (nr_nup.Length < 18 || nr_nup.Length == 19)
                    {
                        return false;
                    }
                }
                int inteiro;
                var nro_tdo = "";
                for (int i = 0; i < nr_nup.Length; i++)
                {
                    bool isNumeroInteiro = int.TryParse(nr_nup[i].ToString(), out inteiro);
                    if (isNumeroInteiro)
                    {
                        nro_tdo += inteiro;
                    }
                }
                int inicio = nro_tdo.Length - 2;
                var nro_part = nro_tdo.Substring(0, nro_tdo.Length - 2);
                var nro_dig = nro_tdo.Substring(inicio);

                var dig1 = Mod_11(nro_part);
                var dig2 = Mod_11(nro_part + dig1);
                var digv = "" + dig1 + dig2;

                if (digv != nro_dig) { return false; }
                return true;
            }
            return false;
        }

        public static int Mod_11(string nro)
        {
            if (!string.IsNullOrEmpty(nro))
            {
                var fator = 2;
                var Soma = 0;
                var i = 0;
                var tam = 0;
                int inteiro;
                var modulo = new int[20];
                for (i = 0; i < nro.Length; i++)
                {
                    bool isNumeroInteiro = int.TryParse(nro[i].ToString(), out inteiro);
                    if (isNumeroInteiro)
                    {
                        tam++;
                        modulo[tam] = inteiro;
                    }
                }
                for (i = tam; i > 0; i--)
                {
                    Soma = Soma + (modulo[i] * fator);
                    fator++;
                }
                var Dig = Soma % 11;
                Dig = 11 - Dig;
                if (Dig > 9) { Dig = Dig - 10; }
                return (Dig);
            }
            else
            {
                return 0;
            }
        }

        public static bool Email(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
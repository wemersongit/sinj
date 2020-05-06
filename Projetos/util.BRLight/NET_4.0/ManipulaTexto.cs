using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System;
using System.Text.RegularExpressions;

namespace util.BRLight
{
    /// <summary>
    /// Classe responsável por manipular o conteúdo de textos em geral.
    /// </summary>
    public static class ManipulaTexto
    {
        /// <summary>
        /// Remove a acentuação do texto fornecido.
        /// </summary>
        /// <param name="texto">Texto cuja acentuação será removida.</param>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">System.ArgumentOutOfRangeException</exception>
        public static void RemoverAcentuacao(ref string texto)
        {
            // Verifica se o parâmetro veio nulo ou vazio, antes de iniciar a operação.
            if (string.IsNullOrEmpty(texto))
                throw new ArgumentNullException("texto");

            // Normaliza a string.
            string textoNormalizado = texto.Normalize(NormalizationForm.FormD);

            // Cria um buffer de memória que vai armazenar uma cópia do texto fornecido sem a acentuação.
            StringBuilder textoSemAcentos = new StringBuilder();
            
            // Varre o texto e remove qualquer acentuação.
            for (int i = 0; i < textoNormalizado.Length; i++)
            {
                UnicodeCategory formatoUnicode = CharUnicodeInfo.GetUnicodeCategory(textoNormalizado[i]);

                if (formatoUnicode != UnicodeCategory.NonSpacingMark)
                    textoSemAcentos.Append(textoNormalizado[i]);
            }
            
            // Atualiza o conteúdo do texto.
            texto = textoSemAcentos.ToString();
        }

        /// <summary>
        /// Remove os caracteres especiais contidos em um texto. '=', '\\', ';', '.', ':', ',', '+', '*' 
        /// </summary>
        /// <param name="texto">Texto cujos caracteres especiais serão removidos.</param>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">System.ArgumentOutOfRangeException</exception>
        public static void RemoverCaracteresEspeciais(ref string texto)
        {
            // Verifica se o parâmetro veio nulo ou vazio, antes de iniciar a operação.
            if (string.IsNullOrEmpty(texto))
                throw new ArgumentNullException("texto");

            char[] trim = { '=', '\\', ';', '.', ':', ',', '+', '*' };
            int pos;
            while ((pos = texto.IndexOfAny(trim)) >= 0)
            {
                texto = texto.Remove(pos, 1);
            }
        }

        public static string RetiraCaracteresEspeciais(string texto, bool removeEspaco)
        {
            var normalizedString = texto;

            // Prepara a tabela de símbolos.
            var symbolTable = new Dictionary<char, char[]>
                                      {
                                          {'a', new[] {'à', 'á', 'ä', 'â', 'ã'}},
                                          {'c', new[] {'ç'}},
                                          {'e', new[] {'è', 'é', 'ë', 'ê'}},
                                          {'i', new[] {'ì', 'í', 'ï', 'î'}},
                                          {'o', new[] {'ò', 'ó', 'ö', 'ô', 'õ'}},
                                          {'u', new[] {'ù', 'ú', 'ü', 'û'}},
                                          {'n', new[] {'ñ'}},
                                          {'A', new[] {'À', 'Á', 'Ä', 'Â', 'Ã'}},
                                          {'C', new[] {'Ç'}},
                                          {'E', new[] {'È', 'É', 'Ë', 'Ê'}},
                                          {'I', new[] {'Ì', 'Í', 'Ï', 'Î'}},
                                          {'O', new[] {'Ò', 'Ó', 'Ö', 'Ô', 'Õ'}},
                                          {'U', new[] {'Ù', 'Ú', 'Ü', 'Û'}},
                                          {'N', new[] {'Ñ'}}
                                      };

            // Substitui os símbolos.
            foreach (var key in symbolTable.Keys)
                foreach (var symbol in symbolTable[key])
                    normalizedString = normalizedString.Replace(symbol, key);

            // Remove os outros caracteres especiais.
            if (removeEspaco)
                normalizedString = Regex.Replace(normalizedString, "[^0-9a-zA-Z]+?", "");
            else
                normalizedString = Regex.Replace(normalizedString, "[^0-9a-zA-Z\\s]+?", "");

            return normalizedString;
        }

        #region Escrever números por extenso

        /// <summary>
        /// Retorna o nome em extenso de um valor monetário no intervalo de 1 à 9 bilhões.
        /// </summary>
        /// <param name="valor">Qualquer valor monetário no intervalo entre 1 à 9 bilhões.</param>
        /// <returns>Número por extenso.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">System.ArgumentOutOfRangeException</exception>
        public static string EscreverValorMonetarioExtenso(decimal valor)
        {
            // Verifica se o valor informado se encontra no intervalo de números suportado pelo método.
            if (valor <= 0 || valor > (decimal)9999999999.99)
                throw new ParametroInvalidoException("O método aceita apenas números positivos e menores do que 9 bilhões.");

            string valorExtenso = string.Empty; // Armazena o número informado como parâmetro escrito por extenso.
            string numero; // Armazena o número informado como parâmetro para manipulação.
            string centena;
            string dezena;
            string dezCentavo;

            decimal centavos;
            decimal valorInteiro;
            var intContador = 0;
            var bilhao = false;
            var milhao = false;
            var mil = false;
            var unidade = false;

            //Gerar Extenso Centavos 
            valor = (decimal.Round(valor, 2));
            centavos = valor - (long)valor;

            //Gerar Extenso parte Inteira
            valorInteiro = (long)valor;
            if (valorInteiro > 0)
            {
                if (valorInteiro > 999)
                    mil = true;

                if (valorInteiro > 999999)
                {
                    milhao = true;
                    mil = false;
                }

                if (valorInteiro > 999999999)
                {
                    mil = false;
                    milhao = false;
                    bilhao = true;
                }

                for (int i = (valorInteiro.ToString().Trim().Length) - 1; i >= 0; i--)
                {
                    numero = valorInteiro.ToString().Trim().Substring((valorInteiro.ToString().Trim().Length - i) - 1, 1);

                    switch (i)
                    {            /*******/
                        case 9:  /*Bilhão*
                             /*******/
                            {
                                valorExtenso = string.Concat(EscreverExtensoUnidade(numero),
                                    ((int.Parse(numero) > 1) ? " bilhões e" : " bilhão e"));

                                bilhao = true;
                                break;
                            }
                        case 8: /********/
                        case 5: //Centena*
                        case 2: /********/
                            {
                                if (int.Parse(numero) > 0)
                                {
                                    centena = valorInteiro.ToString().Trim().Substring((valorInteiro.ToString().Trim().Length - i) - 1, 3);

                                    if (int.Parse(centena) > 100 && int.Parse(centena) < 200)
                                        valorExtenso += " cento e ";
                                    else
                                        valorExtenso = string.Concat(valorExtenso, " ", EscreverExtensoGrupoCentenas(numero));

                                    if (intContador == 8)
                                        milhao = true;
                                    else if (intContador == 5)
                                        mil = true;
                                }
                                break;
                            }
                        case 7: /*****************/
                        case 4: //Dezena de Milhão*
                        case 1: /*****************/
                            {
                                if (int.Parse(numero) > 0)
                                {
                                    dezena = valorInteiro.ToString().Trim().Substring((valorInteiro.ToString().Trim().Length - i) - 1, 2);

                                    if (int.Parse(dezena) > 10 && int.Parse(dezena) < 20)
                                    {
                                        valorExtenso = string.Concat(valorExtenso, (ManipulaStrings.SubstringDireita(valorExtenso, 5).Trim() == "entos" ? " e " : " "),
                                            EscreverExtensoDezenaUnidade(ManipulaStrings.SubstringDireita(dezena, 1)));

                                        unidade = true;
                                    }
                                    else
                                    {
                                        valorExtenso = string.Concat(valorExtenso, (ManipulaStrings.SubstringDireita(valorExtenso, 5).Trim() == "entos" ? " e " : " "),
                                            EscreverExtensoGrupoDezenas(dezena.Substring(0, 1)));

                                        unidade = false;
                                    }

                                    if (intContador == 7)
                                        milhao = true;
                                    else if (intContador == 4)
                                        mil = true;
                                }
                                break;
                            }
                        case 6: /******************/
                        case 3: //Unidade de Milhão* 
                        case 0: /******************/
                            {
                                if (int.Parse(numero) > 0 && !unidade)
                                {
                                    if ((ManipulaStrings.SubstringDireita(valorExtenso, 5).Trim()) == "entos"
                                    || (ManipulaStrings.SubstringDireita(valorExtenso, 3).Trim()) == "nte"
                                    || (ManipulaStrings.SubstringDireita(valorExtenso, 3).Trim()) == "nta")
                                    {
                                        valorExtenso += " e ";
                                    }
                                    else
                                    {
                                        valorExtenso += " ";
                                    }

                                    valorExtenso += EscreverExtensoUnidade(numero);
                                }
                                if (i == 6)
                                {
                                    if (milhao || int.Parse(numero) > 0)
                                    {
                                        valorExtenso += ((int.Parse(numero) == 1) && !unidade ? " milhão" : " milhões");
                                        valorExtenso += ((int.Parse(numero) > 1000000) ? " " : " e");
                                        milhao = true;
                                    }
                                }
                                if (i == 3)
                                {
                                    if (mil || int.Parse(numero) > 0)
                                    {
                                        valorExtenso += " Mil";
                                        valorExtenso += ((int.Parse(numero) > 1000) ? " " : " e");
                                        mil = true;
                                    }
                                }
                                if (i == 0)
                                {
                                    if ((bilhao && !milhao && !mil
                                    && ManipulaStrings.SubstringDireita((valorInteiro.ToString().Trim()), 3) == "0")
                                    || (!bilhao && milhao && !mil
                                    && ManipulaStrings.SubstringDireita((valorInteiro.ToString().Trim()), 3) == "0"))
                                    {
                                        valorExtenso = valorExtenso + " e ";
                                    }

                                    valorExtenso = string.Concat(valorExtenso, ((Int64.Parse(valorInteiro.ToString())) > 1 ? " reais" : " real"));
                                }

                                unidade = false;
                                break;
                            }
                    }
                }
            }
            if (centavos > 0)
            {
                if (centavos > 0 && centavos < 0.1M)
                {
                    numero = ManipulaStrings.SubstringDireita((decimal.Round(centavos, 2)).ToString().Trim(), 1);

                    valorExtenso = string.Concat(valorExtenso, ((centavos > 0) ? " e " : " "),
                        EscreverExtensoUnidade(numero), ((centavos > 0.01M) ? " centavos" : " centavo"));
                }
                else if (centavos > 0.1M && centavos < 0.2M)
                {
                    numero = ManipulaStrings.SubstringDireita(((decimal.Round(centavos, 2) - (decimal)0.1).ToString().Trim()), 1);

                    valorExtenso = string.Concat(valorExtenso, ((centavos > 0) ? " " : " e "),
                        EscreverExtensoDezenaUnidade(numero));
                }
                else
                {
                    numero = ManipulaStrings.SubstringDireita(centavos.ToString().Trim(), 2);
                    dezCentavo = centavos.ToString().Trim().Substring(2, 1);

                    valorExtenso = string.Concat(valorExtenso, ((int.Parse(numero) > 0) ? " e " : " "));
                    valorExtenso = string.Concat(valorExtenso, EscreverExtensoGrupoDezenas(dezCentavo.Substring(0, 1)));

                    if ((centavos.ToString().Trim().Length) > 2)
                    {
                        numero = ManipulaStrings.SubstringDireita((decimal.Round(centavos, 2)).ToString().Trim(), 1);

                        if (int.Parse(numero) > 0)
                        {
                            if (valorInteiro <= 0)
                            {
                                if (valorExtenso.Trim().Substring(valorExtenso.Trim().Length - 2, 1) == "e")
                                    valorExtenso = string.Concat(valorExtenso, " e ", EscreverExtensoUnidade(numero));
                                else
                                    valorExtenso = string.Concat(valorExtenso, " e ", EscreverExtensoUnidade(numero));
                            }
                            else
                            {
                                valorExtenso = string.Concat(valorExtenso, " e ", EscreverExtensoUnidade(numero));
                            }
                        }
                    }

                    valorExtenso = string.Concat(valorExtenso, " centavos ");
                }
            }

            if (valorInteiro < 1)
                valorExtenso = valorExtenso.Trim().Substring(2, valorExtenso.Trim().Length - 2);

            return valorExtenso.Trim();
        }

        /// <summary>
        /// Retorna o nome em extenso de um número menor que dez.
        /// </summary>
        /// <param name="numero">Número menor que dez.</param>
        private static string EscreverExtensoUnidade(string numero)
        {
            // Array com o nome dos números menores que dez.
            string[] nomesUnidades = { "um", "dois", "três", "quatro", "cinco", "seis", "sete", "oito", "nove" };
            return nomesUnidades[(int.Parse(numero) - 1)];
        }

        /// <summary>
        /// Retorna o nome em extenso de um número contido no intervalo entre 11 e 19.
        /// </summary>
        /// <param name="numero">Número entre 11 e 19.</param>
        private static string EscreverExtensoDezenaUnidade(string numero)
        {
            // Array com o nome dos números entre 11 e 19.
            string[] nomesDezenasUnidades = { "onze", "doze", "treze", "catorze", "quinze", "dezesseis", "dezessete", "dezoito", "dezenove" };
            return nomesDezenasUnidades[(int.Parse(numero) - 1)];
        }

        /// <summary>
        /// Retorna o nome em extenso de um número contido no grupo das dezenas: { 10, 20, 30, 40, 50, 60, 70, 80, 90 }.
        /// </summary>
        /// <param name="numero">Número contido no grupo das dezenas: { 10, 20, 30, 40, 50, 60, 70, 80, 90 }.</param>
        private static string EscreverExtensoGrupoDezenas(string numero)
        {
            // Array com o nome dos números contidos no grupo das dezenas.
            string[] nomesDezenas = { "dez", "vinte", "trinta", "quarenta", "cinquenta", "sessenta", "setenta", "oitenta", "noventa" };
            return nomesDezenas[(int.Parse(numero) - 1)];
        }

        /// <summary>
        /// Retorna o nome em extenso de um número contido no grupo das centenas: { 100, 200, 300, 400, 500, 600, 700, 800, 900 }.
        /// </summary>
        /// <param name="numero">Número contido no grupo das centenas: { 100, 200, 300, 400, 500, 600, 700, 800, 900 }.</param>
        private static string EscreverExtensoGrupoCentenas(string numero)
        {
            // Array com o nome dos números contidos no grupo das dezenas.
            string[] nomesDezenas = { "cem", "duzentos", "trezentos", "quatrocentos", "quinhentos", "seiscentos", "setecentos", "oitocentos", "novecentos" };
            return nomesDezenas[(int.Parse(numero) - 1)];
        } 

        #endregion
    }
}
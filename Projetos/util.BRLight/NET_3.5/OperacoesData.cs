using System;

namespace util.BRLight {
    /// <summary>
    /// Classe responsável por realizar operações diversas com datas.
    /// </summary>
    public static class OperacoesData
    {
        /// <summary>
        /// Calcula o intervalo de tempo entre duas datas.
        /// </summary>
        /// <param name="dataInicial">Data inicial.</param>
        /// <param name="dataFinal">Data final.</param>
        /// <returns>Intervalo de tempo entre as duas datas informadas.</returns>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        public static TimeSpan CalcularIntervaloTempo(DateTime dataInicial, DateTime dataFinal)
        {
            // Valida se a data inicial é menor do que a data final.
            if (dataInicial > dataFinal)
                throw new ArgumentException("A data final deve ser maior do que a data de início.");

            TimeSpan intervaloTempo = dataFinal - dataInicial;
            return intervaloTempo;
        }

        /// <summary>
        /// Calcula o intervalo de tempo entre duas datas.
        /// </summary>
        /// <param name="dataInicial">Data inicial.</param>
        /// <param name="dataFinal">Data final.</param>
        /// <returns>
        /// Intervalo de tempo entre as duas datas informadas. 
        /// Caso alguma data informada como parâmetro de entrada seja inválida é retornada uma data vazia.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">System.ArgumentNullException</exception>
        /// <exception cref="System.ArgumentException">System.ArgumentException</exception>
        /// <exception cref="System.FormatException">System.FormatException</exception>
        public static TimeSpan CalcularIntervaloTempo(string dataInicial, string dataFinal)
        {
            // Verifica se os parâmetros estão nulos ou vazios, antes de iniciar a operação.
            if (string.IsNullOrEmpty(dataInicial))
                throw new ArgumentNullException("dataInicial", "A data inicial não foi informada.");

            if (string.IsNullOrEmpty(dataFinal))
                throw new ArgumentNullException("dataFinal", "A data final não foi informada.");

            var dataInicio = DateTime.Parse(dataInicial);
            var dataFim = DateTime.Parse(dataFinal);

            return CalcularIntervaloTempo(dataInicio, dataFim);
        }

        /// <summary>
        /// Função retorna quantidade de dias entre um intervalo de datas
        /// </summary>
        /// <param name="dataInicio">Data de início do período a ser verificada</param>
        /// <param name="dataFim">Data de término do período a ser verificada</param>
        /// <returns></returns>
        public static string RetornarDiasEntreIntervaloDatas(string dataInicio, string dataFim)
        {
            try
            {
                string retorno;
                if (!string.IsNullOrEmpty(dataInicio) && !string.IsNullOrEmpty(dataFim))
                {
                    if (DateTime.Parse(dataInicio) < DateTime.Parse(dataFim))
                    {
                        var tsDuration = DateTime.Parse(dataFim).Subtract(DateTime.Parse(dataInicio));
                        retorno = tsDuration.TotalDays.ToString();
                    }
                    else
                    {
                        throw new Exception("A data fim deve ser maior que a data início!");
                    }
                }
                else
                {
                    throw new Exception("A data de início e a data fim devem estar preenchidas!");
                }
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Verifica se a data informada é menor que a data atual.
        /// </summary>
        /// <param name="data">Data a ser verificada</param>
        /// <returns>True - data menor que a atual; False - data maior ou igual a atual.</returns>
        public static Boolean DataMenorAtual(string data)
        {
            if (!String.IsNullOrEmpty(data))
            {
                var dataInformada = DateTime.Parse(data);
                var dataAtual = DateTime.Now;
                return dataAtual > dataInformada;
            }
            return false;
        }

    }
}
using System;
using System.Data;
using System.Globalization;

namespace Exportador_LB_to_ES.AD.Models
{
    [Serializable]
    public class Fonte : IComparable<Fonte>
    {

        public Fonte()
        {
            Pesquisavel = true;
            Id = Guid.NewGuid();
        }

        private static object ObtemValorSemDestaque(string entrada)
        {
            return entrada;
        }

        public Fonte(DataRow dataRow, string id)
        {
            CaminhoArquivoTexto = dataRow["CaminhoArquivoTexto"] as string;
            ConteudoArquivoTexto = dataRow["ConteudoArquivoTexto"] as string;

            if (dataRow["DataPublicacao"] != null)
            {
                DataPublicacao =
                    Convert.ToDateTime(ObtemValorSemDestaque(dataRow["DataPublicacao"].ToString())).ToString(
                        DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern);
                DataPublicacaoCompare = Convert.ToDateTime(DataPublicacao);
            }
            MotivoReduplicacao = dataRow["MotivoReduplicacao"] as string;
            NomeArquivoTexto = dataRow["NomeArquivoTexto"] as string;
            Id = new Guid(id);
            TipoFonte = new TipoDeFonteBO(dataRow.ItemArray[1] as string);

            if (dataRow.ItemArray[2] != null)
                TipoEdicao =
                    (TipoDeEdicao)
                    Enum.Parse(typeof(TipoDeEdicao),
                               Convert.ToString(ObtemValorSemDestaque(dataRow.ItemArray[2].ToString())));

            int pagina = 0;
            int coluna = 0;

            if (dataRow.ItemArray[5] != null) int.TryParse(dataRow.ItemArray[5].ToString(), out pagina);
            if (dataRow.ItemArray[6] != null) int.TryParse(dataRow.ItemArray[6].ToString(), out coluna);

            Pagina = pagina != 0 ? pagina : (int?)null;
            Coluna = coluna != 0 ? coluna : (int?)null;
            Observacao = dataRow.ItemArray[7] as string;
            MotivoReduplicacao = dataRow.ItemArray[8] as string;
            ConteudoArquivoTexto = dataRow.ItemArray[9] as string;
            NomeArquivoTexto = dataRow.ItemArray[10] as string;
            CaminhoArquivoTexto = dataRow.ItemArray[11] as string;
            //dataRow.ItemArray[12];
            TipoPublicacao = new TipoDePublicacaoBO(dataRow["TipoPublicacao"] as string);
        }

        public Fonte(DataRow dataRow)
        {
            CaminhoArquivoTexto = dataRow["CaminhoArquivoTexto"] as string;
            ConteudoArquivoTexto = dataRow["ConteudoArquivoTexto"] as string;

            if (dataRow["DataPublicacao"] != null)
            {
                DataPublicacao =
                    (Convert.ToDateTime(ObtemValorSemDestaque(dataRow["DataPublicacao"].ToString()))).ToString(
                        DateTimeFormatInfo.InvariantInfo.SortableDateTimePattern);
                DataPublicacaoCompare = Convert.ToDateTime(DataPublicacao);
            }
            MotivoReduplicacao = dataRow["MotivoReduplicacao"] as string;
            NomeArquivoTexto = dataRow["NomeArquivoTexto"] as string;
            Id = new Guid((string)dataRow.ItemArray[0]);
            TipoFonte = new TipoDeFonteBO(dataRow.ItemArray[1] as string);

            if (dataRow.ItemArray[2] != null)
                TipoEdicao =
                    (TipoDeEdicao)
                    Enum.Parse(typeof(TipoDeEdicao),
                               Convert.ToString(ObtemValorSemDestaque(dataRow.ItemArray[2].ToString())));

            int pagina = 0;
            int coluna = 0;

            if (dataRow.ItemArray[5] != null) int.TryParse(dataRow.ItemArray[5].ToString(), out pagina);
            if (dataRow.ItemArray[6] != null) int.TryParse(dataRow.ItemArray[6].ToString(), out coluna);

            Pagina = pagina != 0 ? pagina : (int?)null;
            Coluna = coluna != 0 ? coluna : (int?)null;
            Observacao = dataRow.ItemArray[7] as string;
            MotivoReduplicacao = dataRow.ItemArray[8] as string;
            ConteudoArquivoTexto = dataRow.ItemArray[9] as string;
            NomeArquivoTexto = dataRow.ItemArray[10] as string;
            CaminhoArquivoTexto = dataRow.ItemArray[11] as string;
            //dataRow.ItemArray[12];
            TipoPublicacao = new TipoDePublicacaoBO(dataRow["TipoPublicacao"] as string);

        }

        public TipoDePublicacaoBO TipoPublicacao { get; set; }
        public TipoDeFonteBO TipoFonte { get; set; }
        public string DataPublicacao { get; set; }
        public DateTime? DataPublicacaoCompare { get; set; }

        public Guid Id { get; set; }
        public int? Pagina { get; set; }
        public int? Coluna { get; set; }
        public int IdNorma { get; set; }
        public bool Pesquisavel { get; set; }
        public string Observacao { get; set; }
        public string MotivoReduplicacao { get; set; }
        public string ConteudoArquivoTexto { get; set; }
        public string CaminhoArquivoTexto { get; set; }
        public string NomeArquivoTexto { get; set; }
        public string CaminhoDoArquivoTemporario { get; set; }
        public byte[] DadosDoArquivoTemporario;

        /// <summary>
        /// Propriedade que retorna apenas a nome do tipo da fonte. Apenas usado para o relatório.
        /// </summary>
        public string TipoDeFonteParaRelatorio
        {
            get
            {
                return TipoFonte != null ? TipoFonte.Nome : "Tipo não definido.";
            }
        }

        public TipoDeEdicao? TipoEdicao { get; set; }

        /// <summary>
        /// Propriedade que retorna apenas a nome do tipo da edição. Apenas usado para o relatório.
        /// </summary>
        public string TipoDeEdicaoParaRelatorio
        {
            get
            {
                if (TipoEdicao != null)
                {
                    TipoDeEdicao edicao = (TipoDeEdicao)TipoEdicao;
                    return edicao.ToString();
                }
                return "Não existe.";
            }
        }

        /// <summary>
        /// Propriedade que retorna apenas a nome do tipo da edição. Apenas usado para o relatório.
        /// </summary>
        public string TipoDePublicacaoParaRelatorio
        {
            get
            {
                if (TipoPublicacao != null)
                {
                    return TipoPublicacao.Nome;
                }
                return "Não existe.";
            }
        }

        public void ArmazenaDadosDoArquivoTemporario(byte[] arquivo)
        {
            DadosDoArquivoTemporario = arquivo;
        }

        #region Implementation of IValidavel

        public void Valida()
        {
            if (TipoFonte == null) throw new Exception("Tipo de Fonte deve ser informado");
            if (TipoEdicao == null) throw new Exception("Tipo de Edição da fonte deve ser informado");
            if (TipoPublicacao == null) throw new Exception("Tipo de Publicação da fonte deve ser informado");
            if (DataPublicacao == null) throw new Exception("Data de Publicação da fonte deve ser informada");
        }

        #endregion

        public int CompareTo(Fonte fonte)
        {
            try
            {
                return DataPublicacaoCompare.Value.CompareTo(fonte.DataPublicacaoCompare.Value);
            }
            catch (Exception)
            {
                return 0;
            }
        }

    }
}

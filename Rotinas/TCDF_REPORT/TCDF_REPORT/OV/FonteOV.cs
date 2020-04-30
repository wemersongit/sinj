using System;
using System.Data;

namespace TCDF_REPORT.OV
{
    public class FonteOV
    {
        public Guid Id { get; set; }
        public int Pagina { get; set; }
        public int Coluna { get; set; }
        public int IdNorma { get; set; }
        public bool Pesquisavel { get; set; }
        public string Observacao { get; set; }
        public string MotivoReduplicacao { get; set; }
        public string ConteudoArquivoTexto { get; set; }
        public string CaminhoArquivoTexto { get; set; }
        public string NomeArquivoTexto { get; set; }
        public string CaminhoDoArquivoTemporario { get; set; }
        public byte[] DadosDoArquivoTemporario;
        public TipoDePublicacaoBOOV TipoPublicacao { get; set; }
        public TipoDeFonteBOOV TipoFonte { get; set; }
        public TipoDeEdicao _tipoEdicao { private get; set; }
        public string TipoEdicao
        {
            get
            {
                switch (_tipoEdicao)
                {
                    case TipoDeEdicao.Normal:
                        return "Normal";
                    case TipoDeEdicao.Extra:
                        return "Extra";
                    case TipoDeEdicao.Suplemento:
                        return "Suplemento";
                    default:
                        return "Não tem Definido";
                }
            }
        }

        private DateTime? _dataPublicacao;
        public DateTime? DataPublicacao
        {
            get { return _dataPublicacao; }
            set { _dataPublicacao = value; }
        }

        public FonteOV()
        {
            Id = Guid.NewGuid();
        }

        private static object ObtemValorSemDestaque(string entrada)
        {
            return entrada;
        }

        public FonteOV(DataRow dataRow, string id)
        {
            CaminhoArquivoTexto = dataRow["CaminhoArquivoTexto"] as string;
            ConteudoArquivoTexto = dataRow["ConteudoArquivoTexto"] as string;

            if (dataRow["DataPublicacao"] != null)
                DataPublicacao = Convert.ToDateTime(ObtemValorSemDestaque(dataRow["DataPublicacao"].ToString()));

            MotivoReduplicacao = dataRow["MotivoReduplicacao"] as string;
            NomeArquivoTexto = dataRow["NomeArquivoTexto"] as string;
            Id = new Guid(id);
            TipoFonte = new TipoDeFonteBOOV(dataRow.ItemArray[1] as string);

            if (dataRow.ItemArray[2] != null) _tipoEdicao = (TipoDeEdicao)Enum.Parse(typeof(TipoDeEdicao), Convert.ToString(ObtemValorSemDestaque(dataRow.ItemArray[2].ToString())));

            int pagina = 0;
            int coluna = 0;

            if (dataRow.ItemArray[5] != null) int.TryParse(dataRow.ItemArray[5].ToString(), out pagina);
            if (dataRow.ItemArray[6] != null) int.TryParse(dataRow.ItemArray[6].ToString(), out coluna);

            Observacao = dataRow.ItemArray[7] as string;
            MotivoReduplicacao = dataRow.ItemArray[8] as string;
            ConteudoArquivoTexto = dataRow.ItemArray[9] as string;
            NomeArquivoTexto = dataRow.ItemArray[10] as string;
            CaminhoArquivoTexto = dataRow.ItemArray[11] as string;

            TipoPublicacao = new TipoDePublicacaoBOOV(dataRow["TipoPublicacao"] as string);
        }

        public FonteOV(DataRow dataRow)
        {
            CaminhoArquivoTexto = dataRow["CaminhoArquivoTexto"] as string;
            ConteudoArquivoTexto = dataRow["ConteudoArquivoTexto"] as string;

            if (dataRow["DataPublicacao"] != null)
                DataPublicacao = Convert.ToDateTime(ObtemValorSemDestaque(dataRow["DataPublicacao"].ToString()));

            MotivoReduplicacao = dataRow["MotivoReduplicacao"] as string;
            NomeArquivoTexto = dataRow["NomeArquivoTexto"] as string;
            Id = new Guid((string)dataRow.ItemArray[0]);
            TipoFonte = new TipoDeFonteBOOV(dataRow.ItemArray[1] as string);

            if (dataRow.ItemArray[2] != null) _tipoEdicao = (TipoDeEdicao)Enum.Parse(typeof(TipoDeEdicao), Convert.ToString(ObtemValorSemDestaque(dataRow.ItemArray[2].ToString())));

            int pagina = 0;
            int coluna = 0;

            if (dataRow.ItemArray[5] != null) int.TryParse(dataRow.ItemArray[5].ToString(), out pagina);
            if (dataRow.ItemArray[6] != null) int.TryParse(dataRow.ItemArray[6].ToString(), out coluna);

            Observacao = dataRow.ItemArray[7] as string;
            MotivoReduplicacao = dataRow.ItemArray[8] as string;
            ConteudoArquivoTexto = dataRow.ItemArray[9] as string;
            NomeArquivoTexto = dataRow.ItemArray[10] as string;
            CaminhoArquivoTexto = dataRow.ItemArray[11] as string;
            //dataRow.ItemArray[12];
            TipoPublicacao = new TipoDePublicacaoBOOV(dataRow["TipoPublicacao"] as string);
        }

    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Exportador_LB_to_ES.AD.Models
{
    public abstract class Norma
    {
        public int IdSileg { get; set; }
        public string Rotulo { get; set; }
        public string Comentario { get; set; }
        private List<Orgao> origens;
        private HashSet<string> autorias;
        private List<Requerente> requerentes;
        private List<Requerido> requeridos;
		private List<Relator> relatores;
        private List<ProcuradorResponsavel> procuradoresResponsaveis;
        private List<Interessado> interessados;
        public List<Indexacao> NeoIndexacao { get; set; }
        //public ParamsForIndexer paramsForIndexer;
        public HashSet<string> AuxiliarDeRankeamento { get; set; }

        public Norma()
        {
            origens = new List<Orgao>();
            NeoIndexacao = new List<Indexacao>();
            autorias = new HashSet<string>();
            Fontes = new List<Fonte>();
            requerentes = new List<Requerente>();
            requeridos = new List<Requerido>();
			relatores = new List<Relator>();
            procuradoresResponsaveis = new List<ProcuradorResponsavel>();
            interessados = new List<Interessado>();
            Vides = new List<VideEntreNormas>();
            AuxiliarDeRankeamento = new HashSet<string>();
        }

        /// <summary>
        /// Essa propriedade obtem a data mais antiga dentre as fontes da norma.
        /// </summary>
        /// <exception cref="ValidacaoException">
        /// Caso as fontes não tenham data de Publicação, retorna um erro de validação.
        /// </exception>
        public DateTime PrimeiraPublicacao
        {
            get
            {
                DateTime menorData = DateTime.Today;
                foreach (Fonte fonte in Fontes)
                {
                    if (fonte.DataPublicacaoCompare != null)
                        menorData = (DateTime) (fonte.DataPublicacaoCompare < menorData ? fonte.DataPublicacaoCompare : menorData);
                }
                //if (menorData == DateTime.Today)
                //    throw new Exception("A norma não tem data de publicação em suas fontes.");
                return menorData;
            }
        }

        /// <summary>
        /// Esse campo apenas é preenchido caso seja uma ação PGDF. (G2)
        /// </summary>
        public List<Decisao> HistoricoDeDecisoes { get; set; }

        /// <summary>
        /// Obtem a propriedade HistoricoDeDecisoes da Norma
        /// </summary>
        /// <returns>HistoricoDeDecisoes</returns>
        /// <seealso cref="HistoricoDeDecisoes"/>
        [Obsolete("Utilize a propriedade HistoricoDeDecisoes")]
        public List<Decisao> ObtemHistoricoDeDecisoes()
        {
            return HistoricoDeDecisoes;
        }

        public void ArmazenaHistoricoDeDecisoes(List<Decisao> historico)
        {
            HistoricoDeDecisoes = historico;
        }

        public int Id { get; set; }
        
        public TipoDeNorma Tipo { get; set; }


        /// <summary>
        /// Qualifica a norma no ordenamento juridico.(ID por Tipo de Norma)
        /// </summary>
        [Description("Número")]
        public string Numero { get; set; }

        /// <summary>
        /// Qualifica a norma no ordenamento juridico, é usado.(ID por Tipo de Norma)
        /// </summary>
        [Description("Número em formato de texto")]
        public string NumeroString { get; set; }

        /// <summary>
        /// Letra é usada para evitar a duplicação na hora de cadastro
        /// </summary>
        public char? Letra { get; set; }

        /// <summary>
        /// Data em que a norma se insere.
        /// </summary>
        [Description("Data da Assinatura")]

        public string DataAssinatura { get; set; }
        
        public long? NumeroSequencial { get; set; }

        [Description("Âmbito")]
        public string Ambito { get;set; }

        /// <summary>
        /// Apelido popularmente conhecido.
        /// </summary>
        [Description("Apelido")]
        public string Apelido { get; set; }
        public DateTime? DataDeAutuacao { get; set; }
        public string Procedencia { get; set; }
        public string ParametroConstitucional { get; set; }
        public string Relator { get; set; }
        public string TextoDaAcao { get; set; }
        public string EfeitoDaDecisao { get; set; }
        public string NomeDoOrgao { get; set; }
        public string Situacao { get; set; }
        public string UsuarioQueCadastrou { get; set; }
        public DateTime DataDoCadastro { get; set; }
        public string UsuarioDaUltimaAlteracao { get; set; }
        public DateTime? DataDaUltimaAlteracao { get; set; }
        public string ChaveParaNaoDuplicacao { get; set; }
        public bool HaPendencia { get; set; }
        public bool Destacada { get; set; }
        public string Ementa { get; set; }
        public string ConteudoArquivoTextoConsolidado { get; set; }
        public string CaminhoArquivoTextoConsolidado { get; set; }
        public string NomeArquivoTextoConsolidado { get; set; }
        public string ConteudoArquivoTextoAcao { get; set; }
        public string CaminhoArquivoTextoAcao { get; set; }
        public string NomeArquivoTextoAcao { get; set; }
        public string ObservacaoNorma { get; set; }
        public string ListaDeNomes { get; set; }
        [Description("CampoDeOrdenamento")]
        public string CampoDeOrdenamento { get; set; }
        public bool AtlzNorma { get; set; }
        public bool NovaNorma { get; set; }


        public HashSet<string> Autorias { get { return autorias; } }

        public List<Requerente> Requerentes
        {
            get
            {
                return requerentes;
            }
            set
            {
                requerentes = value;
            }
        }

        public List<Requerido> Requeridos
        {
            get
            {
                return requeridos;
            }
            set
            {
                requeridos = value;
            }
        }

		

        public List<ProcuradorResponsavel> ProcuradoresResponsaveis
        {
            get
            {
                return procuradoresResponsaveis;
            }
            set
            {
                procuradoresResponsaveis = value;
            }
        }

        public List<Interessado> Interessados
        {
            get
            {
                return interessados;
            }
            set
            {
                interessados = value;
            }
        }


        public List<Orgao> Origens
        {
            get
            {
                return origens;
            }
            set
            {
                origens = value;
            }
        }

        public string[] ListaReferenciasPessoasFisicasEJuridicas { get; set; }
        public string[] ListaAutoridades { get; set; }

        public InformacoesSobreVersao Versao { get; set; }
        public List<Fonte> Fontes { get; set; }
        public List<VideEntreNormas> Vides { get; set; }

        public byte[] DadosDoArquivoTextoConsolidado;
        public byte[] DadosDoArquivoTextoAcao;

        public void ArmazenaDadosDoArquivoTextoConsolidado(byte[] arquivo)
        {
            DadosDoArquivoTextoConsolidado = arquivo;
        }

        public void ArmazenaDadosDoArquivoTextoAcao(byte[] arquivo)
        {
            DadosDoArquivoTextoAcao = arquivo;
        }

        public byte[] ObtemDadosDoArquivoTextoConsolidado()
        {
            return DadosDoArquivoTextoConsolidado;
        }

        public byte[] ObtemDadosDoArquivoTextoAcao()
        {
            return DadosDoArquivoTextoAcao;
        }
    }

    public class Indexacao
    {
        public int InTipoTermo { get; set; }
        public string NmTermo { get; set; }
        public string NmEspecificador { get; set; }
        public string NmTermoAuxiliar { get; set; }
        public string NmEspecificadorAuxiliar { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace TCDF_REPORT.OV
{
    public class NormaOV
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public string NumeroString { get; set; }
        public string Letra { get; set; }
        public DateTime DataAssinatura { get; set; }
        public long NumeroSequencial { get; set; }
        public string Ambito { get; set; }
        public string Apelido { get; set; }
        public DateTime DataDeAutuacao { get; set; }
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
        public string CampoDeOrdenamento { get; set; }
        public bool AtlzNorma { get; set; }
        public bool NovaNorma { get; set; }

        public byte[] DadosDoArquivoTextoConsolidado;
        public byte[] DadosDoArquivoTextoAcao;
        public string[] ListaReferenciasPessoasFisicasEJuridicas { get; set; }
        public string[] ListaAutoridades { get; set; }
        public TipoDeNormaOV Tipo { get; set; }
        public List<string> Autorias { get; set; }
        public List<RequerenteOV> Requerentes { get; set; }
        public List<RequeridoOV> Requeridos { get; set; }
        public List<ProcuradorResponsavelOV> ProcuradoresResponsaveis { get; set; }
        public List<InteressadoOV> Interessados { get; set; }
        public List<OrgaoOV> Origens { get; set; }
        public InformacoesSobreVersaoOV Versao { get; set; }
        public List<FonteOV> Fontes { get; set; }
        public List<VideEntreNormasOV> Vides { get; set; }
        public List<string> NeoIndexacao { get; set; }
        public List<RelatorOV> Relatores { get; set; }

        public string Rotulo { get; set; }
        public string Comentario { get; set; }

        public NormaOV()
        {
            Autorias = new List<string>();
            Requerentes = new List<RequerenteOV>();
            Requeridos = new List<RequeridoOV>();
            ProcuradoresResponsaveis = new List<ProcuradorResponsavelOV>();
            Interessados = new List<InteressadoOV>();
            Origens = new List<OrgaoOV>();
            Versao = new InformacoesSobreVersaoOV();
            Fontes = new List<FonteOV>();
            Vides = new List<VideEntreNormasOV>();
            NeoIndexacao = new List<string>();
            Relatores = new List<RelatorOV>();
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
                foreach (FonteOV fonte in Fontes)
                {
                    if (fonte.DataPublicacao != null)
                        menorData = (DateTime)(fonte.DataPublicacao < menorData ? fonte.DataPublicacao : menorData);
                }
                return menorData;
            }
        }

        /// <summary>
        /// Esse campo apenas é preenchido caso seja uma ação PGDF. (G2)
        /// </summary>
        public List<DecisaoOV> HistoricoDeDecisoes { get; set; }

        public void ArmazenaHistoricoDeDecisoes(List<DecisaoOV> historico)
        {
            HistoricoDeDecisoes = historico;
        }

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
}
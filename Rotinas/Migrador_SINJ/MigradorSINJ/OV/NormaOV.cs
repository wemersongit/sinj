using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{
    public class NormaLBW
    {
        public NormaLBW()
        {
            Autorias = new List<string>();
            Origens = new List<int>();
            Requerentes = new List<int>();
            Requeridos = new List<int>();
            Relatores = new List<string>();
            ProcuradoresResponsaveis = new List<int>();
            Interessados = new List<int>();
            NeoIndexacao = new List<NeoIndexacao>();
            Fontes = new List<FonteLBW>();
            HistoricoDeDecisoes = new List<DecisaoLBW>();
            Vides = new List<VideLBW>();
            AuxiliarDeRankeamento = new List<string>();
            ListaReferenciasPessoasFisicasEJuridicas = new string[0];
        }
        public string Id { get; set; }
        public string Id_Tipo { get; set; }
        public string Numero { get; set; }
        public string Letra { get; set; }
        public string Sequencial { get; set; }
        public string Ementa { get; set; }
        public string ListaDeNomes { get; set; }
        public string ObservacaoNorma { get; set; }
        public string DataAssinatura { get; set; }
        public string DataDeAutuacao { get; set; }
        public string Ambito { get; set; }
        public string Apelido { get; set; }
        public string NumeroSequencial { get; set; }
        public List<string> Autorias { get; set; }
        public string orgao_cadastrador { get; set; }
        public string Situacao { get; set; }
        public string UsuarioQueCadastrou { get; set; }
        public string DataDoCadastro { get; set; }
        public string UsuarioDaUltimaAlteracao { get; set; }
        public string DataDaUltimaAlteracao { get; set; }
        public string ChaveParaNaoDuplicacao { get; set; }
        public bool Destacada { get; set; }
        public List<int> Origens { get; set; }
        public List<int> Requerentes { get; set; }
        public List<int> Requeridos { get; set; }
        public List<string> Relatores { get; set; }
        public List<int> ProcuradoresResponsaveis { get; set; }
        public List<int> Interessados { get; set; }
        public List<NeoIndexacao> NeoIndexacao { get; set; }
        public List<FonteLBW> Fontes { get; set; }
        public string[] ListaReferenciasPessoasFisicasEJuridicas { get; set; }
        public bool HaPendencia { get; set; }
        public string Procedencia { get; set; }
        public string ParametroConstitucional { get; set; }
        public string EfeitoDaDecisao { get; set; }
        public List<DecisaoLBW> HistoricoDeDecisoes { get; set; }
        public string UrlReferenciaExterna { get; set; }
        public List<VideLBW> Vides { get; set; }
        public List<string> AuxiliarDeRankeamento { get; set; }
        public string CaminhoArquivoTextoConsolidado { get; set; }
        public string CaminhoArquivoTextoAcao { get; set; }
    }

    public class LegadoNormaLBW
    {
        public LegadoNormaLBW()
        {
            Autorias = new List<string>();
            Origens = new List<int>();
            Requerentes = new List<int>();
            Requeridos = new List<int>();
            Relatores = new List<string>();
            ProcuradoresResponsaveis = new List<int>();
            Interessados = new List<int>();
            NeoIndexacao = new List<NeoIndexacao>();
            Indexacao = new List<string>();
            Fontes = new List<FonteLBW>();
            HistoricoDeDecisoes = new List<DecisaoLBW>();
            Vides = new List<VideLBW>();
            AuxiliarDeRankeamento = new List<string>();
            ListaReferenciasPessoasFisicasEJuridicas = new string[0];
        }
        public string Id { get; set; }
        public string Id_Tipo { get; set; }
        public string Numero { get; set; }
        public string Letra { get; set; }
        public string Sequencial { get; set; }
        public string Ementa { get; set; }
        public string ListaDeNomes { get; set; }
        public string ObservacaoNorma { get; set; }
        public string DataAssinatura { get; set; }
        public string DataDeAutuacao { get; set; }
        public string Ambito { get; set; }
        public string Apelido { get; set; }
        public string NumeroSequencial { get; set; }
        public List<string> Autorias { get; set; }
        public string orgao_cadastrador { get; set; }
        public string Situacao { get; set; }
        public string UsuarioQueCadastrou { get; set; }
        public string DataDoCadastro { get; set; }
        public string UsuarioDaUltimaAlteracao { get; set; }
        public string DataDaUltimaAlteracao { get; set; }
        public string ChaveParaNaoDuplicacao { get; set; }
        public int Destacada { get; set; }
        public List<int> Origens { get; set; }
        public List<int> Requerentes { get; set; }
        public List<int> Requeridos { get; set; }
        public List<string> Relatores { get; set; }
        public List<int> ProcuradoresResponsaveis { get; set; }
        public List<int> Interessados { get; set; }
        public List<NeoIndexacao> NeoIndexacao { get; set; }
        public List<string> Indexacao { get; set; }
        public List<FonteLBW> Fontes { get; set; }
        public string[] ListaReferenciasPessoasFisicasEJuridicas { get; set; }
        public bool HaPendencia { get; set; }
        public string Procedencia { get; set; }
        public string ParametroConstitucional { get; set; }
        public string EfeitoDaDecisao { get; set; }
        public List<DecisaoLBW> HistoricoDeDecisoes { get; set; }
        public string UrlReferenciaExterna { get; set; }
        public List<VideLBW> Vides { get; set; }
        public List<string> AuxiliarDeRankeamento { get; set; }
        public string CaminhoArquivoTextoConsolidado { get; set; }
        public string CaminhoArquivoTextoAcao { get; set; }
    }

    public class ErroLegadoNormaLBW
    {
        public String id;
        public String mensagem;
        public String inner_exception;
    }

    [Serializable]
    public class NeoIndexacao
    {
        private int _inTipoTermo;
        private string _nmTermo;
        private string _nmEspecificador;
        /// <summary>
        /// Indica o tipo de termo. 1 - Descritor; 3 - Autoridade; 4 - Lista Auxiliar
        /// Obs.: O 2 é especificador mas não pode ser usado, porque a regra é clara: O especificador é usado para complementar os outros termos. Então
        /// ele é usado na propriedade NmEspecificador, é não pode existir sem um termo;
        /// </summary>
        public int InTipoTermo
        {
            get
            {
                return _inTipoTermo;
            }
            set
            {
                if (value != 1 && value != 3 && value != 4)
                {
                    throw new Exception("Tipo de termo incorreto em NeoIndexação da norma.");
                }
                _inTipoTermo = value;
            }
        }

        public string GetSiglaTipoTermo()
        {
            var tipo = "DE";
            if (_inTipoTermo == 1)
            {
                tipo = "DE";
            }
            else if (_inTipoTermo == 3)
            {
                tipo = "AU";
            }
            else if (_inTipoTermo == 2)
            {
                tipo = "ES";
            }
            else if (_inTipoTermo == 4)
            {
                tipo = "LA";
            }
            return tipo;
        }

        /// <summary>
        /// Nome do Termo oriundo de vocabulário controlado.
        /// Obs.: É necessário informar o tipo do termo antes preencher esta propriedade.
        /// </summary>
        public string NmTermo
        {
            get
            {
                return _nmTermo;
            }
            set
            {
                if (InTipoTermo == 0)
                {
                    throw new Exception("Criando termo sem informar o tipo em NeoIndexação da norma.");
                }
                _nmTermo = value;
            }
        }

        /// <summary>
        /// Nome do Especificador. É oriundo de vicabulário controlado.
        /// Obs.: É necessário informar o nome do Termo (NmTermo) antes de preencher esta propriedade.
        /// </summary>
        public string NmEspecificador
        {
            get
            {
                return _nmEspecificador;
            }
            set
            {
                if (string.IsNullOrEmpty(NmTermo))
                {
                    //throw new Exception("Criando especificador sem informar o termo em NeoIndexação da norma.");
                }
                _nmEspecificador = value;
            }
        }
    }
    public class NormaOV
    {
        public NormaOV()
        {
            nm_pessoa_fisica_e_juridica = new List<string>();
            ar_atualizado = new ArquivoOV();
            ar_acao = new ArquivoOV();
            rankeamentos = new List<string>();
            requerentes = new List<Requerente>();
            requeridos = new List<Requerido>();
            relatores = new List<Relator>();
            procuradores_responsaveis = new List<Procurador>();
            interessados = new List<Interessado>();
            alteracoes = new List<AlteracaoOV>();
            origens = new List<Orgao>();
            autorias = new List<Autoria>();
            fontes = new List<Fonte>();
            decisoes = new List<Decisao>();
            indexacoes = new List<Indexacao>();
            vides = new List<Vide>();
            ch_para_nao_duplicacao = new List<string>();
            _metadata = new metadata();
        }
        public metadata _metadata { get; set; }
        public string ch_tipo_norma { get; set; }
        public string nm_tipo_norma { get; set; }
        public string ch_norma { get; set; }
        public string ds_comentario { get; set; }
        public int nr_norma { get; set; }
        public int nr_sequencial { get; set; }
        public string dt_assinatura { get; set; }
        public string cr_norma { get; set; }
        public int id_ambito { get; set; }
        public string nm_ambito { get; set; }
        public string nm_apelido { get; set; }
        public string dt_autuacao { get; set; }
        public string ds_procedencia { get; set; }
        public string ds_parametro_constitucional { get; set; }
        public string ds_acao { get; set; }
        public string ds_efeito_decisao { get; set; }
        public string url_referencia { get; set; }
        public string url_projeto_lei { get; set; }
        public string nr_projeto_lei { get; set; }
        public int id_orgao_cadastrador { get; set; }
        public string nm_orgao_cadastrador { get; set; }
        public List<string> ch_para_nao_duplicacao { get; set; }
        public bool st_pendencia { get; set; }

        public bool st_habilita_pesquisa { get; set; }
        public bool st_habilita_email { get; set; }

        public string ds_pendencia { get; set; }
        public bool st_destaque { get; set; }
        public string ds_observacao { get; set; }
        public string ds_ementa { get; set; }
        public List<string> nm_pessoa_fisica_e_juridica { get; set; }
        public ArquivoOV ar_atualizado { get; set; }
        public ArquivoOV ar_acao { get; set; }
        public bool st_atualizada { get; set; }
        public bool st_nova { get; set; }
        public bool st_acao { get; set; }
        public List<string> rankeamentos { get; set; }
        public string ch_situacao { get; set; }
        public string nm_situacao { get; set; }
        public List<Requerente> requerentes { get; set; }
        public List<Requerido> requeridos { get; set; }
        public List<Relator> relatores { get; set; }
        public List<Procurador> procuradores_responsaveis { get; set; }
        public List<Interessado> interessados { get; set; }
        public List<Orgao> origens { get; set; }
        public List<Autoria> autorias { get; set; }
        public List<Fonte> fontes { get; set; }
        /// <summary>
        /// Esse campo apenas é preenchido caso seja uma ação PGDF. (G2)
        /// </summary>
        public List<Decisao> decisoes { get; set; }
        public List<Indexacao> indexacoes { get; set; }
        public int in_vides { get { return vides.Count; } }
        public List<Vide> vides { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
    }
}

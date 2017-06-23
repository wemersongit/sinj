using System;
using System.Collections.Generic;
using neo.BRLightREST;

namespace TCDF.Sinj.OV
{
    public class NormaOV : metadata
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
        }

        public string ch_norma { get; set; }
        public string ds_comentario { get; set; }
        public string nr_norma { get; set; }
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
        public int id_orgao_cadastrador { get; set; }
        public string nm_orgao_cadastrador { get; set; }
        public List<string> ch_para_nao_duplicacao { get; set; }
        public bool st_pendencia { get; set; }
        public string ds_pendencia { get; set; }
        public bool st_destaque { get; set; }
        public string ds_observacao { get; set; }
        public string ds_ementa { get; set; }
        public List<string> nm_pessoa_fisica_e_juridica { get; set; }
        public ArquivoOV ar_atualizado { get; set; }
        public ArquivoOV ar_acao { get; set; }
        public bool st_atualizada { get; set; }
        public bool st_nova { get; set; }
        public List<string> rankeamentos { get; set; }
        public bool st_acao { get; set; }
        public string ch_tipo_norma { get; set; }
        public string nm_tipo_norma { get; set; }
        /// <summary>
        /// O administrador do sistema pode escolher a situação da norma e a mesma não é alterada pelos demais usuários e, principalmente, pelos vides
        /// </summary>
        public bool st_situacao_forcada { get; set; }
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
        public bool st_vigor_vide { get; set; }
        public List<Vide> vides { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }

        public string ax_ajuste { get; set; }

        public string getIdFileArquivoVigente()
        {
            var id_file = "";
            if (!string.IsNullOrEmpty(ar_atualizado.id_file))
            {
                id_file = ar_atualizado.id_file;
            }
            else
            {
                if (fontes.Count > 0)
                {
                    if (fontes[0].ar_fonte != null && !string.IsNullOrEmpty(fontes[0].ar_fonte.id_file))
                    {
                        id_file = fontes[0].ar_fonte.id_file;
                    }
                    foreach (var fonte in fontes)
                    {
                        if (!string.IsNullOrEmpty(fonte.ar_fonte.id_file) && (fonte.nm_tipo_publicacao.Equals("republicação", StringComparison.InvariantCultureIgnoreCase) || fonte.nm_tipo_publicacao.Equals("rep", StringComparison.InvariantCultureIgnoreCase)))
                        {
                            id_file = fonte.ar_fonte.id_file;
                        }
                    }
                }
            }
            return id_file;
        }

        public string getNameFileArquivoVigente()
        {
            var name_file = "";
            if (!string.IsNullOrEmpty(ar_atualizado.filename))
            {
                name_file = ar_atualizado.filename;
            }
            else
            {
                if (fontes.Count > 0)
                {
                    if (fontes[0].ar_fonte != null && !string.IsNullOrEmpty(fontes[0].ar_fonte.filename))
                    {
                        name_file = fontes[0].ar_fonte.filename;
                    }
                    foreach (var fonte in fontes)
                    {
                        if (!string.IsNullOrEmpty(fonte.ar_fonte.filename) && (fonte.nm_tipo_publicacao.Equals("republicação", StringComparison.InvariantCultureIgnoreCase) || fonte.nm_tipo_publicacao.Equals("rep", StringComparison.InvariantCultureIgnoreCase)))
                        {
                            name_file = fonte.ar_fonte.filename;
                        }
                    }
                }
            }
            return name_file;
        }

        public string getPreposicaoNorma()
        {
            var nm_tipo_norma = util.BRLight.ManipulaTexto.RetiraCaracteresEspeciais(this.nm_tipo_norma.ToLower(),true);
            var prep = "por";
            var dictionary = new Dictionary<string, string>() {
                {"adc","pela"},
                {"adi","pela"},
                {"ado","pela"},
                {"adpf","pela"},
                {"ail","pela"},
                {"ata","pela"},
                {"atoconjunto","pelo"},
                {"atodamesadiretora","pelo"},
                {"atodeclaratorio","pelo"},
                {"atonormativo","pelo"},
                {"atoregimental","pelo"},
                {"aviso","pelo"},
                {"contratosocial","pelo"},
                {"decisao","pela"},
                {"decisaodopresidente","pela"},
                {"decisaoliminar","pela"},
                {"portaria","pela"},
                {"portariaconjunta","pela"},
                {"decisaonormativa","pela"},
                {"decreto","pelo"},
                {"decretoexecutivo","pelo"},
                {"decretolegislativo","pelo"},
                {"decretolei","pelo"},
                {"despacho","pelo"},
                {"despachodogovernador","pelo"},
                {"despachosingular","pelo"},
                {"diretriz","pela"},
                {"termoaditivo","pelo"},
                {"decisaoadministrativa","pela"},
                {"edital","pelo"},
                {"emendaconstitucional","pela"},
                {"emendaregimental","pela"},
                {"portariareservada","pela"},
                {"instrucaonormativaconjunta","pela"},
                {"ordemdeserviconormativa","pela"},
                {"estatuto","pelo"},
                {"instrucao","pela"},
                {"leicomplementar","pela"},
                {"normatecnica","pela"},
                {"regimento","pelo"},
                {"regulamento","pelo"},
                {"exposicaodemotivos","pela"},
                {"instrucaonormativa","pela"},
                {"normadeservico","pela"},
                {"ordemdeservico","pela"},
                {"regimentointerno","pelo"},
                {"instrucaoconjunta","pela"},
                {"parecernormativo","pelo"},
                {"rejeicaodeveto","pela"},
                {"resolucao","pela"},
                {"instrucaodeservico","pela"},
                {"plano","pelo"},
                {"representacao","pela"},
                {"instrucaodeservicoconjunta","pela"},
                {"leiorganica","pela"},
                {"manual","pelo"},
                {"mensagemdogovernador","pela"},
                {"ordemdeservicoconjunta","pela"},
                {"orientacaonormativa","pela"},
                {"parecer","pelo"},
                {"portarianormativa","pela"},
                {"processo","pelo"},
                {"provimento","pelo"},
                {"recomendacao","pela"},
                {"resolucaoadministrativa","pela"},
                {"resolucaoconjunta","pela"},
                {"resolucaointergovernamental","pela"},
                {"resolucaonormativa","pela"},
                {"sumula","pela"},
                {"lei","pela"},
                {"emendaaleiorganica","pela"}
            };
            if (dictionary.ContainsKey(nm_tipo_norma))
            {
                prep = dictionary[nm_tipo_norma];
            }
            return prep;
        }

        public string getDescricaoDaNorma()
        {
            return nm_tipo_norma + " " + (!string.IsNullOrEmpty(nr_norma) || nr_norma != "0" ? nr_norma : "") + " de " + dt_assinatura;
        }
    }
	
	public class NormaDetalhada : NormaOV
	{
		public NormaDetalhada ()
		{
			origensOv = new List<OrgaoOV>();
		}
		public List<OrgaoOV> origensOv { get; set; }
        public TipoDeNorma tipoDeNorma { get; set; }
	}
}
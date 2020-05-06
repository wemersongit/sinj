using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;

namespace TCDF.Sinj.OV
{
    public class VocabularioOV : metadata
    {
        public VocabularioOV()
        {
            alteracoes = new List<AlteracaoOV>();
            termos_gerais = new List<Vocabulario_TG>();
            termos_especificos = new List<Vocabulario_TE>();
            termos_relacionados = new List<Vocabulario_TR>();
            termos_nao_autorizados = new List<Vocabulario_TNA>();
        }

        public string ch_termo { get; set; }
        public string nm_termo { get; set; }
        /// <summary>
        /// DE = Descritor, ES = Especificador, AU = Autoridade, LA = Lista Auxiliar
        /// </summary>
        public string ch_tipo_termo { get; set; }

        public string ch_orgao { get; set; }

        public bool in_lista { get; set; }
        public string ch_lista_superior { get; set; }
        public string nm_lista_superior { get; set; }

        public bool in_nao_autorizado { get; set; }
        public string ch_termo_use { get; set; }
        public string nm_termo_use { get; set; }

        public bool st_ativo { get; set; }
        public bool st_aprovado { get; set; }
        public bool st_excluir { get; set; }
        public bool st_restaurado { get; set; }

        public string ds_nota_explicativa { get; set; }
        public string ds_fontes_pesquisadas { get; set; }
        public string ds_texto_fonte { get; set; }

        public List<Vocabulario_TG> termos_gerais { get; set; }
        public List<Vocabulario_TE> termos_especificos { get; set; }
        public List<Vocabulario_TR> termos_relacionados { get; set; }
        public List<Vocabulario_TNA> termos_nao_autorizados { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }

        public bool EhTipoDescritor()
        {
            return ch_tipo_termo != null && ch_tipo_termo.ToUpper() == "DE";
        }

        public bool EhTipoEspecificador()
        {
            return ch_tipo_termo != null && ch_tipo_termo.ToUpper() == "ES";
        }

        public bool EhTipoAutoridade()
        {
            return ch_tipo_termo != null && ch_tipo_termo.ToUpper() == "AU";
        }

        public bool EhTipoLista()
        {
            return ch_tipo_termo != null && ch_tipo_termo.ToUpper() == "LA";
        }
    }

    public class Vocabulario_TR
    {
        public string ch_termo_relacionado { get; set; }
        public string nm_termo_relacionado { get; set; }
    }

    public class Vocabulario_TG
    {
        public string ch_termo_geral { get; set; }
        public string nm_termo_geral { get; set; }
    }

    public class Vocabulario_TE
    {
        public string ch_termo_especifico { get; set; }
        public string nm_termo_especifico { get; set; }
    }

    public class Vocabulario_TNA
    {
        public string ch_termo_nao_autorizado { get; set; }
        public string nm_termo_nao_autorizado { get; set; }
    }

    public class Vocabulario_Lista
    {
        public string ch_termo { get; set; }
        public string nm_termo { get; set; }
    }

    public class RelacionamentoDeVocabulario
    {
        public string chave { get; set; }
        public string nome { get; set; }
    }

    public class ArvoreDeVocabulario
    {
        public ArvoreDeVocabulario()
        {
            tgs = new List<ArvoreDeVocabulario>();
            tes = new List<ArvoreDeVocabulario>();
            trs = new List<ArvoreDeVocabulario>();
        }
        public string ch_termo { get; set; }
        public string nm_termo { get; set; }
        public List<ArvoreDeVocabulario> tgs { get; set; }
        public List<ArvoreDeVocabulario> tes { get; set; }
        public List<ArvoreDeVocabulario> trs { get; set; }
    }

    public class Vocabulario
    {
        public string ch_termo { get; set; }
        public string nm_termo { get; set; }
        public string ch_tipo_termo { get; set; }
    }

    public class Indexacao
    {
        public Indexacao()
        {
            vocabulario = new List<Vocabulario>();
        }
        public List<Vocabulario> vocabulario { get; set; }
    }

    public class VocabularioDetalhado : VocabularioOV
    {
        public VocabularioDetalhado()
        {
            lista = new Vocabulario_Lista();
            sublistas = new List<Vocabulario_Lista>();
            itens = new List<Vocabulario_Lista>();
            arvore = new ArvoreDeVocabulario();
        }
        public Vocabulario_Lista lista { get; set; }
        /// <summary>
        /// Usado para exibir as listas em árvore
        /// </summary>
        public VocabularioDetalhado lista_pai { get; set; }
        public List<Vocabulario_Lista> sublistas { get; set; }
        public List<Vocabulario_Lista> itens { get; set; }
        public ArvoreDeVocabulario arvore { get; set; }
        public string nm_tipo_termo
        {
            get
            {
                switch (ch_tipo_termo)
                {
                    case "DE":
                        return "Descritor";
                    case "ES":
                        return "Especificador";
                    case "AU":
                        return "Autoridade";
                    case "LA":
                        if (in_lista && string.IsNullOrEmpty(ch_lista_superior)) return "Lista Auxiliar";
                        if (in_lista) return "Sublista";
                        return "Item";
                    default:
                        return "Tipo não identificado";
                }
            }
        }
        public bool eh_descritor { get { return EhTipoDescritor(); } }
        public bool eh_especificador { get { return EhTipoEspecificador(); } }
        public bool eh_autoridade { get { return EhTipoAutoridade(); } }
        public bool eh_lista_auxiliar { get { return EhTipoLista(); } }

        public string ds_autocomplete { get; set; }
    }

    public class TermoHomonimo
    {
        public int nr_total { get; set; }
        public string nm_termo { get; set; }
    }

    public class VocabularioTroca
    {
        public VocabularioTroca()
        {
            id_docs_erro = new List<ulong>();
            id_docs_sucesso = new List<ulong>();
        }
        public string nm_termo_trocado { get; set; }
        public ulong total_de_normas { get; set; }
        public List<ulong> id_docs_erro { get; set; }
        public List<ulong> id_docs_sucesso { get; set; }
        public string error_message { get; set; }
        public bool bExcluido { get; set; }
        public bool bAtualizado { get; set; }
    }
}

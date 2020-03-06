using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{
    public class VocabularioControladoLBW
    {
        public VocabularioControladoLBW()
        {
            In_Ativo = true;
            TermosEspecificos = new List<TermoVocabularioControladoRelacionamento>();
            TermosGerais = new List<TermoVocabularioControladoRelacionamento>();
            TermosRelacionados = new List<TermoVocabularioControladoRelacionamento>();
            TermosNaoAutorizados = new List<TermoVocabularioControladoRelacionamento>();
            TermosItensDaLista = new List<TermoVocabularioControladoRelacionamento>();
            TermosSublistas = new List<TermoVocabularioControladoRelacionamento>();
            TermosListas = new List<TermoVocabularioControladoRelacionamento>();
            Ids_TermosGerais = new string[0];
            Ids_TermosEspecificos = new string[0];
            Ids_TermosRelacionados = new string[0];
            Ids_TermosNaoAutorizados = new string[0];
            Ids_TermosSubListas = new string[0];
            Ids_TermosItensDaLista = new string[0];
        }
        public string Id_Termo { get; set; }
        public string ch_termo { get; set; }
        public string Id_ListaRelacao { get; set; }
        public int Id_Orgao { get; set; }
        public string Nm_Termo { get; set; }
        public string Nm_Auxiliar { get; set; }
        public string Nm_ListaRelacao { get; set; }
        public bool In_TermoNaoAutorizado { get; set; }

        //1-Descritor; 2-Especificador; 3-Autoridade;
        public int In_TipoTermo { get; set; }
        public bool In_Lista { get; set; }
        public int In_NivelLista { get; set; }
        public string NotaExplicativa { get; set; }
        public string FonteCatalogadora { get; set; }
        public string FonteAlteradora { get; set; }
        public string TextoFonte { get; set; }
        public bool In_Aprovado { get; set; }
        public bool In_Ativo { get; set; }
        public bool In_Excluir { get; set; }
        public string FontesPesquisadas { get; set; }

        public string Dt_Cadastro { get; set; }
        public string Dt_UltimaAlteracao { get; set; }
        public string Nm_UsuarioUltimaAlteracao { get; set; }
        public string Nm_UsuarioCadastro { get; set; }

        public string[] Ids_TermosGerais { get; set; }
        public string[] Ids_TermosEspecificos { get; set; }
        public string[] Ids_TermosRelacionados { get; set; }
        public string[] Ids_TermosNaoAutorizados { get; set; }
        public string[] Ids_TermosSubListas { get; set; }
        public string[] Ids_TermosItensDaLista { get; set; }

        public VocabularioControladoLBW ListaPai { get; set; }

        public List<TermoVocabularioControladoRelacionamento> TermosItensDaLista { get; set; }
        public List<TermoVocabularioControladoRelacionamento> TermosSublistas { get; set; }
        public List<TermoVocabularioControladoRelacionamento> TermosListas { get; set; }
        public List<TermoVocabularioControladoRelacionamento> TermosEspecificos { get; set; }
        public List<TermoVocabularioControladoRelacionamento> TermosGerais { get; set; }
        public List<TermoVocabularioControladoRelacionamento> TermosRelacionados { get; set; }
        public List<TermoVocabularioControladoRelacionamento> TermosNaoAutorizados { get; set; }
        public TermoVocabularioControladoRelacionamento TermoUse { get; set; }

        public string getTipoTermoMigracao()
        {
            switch (In_TipoTermo)
            {
                case 1:
                    return "DE";
                case 2:
                    return "ES";
                case 3:
                    return "AU";
                case 4:
                    return "LA";
            }
            return "DE";
        }
    }

    public class TermoVocabularioControladoRelacionamento
    {
        public string Id_ListaRelacao { get; set; }
        public string Id_Termo { get; set; }
        public string Nm_Termo { get; set; }
    }

    public class TermoRelacaoTermoGeralEEspecifico
    {
        public string Id_TermoGeral { get; set; }
        public string Nm_TermoGeral { get; set; }
        public string Id_TermoEspecifico { get; set; }
        public string Nm_TermoEspecifico { get; set; }
    }

    public class TermoRelacaoTermoRelacionado
    {
        public string Id_Termo { get; set; }
        public string Nm_Termo { get; set; }
        public string Id_TermoRelacionado { get; set; }
        public string Nm_TermoRelacionado { get; set; }
    }

    public class TermoRelacaoTermoNaoAutorizado
    {
        public string Id_TermoUse { get; set; }
        public string Nm_TermoUse { get; set; }
        public string Id_TermoNaoAutorizado { get; set; }
        public string Nm_TermoNaoAutorizado { get; set; }
    }

    public class TermoRelacaoTermoDescritorEEspecificador
    {
        public string Id_TermoDescritor { get; set; }
        public string Nm_TermoDescritor { get; set; }
        public string Id_TermoEspecificador { get; set; }
        public string Nm_TermoEspecificador { get; set; }
    }



    public class VocabularioControladoOV
    {
        public VocabularioControladoOV()
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
        public string ch_termo_use{get;set;}
        public string nm_termo_use{get;set;}

        public bool st_ativo { get; set; }
        public bool st_aprovado { get; set; }
        public bool st_excluir { get; set; }

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
    }

    public class Vocabulario_TR{
        public string ch_termo_relacionado {get;set;}
        public string nm_termo_relacionado {get;set;}
    }

    public class Vocabulario_TG{
        public string ch_termo_geral { get; set; }
        public string nm_termo_geral { get; set; }
    }

    public class Vocabulario_TE{
        public string ch_termo_especifico { get; set; }
        public string nm_termo_especifico { get; set; }
    }

    public class Vocabulario_TNA{
        public string ch_termo_nao_autorizado { get; set; }
        public string nm_termo_nao_autorizado { get; set; }
    }

    public class Vocabulario
    {
        public string ch_termo { get; set; }
        public string nm_termo { get; set; }
        public string ch_tipo_termo { get; set; }
    }

    public class AuxReindexacao
    {
        public int index { get; set; }
        public Indexacao indexacao { get; set; }
    }

    public class Indexacao
    {
        public Indexacao()
        {
            vocabulario = new List<Vocabulario>();
        }
        public List<Vocabulario> vocabulario { get; set; }
    }
}

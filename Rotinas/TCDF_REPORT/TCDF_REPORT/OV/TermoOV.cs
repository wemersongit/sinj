using System;
using System.Collections.Generic;

namespace TCDF_REPORT.OV
{
    public class TermoOV : IComparable
    {
        public TermoOV()
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

        public TermoOV ListaPai { get; set; }

        public List<TermoVocabularioControladoRelacionamento> TermosItensDaLista { get; set; }
        public List<TermoVocabularioControladoRelacionamento> TermosSublistas { get; set; }
        public List<TermoVocabularioControladoRelacionamento> TermosListas { get; set; }
        public List<TermoVocabularioControladoRelacionamento> TermosEspecificos { get; set; }
        public List<TermoVocabularioControladoRelacionamento> TermosGerais { get; set; }
        public List<TermoVocabularioControladoRelacionamento> TermosRelacionados { get; set; }
        public List<TermoVocabularioControladoRelacionamento> TermosNaoAutorizados { get; set; }
        public TermoVocabularioControladoRelacionamento TermoUse { get; set; }

        public string getTipoTermo()
        {
            switch (In_TipoTermo)
            {
                case 1:
                    return "Descritor";
                case 2:
                    return "Especificador";
                case 3:
                    return "Autoridade";
                case 4:
                    return "Lista Auxiliar";
            }
            return "";
        }

        public int CompareTo(object obj)
        {
            return Nm_Termo.CompareTo(((TermoOV)obj).Nm_Termo);
        }

        public string CampoOrdenavel { get; set; }
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
}
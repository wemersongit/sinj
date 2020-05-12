using System;
using System.Collections.Generic;
using System.Data;

namespace Sinj.Notifica.Objetos
{
    [Serializable]
    public class Push
    {
        public int? Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string passNoMd5 { get; set; }
        public DateTime DtCadUsuario { get; set; }
        public DateTime DtCadAssinatura { get; set; }
        public bool AtivoUsuario { get; set; }
        public List<AtosVerifAtlzcao> AtosVerifAtlzcaoValue { get; set; }
        public List<NovosAtosPorCriterios> NovosAtosPorCriteriosValue { get; set; }
    }

    public class AtosVerifAtlzcao
    {
        public string IdAtosVerifAtlzcao { get; set; }
        public string IdNorma { get; set; }
        public DateTime DtCadAtosVerifAtlzcao { get; set; }
        public bool AtivoItemAtosVerifAtlzcao { get; set; }
        public string IdentificadorNorma { get; set; }
        public string TipoDeNorma { get; set; }
        public string Numero { get; set; }
        public DateTime DataAssinatura { get; set; }

        public AtosVerifAtlzcao(DataRow dataRow)
        {
            IdAtosVerifAtlzcao = dataRow["IdAtosVerifAtlzcao"] as string;
            IdNorma = dataRow["IdNorma"].ToString();
            DtCadAtosVerifAtlzcao = Convert.ToDateTime(dataRow["DtCadAtosVerifAtlzcao"]);
            AtivoItemAtosVerifAtlzcao = dataRow["AtivoItemAtosVerifAtlzcao"] is bool ? (bool)dataRow["AtivoItemAtosVerifAtlzcao"] : false;
        }

        public AtosVerifAtlzcao()
        {

        }
    }

    public class NovosAtosPorCriterios
    {
        const string CONECTORE = "CONECTORE", CONECTOROU = "CONECTOROU", CONECTORERTRN = "E", CONECTOROURTRN = "OU";

        public string IdNovosAtosPorCriterios { get; set; }

        public int TipoAto { get; set; }

        public string TipoAtoDescricao { get; set; }

        private string primeiroConec;

        public string PrimeiroConec
        {
            get
            {
                if (primeiroConec == CONECTORE)
                {
                    return CONECTORERTRN;
                }
                else if (primeiroConec == CONECTOROU)
                {
                    return CONECTOROURTRN;
                }
                else if (primeiroConec == "")
                {
                    return "";
                }
                return null;
            }
            set
            {
                if (value == CONECTORE)
                {
                    primeiroConec = CONECTORE;
                }
                else if (value == CONECTOROU)
                {
                    primeiroConec = CONECTOROU;
                }
                else if (value == "E")
                {
                    primeiroConec = CONECTORE;
                }
                else if (value == "OU")
                {
                    primeiroConec = CONECTOROU;
                }
                else if (value == "")
                {
                    primeiroConec = "";
                }
            }
        }

        public int Origem { get; set; }

        public string OrigemDescricao { get; set; }

        private string segundoConec;

        public string SegundoConec
        {
            get
            {
                if (segundoConec == CONECTORE)
                {
                    return CONECTORERTRN;
                }
                else if (segundoConec == CONECTOROU)
                {
                    return CONECTOROURTRN;
                }
                else if (segundoConec == "")
                {
                    return "";
                }
                return null;
            }
            set
            {
                if (value == CONECTORE)
                {
                    segundoConec = CONECTORE;
                }
                else if (value == CONECTOROU)
                {
                    segundoConec = CONECTOROU;
                }
                else if (value == "E")
                {
                    segundoConec = CONECTORE;
                }
                else if (value == "OU")
                {
                    segundoConec = CONECTOROU;
                }
                else if (value == "")
                {
                    segundoConec = "";
                }
            }
        }

        public string Indexacao { get; set; }
        public DateTime DtCadNovosAtosPorCriterios { get; set; }
        public bool AtivoItemNovosAtosPorCriterios { get; set; }

        public NovosAtosPorCriterios(DataRow dataRow)
        {
            IdNovosAtosPorCriterios = dataRow["IdNovosAtosPorCriterios"] as string;
            TipoAto = dataRow["TipoAto"] is int ? (int)dataRow["TipoAto"] : 0;
            Origem = dataRow["Origem"] is int ? (int)dataRow["Origem"] : 0;
            Indexacao = dataRow["Indexacao"] as string;
            DtCadNovosAtosPorCriterios = Convert.ToDateTime(dataRow["DtCadNovosAtosPorCriterios"]);
            AtivoItemNovosAtosPorCriterios = dataRow["AtivoItemNovosAtosPorCriterios"] is bool ? (bool)dataRow["AtivoItemNovosAtosPorCriterios"] : false;
        }

        public NovosAtosPorCriterios()
        {

        }
    }
}
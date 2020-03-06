using System;
using System.Data;

namespace Exportador_LB_to_ES.AD.Models
{
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

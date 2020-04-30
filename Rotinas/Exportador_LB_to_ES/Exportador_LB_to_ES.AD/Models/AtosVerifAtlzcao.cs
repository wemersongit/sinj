using System;
using System.Data;

namespace Exportador_LB_to_ES.AD.Models
{
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
}

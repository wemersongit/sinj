using TCDF_REPORT.OV;

namespace TCDF_REPORT.AD
{
    public class NormaAD
    {
        private AcessaDados _ad;
        public NormaAD(string stringConnection)
        {
            _ad = new AcessaDados(stringConnection);
        }

        public int ContarNormasQueContemOTermo(TermoOV termoOv)
        {
            int total;
            string sql = string.Format("textsearch in {0} \"{1}\"[{3}] OU \"{2}\"[{3}] OU \" {1}\"[{3}] OU \" {2}\"[{3}]", "VersoesDasNormas", termoOv.Nm_Termo, termoOv.Nm_Auxiliar, (termoOv.In_TipoTermo == 2 ? "NmEspecificadorAuxiliar" : "NmTermoAuxiliar"));
            using(var dr = _ad.ExecuteDataReader(sql))
            {
                total = dr.Count;
                dr.Close();
            }
            return total;
        }
    }
}
using neo.BRLightREST;

namespace TCDF.Sinj.OV
{
    public class OrgaoCadastradorOV : metadata
    {
        public int id_orgao_cadastrador { get; set; }
        public string nm_orgao_cadastrador { get; set; }
    }

    public class OrgaoCadastrador
    {
        public int id_orgao_cadastrador { get; set; }
        public string nm_orgao_cadastrador { get; set; }
    }
}
using System;

namespace TCDF_REPORT.OV
{
    public class DecisaoOV
    {
        public long IdDaNorma { get; set; }
        public long Id { get; set; }
        public DateTime DataDaPublicacao { get; set; }
        public TipoDeDecisaoOV Tipo { private get; set; }
        public string Complemento { get; set; }

        public string DescricaoDoTipo
        {
            get
            {
                switch (Tipo)
                {
                    case TipoDeDecisaoOV.Extinta:
                        return "Extinta";
                    case TipoDeDecisaoOV.LiminarDeferida:
                        return "Liminar Deferida";
                    case TipoDeDecisaoOV.LiminarIndeferida:
                        return "Liminar Indeferida";
                    case TipoDeDecisaoOV.MeritoProcedente:
                        return "Mérito Procedente";
                    case TipoDeDecisaoOV.MeritoImprocedente:
                        return "Mérito Improcedente";
                    default:
                        return "Não tem definido";
                }
            }
        }
    }
}
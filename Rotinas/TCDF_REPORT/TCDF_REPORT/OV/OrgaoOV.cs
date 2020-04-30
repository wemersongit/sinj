using System.Text;

namespace TCDF_REPORT.OV
{
    public class OrgaoOV
    {
        public int Id { get; set; }
        public string IdString { get; set; }
        public string Descricao { get; set; }
        public string Sigla { get; set; }
        public string CodigoOrgaoPai { get; set; }
        public string Codigo { get; set; }
        public string CodigoAssociacao { get; set; }
        public string Status { get; set; }
        public string Ambito { get; set; }
        public string OrgaoCadastrador { get; set; }
        //public string OrgaoCadastrador { get; set; }
        public string DtVigencia { get; set; }
        public string DtFimVigencia { get; set; }

        public string DtCriacao { get; set; }
        public string DtExtincao { get; set; }
        public string DescricaoComSigla { get; set; }

        public OrgaoOV OrgaoPaI {get;set;}
        public string HierarquiaApenasSiglas
        {
            get
            {
                StringBuilder hierarquia = new StringBuilder(Sigla);
                OrgaoOV pai = OrgaoPaI;
                while (pai != null)
                {
                    if (!hierarquia.ToString().Contains(pai.Sigla))
                    {
                        hierarquia.Append("/");
                        hierarquia.Append(pai.Sigla);
                    }
                    pai = pai.OrgaoPaI;
                }
                return hierarquia.ToString();
            }
        }

        public string HierarquiaInvertidaApenasSiglas
        {
            get
            {
                string[] hierarquia = HierarquiaApenasSiglas.Split('/');
                string[] hierarquiaInvertida = new string[hierarquia.Length];
                int j = 0;
                for (int i = hierarquia.Length - 1; i >= 0; i--) hierarquiaInvertida[j++] = hierarquia[i];
                return string.Join(", ", hierarquiaInvertida);
            }
        }

        public string Hierarquia
        {
            get
            {
                StringBuilder hierarquia = new StringBuilder(Descricao);
                OrgaoOV pai = OrgaoPaI;
                while (pai != null)
                {
                    if (!hierarquia.ToString().Contains(pai.Descricao))
                    {
                        hierarquia.Append("/");
                        hierarquia.Append(pai.Descricao);
                    }
                    pai = pai.OrgaoPaI;
                }
                return hierarquia.ToString();
            }
        }

        public string HierarquiaInvertida
        {
            get
            {
                string[] hierarquia = Hierarquia.Split('/');
                string[] hierarquiaInvertida = new string[hierarquia.Length];
                int j = 0;
                for (int i = hierarquia.Length - 1; i >= 0; i--) hierarquiaInvertida[j++] = hierarquia[i];
                return string.Join(", ", hierarquiaInvertida);
            }
        }

        public string HieraraquiaInvertidaComSigla
        {
            get
            {
                return string.Format("{0} - {1}", Sigla, HierarquiaInvertida);
            }
        }

        public string HieraraquiaComSigla
        {
            get
            {
                return string.Format("{0} - {1}", Sigla, Hierarquia);
            }
        }

        public string Texto
        {
            get { return string.Format("{0} ({1})", Descricao, Sigla); }
        }
    }
}
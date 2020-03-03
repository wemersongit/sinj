using System;
using System.Text;

namespace Sinj.Notifica.Objetos
{
    public class OrgaoSinj : IComparable
    {
        public int? Id { get; set; }
        public string IdString { get; set; }
        public int? IdSileg { get; set; }
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
        public string DescricaoComSigla
        {
            get
            {
                return string.Format("{0} - {1}", Sigla, Descricao);
            }
        }

        public OrgaoSinj OrgaoPaI
        {
            get;
            set;
        }
        public string HierarquiaApenasSiglas
        {
            get
            {
                StringBuilder hierarquia = new StringBuilder(Sigla);
                OrgaoSinj pai = OrgaoPaI;
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
                OrgaoSinj pai = OrgaoPaI;
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

        public override bool Equals(object obj)
        {
            if (!(obj is OrgaoSinj))
            {
                return false;
            }
            return Equals(((OrgaoSinj)obj).Codigo, Codigo);
        }

        public override int GetHashCode()
        {
            if (Codigo == null)
            {
                return string.Empty.GetHashCode();
            }
            return Codigo.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            return Descricao.CompareTo(((OrgaoSinj)obj).Descricao);
        }

        #region Implementation of IValidavel

        public void Valida()
        {


            if (string.IsNullOrEmpty(Descricao))
            {
                throw new Exception("Descrição é obrigatório");
            }
            if (string.IsNullOrEmpty(Sigla))
            {
                throw new Exception("Sigla é obrigatório");
            }
            if (string.IsNullOrEmpty(DtVigencia))
            {
                throw new Exception("Data de criação é obrigatório");
            }
        }

        #endregion
    }
}
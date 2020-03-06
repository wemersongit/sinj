using System;

namespace Exportador_LB_to_ES.AD.Models
{
    [Serializable]
    public class Decisao : IComparable
    {

        public long IdDaNorma { get; set; }
        public long Id { get; set; }
        public DateTime? DataDaPublicacao { get; set; }
        public TipoDeDecisao Tipo { get; set; }
        public string Complemento { get; set; }

        /// <summary>
        /// Decrescente
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            Decisao outraDecisao = obj as Decisao;
            if (outraDecisao != null)
            {
                if (DataDaPublicacao > outraDecisao.DataDaPublicacao) return -1;
                if (DataDaPublicacao < outraDecisao.DataDaPublicacao) return +1;
            }
            return 0;
        }

    }
}

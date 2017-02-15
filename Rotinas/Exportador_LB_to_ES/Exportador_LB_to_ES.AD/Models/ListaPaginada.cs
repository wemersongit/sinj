using System.Collections;
using System.Collections.Generic;
using Exportador_LB_to_ES.AD.Pesquisas;

namespace Exportador_LB_to_ES.AD.Models
{
    public class ListaPaginada<t> : IEnumerable<t>, ITotalizavel
    {
        private int total;
        private IEnumerable<t> listaInterna;

        public ListaPaginada(IEnumerable<t> lista, int totalReal)
        {
            listaInterna = lista;
            total = totalReal;
        }

        public void Sort()
        {
            ((List<t>)listaInterna).Sort();
        }

        public int Total
        {
            get { return total; }
        }

        #region IEnumerable<t> Members

        public IEnumerator<t> GetEnumerator()
        {
            return listaInterna.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {

            //Comum.LogsSistema.DebugLogsSistema.Update[REMOVE-ME!]Log("", "Cód. Rastreamento 20111118 - " + "IEnumerator IEnumerable.GetEnumerator()");

            return listaInterna.GetEnumerator();
        }

        #endregion
    }
}

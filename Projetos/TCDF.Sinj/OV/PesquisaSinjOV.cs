using System.Collections.Generic;

namespace TCDF.Sinj.OV
{
    public class PesquisaSinjOV
    {
        public string busca { get; set; }
        public string tipo { get; set; }
        public string tipoDeNorma { get; set; }
        public string numero { get; set; }
        public string ano { get; set; }
        public string origem { get; set; }
        public string hierarquia { get; set; }
        public List<ParametroPesquisaSinj> parametros { get; set; }
    }
    public class ParametroPesquisaSinj
    {
        //�mbito_igual a_DistritoFederal_e_lista|Ementa_cont�m_teste_e_null|Indexa��o_igual a_PERICULOSIDADE_e_lista
        public string campo { get; set; }
        public string operador { get; set; }
        public string valor { get; set; }
        public string tipo { get; set; }
        public string conector { get; set; }
    }
}
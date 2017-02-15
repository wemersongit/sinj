using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;

namespace TCDF.Sinj.OV
{
    public class HistoricoDePesquisaOV : metadata
    {
        public HistoricoDePesquisaOV()
        {
            argumentos = new List<ArgumentoOV>();
        }
        public string ch_consulta { get; set; }
        public string ch_usuario { get; set; }
        public string dt_historico { get; set; }
        public string ds_historico { get; set; }
        public string consulta { get; set; }
        public List<ArgumentoOV> argumentos { get; set; }
    }

    public class ArgumentoOV
    {
        public string campo { get; set; }
        public string operador { get; set; }
        public string valor { get; set; }
        public string conector { get; set; }
    }
}

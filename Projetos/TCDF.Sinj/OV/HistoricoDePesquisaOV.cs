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
            total = new List<TotalOV>();
            registros_clicados = new List<RegistroClicado>();
        }
        public string ch_consulta { get; set; }
        public string ch_usuario { get; set; }
        public string nm_tipo_pesquisa { get; set; }
        public string dt_historico { get; set; }
        public string ds_historico { get; set; }
        public string consulta { get; set; }
        public List<TotalOV> total { get; set; }
        /// <summary>
        /// conta quantas vezes o mesmo usuario efetuou a mesma pesquisa
        /// </summary>
        public long contador { get; set; }
        public List<ArgumentoOV> argumentos { get; set; }
        public List<RegistroClicado> registros_clicados { get; set; }
    }

    public class ArgumentoOV
    {
        public string campo { get; set; }
        public string operador { get; set; }
        public string valor { get; set; }
        public string conector { get; set; }
    }

    /// <summary>
    /// Quantidade de registros encontrados por base na consulta
    /// </summary>
    public class TotalOV
    {
        public string nm_base { get; set; }
        public string ds_base { get; set; }
        public long nr_total { get; set; }
    }

    public class RegistroClicado
    {
        public string ch_registro_clicado { get; set; }
        public string nm_base_clicado { get; set; }
        public string ds_registro_clicado { get; set; }
        public long contador_clicado { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{
    public class InteressadoLBW
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
    public class InteressadoOV
    {
        public InteressadoOV()
        {
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_interessado { get; set; }
        public string nm_interessado { get; set; }
        public string ds_interessado { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
    }

    public class Interessado
    {
        public string ch_interessado { get; set; }
        public string nm_interessado { get; set; }
    }
}

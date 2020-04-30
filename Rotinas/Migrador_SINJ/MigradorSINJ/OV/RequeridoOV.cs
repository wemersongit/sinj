using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{
    public class RequeridoLBW
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
    public class RequeridoOV
    {
        public RequeridoOV()
        {
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_requerido { get; set; }
        public string nm_requerido { get; set; }
        public string ds_requerido { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
    }

    public class Requerido
    {
        public string ch_requerido { get; set; }
        public string nm_requerido { get; set; }
    }
}

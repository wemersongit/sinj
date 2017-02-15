using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{
    public class RequerenteLBW
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
    public class RequerenteOV
    {
        public RequerenteOV()
        {
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_requerente { get; set; }
        public string nm_requerente { get; set; }
        public string ds_requerente { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
    }

    public class Requerente
    {
        public string ch_requerente { get; set; }
        public string nm_requerente { get; set; }
    }
}

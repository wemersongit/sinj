using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{
    public class RelatorLBW
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
    public class RelatorOV
    {
        public RelatorOV()
        {
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_relator { get; set; }
        public string nm_relator { get; set; }
        public string ds_relator { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
    }

    public class Relator
    {
        public string ch_relator { get; set; }
        public string nm_relator { get; set; }
    }
}

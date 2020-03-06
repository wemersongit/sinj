using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{
    public class AutoriaLBW
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
    public class AutoriaOV
    {
        public AutoriaOV(){
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_autoria { get; set; }
        public string nm_autoria { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
    }

    public class Autoria
    {
        public string ch_autoria { get; set; }
        public string nm_autoria { get; set; }
    }
}

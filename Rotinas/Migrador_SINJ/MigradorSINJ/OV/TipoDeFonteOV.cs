using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{
    public class TipoDeFonteLBW
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
    public class TipoDeFonteOV
    {
        public TipoDeFonteOV()
        {
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_tipo_fonte { get; set; }
        public string nm_tipo_fonte { get; set; }
        public string ds_tipo_fonte { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
    }
}

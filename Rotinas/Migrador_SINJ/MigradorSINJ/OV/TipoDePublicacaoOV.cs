using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{
    public class TipoDePublicacaoLBW
    {
        public int Id { get; set; }
        public string Nome { get; set; }
    }
    public class TipoDePublicacaoOV
    {
        public TipoDePublicacaoOV()
        {
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_tipo_publicacao { get; set; }
        public string nm_tipo_publicacao { get; set; }
        public string ds_tipo_publicacao { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
    }
}

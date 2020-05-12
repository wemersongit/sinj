using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MigradorSINJ.OV
{
    public class SituacaoLBW
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public int Peso { get; set; }
    }
    public class SituacaoOV
    {
        public SituacaoOV()
        {
            alteracoes = new List<AlteracaoOV>();
        }
        public string ch_situacao { get; set; }
        public string nm_situacao { get; set; }
        public string ds_situacao { get; set; }
        public int nr_peso_situacao { get; set; }

        public string nm_login_usuario_cadastro { get; set; }
        public string dt_cadastro { get; set; }
        public List<AlteracaoOV> alteracoes { get; set; }
    }
}

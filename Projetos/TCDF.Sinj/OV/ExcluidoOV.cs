using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;

namespace TCDF.Sinj.OV
{
    public class ExcluidoOV : metadata
    {
        public ulong id_doc_excluido { get; set; }
        public string nm_base_excluido { get; set; }

        public string ds_justificativa { get; set; }
        public string dt_exclusao { get; set; }
        public string nm_login_usuario_exclusao { get; set; }

        public string json_doc_excluido { get; set; }

        public List<Arquivo_ExcluidoOV> arquivos { get; set; }
    }

    public class Arquivo_ExcluidoOV
    {
        public Arquivo_ExcluidoOV()
        {
            ar_antigo = new ArquivoOV();
        }

        public string id_file_old { get; set; }
        public ArquivoOV ar_antigo { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;

namespace MigradorSINJ.Arquivos.x64.OV
{
    public class ArquivoErroMigracaoOV : metadata
    {
        public string ch_para_nao_duplicacao { get; set; }
        public string nm_base { get; set; }
        public ulong id_doc_arquivo { get; set; }
        public string path_file { get; set; }
        public string path_put { get; set; }
        public string ds_erro { get; set; }
    }
}

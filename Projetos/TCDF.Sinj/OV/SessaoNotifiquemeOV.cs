using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TCDF.Sinj.OV
{
    public class SessaoNotifiquemeOV
    {
        public SessaoNotifiquemeOV()
        {
            favoritos = new List<string>();
            ch_normas_monitoradas = new List<string>();
        }
        public ulong id_doc { get; set; }
        public string nm_usuario_push { get; set; }
        public string email_usuario_push { get; set; }
        public List<string> ch_normas_monitoradas { get; set; }
        public ulong sessao_id { get; set; }
        public string sessao_chave { get; set; }
        // Contém uma lista de chaves concatenadas com identificadores de base (norma ou diario). Ex.: norma_78993
        public List<string> favoritos { get; set; }
    }
}

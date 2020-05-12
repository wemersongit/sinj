using System ;
using neo.BRLightREST;

namespace neo.BRLightSession.OV
{
    public class SessionOV : metadata
    {
        public string id_session { get; set; }
        public string ds_valor { get; set; }
        public string dt_criacao { get; set; }
        public string dt_expiracao { get; set; }
        public string ds_user { get; set; }
        public string ds_origem { get; set; }
        public string ds_tipo_acesso { get; set; }
    }
}
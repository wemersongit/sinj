using neo.BRLightREST;

namespace TCDF.Sinj.Log.OV
{
    public class log_operacaoOV : metadata
    {

        public string ch_operacao { get; set; }
        public string ds_operacao_detalhes { get; set; }

        public string nr_ip_usuario { get; set; }
        public string dt_inicio { get; set; }
        public string dt_fim { get; set; }
        public string nm_login_user_operacao { get; set; }
        public string nm_user_operacao { get; set; }
        public ulong? id_doc_origem { get; set; }
    }

    public class operacaoOv
    {
        public string retorno { get; set; }
    }
}
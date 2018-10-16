using neo.BRLightREST;

namespace TCDF.Sinj.Log.OV
{
    public class log_acessoOV : metadata
    {

        public string nm_apelido { get; set; }
        public string nm_aplicacao { get; set; }

        public string nr_ip_usuario { get; set; }
        public string ds_browser { get; set; }
        public string ds_login { get; set; }
        public string ip_servidor_porta { get; set; }
        public string ds_obs_login { get; set; }

        public string nm_user_acesso { get; set; }
        public string nm_login_user_acesso { get; set; }
        public string dt_acesso { get; set; }

        public bool in_login_sucesso { get; set; }

    }
}

using neo.BRLightREST;

namespace TCDF.Sinj.Log.OV
{
    public class log_erroOV : metadata
    {

        public string nm_tipo { get; set; }
        public string ds_erro { get; set; }

        public string nr_ip_usuario { get; set; }
        public string ds_browser { get; set; }

        public string nm_user_erro { get; set; }
        public string nm_login_user_erro { get; set; }
        public string dt_log_erro { get; set; }

    }
}
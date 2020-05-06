using neo.BRLightREST;

namespace TCDF.Sinj.Log.OV
{
    public class log_lbconverterOV : metadata
    {
        public string file_name { get; set; }
        public string error_msg { get; set; }
        public string nm_base { get; set; }
        public string id_file_orig { get; set; }
        public ulong id_doc_orig { get; set; }
        public string dt_error { get; set; }

    }
}
using System.Collections.Generic;

namespace TCDF.Sinj.OV
{
    public class SessaoUsuarioOV
    {
        public SessaoUsuarioOV()
        {

        }

        public ulong id_doc { get; set; }
        public string nm_usuario { get; set; }
        public string nm_login_usuario { get; set; }
        public string email_usuario { get; set; }
        public string ch_push { get; set; }
        public string ch_perfil { get; set; }
        public string nm_perfil { get; set; }
        public string pagina_inicial { get; set; }
        public string ch_tema { get; set; }
        public OrgaoCadastrador orgao_cadastrador { get; set; }
        public List<string> grupos { get; set; }
		public bool in_alterar_senha { get; set; }

        public ulong sessao_id { get; set; }
        public string sessao_chave { get; set; }
    }
}

using System;
using neo.BRLightSession;
using neo.BRLightSession.OV;
using neo.BRLightSession.RN;
using TCDF.Sinj.OV;
using util.BRLight;
using System.Linq;
using neo.BRLightREST;

namespace TCDF.Sinj.RN
{
    public class SessaoRN
    {       
        private string nm_cookie;
        private string nm_cookie_look;

        public SessaoRN()
        {
            nm_cookie = Config.ValorChave("NmCookie");
            nm_cookie_look = Config.ValorChave("NmCookieLook");
        }

        public string jsonReg(Pesquisa query)
        {
            return new SessionRN().ObterjsonReg(query);
        }

        public bool Excluir(ulong id_doc)
        {
            return new SessionRN().Deletar(id_doc);
        }

        public ulong Excluir(string key)
        {
            ulong cont = 0;
            var query = new Pesquisa();
            if (!string.IsNullOrEmpty(key))
            {
                query.literal = "id_session like '%" + key + "'";
            }
            query.select = new string[]{"id_doc"};
            var result = new SessionRN().Consultar(query);
            foreach(var session in result.results){
                if(Excluir(session._metadata.id_doc)){
                    cont++;
                }
            }
            return cont;
        }

        public SessaoUsuarioOV CriarSessao(UsuarioOV usuarioOv, bool manterAtiva)
        {
            SessaoUsuarioOV sessaoUsuarioOv = new SessaoUsuarioOV();
            FazerParseSessaoUsuarioOV(usuarioOv, sessaoUsuarioOv);
            var oSession = new Session();
            oSession.Create(manterAtiva);

            var success = oSession.Post<SessaoUsuarioOV>("usuariologin", sessaoUsuarioOv);
            if (success)
            {
                sessaoUsuarioOv.sessao_id = oSession.db_id;
                sessaoUsuarioOv.sessao_chave = oSession.db_chave;
                success = oSession.Put<SessaoUsuarioOV>("usuariologin", sessaoUsuarioOv);
                if (success)
                {
                    return sessaoUsuarioOv;
                }
            }
            return null;
        }

        public bool AtualizarSessao(UsuarioOV usuarioOv)
        {
            var oSession = new Session();
            var sessaoUsuarioOv = LerSessaoUsuarioOv();
            FazerParseSessaoUsuarioOV(usuarioOv, sessaoUsuarioOv);
            return oSession.Put<SessaoUsuarioOV>("usuariologin",sessaoUsuarioOv);
        }

        public void FazerParseSessaoUsuarioOV(UsuarioOV usuarioOv, SessaoUsuarioOV sessaoUsuarioOv)
        {
            sessaoUsuarioOv.id_doc = usuarioOv._metadata.id_doc;
            sessaoUsuarioOv.nm_usuario = usuarioOv.nm_usuario;
            sessaoUsuarioOv.nm_login_usuario = usuarioOv.nm_login_usuario;
            sessaoUsuarioOv.email_usuario = usuarioOv.email_usuario;
            sessaoUsuarioOv.ch_push = usuarioOv.ch_push_usuario;
            sessaoUsuarioOv.ch_perfil = usuarioOv.ch_perfil;
            sessaoUsuarioOv.nm_perfil = usuarioOv.nm_perfil;
            sessaoUsuarioOv.pagina_inicial = usuarioOv.pagina_inicial;
            sessaoUsuarioOv.ch_tema = usuarioOv.ch_tema;
            sessaoUsuarioOv.grupos = usuarioOv.grupos;
            sessaoUsuarioOv.orgao_cadastrador = usuarioOv.orgao_cadastrador;
			sessaoUsuarioOv.in_alterar_senha = usuarioOv.in_alterar_senha;
        }

        public SessionOV LerSessao()
        {
            return new Session().Get<SessionOV>("usuariologin");
        }

        public bool VerificarLogin()
        {
            try
            {
                var sessionOv = LerSessao();
                if (sessionOv != null)
                {
                    SessaoUsuarioOV sessaoUsuarioOv = JSON.Deserializa<SessaoUsuarioOV>(sessionOv.ds_valor);
                    if (!string.IsNullOrEmpty(sessaoUsuarioOv.nm_login_usuario))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public SessaoUsuarioOV LerSessaoUsuarioOv()
        {
            try
            {
                var sessao = LerSessao();
                if (sessao != null)
                {
                    return JSON.Deserializa<SessaoUsuarioOV>(sessao.ds_valor);
                }
                throw new SessionNotFoundException("LerSessaoUsuarioOv sessao=null.");
            }
            catch
            {
                return null;
            }
        }

        public string LerSessaoUsuario()
        {
            var sessionOv = LerSessao();
            if (sessionOv != null)
            {
                return sessionOv.ds_valor;
            }
            return "null";
        }

        public SessionOV BuscarSessao(ulong id_doc)
        {
            var sessionRn = new SessionRN();
            return sessionRn.Consultar(id_doc);
        }

        public void Finalizar()
        {
            var oSession = new Session();
            oSession.Delete("usuariologin");
            Cookies.DeleteCookie(nm_cookie);
            Cookies.DeleteCookie(nm_cookie_look);
        }

        public bool ChecarSessaoAtiva()
        {
            try
            {
                var sessao = LerSessao();
                if (sessao != null)
                {
                    var sessaoUsuarioOv = JSON.Deserializa<SessaoUsuarioOV>(sessao.ds_valor);
                    if (!string.IsNullOrEmpty(sessaoUsuarioOv.nm_login_usuario))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}

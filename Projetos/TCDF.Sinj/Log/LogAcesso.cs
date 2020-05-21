using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.Log.OV;
using TCDF.Sinj.Log.RN;

namespace TCDF.Sinj.Log
{
    public class LogAcesso
    {
        //public static ulong gravar_acesso(string login, bool sucesso, string msg, SessaoUsuarioOV oSessaoUsuario)
        //{
        //    return gravar_acesso(login, sucesso, msg, oSessaoUsuario);
        //}

        public static ulong gravar_acesso(string login, bool sucesso, string msg, string nm_user, string nm_login_user)
        {
            ulong id_log = 0;
            if (Util.ehReplica())
            {
                return id_log;
            }
            try
            {
                var servidor = util.BRLight.Util.Variables("LOCAL_ADDR");
                var port = util.BRLight.Util.Variables("SERVER_PORT");

                var _user_ip = util.BRLight.Util.GetUserIp();
                var _ip_porta = servidor + ":" + port;
                var _navegador = util.BRLight.Util.Variables("HTTP_USER_AGENT");

                var olog_acessoOV = new log_acessoOV();

                olog_acessoOV.nm_apelido = "sinj";
                olog_acessoOV.nm_aplicacao = "Sistema Integrado de Normas Jurídicas";

                olog_acessoOV.nr_ip_usuario = _user_ip;
                olog_acessoOV.ds_browser = _navegador;
                olog_acessoOV.ds_login = login;
                olog_acessoOV.ip_servidor_porta = _ip_porta;
                olog_acessoOV.ds_obs_login = msg;

                //if (!string.IsNullOrEmpty(nm_usuario) || !string.IsNullOrEmpty(nm_login_usuario))
                //{
                //    olog_acessoOV.nm_user_acesso = nm_usuario;
                //    olog_acessoOV.nm_login_user_acesso = nm_login_usuario;
                //}
                //else
                //{
                //    var oSessaoUsuario = new SessaoRN().LerSessaoUsuarioOv();
                //    if (oSessaoUsuario != null)
                //    {
                //        olog_acessoOV.nm_user_acesso = oSessaoUsuario.nm_usuario;
                //        olog_acessoOV.nm_login_user_acesso = oSessaoUsuario.nm_login_usuario;
                //    }
                //}
                olog_acessoOV.nm_user_acesso = nm_user;
                olog_acessoOV.nm_login_user_acesso = nm_login_user;

                olog_acessoOV.dt_acesso = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                olog_acessoOV.in_login_sucesso = sucesso;

                id_log = new log_acessoRN().Incluir(olog_acessoOV);

            }
            catch
            {
                // gerar log em txt 
                // ou no sistema de eventos do sistema operaciona 
                // ou disparar email
            }

            return id_log;
        }
    }
}

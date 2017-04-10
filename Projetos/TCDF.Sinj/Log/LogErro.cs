using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using util.BRLight;
using TCDF.Sinj.Log.OV;
using TCDF.Sinj.Log.RN;

namespace TCDF.Sinj.Log
{
    public class LogErro
	{
        public static ulong gravar_erro(string _nm_tipo, ErroRequest _erro, string nm_user, string nm_login_user)
		{
			var erro = new { ch_erro = "request", Erro = _erro };
            return gravar_erro(erro.ch_erro, _nm_tipo, JSON.Serialize<object>(erro), nm_user, nm_login_user);
		}

        public static ulong gravar_erro(string _nm_tipo, ErroPush _erro, string nm_user, string nm_login_user)
		{
			var erro = new { ch_erro = "push", Erro = _erro };
            return gravar_erro(erro.ch_erro, _nm_tipo, JSON.Serialize<object>(erro), nm_user, nm_login_user);
		}

        //public static ulong gravar_erro(string _nm_tipo, Erro _erro, string nm_user, string nm_login_user)
        //{
        //    var erro = new { ch_erro = "erro", Erro = _erro };
        //    return gravar_erro(_nm_tipo, JSON.Serialize<object>(erro), nm_user, nm_login_user);
        //}
        
        public static ulong gravar_erro(string _nm_tipo, ErroNET _erro, string nm_user, string nm_login_user)
        {
            var erro = new { ch_erro = "erro_net", Erro = _erro };
            return gravar_erro(erro.ch_erro, _nm_tipo, JSON.Serialize<object>(erro), nm_user, nm_login_user);
        }

        public static ulong gravar_erro(string _nm_tipo, ErroJS _erro, string nm_user, string nm_login_user)
        {
            var erro = new { ch_erro = "erro_js", Erro = _erro };
            return gravar_erro(erro.ch_erro, _nm_tipo, JSON.Serialize<object>(erro), nm_user, nm_login_user);
        }

        public static ulong gravar_erro(string _nm_tipo, ErroAjax _erro, string nm_user, string nm_login_user)
        {
            var erro = new { ch_erro = "erro_ajax", Erro = _erro };
            return gravar_erro(erro.ch_erro, _nm_tipo, JSON.Serialize<object>(erro), nm_user, nm_login_user);
        }

        private static ulong gravar_erro(string _nm_tipo, string _ch_operacao, string _ds_erro, string nm_user, string nm_login_user)
        {
            ulong id_log = 0;
            try
            {
                var olog_erroOV = new log_erroOV();
                olog_erroOV.nm_tipo = _nm_tipo;
                olog_erroOV.ch_operacao = _ch_operacao;
                olog_erroOV.ds_erro = _ds_erro;

                olog_erroOV.nr_ip_usuario = util.BRLight.Util.GetUserIp();
                olog_erroOV.ds_browser = util.BRLight.Util.Variables("HTTP_USER_AGENT");

                olog_erroOV.nm_user_erro = nm_user;
                olog_erroOV.nm_login_user_erro = nm_login_user;

                olog_erroOV.dt_log_erro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");

                id_log = new log_erroRN().Incluir(olog_erroOV);
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

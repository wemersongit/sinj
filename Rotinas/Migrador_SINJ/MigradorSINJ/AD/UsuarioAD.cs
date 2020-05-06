using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BRLight.DataAccess.LBW.Provider;
using MigradorSINJ.OV;
using util.BRLight;
using LightInfocon.GoldenAccess.General;

namespace MigradorSINJ.AD
{
    public class UsuarioAD
    {
        /// <summary>
        /// É o acessa dados do LBW
        /// </summary>
        private AcessaDados _ad;

        private REST _rest;

        private GoldenAccessService servicoGoldenAccess;
        private User usuario;

        public UsuarioAD()
        {
            _ad = new AcessaDados(Config.ValorChave("string_connection_lbw",true));
            _rest = new REST(Config.ValorChave("NmBaseUsuario", true));
        }

        internal List<User> BuscarUsuariosLBW()
        {
            List<User> usuariosLbw = new List<User>();
            GoldenAccess ga = new GoldenAccess(Config.ValorChave("golden_access_connection_string", true));
            usuario = ga.Authenticate("adm", "sysadm");
            if (usuario != null)
            {
                servicoGoldenAccess = new GoldenAccessService(usuario);
                var users = servicoGoldenAccess.GetUsers();
                foreach(var user in users){
                    usuariosLbw.Add(servicoGoldenAccess.GetFullUser(user.Login));
                }
                
            }
            return usuariosLbw;
        }

        internal ulong Incluir(UsuarioOV usuarioOv)
        {
            return _rest.Incluir(usuarioOv);
        }
    }
}

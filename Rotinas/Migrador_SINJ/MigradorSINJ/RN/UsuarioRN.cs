using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MigradorSINJ.AD;
using MigradorSINJ.OV;
using LightInfocon.GoldenAccess.General;

namespace MigradorSINJ.RN
{
    public class UsuarioRN
    {
        private UsuarioAD _usuarioAd;

        public UsuarioRN()
        {
            _usuarioAd = new UsuarioAD();
        }

        public List<User> BuscarUsuariosLBW()
        {
            return _usuarioAd.BuscarUsuariosLBW();
        }

        public ulong Incluir(UsuarioOV usuarioOv)
        {
            return _usuarioAd.Incluir(usuarioOv);
        }
    }
}

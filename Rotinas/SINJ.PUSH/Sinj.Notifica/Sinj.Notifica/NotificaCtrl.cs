using System;
using System.Collections.Generic;
using System.Threading;
using Sinj.Notifica.Objetos;
using Sinj.Notifica.Regras;

namespace Sinj.Notifica
{
    public class NotificaCtrl
    {
        private PushRN _pushRn;
        private NormaRN _normaRn;

        public NotificaCtrl(string stringConnection)
        {
            _pushRn = new PushRN(stringConnection);
            _normaRn = new NormaRN(stringConnection);
        }

        public int AtualizaAtosNotificados()
        {
            return _normaRn.AtualizaCamposAtlzENovaDeTodasAsNormas();
        }

        public List<Push> BuscaUsuariosPush()
        {
            return _pushRn.BuscaAtivosPush();
        }

        public List<Norma> BuscaNormasAlteradas()
        {
            return _normaRn.BuscaNormasAlteradas();
        }

        public List<Norma> BuscaNormasNovas()
        {
            return _normaRn.BuscaNormasNovas();
        }
    }
}
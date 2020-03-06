using System.Collections.Generic;
using Sinj.Notifica.AcessaDados;
using Sinj.Notifica.Objetos;

namespace Sinj.Notifica.Regras
{
    public class NormaRN
    {
        private NormaAD _normaAd;

        public NormaRN(string stringConnection)
        {
            _normaAd = new NormaAD(stringConnection);
        }

        public List<Norma> BuscaNormasAlteradas()
        {
            return _normaAd.BuscaNormasAlteradas();
        }

        public List<Norma> BuscaNormasNovas()
        {
            return _normaAd.BuscaNormasNovas();
        }

        public int AtualizaCamposAtlzENovaDeTodasAsNormas()
        {
            return _normaAd.UpdateNormas();
        }
    }
}
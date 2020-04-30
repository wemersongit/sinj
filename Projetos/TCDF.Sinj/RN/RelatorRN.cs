using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using TCDF.Sinj.AD;

namespace TCDF.Sinj.RN
{
    public class RelatorRN
    {
        private RelatorAD _relatorAd;

        public RelatorRN()
        {
            _relatorAd = new RelatorAD();
        }

        public Results<RelatorOV> Consultar(Pesquisa query)
        {
            return _relatorAd.Consultar(query);
        }

        public RelatorOV Doc(ulong id_doc)
        {
            var relatorOv = _relatorAd.Doc(id_doc);
            if (relatorOv == null)
            {
                throw new DocNotFoundException("Registro não Encontrado.");
            }
            return relatorOv;
        }

        public RelatorOV Doc(string ch_tipo_relacao)
        {
            return _relatorAd.Doc(ch_tipo_relacao);
        }

        public string JsonReg(ulong id_doc)
        {
            return _relatorAd.JsonReg(id_doc);
        }

        public string JsonReg(Pesquisa query)
        {
            return _relatorAd.JsonReg(query);
        }

        public ulong Incluir(RelatorOV relatorOv)
        {
            relatorOv.ch_relator = Guid.NewGuid().ToString("N");
            return _relatorAd.Incluir(relatorOv);
        }

        public bool Atualizar(ulong id_doc, RelatorOV relatorOv)
        {
            Validar(relatorOv);
            return _relatorAd.Atualizar(id_doc, relatorOv);
        }

        public bool Excluir(ulong id_doc)
        {
            var relatorOv = Doc(id_doc);
            ValidarDepencias(relatorOv);
            return _relatorAd.Excluir(id_doc);
        }

        public void ValidarDepencias(RelatorOV relatorOv)
        {
            var normas = new NormaRN().BuscarNormasDoRelator(relatorOv.ch_relator);
            if (normas.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por uma ou mais normas.");
            }
        }

        private void Validar(RelatorOV relatorOv)
        {
            if (string.IsNullOrEmpty(relatorOv.nm_relator))
            {
                throw new DocValidacaoException("Nome inválido.");
            }

        }
    }
}

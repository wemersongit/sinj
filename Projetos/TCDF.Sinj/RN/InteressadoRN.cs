using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using TCDF.Sinj.AD;

namespace TCDF.Sinj.RN
{
    public class InteressadoRN
    {
        private InteressadoAD _interessadoAd;

        public InteressadoRN()
        {
            _interessadoAd = new InteressadoAD();
        }

        public Results<InteressadoOV> Consultar(Pesquisa query)
        {
            return _interessadoAd.Consultar(query);
        }

        public InteressadoOV Doc(ulong id_doc)
        {
            var interessadoOv = _interessadoAd.Doc(id_doc);
            if (interessadoOv == null)
            {
                throw new DocNotFoundException("Registro não Encontrado.");
            }
            return interessadoOv;
        }

        public InteressadoOV Doc(string ch_interessado)
        {
            return _interessadoAd.Doc(ch_interessado);
        }

        public string JsonReg(ulong id_doc)
        {
            return _interessadoAd.JsonReg(id_doc);
        }

        public string JsonReg(Pesquisa query)
        {
            return _interessadoAd.JsonReg(query);
        }

        public ulong Incluir(InteressadoOV interessadoOv)
        {
            interessadoOv.ch_interessado = Guid.NewGuid().ToString("N");
            return _interessadoAd.Incluir(interessadoOv);
        }

        public bool Atualizar(ulong id_doc, InteressadoOV interessadoOv)
        {
            Validar(interessadoOv);
            return _interessadoAd.Atualizar(id_doc, interessadoOv);
        }

        public bool Excluir(ulong id_doc)
        {
            var interessadoOv = Doc(id_doc);
            ValidarDepencias(interessadoOv);
            return _interessadoAd.Excluir(id_doc);
        }

        public void ValidarDepencias(InteressadoOV interessadoOv)
        {
            var normas_do_interessado = new NormaRN().BuscarNormasDoInteressado(interessadoOv.ch_interessado);
            if (normas_do_interessado.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Interessado está sendo usado por uma ou mais normas.");
            }
        }

        private void Validar(InteressadoOV interessadoOv)
        {
            if (string.IsNullOrEmpty(interessadoOv.nm_interessado))
            {
                throw new DocValidacaoException("Nome inválido.");
            }
        }
    }
}

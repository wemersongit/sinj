using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using TCDF.Sinj.AD;

namespace TCDF.Sinj.RN
{
    public class RequeridoRN
    {
        private RequeridoAD _requeridoAd;

        public RequeridoRN()
        {
            _requeridoAd = new RequeridoAD();
        }

        public Results<RequeridoOV> Consultar(Pesquisa query)
        {
            return _requeridoAd.Consultar(query);
        }

        public RequeridoOV Doc(ulong id_doc)
        {
            var requeridoOv = _requeridoAd.Doc(id_doc);
            if (requeridoOv == null)
            {
                throw new DocNotFoundException("Registro não Encontrado.");
            }
            return requeridoOv;
        }

        public RequeridoOV Doc(string ch_tipo_relacao)
        {
            return _requeridoAd.Doc(ch_tipo_relacao);
        }

        public string JsonReg(ulong id_doc)
        {
            return _requeridoAd.JsonReg(id_doc);
        }

        public string JsonReg(Pesquisa query)
        {
            return _requeridoAd.JsonReg(query);
        }

        public ulong Incluir(RequeridoOV requeridoOv)
        {
            requeridoOv.ch_requerido = Guid.NewGuid().ToString("N");
            return _requeridoAd.Incluir(requeridoOv);
        }

        public bool Atualizar(ulong id_doc, RequeridoOV requeridoOv)
        {
            Validar(requeridoOv);
            return _requeridoAd.Atualizar(id_doc, requeridoOv);
        }

        public bool Excluir(ulong id_doc)
        {
            var requeridoOv = Doc(id_doc);
            ValidarDepencias(requeridoOv);
            return _requeridoAd.Excluir(id_doc);
        }

        public void ValidarDepencias(RequeridoOV requeridoOv)
        {
            var normas = new NormaRN().BuscarNormasDoRequerido(requeridoOv.ch_requerido);
            if (normas.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por uma ou mais normas.");
            }
        }

        private void Validar(RequeridoOV requeridoOv)
        {
            if (string.IsNullOrEmpty(requeridoOv.nm_requerido))
            {
                throw new DocValidacaoException("Nome inválido.");
            }

        }
    }
}

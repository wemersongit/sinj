using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using TCDF.Sinj.AD;

namespace TCDF.Sinj.RN
{
    public class RequerenteRN
    {
        private RequerenteAD _requerenteAd;

        public RequerenteRN()
        {
            _requerenteAd = new RequerenteAD();
        }

        public Results<RequerenteOV> Consultar(Pesquisa query)
        {
            return _requerenteAd.Consultar(query);
        }

        public RequerenteOV Doc(ulong id_doc)
        {
            var requerenteOv = _requerenteAd.Doc(id_doc);
            if (requerenteOv == null)
            {
                throw new DocNotFoundException("Registro não Encontrado.");
            }
            return requerenteOv;
        }

        public RequerenteOV Doc(string ch_tipo_relacao)
        {
            return _requerenteAd.Doc(ch_tipo_relacao);
        }

        public string JsonReg(ulong id_doc)
        {
            return _requerenteAd.JsonReg(id_doc);
        }

        public string JsonReg(Pesquisa query)
        {
            return _requerenteAd.JsonReg(query);
        }

        public ulong Incluir(RequerenteOV requerenteOv)
        {
            requerenteOv.ch_requerente = Guid.NewGuid().ToString("N");
            return _requerenteAd.Incluir(requerenteOv);
        }

        public bool Atualizar(ulong id_doc, RequerenteOV requerenteOv)
        {
            Validar(requerenteOv);
            return _requerenteAd.Atualizar(id_doc, requerenteOv);
        }

        public bool Excluir(ulong id_doc)
        {
            var requerenteOv = Doc(id_doc);
            ValidarDepencias(requerenteOv);
            return _requerenteAd.Excluir(id_doc);
        }

        public void ValidarDepencias(RequerenteOV requerenteOv)
        {
            var normas = new NormaRN().BuscarNormasDoRequerente(requerenteOv.ch_requerente);
            if (normas.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por uma ou mais normas.");
            }
        }

        private void Validar(RequerenteOV requerenteOv)
        {
            if (string.IsNullOrEmpty(requerenteOv.nm_requerente))
            {
                throw new DocValidacaoException("Nome inválido.");
            }

        }
    }
}

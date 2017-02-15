using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.AD;
using TCDF.Sinj.OV;
using neo.BRLightREST;

namespace TCDF.Sinj.RN
{
    public class TipoDeRelacaoRN
    {
        private TipoDeRelacaoAD _tipoDeRelacaoAd;

        public TipoDeRelacaoRN()
        {
            _tipoDeRelacaoAd = new TipoDeRelacaoAD();
        }

        public Results<TipoDeRelacaoOV> Consultar(Pesquisa query)
        {
            return _tipoDeRelacaoAd.Consultar(query);
        }

        public TipoDeRelacaoOV Doc(ulong id_doc)
        {
            var tipoDeRelacaoOv = _tipoDeRelacaoAd.Doc(id_doc);
            if (tipoDeRelacaoOv == null)
            {
                throw new DocNotFoundException("Registro não Encontrado.");
            }
            return tipoDeRelacaoOv;
        }

        public TipoDeRelacaoOV Doc(string ch_tipo_relacao)
        {
            return _tipoDeRelacaoAd.Doc(ch_tipo_relacao);
        }

        public string JsonReg(ulong id_doc)
        {
            return _tipoDeRelacaoAd.JsonReg(id_doc);
        }

        public string JsonReg(Pesquisa query)
        {
            return _tipoDeRelacaoAd.JsonReg(query);
        }

        public ulong Incluir(TipoDeRelacaoOV tipoDeRelacaoOv)
        {
            tipoDeRelacaoOv.ch_tipo_relacao = Guid.NewGuid().ToString("N");
            return _tipoDeRelacaoAd.Incluir(tipoDeRelacaoOv);
        }

        public bool Atualizar(ulong id_doc, TipoDeRelacaoOV tipoDeRelacaoOv)
        {
            Validar(tipoDeRelacaoOv);
            return _tipoDeRelacaoAd.Atualizar(id_doc, tipoDeRelacaoOv);
        }

        public bool Excluir(ulong id_doc)
        {
            var tipoDeRelacaoOv = Doc(id_doc);
            ValidarDepencias(tipoDeRelacaoOv);
            return _tipoDeRelacaoAd.Excluir(id_doc);
        }

        public void ValidarDepencias(TipoDeRelacaoOV tipoDeRelacaoOv)
        {
            var normas = new NormaRN().BuscarNormasDoTipoDeRelacao(tipoDeRelacaoOv.ch_tipo_relacao);
            if (normas.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por uma ou mais normas.");
            }
        }

        private void Validar(TipoDeRelacaoOV tipoDeRelacaoOv)
        {
            if (string.IsNullOrEmpty(tipoDeRelacaoOv.nm_tipo_relacao))
            {
                throw new DocValidacaoException("Nome inválido.");
            }
            if (tipoDeRelacaoOv.nr_importancia < 0)
            {
                throw new DocValidacaoException("Importância inválida.");
            }

        }
    }
}

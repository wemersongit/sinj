using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.AD;
using neo.BRLightREST;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.RN
{
    public class TipoDePublicacaoRN
    {
        private TipoDePublicacaoAD _tipoDePublicacaoAd;

        public TipoDePublicacaoRN()
        {
            _tipoDePublicacaoAd = new TipoDePublicacaoAD();
        }

        public Results<TipoDePublicacaoOV> Consultar(Pesquisa query)
        {
            return _tipoDePublicacaoAd.Consultar(query);
        }

        public TipoDePublicacaoOV Doc(ulong id_doc)
        {
            var tipoDePublicacaoOV = _tipoDePublicacaoAd.Doc(id_doc);
            if (tipoDePublicacaoOV == null)
            {
                throw new DocNotFoundException("Registro não Encontrado.");
            }
            return tipoDePublicacaoOV;
        }

        public TipoDePublicacaoOV Doc(string ch_tipo_publicacao)
        {
            return _tipoDePublicacaoAd.Doc(ch_tipo_publicacao);
        }

        public string JsonReg(ulong id_doc)
        {
            return _tipoDePublicacaoAd.JsonReg(id_doc);
        }

        public string JsonReg(Pesquisa query)
        {
            return _tipoDePublicacaoAd.JsonReg(query);
        }

        public ulong Incluir(TipoDePublicacaoOV tipoDePublicacaoOv)
        {
            tipoDePublicacaoOv.ch_tipo_publicacao = Guid.NewGuid().ToString("N");
            return _tipoDePublicacaoAd.Incluir(tipoDePublicacaoOv);
        }

        public bool Atualizar(ulong id_doc, TipoDePublicacaoOV tipoDePublicacaoOv)
        {
            Validar(tipoDePublicacaoOv);
            return _tipoDePublicacaoAd.Atualizar(id_doc, tipoDePublicacaoOv);
        }

        public bool Excluir(ulong id_doc)
        {
            var tipoDePublicacaoOv = Doc(id_doc);
            ValidarDepencias(tipoDePublicacaoOv);
            return _tipoDePublicacaoAd.Excluir(id_doc);
        }

        public void ValidarDepencias(TipoDePublicacaoOV tipoDePublicacaoOV)
        {
            var normas = new NormaRN().BuscarNormasDoTipoDePublicacao(tipoDePublicacaoOV.ch_tipo_publicacao);
            if (normas.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por uma ou mais normas.");
            }
        }

        private void Validar(TipoDePublicacaoOV tipoDePublicacaoOv)
        {
            if (string.IsNullOrEmpty(tipoDePublicacaoOv.nm_tipo_publicacao))
            {
                throw new DocValidacaoException("Nome inválido.");
            }
        }

    }
}

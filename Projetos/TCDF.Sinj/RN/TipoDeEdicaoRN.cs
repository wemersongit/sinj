using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using TCDF.Sinj.AD;

namespace TCDF.Sinj.RN
{
    public class TipoDeEdicaoRN
    {
        private TipoDeEdicaoAD _tipoDeEdicaoAd;

        public TipoDeEdicaoRN()
        {
            _tipoDeEdicaoAd = new TipoDeEdicaoAD();
        }

        public Results<TipoDeEdicaoOV> Consultar(Pesquisa query)
        {
            return _tipoDeEdicaoAd.Consultar(query);
        }

        public TipoDeEdicaoOV Doc(ulong id_doc)
        {
            var TipoDeEdicaoRN = _tipoDeEdicaoAd.Doc(id_doc);
            if (TipoDeEdicaoRN == null)
            {
                throw new DocNotFoundException("Registro não Encontrado.");
            }
            return TipoDeEdicaoRN;
        }

        public TipoDeEdicaoOV Doc(string ch_tipo_edicao)
        {
            return _tipoDeEdicaoAd.Doc(ch_tipo_edicao);
        }

        public string JsonReg(ulong id_doc)
        {
            return _tipoDeEdicaoAd.JsonReg(id_doc);
        }

        public string JsonReg(Pesquisa query)
        {
            return _tipoDeEdicaoAd.JsonReg(query);
        }

        public ulong Incluir(TipoDeEdicaoOV TipoDeEdicaoRN)
        {
            TipoDeEdicaoRN.ch_tipo_edicao = Guid.NewGuid().ToString("N");
            return _tipoDeEdicaoAd.Incluir(TipoDeEdicaoRN);
        }

        public bool Atualizar(ulong id_doc, TipoDeEdicaoOV TipoDeEdicaoRN)
        {
            Validar(TipoDeEdicaoRN);
            return _tipoDeEdicaoAd.Atualizar(id_doc, TipoDeEdicaoRN);
        }

        public bool Excluir(ulong id_doc)
        {
            var TipoDeEdicaoRN = Doc(id_doc);
            ValidarDepencias(TipoDeEdicaoRN);
            return _tipoDeEdicaoAd.Excluir(id_doc);
        }

        public void ValidarDepencias(TipoDeEdicaoOV TipoDeEdicaoRN)
        {
            var diarios = new DiarioRN().BuscarDiariosDoTipoDeEdicao(TipoDeEdicaoRN.ch_tipo_edicao);
            if (diarios.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por um ou mais diários.");
            }
        }

        private void Validar(TipoDeEdicaoOV TipoDeEdicaoRN)
        {
            if (string.IsNullOrEmpty(TipoDeEdicaoRN.nm_tipo_edicao))
            {
                throw new DocValidacaoException("Nome inválido.");
            }
        }
    }
}

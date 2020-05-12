using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using TCDF.Sinj.AD;

namespace TCDF.Sinj.RN
{
    public class TipoDeFonteRN
    {
        private TipoDeFonteAD _tipoDeFonteAd;

        public TipoDeFonteRN()
        {
            _tipoDeFonteAd = new TipoDeFonteAD();
        }

        public Results<TipoDeFonteOV> Consultar(Pesquisa query)
        {
            return _tipoDeFonteAd.Consultar(query);
        }

        public TipoDeFonteOV Doc(ulong id_doc)
        {
            var tipoDeFonteOV = _tipoDeFonteAd.Doc(id_doc);
            if (tipoDeFonteOV == null)
            {
                throw new DocNotFoundException("Registro não Encontrado.");
            }
            return tipoDeFonteOV;
        }

        public TipoDeFonteOV Doc(string ch_tipo_fonte)
        {
            return _tipoDeFonteAd.Doc(ch_tipo_fonte);
        }

        public string JsonReg(ulong id_doc)
        {
            return _tipoDeFonteAd.JsonReg(id_doc);
        }

        public string JsonReg(Pesquisa query)
        {
            return _tipoDeFonteAd.JsonReg(query);
        }

        public ulong Incluir(TipoDeFonteOV tipoDeFonteOV)
        {
            tipoDeFonteOV.ch_tipo_fonte = Guid.NewGuid().ToString("N");
            return _tipoDeFonteAd.Incluir(tipoDeFonteOV);
        }

        public bool Atualizar(ulong id_doc, TipoDeFonteOV tipoDeFonteOV)
        {
            Validar(tipoDeFonteOV);
            return _tipoDeFonteAd.Atualizar(id_doc, tipoDeFonteOV);
        }

        public bool Excluir(ulong id_doc)
        {
            var tipoDeFonteOV = Doc(id_doc);
            ValidarDepencias(tipoDeFonteOV);
            return _tipoDeFonteAd.Excluir(id_doc);
        }

        public void ValidarDepencias(TipoDeFonteOV tipoDeFonteOV)
        {
            var normas = new NormaRN().BuscarNormasDoTipoDeFonte(tipoDeFonteOV.ch_tipo_fonte);
            if (normas.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por uma ou mais normas.");
            }
        }

        private void Validar(TipoDeFonteOV tipoDeFonteOV)
        {
            if (string.IsNullOrEmpty(tipoDeFonteOV.nm_tipo_fonte))
            {
                throw new DocValidacaoException("Nome inválido.");
            }
        }
    }
}

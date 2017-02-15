using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.AD;
using neo.BRLightREST;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.RN
{
    public class SINJ_ArquivoRN
    {
        private SINJ_ArquivoAD _arquivoAd;

        public SINJ_ArquivoRN()
        {
            _arquivoAd = new SINJ_ArquivoAD();
        }

        public Results<SINJ_ArquivoOV> Consultar(Pesquisa query)
        {
            return _arquivoAd.Consultar(query);
        }

        public SINJ_ArquivoOV Doc(ulong id_doc)
        {
            var SINJ_ArquivoOV = _arquivoAd.Doc(id_doc);
            if (SINJ_ArquivoOV == null)
            {
                throw new DocNotFoundException("Arquivo não Encontrado.");
            }
            return SINJ_ArquivoOV;
        }

        public SINJ_ArquivoOV Doc(string ch_arquivo)
        {
            return _arquivoAd.Doc(ch_arquivo);
        }

        public string JsonReg(ulong id_doc)
        {
            return _arquivoAd.JsonReg(id_doc);
        }

        public string JsonReg(Pesquisa query)
        {
            return _arquivoAd.JsonReg(query);
        }

        public ulong Incluir(SINJ_ArquivoOV sinj_arquivoOV)
        {
            
            return _arquivoAd.Incluir(sinj_arquivoOV);
        }

        public bool Atualizar(ulong id_doc, SINJ_ArquivoOV sinj_arquivoOV)
        {
            Validar(sinj_arquivoOV);
            return _arquivoAd.Atualizar(id_doc, sinj_arquivoOV);
        }

        public bool Excluir(ulong id_doc)
        {
            return _arquivoAd.Excluir(id_doc);
        }

        private void Validar(SINJ_ArquivoOV sinj_arquivoOV)
        {
            if (string.IsNullOrEmpty(sinj_arquivoOV.nm_arquivo))
            {
                throw new DocValidacaoException("Nome inválido.");
            }
            if (sinj_arquivoOV.nr_tipo_arquivo == 1)
            {
                if (sinj_arquivoOV.ar_arquivo == null || string.IsNullOrEmpty(sinj_arquivoOV.ar_arquivo.id_file))
                {
                    throw new DocValidacaoException("Arquivo inválido.");
                }
            }
        }
    }
}

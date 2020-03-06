using neo.BRLightREST;
using TCDF.Sinj.AD;
using TCDF.Sinj.OV;
using System;
using System.Text.RegularExpressions;
using util.BRLight;
using System.Collections.Generic;

namespace TCDF.Sinj.RN
{
    public class SituacaoRN
    {
        private SituacaoAD _situacaoDeNormaAd;

        public SituacaoRN()
        {
            _situacaoDeNormaAd = new SituacaoAD();
        }

        public Results<SituacaoOV> Consultar(Pesquisa query)
        {
            return _situacaoDeNormaAd.Consultar(query);
        }

        public List<SituacaoOV> BuscarTodos()
        {
            return _situacaoDeNormaAd.BuscarTodos();
        }

        public SituacaoOV Doc(ulong id_doc)
        {
            var situacaoOv = _situacaoDeNormaAd.Doc(id_doc);
            if (situacaoOv == null)
            {
                throw new DocNotFoundException("Registro não Encontrado.");
            }
            return situacaoOv;
        }

        public SituacaoOV Doc(string ch_situacao)
        {
            return _situacaoDeNormaAd.Doc(ch_situacao);
        }

        public string JsonReg(ulong id_doc)
        {
            return _situacaoDeNormaAd.JsonReg(id_doc);
        }

        public string JsonReg(Pesquisa query)
        {
            return _situacaoDeNormaAd.JsonReg(query);
        }

        public ulong Incluir(SituacaoOV situacaoOv)
        {
            situacaoOv.ch_situacao = ManipulaTexto.RetiraCaracteresEspeciais(situacaoOv.nm_situacao, true).ToLower();
            return _situacaoDeNormaAd.Incluir(situacaoOv);
        }

        public bool Atualizar(ulong id_doc, SituacaoOV situacaoOv)
        {
            Validar(situacaoOv);
            return _situacaoDeNormaAd.Atualizar(id_doc, situacaoOv);
        }

        public bool Excluir(ulong id_doc)
        {
            var situacaoOv = Doc(id_doc);
            ValidarDepencias(situacaoOv);
            return _situacaoDeNormaAd.Excluir(id_doc);
        }

        public void ValidarDepencias(SituacaoOV situacaoOv)
        {
            var normas = new NormaRN().BuscarNormasDaSituacao(situacaoOv.ch_situacao);
            if (normas.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por uma ou mais normas.");
            }
        }

        private void Validar(SituacaoOV situacaoOv)
        {
            if (string.IsNullOrEmpty(situacaoOv.nm_situacao))
            {
                throw new DocValidacaoException("Nome inválido.");
            }
            if (situacaoOv.nr_peso_situacao < 0)
            {
                throw new DocValidacaoException("Peso inválido.");
            }

        }
    }
}

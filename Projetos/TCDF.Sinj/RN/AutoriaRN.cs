using TCDF.Sinj.AD;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using System;
using System.Collections.Generic;

namespace TCDF.Sinj.RN
{
    public class AutoriaRN
    {
        private AutoriaAD _autoriaAd;

        public AutoriaRN()
        {
            _autoriaAd = new AutoriaAD();
        }

        public Results<AutoriaOV> Consultar(Pesquisa query)
        {
            return _autoriaAd.Consultar(query);
        }

        public AutoriaOV Doc(ulong id_doc)
        {
            var autoriaOv = _autoriaAd.Doc(id_doc);
            if (autoriaOv == null)
            {
                throw new DocNotFoundException("Autoria não Encontrada.");
            }
            return autoriaOv;
        }

        public AutoriaOV Doc(string ch_autoria)
        {
            return _autoriaAd.Doc(ch_autoria);
        }

        public string JsonReg(ulong id_doc)
        {
            return _autoriaAd.JsonReg(id_doc);
        }

        public string JsonReg(Pesquisa query)
        {
            return _autoriaAd.JsonReg(query);
        }

        public ulong Incluir(AutoriaOV autoriaOv)
        {
            autoriaOv.ch_autoria = Guid.NewGuid().ToString("N");
            return _autoriaAd.Incluir(autoriaOv);
        }

        public bool Atualizar(ulong id_doc, AutoriaOV autoriaOv)
        {
            Validar(autoriaOv);
            return _autoriaAd.Atualizar(id_doc, autoriaOv);
        }

        public bool Excluir(ulong id_doc)
        {
            var autoriaOv = Doc(id_doc);
            ValidarDepencias(autoriaOv);
            return _autoriaAd.Excluir(id_doc);
        }

        public void ValidarDepencias(AutoriaOV autoriaOv)
        {
            var normas_da_autoria = new NormaRN().BuscarNormasDaAutoria(autoriaOv.ch_autoria);
            if (normas_da_autoria.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. A Autoria está sendo usada por uma ou mais normas.");
            }
        }

        private void Validar(AutoriaOV autoriaOv)
        {
            if (string.IsNullOrEmpty(autoriaOv.nm_autoria))
            {
                throw new DocValidacaoException("Nome inválido.");
            }
        }
    }
}

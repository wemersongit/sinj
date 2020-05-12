using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.AD;
using TCDF.Sinj.OV;
using neo.BRLightREST;

namespace TCDF.Sinj.RN
{
    public class ArquivoVersionadoNormaRN
    {
        private ArquivoVersionadoNormaAD _arquivoVersionadoNormaAd;

        public ArquivoVersionadoNormaRN()
        {
            _arquivoVersionadoNormaAd = new ArquivoVersionadoNormaAD();
        }

        public Results<ArquivoVersionadoNormaOV> Consultar(Pesquisa query)
        {
            return _arquivoVersionadoNormaAd.Consultar(query);
        }

        public ArquivoVersionadoNormaOV Doc(ulong id_doc)
        {
            return _arquivoVersionadoNormaAd.Doc(id_doc);
        }

        public string JsonReg(Pesquisa query)
        {
            return _arquivoVersionadoNormaAd.JsonReg(query);
        }

        public string JsonReg(ulong id_doc)
        {
            return _arquivoVersionadoNormaAd.JsonReg(id_doc);
        }

        public ulong Incluir(ArquivoVersionadoNormaOV arquivoVersionadoNormaOv)
        {
            arquivoVersionadoNormaOv.ch_arquivo_versionado = Guid.NewGuid().ToString("N");
            return _arquivoVersionadoNormaAd.Incluir(arquivoVersionadoNormaOv);
        }

        public string AnexarArquivo(util.BRLight.FileParameter fileParameter)
        {
            return _arquivoVersionadoNormaAd.AnexarArquivo(fileParameter);
        }

        public string GetDoc(string id_file)
        {
            return _arquivoVersionadoNormaAd.GetDoc(id_file);
        }

        public byte[] Download(string id_file)
        {
            return _arquivoVersionadoNormaAd.Download(id_file);
        }
    }
}

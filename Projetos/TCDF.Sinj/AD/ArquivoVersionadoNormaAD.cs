using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using util.BRLight;

namespace TCDF.Sinj.AD
{
    public class ArquivoVersionadoNormaAD
    {
        private AcessoAD<ArquivoVersionadoNormaOV> _acessoAd;
        private string _nm_base;

        public ArquivoVersionadoNormaAD()
        {
            _nm_base = util.BRLight.Util.GetVariavel("NmBaseArquivoVersionadoNorma", true);
            _acessoAd = new AcessoAD<ArquivoVersionadoNormaOV>(_nm_base);
        }

        internal Results<ArquivoVersionadoNormaOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        internal ArquivoVersionadoNormaOV Doc(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

        internal string JsonReg(Pesquisa query)
        {
            return _acessoAd.jsonReg(query);
        }

        internal string JsonReg(ulong id_doc)
        {
            return _acessoAd.jsonReg(id_doc);
        }

        /// <summary>
        /// Inclui um registro e retorna o id_doc
        /// </summary>
        /// <param name="ArquivoVersionadoNormaOV"></param>
        /// <returns></returns>
        internal ulong Incluir(ArquivoVersionadoNormaOV arquivoVersionadoNormaOv)
        {
            try
            {
                return _acessoAd.Incluir(arquivoVersionadoNormaOv);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && (ex.InnerException.Message.IndexOf("duplicate key") > -1 || ex.InnerException.Message.IndexOf("duplicar valor da chave") > -1))
                {
                    throw new DocDuplicateKeyException("Registro duplicado!!!");
                }
                throw ex;
            }
        }

        public string GetDoc(string id_file)
        {
            var doc = new Doc(_nm_base);
            return doc.pesquisarDoc(id_file);
        }

        public string AnexarArquivo(FileParameter fileParameter)
        {
            string resultado;
            try
            {
                var doc = new Doc(_nm_base);
                var dicionario = new Dictionary<string, object>();
                dicionario.Add("file", fileParameter);
                resultado = doc.incluir(dicionario);
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("Não foi possível anexar o arquivo", ex);
            }
            return resultado;
        }

        public byte[] Download(string id_file)
        {
            var doc = new Doc(_nm_base);
            return doc.download(id_file);
        }
    }
}

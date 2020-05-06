using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.AD
{
    public class SINJ_ArquivoAD
    {
        private AcessoAD<SINJ_ArquivoOV> _acessoAd;

        public SINJ_ArquivoAD()
        {
            _acessoAd = new AcessoAD<SINJ_ArquivoOV>(util.BRLight.Util.GetVariavel("NmBaseArquivo", true));
        }

        internal Results<SINJ_ArquivoOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        internal SINJ_ArquivoOV Doc(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

        internal SINJ_ArquivoOV Doc(string ch_arquivo)
        {
            Pesquisa query = new Pesquisa();
            query.limit = "1";
            query.offset = "0";
            query.literal = string.Format("ch_arquivo='{0}'", ch_arquivo);
            var result = Consultar(query);
            if (result.result_count > 1)
            {
                throw new Exception("Foi verificado mais de um arquivo com a mesma chave.");
            }
            if (result.result_count > 0)
            {
                return result.results[0];
            }
            throw new Exception("Nenhum arquivo foi encontrado. É possível que tenha sido excluído.");
        }

        internal string JsonReg(ulong id_doc)
        {
            return _acessoAd.jsonReg(id_doc);
        }

        internal string JsonReg(Pesquisa query)
        {
            return _acessoAd.jsonReg(query);
        }

        internal ulong Incluir(SINJ_ArquivoOV SINJ_ArquivoOV)
        {
            try
            {
                return _acessoAd.Incluir(SINJ_ArquivoOV);
            }
            catch (Exception ex)
            {
                if ((ex.Message.IndexOf("duplicate key") > -1 || ex.Message.IndexOf("duplicar valor da chave") > -1 ) || (ex.InnerException != null && (ex.InnerException.Message.IndexOf("duplicate key") > -1 || ex.InnerException.Message.IndexOf("duplicar valor da chave") > -1)))
                {
                    throw new DocDuplicateKeyException("Registro já existente na base de dados!!!");
                }
                throw ex;
            }
        }

        /// <summary>
        /// Atualiza uma autoria e retorna true ou false indicando o sucesso da operação
        /// </summary>
        /// <param name="orgaoOv"></param>
        /// <returns></returns>
        internal bool Atualizar(ulong id_doc, SINJ_ArquivoOV SINJ_ArquivoOV)
        {
            try
            {
                return _acessoAd.Alterar(id_doc, SINJ_ArquivoOV);
            }
            catch (Exception ex)
            {
                if ((ex.Message.IndexOf("duplicate key") > -1 || ex.Message.IndexOf("duplicar valor da chave") > -1 ) || (ex.InnerException != null && (ex.InnerException.Message.IndexOf("duplicate key") > -1 || ex.InnerException.Message.IndexOf("duplicar valor da chave") > -1)))
                {
                    throw new DocDuplicateKeyException("Registro já existente na base de dados!!!");
                }
                throw ex;
            }
        }

        internal bool Excluir(ulong id_doc)
        {
            return _acessoAd.Excluir(id_doc);
        }
    }
}

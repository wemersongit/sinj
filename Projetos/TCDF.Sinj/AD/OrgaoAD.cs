using System;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using util.BRLight;
using System.Collections.Generic;

namespace TCDF.Sinj.AD
{
    public class OrgaoAD
    {
        private AcessoAD<OrgaoOV> _acessoAd;

        public OrgaoAD()
        {
            _acessoAd = new AcessoAD<OrgaoOV>(util.BRLight.Util.GetVariavel("NmBaseOrgao", true));
        }

        internal Results<OrgaoOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        internal string JsonReg(ulong id_doc)
        {
            return _acessoAd.jsonReg(id_doc);
        }

        internal string JsonReg(Pesquisa query)
        {
            return _acessoAd.jsonReg(query);
        }

        internal OrgaoOV Doc(string ch_orgao)
        {
            Pesquisa query = new Pesquisa();
            query.limit = "1";
            query.offset = "0";
            query.literal = string.Format("ch_orgao='{0}'", ch_orgao);
            var result = Consultar(query);
            if (result.result_count > 1)
            {
                throw new Exception("Foi verificado mais de um ch_orgao com a mesma chave.");
            }
            if (result.result_count > 0)
            {
                return result.results[0];
            }
            throw new Exception("Nenhum órgão foi encontrado. É possível que o mesmo já tenha sido excluído.");
        }

        internal OrgaoOV Doc(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

        internal string PathGet(ulong id_doc, string path)
        {
            return _acessoAd.pathGet(id_doc, path);
        }

        internal string PathPut(ulong id_doc, string path, string value, string retorno)
        {
            return _acessoAd.pathPut(id_doc, path, value, retorno);
        }

        internal string PathPut(Pesquisa pesquisa, List<opMode<OrgaoOV>> listopMode)
        {
            return _acessoAd.OP(pesquisa, listopMode);
        }

        /// <summary>
        /// Inclui um orgao e retorna o id_doc
        /// </summary>
        /// <param name="orgaoOv"></param>
        /// <returns></returns>
        internal ulong Incluir(OrgaoOV orgaoOv)
        {
            try
            {
                return _acessoAd.Incluir(orgaoOv);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("duplicate key") > -1 || ex.Message.IndexOf("duplicar valor da chave") > -1)
                {
                    throw new DocDuplicateKeyException("Registro já existente na base de dados!!!");
                }
                throw ex;
            }
        }

        /// <summary>
        /// Atualiza um orgao e retorna true ou false indicando o sucesso da operação
        /// </summary>
        /// <param name="orgaoOv"></param>
        /// <returns></returns>
        internal bool Atualizar(ulong id_doc, OrgaoOV orgaoOv)
        {
            try
            {
                return _acessoAd.Alterar(id_doc, orgaoOv);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("duplicate key") > -1 || ex.Message.IndexOf("duplicar valor da chave") > -1)
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
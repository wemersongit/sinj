using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using util.BRLight;

namespace TCDF.Sinj.AD
{
    public class TipoDeEdicaoAD
    {
        private AcessoAD<TipoDeEdicaoOV> _acessoAd;

        public TipoDeEdicaoAD()
        {
            _acessoAd = new AcessoAD<TipoDeEdicaoOV>(util.BRLight.Util.GetVariavel("NmBaseTipoDeEdicao", true));
        }

        internal Results<TipoDeEdicaoOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        internal TipoDeEdicaoOV Doc(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

        internal TipoDeEdicaoOV Doc(string ch_tipo_edicao)
        {
            Pesquisa query = new Pesquisa();
            query.limit = "1";
            query.offset = "0";
            query.literal = string.Format("ch_tipo_edicao='{0}'", ch_tipo_edicao);
            var result = Consultar(query);
            if (result.result_count > 1)
            {
                throw new Exception("Foi verificado mais de um registro com a mesma chave.");
            }
            if (result.result_count > 0)
            {
                return result.results[0];
            }
            throw new Exception("Nenhum registro foi encontrada. É possível que o mesma já tenha sido excluído.");
        }

        internal string JsonReg(ulong id_doc)
        {
            return _acessoAd.jsonReg(id_doc);
        }

        internal string JsonReg(Pesquisa query)
        {
            return _acessoAd.jsonReg(query);
        }

        internal ulong Incluir(TipoDeEdicaoOV TipoDeEdicaoOV)
        {
            try
            {
                return _acessoAd.Incluir(TipoDeEdicaoOV);
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
        /// Atualiza uma autoria e retorna true ou false indicando o sucesso da operação
        /// </summary>
        /// <param name="orgaoOv"></param>
        /// <returns></returns>
        internal bool Atualizar(ulong id_doc, TipoDeEdicaoOV TipoDeEdicaoOV)
        {
            try
            {
                return _acessoAd.Alterar(id_doc, TipoDeEdicaoOV);
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

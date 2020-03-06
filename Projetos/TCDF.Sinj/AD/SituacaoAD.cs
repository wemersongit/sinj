using neo.BRLightREST;
using TCDF.Sinj.OV;
using util.BRLight;
using System;
using System.Collections.Generic;

namespace TCDF.Sinj.AD
{
    public class SituacaoAD
    {
        private AcessoAD<SituacaoOV> _acessoAd;
        private static List<SituacaoOV> oSituacoes;

        public SituacaoAD()
        {
            _acessoAd = new AcessoAD<SituacaoOV>(util.BRLight.Util.GetVariavel("NmBaseSituacao", true));
        }


        internal Results<SituacaoOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        public List<SituacaoOV> BuscarTodos()
        {
            if (oSituacoes == null || oSituacoes.Count <= 0)
            {
                Pesquisa query = new Pesquisa();
                query.limit = null;
                oSituacoes = Consultar(query).results;
            }
            return oSituacoes;
        }

        internal SituacaoOV Doc(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

        internal SituacaoOV Doc(string ch_situacao)
        {
            Pesquisa query = new Pesquisa();
            query.limit = "1";
            query.offset = "0";
            query.literal = string.Format("ch_situacao='{0}'", ch_situacao);
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

        internal ulong Incluir(SituacaoOV situacaoOv)
        {
            try
            {
                return _acessoAd.Incluir(situacaoOv);
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

        internal bool Atualizar(ulong id_doc, SituacaoOV situacaoOv)
        {
            try
            {
                return _acessoAd.Alterar(id_doc, situacaoOv);
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

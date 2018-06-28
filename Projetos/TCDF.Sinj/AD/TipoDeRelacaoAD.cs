using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using util.BRLight;

namespace TCDF.Sinj.AD
{
    public class TipoDeRelacaoAD
    {
        private AcessoAD<TipoDeRelacaoOV> _acessoAd;

        public TipoDeRelacaoAD()
        {
            _acessoAd = new AcessoAD<TipoDeRelacaoOV>(util.BRLight.Util.GetVariavel("NmBaseTipoDeRelacao", true));
        }

        internal Results<TipoDeRelacaoOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        internal TipoDeRelacaoOV Doc(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

        internal TipoDeRelacaoOV Doc(string ch_tipo_relacao)
        {
            Pesquisa query = new Pesquisa();
            query.limit = "1";
            query.offset = "0";
            query.literal = string.Format("ch_tipo_relacao='{0}'", ch_tipo_relacao);
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

        internal ulong Incluir(TipoDeRelacaoOV tipoDeRelacaoOv)
        {
            try
            {
                return _acessoAd.Incluir(tipoDeRelacaoOv);
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

        internal bool Atualizar(ulong id_doc, TipoDeRelacaoOV tipoDeRelacaoOv)
        {
            try
            {
                return _acessoAd.Alterar(id_doc, tipoDeRelacaoOv);
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

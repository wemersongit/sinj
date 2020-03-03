using neo.BRLightREST;
using TCDF.Sinj.OV;
using util.BRLight;
using System;

namespace TCDF.Sinj.AD
{
    public class TipoDeNormaAD
    {
        private AcessoAD<TipoDeNormaOV> _acessoAd;

        public TipoDeNormaAD()
        {
            _acessoAd = new AcessoAD<TipoDeNormaOV>(util.BRLight.Util.GetVariavel("NmBaseTipoDeNorma", true));
        }

        internal Results<TipoDeNormaOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        internal TipoDeNormaOV Doc(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

        internal TipoDeNormaOV Doc(string ch_tipo_norma)
        {
            Pesquisa query = new Pesquisa();
            query.limit = "1";
            query.offset = "0";
            query.literal = string.Format("ch_tipo_norma='{0}'", ch_tipo_norma);
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

        internal ulong Incluir(TipoDeNormaOV tipoDeNormaOv)
        {
            try
            {
                return _acessoAd.Incluir(tipoDeNormaOv);
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

        internal bool Atualizar(ulong id_doc, TipoDeNormaOV TipoDeNormaOV)
        {
            try
            {
                return _acessoAd.Alterar(id_doc, TipoDeNormaOV);
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

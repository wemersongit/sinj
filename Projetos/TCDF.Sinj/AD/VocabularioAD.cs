using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using util.BRLight;

namespace TCDF.Sinj.AD
{
    public class VocabularioAD
    {
        private AcessoAD<VocabularioOV> _acessoAd;

        public VocabularioAD()
        {
            _acessoAd = new AcessoAD<VocabularioOV>(util.BRLight.Util.GetVariavel("NmBaseVocabulario", true));
        }

        internal Results<VocabularioOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }
        internal VocabularioOV Doc(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

        internal VocabularioOV Doc(string ch_termo)
        {
            Pesquisa query = new Pesquisa();
            query.limit = "1";
            query.offset = "0";
            query.literal = string.Format("ch_termo='{0}'", ch_termo);
            var result = Consultar(query);
            if (result.result_count > 1)
            {
                throw new Exception("Foi verificado mais de um registro com a mesma chave.");
            }
            if (result.result_count > 0)
            {
                return result.results[0];
            }
            throw new Exception("Nenhum registro foi encontrado, é possível que tenha sido excluído.");
        }

        internal string JsonReg(ulong id_doc)
        {
            return _acessoAd.jsonReg(id_doc);
        }

        internal string JsonReg(Pesquisa query)
        {
            return _acessoAd.jsonReg(query);
        }

        internal string PathGet(ulong id_doc, string path)
        {
            return _acessoAd.pathGet(id_doc, path);
        }

        internal string PathPut(ulong id_doc, string path, string value, string retorno)
        {
            return _acessoAd.pathPut(id_doc, path, value, retorno);
        }

        internal ulong Incluir(VocabularioOV vocabularioOv)
        {
            try
            {
                return _acessoAd.Incluir(vocabularioOv);
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

        internal bool Atualizar(ulong id_doc, VocabularioOV vocabularioOv)
        {
            try
            {
                return _acessoAd.Alterar(id_doc, vocabularioOv);
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

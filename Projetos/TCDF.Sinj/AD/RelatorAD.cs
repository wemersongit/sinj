using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;
using util.BRLight;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.AD
{
    public class RelatorAD
    {
        private AcessoAD<RelatorOV> _acessoAd;

        public RelatorAD()
        {
            _acessoAd = new AcessoAD<RelatorOV>(util.BRLight.Util.GetVariavel("NmBaseRelator", true));
        }

        internal Results<RelatorOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        internal RelatorOV Doc(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

        internal RelatorOV Doc(string ch_relator)
        {
            Pesquisa query = new Pesquisa();
            query.limit = "1";
            query.offset = "0";
            query.literal = string.Format("ch_relator='{0}'", ch_relator);
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

        internal ulong Incluir(RelatorOV relatorOv)
        {
            try
            {
                return _acessoAd.Incluir(relatorOv);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && (ex.InnerException.Message.IndexOf("duplicate key") > -1 || ex.InnerException.Message.IndexOf("duplicar valor da chave") > -1))
                {
                    throw new DocDuplicateKeyException("Registro já existente na base de dados!!!");
                }
                throw ex;
            }
        }

        internal bool Atualizar(ulong id_doc, RelatorOV relatorOv)
        {
            try
            {
                return _acessoAd.Alterar(id_doc, relatorOv);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && (ex.InnerException.Message.IndexOf("duplicate key") > -1 || ex.InnerException.Message.IndexOf("duplicar valor da chave") > -1))
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

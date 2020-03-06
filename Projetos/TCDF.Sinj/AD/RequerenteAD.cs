using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using util.BRLight;

namespace TCDF.Sinj.AD
{
    public class RequerenteAD
    {
        private AcessoAD<RequerenteOV> _acessoAd;

        public RequerenteAD()
        {
            _acessoAd = new AcessoAD<RequerenteOV>(util.BRLight.Util.GetVariavel("NmBaseRequerente", true));
        }

        internal Results<RequerenteOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        internal RequerenteOV Doc(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

        internal RequerenteOV Doc(string ch_requerente)
        {
            Pesquisa query = new Pesquisa();
            query.limit = "1";
            query.offset = "0";
            query.literal = string.Format("ch_requerente='{0}'", ch_requerente);
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

        internal ulong Incluir(RequerenteOV requerenteOv)
        {
            try
            {
                return _acessoAd.Incluir(requerenteOv);
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

        internal bool Atualizar(ulong id_doc, RequerenteOV requerenteOv)
        {
            try
            {
                return _acessoAd.Alterar(id_doc, requerenteOv);
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

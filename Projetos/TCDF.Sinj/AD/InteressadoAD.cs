using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using util.BRLight;

namespace TCDF.Sinj.AD
{
    public class InteressadoAD
    {
        private AcessoAD<InteressadoOV> _acessoAd;

        public InteressadoAD()
        {
            _acessoAd = new AcessoAD<InteressadoOV>(util.BRLight.Util.GetVariavel("NmBaseInteressado", true));
        }

        internal Results<InteressadoOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        internal InteressadoOV Doc(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

        internal InteressadoOV Doc(string ch_interessado)
        {
            Pesquisa query = new Pesquisa();
            query.limit = "1";
            query.offset = "0";
            query.literal = string.Format("ch_interessado='{0}'", ch_interessado);
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

        internal ulong Incluir(InteressadoOV interessadoOv)
        {
            try
            {
                return _acessoAd.Incluir(interessadoOv);
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

        internal bool Atualizar(ulong id_doc, InteressadoOV interessadoOv)
        {
            try
            {
                return _acessoAd.Alterar(id_doc, interessadoOv);
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

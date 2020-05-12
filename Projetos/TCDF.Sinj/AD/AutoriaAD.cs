using TCDF.Sinj.OV;
using neo.BRLightREST;
using util.BRLight;
using System;

namespace TCDF.Sinj.AD
{
    public class AutoriaAD
    {
        private AcessoAD<AutoriaOV> _acessoAd;

        public AutoriaAD()
        {
            _acessoAd = new AcessoAD<AutoriaOV>(util.BRLight.Util.GetVariavel("NmBaseAutoria", true));
        }

        internal Results<AutoriaOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        internal AutoriaOV Doc(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

        internal AutoriaOV Doc(string ch_autoria)
        {
            Pesquisa query = new Pesquisa();
            query.limit = "1";
            query.offset = "0";
            query.literal = string.Format("ch_autoria='{0}'", ch_autoria);
            var result = Consultar(query);
            if (result.result_count > 1)
            {
                throw new Exception("Foi verificado mais de uma autoria com a mesma chave.");
            }
            if (result.result_count > 0)
            {
                return result.results[0];
            }
            throw new Exception("Nenhuma autoria foi encontrada. É possível que a mesma já tenha sido excluída.");
        }

        internal string JsonReg(ulong id_doc)
        {
            return _acessoAd.jsonReg(id_doc);
        }

        internal string JsonReg(Pesquisa query)
        {
            return _acessoAd.jsonReg(query);
        }

        internal ulong Incluir(AutoriaOV autoriaOV)
        {
            try
            {
                return _acessoAd.Incluir(autoriaOV);
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
        internal bool Atualizar(ulong id_doc, AutoriaOV autoriaOV)
        {
            try
            {
                return _acessoAd.Alterar(id_doc, autoriaOV);
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

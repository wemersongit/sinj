using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using util.BRLight;

namespace TCDF.Sinj.AD
{
    public class NotifiquemeAD
    {
        private AcessoAD<NotifiquemeOV> _acessoAd;

        public NotifiquemeAD()
        {
            _acessoAd = new AcessoAD<NotifiquemeOV>(util.BRLight.Util.GetVariavel("NmBasePush", true));
        }

        internal Results<NotifiquemeOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        internal NotifiquemeOV Doc(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

        internal string JsonReg(Pesquisa query)
        {
            return _acessoAd.jsonReg(query);
        }

        internal NotifiquemeOV Doc(string email_usuario_push)
        {
            Pesquisa query = new Pesquisa();
            query.limit = "1";
            query.offset = "0";
            query.literal = string.Format("email_usuario_push='{0}'", email_usuario_push);
            var result = Consultar(query);
            if (result.result_count > 1)
            {
                throw new Exception("Foi verificado mais de um usuario com o mesmo login.");
            }
            if (result.result_count > 0)
            {
                return result.results[0];
            }
            throw new DocNotFoundException("Usuário não encontrado.");
        }

        internal string PathPost(ulong id_doc, string path, string value, string retorno)
        {
            return _acessoAd.pathPost(id_doc, path, value, retorno);
        }

        internal string PathPut(ulong id_doc, string path, string value, string retorno)
        {
            return _acessoAd.pathPut(id_doc, path, value, retorno);
        }

        internal string PathDelete(ulong id_doc, string path, string retorno)
        {
            return _acessoAd.pathDelete(id_doc, path, retorno);
        }

        internal ulong Incluir(NotifiquemeOV pushOv)
        {
            try
            {
                return _acessoAd.Incluir(pushOv);
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

        internal bool Atualizar(ulong id_doc, NotifiquemeOV pushOv)
        {
            try
            {
                return _acessoAd.Alterar(id_doc, pushOv);
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
    }
}

using neo.BRLightREST;
using TCDF.Sinj.OV;
using util.BRLight;
using System;

namespace TCDF.Sinj.AD
{
    public class UsuarioAD
    {
        private AcessoAD<UsuarioOV> _acessoAd;

        public UsuarioAD()
        {
            _acessoAd = new AcessoAD<UsuarioOV>(util.BRLight.Util.GetVariavel("NmBaseUsuario", true));
        }

        internal Results<UsuarioOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        internal UsuarioOV Doc(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

        internal string JsonReg(ulong id_doc)
        {
            return _acessoAd.jsonReg(id_doc);
        }

        internal string JsonReg(Pesquisa query)
        {
            return _acessoAd.jsonReg(query);
        }

        internal UsuarioOV Doc(string nm_login_usuario)
        {
            Pesquisa query = new Pesquisa();
            query.limit = "1";
            query.offset = "0";
            query.literal = string.Format("nm_login_usuario='{0}'", nm_login_usuario);
            var result = Consultar(query);
            if (result.result_count > 1)
            {
                throw new Exception("Foi verificado mais de um usu&aacute;rio com o mesmo login.");
            }
            if (result.result_count > 0)
            {
                return result.results[0];
            }
            throw new Exception("Usu&aacute;rio n&atilde;o encontrado.");
        }

        internal string PathPut(ulong id_doc, string path, string value, string retorno)
        {
            return _acessoAd.pathPut(id_doc, path, value, retorno);
        }

        internal ulong Incluir(UsuarioOV usuarioOv)
        {
            try
            {
                return _acessoAd.Incluir(usuarioOv);
            }
            catch (Exception ex)
            {
                if ((ex.Message.IndexOf("duplicate key") > -1 || ex.Message.IndexOf("duplicar valor da chave") > -1 ) || (ex.InnerException != null && (ex.InnerException.Message.IndexOf("duplicate key") > -1 || ex.InnerException.Message.IndexOf("duplicar valor da chave") > -1)))
                {
                    throw new DocDuplicateKeyException("Registro j&aacute; existente na base de dados!!!");
                }
                throw ex;
            }
        }

        internal bool Atualizar(ulong id_doc, UsuarioOV usuarioOv)
        {
            try
            {
                return _acessoAd.Alterar(id_doc, usuarioOv);
            }
            catch (Exception ex)
            {
                if ((ex.Message.IndexOf("duplicate key") > -1 || ex.Message.IndexOf("duplicar valor da chave") > -1 ) || (ex.InnerException != null && (ex.InnerException.Message.IndexOf("duplicate key") > -1 || ex.InnerException.Message.IndexOf("duplicar valor da chave") > -1)))
                {
                    throw new DocDuplicateKeyException("Registro j&aacute; existente na base de dados!!!");
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

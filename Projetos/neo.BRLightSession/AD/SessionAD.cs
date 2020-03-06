using System;
using System.Collections.Generic;
using neo.BRLightREST;
using neo.BRLightSession.OV;
using util.BRLight;

namespace neo.BRLightSession.AD
{
    public class SessionAD
    {
        private const string nm_base = "session";
        private const string uri = "";

        public string Pesquisar(Pesquisa oPesquisa)
        {
            return new Reg(nm_base, uri).pesquisar(oPesquisa);
        }

        public string Pesquisar(UInt64 id_doc)
        {
            return new Reg(nm_base, uri).pesquisarReg(id_doc);
        }

        public UInt64 Incluir(SessionOV SessionOV)
        {
            try {
                return new AcessoAD<SessionOV>(nm_base).Incluir(SessionOV);
            } catch (Exception exception) {
                throw new Exception(exception.Message);
            }
        }

        internal bool Deletar(ulong id_doc)
        {
            return new Reg(nm_base, uri).excluir(id_doc);
        }

        public bool Alterar(ulong id_doc, SessionOV oSession)
        {
            var postParameters = new Dictionary<string, object> { { "value", JSON.Serialize<SessionOV>(oSession) } };
            return new Reg(nm_base, uri).alterar(id_doc, postParameters);
        }

        public SessionOV ConsultarReg(ulong id_doc)
        {
            return new AcessoAD<SessionOV>(nm_base).ConsultarReg(id_doc);
        }

        public string jsonReg(Pesquisa opesquisa)
        {
            return new AcessoAD<SessionOV>(nm_base).jsonReg(opesquisa);
        }

        public string jsonReg(ulong id_doc)
        {
            return new AcessoAD<SessionOV>(nm_base).jsonReg(id_doc);
        }

    }
}

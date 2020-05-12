using System;
using neo.BRLightREST;
using neo.BRLightSession.AD;
using neo.BRLightSession.OV;
using util.BRLight;

namespace neo.BRLightSession.RN
{
    public class SessionRN
    {

        public Results<SessionOV> ConsultarTodos()
        {
            var oPesquisa = new Pesquisa();
            var resultado = new SessionAD().Pesquisar(oPesquisa);
            return JSON.Deserializa<Results<SessionOV>>(resultado);
        }


        public string ObterjsonReg(Pesquisa opesquisa)
        {
            return new SessionAD().Pesquisar(opesquisa);
        }

        public Results<SessionOV> Consultar(Pesquisa oPesquisa)
        {
            var resultado = new SessionAD().Pesquisar(oPesquisa);
            return JSON.Deserializa<Results<SessionOV>>(resultado);
        }

        public SessionOV Consultar(UInt64 id_doc)
        {

            Params.CheckNotZeroOrNull("id_doc", id_doc);

            var resultado = new SessionAD().Pesquisar(id_doc);
            return JSON.Deserializa<SessionOV>(resultado);
        }

        public UInt64 Incluir(SessionOV SessionOV) {

            Params.CheckNotNullOrEmpty("id_session", SessionOV.id_session);
            Params.CheckNotNullOrEmpty("valor", SessionOV.ds_valor);
            Params.CheckNotNullOrEmpty("data", SessionOV.dt_criacao);
            Params.CheckNotNullOrEmpty("expira", SessionOV.dt_expiracao);

            return new SessionAD().Incluir(SessionOV);
        }

        public bool Deletar(UInt64 id_doc)  {
            Params.CheckNotZeroOrNull("id_doc", id_doc);
            return new SessionAD().Deletar(id_doc);
        }

        public bool Alterar(UInt64 id_doc, SessionOV oSession) {
            Params.CheckNotNullOrEmpty("id_session", oSession.id_session);
            Params.CheckNotNullOrEmpty("valor", oSession.ds_valor);
            Params.CheckNotNullOrEmpty("data", oSession.dt_criacao);
            Params.CheckNotNullOrEmpty("expira", oSession.dt_expiracao);
            return new SessionAD().Alterar(id_doc, oSession);
        }

        public SessionOV ConsultarReg(ulong id_doc) {
            Params.CheckNotZeroOrNull("id_doc", id_doc);
            return new SessionAD().ConsultarReg(id_doc);
        }

        public string jsonReg(Pesquisa oPesquisa) {
            return new SessionAD().jsonReg(oPesquisa);
        }

        public string jsonReg(ulong id_doc) {
            Params.CheckNotZeroOrNull("id_doc", id_doc);
            return new SessionAD().jsonReg(id_doc);
        }

    }
}
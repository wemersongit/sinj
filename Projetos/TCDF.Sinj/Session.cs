using System;
using System.Threading;
using neo.BRLightREST;
using neo.BRLightSession.OV;
using neo.BRLightSession.RN;
using util.BRLight;

namespace neo.BRLightSession
{
    public class Session {

        public ulong db_id { get; private set; }
        public string db_chave{ get; private set; }

        //public string id_session { get; private set; }
       // public string id_sessionLook { get; private set; }
        private ulong timeout { get; set; }
		private string cookieName;
        private string cookieNameLook;

        public Session(string cookieName, string cookieNameLook)
        {
            this.cookieName = cookieName;
            this.cookieNameLook = cookieNameLook;
            if (timeout == 0) timeout = 2;
        }

        private String createIdSession(bool manter_ativa)
        {
			var cok_id_session = Cookies.ReadCookie(cookieName);
            
            if (string.IsNullOrEmpty(cok_id_session))
                cok_id_session = Guid.NewGuid().ToString();

            try {
                Cookies.RemoveCookie(cookieName);
                Cookies.RemoveCookie(cookieNameLook);
            } catch  {

            }

            if (manter_ativa) {
                Cookies.WriteCookie(cookieName, cok_id_session, true, DateTime.Now.AddDays(timeout));
                Cookies.WriteCookie(cookieNameLook, Guid.NewGuid().ToString(), false, DateTime.Now.AddDays(timeout));
            } else {
                Cookies.WriteCookie(cookieName, cok_id_session, true);
                Cookies.WriteCookie(cookieNameLook, Guid.NewGuid().ToString(), false);
            }

            return cok_id_session;    
        }

        public Session()
        {
            this.cookieName = "neoLightBase";
            this.cookieNameLook = "neoLightBaseLook";
            if (timeout == 0) timeout = 2;
        }

        ~Session() {
            try {
               
            } catch {

            }
        }

        public void Create(bool manter_ativa) {
            if (timeout == 0) timeout = 2;
            createIdSession(manter_ativa);
        }

        public bool Put(string key, string value) {
            return Put<string>(key, value);
        }

        public bool Put<T>(string key, T value) {
            var resultado = false;
            var result = getResult(key, new[] { "*" }); 

            if (result.result_count == 1){
                result.results[0].ds_valor = JSON.Serialize<T>(value);
                result.results[0].dt_expiracao = Convert.ToDateTime(DateTime.Now).AddDays(timeout).ToString("dd'/'MM'/'yyyy HH:mm:ss");
                resultado = new SessionRN().Alterar(result.results[0]._metadata.id_doc, result.results[0]);
                // expoem o id do banco e chave da session
                db_id = result.results[0]._metadata.id_doc;
                db_chave = result.results[0].id_session;
            }

            if (result.result_count > 1) {
                deleteResult(ref result);
            }

            return resultado ;

        }

        public bool Post(string key, string value)
        {
            return Post<string>(key, value);
        }

        public bool Post<T>(string key, T value) where T : class
		{
            var id_session = Cookies.ReadCookie(cookieName);
            var oSession = new SessionOV
                               {
                                   id_session = id_session + "" + key,
                                   ds_valor = JSON.Serialize<T>(value),
                                   dt_criacao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"),
                                   ds_user = Util.GetUser(),
                                   ds_origem = Util.GetUserIp(),
                                   ds_tipo_acesso = Util.GetUA()
                               };

            oSession.dt_expiracao = Convert.ToDateTime(oSession.dt_criacao).AddDays(timeout).ToString("dd'/'MM'/'yyyy HH:mm:ss");

           var id_sessao = new SessionRN().Incluir(oSession);
           // expoem o id do banco e chave da session
           db_id = id_sessao;
           db_chave = id_session + key;

           return (id_sessao != 0);

        }

        public string Get(string key) {
            return Get<string>(key);
        }

        public T Get<T>(string key) where T : class
        {
            object sValor = null;

            var result = getResult(key, new[] { "*" }); 
            if (result.result_count == 1) {
                if (Convert.ToDateTime(result.results[0].dt_expiracao) >= DateTime.Now){
                    sValor = result.results[0];
                } else {
                    deleteResult(ref result);
                }
                // expoem o id do banco e chave da session
                db_id = result.results[0]._metadata.id_doc;
                db_chave = result.results[0].id_session;
            }
            if (result.result_count > 1) {
                deleteResult(ref result);
            }
            return (T)sValor;
        }

        public void Delete(string key)
        {
            var result = getResult(key, new[] { "id_doc" });
            if (result.result_count > 0) 
                deleteResult(ref result);

        }

        public void DeleteAll()
        {
            var result = getResult("%", new[] { "id_doc" });
            if (result.result_count > 0)
                deleteResult(ref result);
			Cookies.DeleteCookie(cookieName);
            Cookies.DeleteCookie(cookieNameLook);
        }

        private Results<SessionOV> getResult(string key, string[] sCampos)
        {
            var id_session = Cookies.ReadCookie(cookieName);
            var oPesquisa = new Pesquisa
                                {
                                    literal = "id_session='" + id_session + "" + key + "'",
                                    select = sCampos
                                };
            var pesq = new SessionRN();
            return pesq.Consultar(oPesquisa);
        }

        private static void deleteResult(ref Results<SessionOV> result)  {
            for (var i = 0; i < (int)result.result_count; i++)
                new SessionRN().Deletar(result.results[i]._metadata.id_doc);
        }

        public void deleteExperi()
        {
            new Thread(delete).Start();
        }

        private static void delete()
        {
            try
            {
                var oPesquisa = new Pesquisa();
                oPesquisa.literal = "CAST(dt_expiracao AS DATE) <= '" + DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss") + "'";
                var results = new SessionRN().Consultar(oPesquisa);
                if (results.result_count > 0)
                    deleteResult(ref results);
            } catch {

            }

        }

    }
}
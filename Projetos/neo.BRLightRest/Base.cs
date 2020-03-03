using System;
using System.Collections.Generic;
using System.Web;
using util.BRLight;

namespace neo.BRLightREST
{
    public class Base : neoBRLightREST
    {

        public Base()
        {
            BaseUrl = BaseUrl;
            Params.CheckNotNullOrEmpty("BaseUrl", BaseUrl);
        }

        public Base(string pBaseUrl)
        {
            BaseUrl = pBaseUrl;
            Params.CheckNotNullOrEmpty("BaseUrl", BaseUrl);
        }

        public string pesquisarBase(string nm_base)
        {

            Params.CheckIsNullOrEmpty("nm_base", nm_base);

            iUri = BaseUrl + "/" + nm_base;

            string resultado;
            try {
                var objRest = new REST(iUri, HttpVerb.GET, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                resultado = Response;

            } catch (Exception ex) {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível pesquisar Base: " + nm_base + " URI: " + iUri, ex);
            }

            return resultado;
        }

        public string GetPathBase(string nm_base, string path)
        {
            Params.CheckNotNullOrEmpty("nm_base", nm_base);
            Params.CheckNotNullOrEmpty("path", path);

            iUri = BaseUrl + "/" + nm_base + "/" + path;

            string resultado;
            try
            {
                var objRest = new REST(iUri, HttpVerb.GET, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                resultado = Response;

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível pesquisar path da Base: " + nm_base + " URI: " + iUri, ex);
            }

            return resultado.Replace("\"","");
        }

        public string pesquisar(Pesquisa oPesquisa)
        {
            if (oPesquisa == null) {
                iUri = BaseUrl;
            } else {
                iUri = BaseUrl + "?$$=" + HttpUtility.UrlEncode(JSON.Serialize<Pesquisa>(oPesquisa));
            }
            string resultado;
            try
            {
                var objRest = new REST(iUri, HttpVerb.GET, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                resultado = Response;

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível pesquisar em base." + " URI: " + iUri, ex);
            }
            return resultado;
        }

        public UInt64 incluir(Dictionary<string, object> Parameters)
        {
            //   Params.CheckExistKeyInDictionary("json_base", Parameters);

            iUri = BaseUrl ;

            UInt64 idNovo;
            try
            {
                var objRest = new REST(iUri, HttpVerb.POST, Parameters) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                try
                {
                    idNovo = Convert.ToUInt64(Response);
                }
                catch
                {
                    idNovo = 0;
                }

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível incluir em base. URI: " + BaseUrl, ex);
            }
            return idNovo;
        }

        public bool alterar(string nm_base, Dictionary<string, object> Parameters)
        {
            //     Params.CheckExistKeyInDictionary("struct", Parameters);

            iUri = BaseUrl + "/" + nm_base;

            var resultado = false;
            try
            {
                var objRest = new REST(iUri, HttpVerb.PUT, Parameters) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                if (Response.ToUpper() == "UPDATED") {
                    resultado = true;
                }

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível alterar base: " + nm_base + " URI: " + iUri, ex);
            }

            return resultado;
        }

        public bool excluir(string nm_base)
        {
            Params.CheckNotNullOrEmpty("nm_base", nm_base);

            iUri = BaseUrl + "/" + nm_base;

            var resultado = false;
            try
            {
                var parametros = new Dictionary<string, object> { };
                var objRest = new REST(iUri, HttpVerb.DELETE, parametros) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                if (Response.ToUpper() == "DELETED")
                {
                    resultado = true;
                }
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível excluir base: " + nm_base + " URI: " + iUri, ex);
            }

            return resultado;
        }
    }
}
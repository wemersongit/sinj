using System;
using System.Collections.Generic;
using System.Web;
using util.BRLight;

namespace neo.BRLightREST
{
    public class Reg : neoBRLightREST
    {

        public Reg(string pBaseNome)
        {
            BaseUrl = BaseUrl;
            BaseNome = pBaseNome;
            Params.CheckNotNullOrEmpty("BaseNome", BaseNome);
            Params.CheckNotNullOrEmpty("BaseUrl", BaseUrl);
        }

        public Reg(string pBaseNome, string pBaseUrl)
        {
            BaseNome = pBaseNome;
            BaseUrl = pBaseUrl;
            Params.CheckNotNullOrEmpty("BaseNome", BaseNome);
            Params.CheckNotNullOrEmpty("BaseUrl", BaseUrl);
        }

        public string pesquisarReg(ulong id_doc)
        {

            Params.CheckNotZeroOrNull("id_doc", id_doc);

            iUri = BaseUrl + "/" + BaseNome + "/doc/" + id_doc;

            var resultado = "";
            try
            {
                var objRest = new REST(iUri, HttpVerb.GET, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                resultado = Response;

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível pesquisarReg em Doc: " + id_doc + " URI: " + iUri, ex);
            }

            return resultado;
        }

        public string pesquisarRegFull(ulong id_doc)
        {

            Params.CheckNotZeroOrNull("id_doc", id_doc);

            iUri = BaseUrl + "/" + BaseNome + "/doc/" + id_doc + "/full";

            var resultado = "";
            try
            {
                var objRest = new REST(iUri, HttpVerb.GET, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                resultado = Response;

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível pesquisarRegFull em Doc: " + id_doc + " URI: " + iUri, ex);
            }

            return resultado;
        }

        public T pesquisarReg<T>(ulong id_doc)
        {
            Params.CheckNotZeroOrNull("id_doc", id_doc);

            iUri = BaseUrl + "/" + BaseNome + "/doc/" + id_doc;
            T resultado;
            try
            {
                var objRest = new REST(iUri, HttpVerb.GET, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                resultado = JSON.Deserializa<T>(Response);
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível pesquisarReg<T> em Doc: " + id_doc + " URI: " + iUri, ex);
            }
            return resultado;
        }

        public T pesquisarRegFull<T>(ulong id_doc)
        {
            Params.CheckNotZeroOrNull("id_doc", id_doc);

            iUri = BaseUrl + "/" + BaseNome + "/doc/" + id_doc;
            T resultado;
            try
            {
                var objRest = new REST(iUri, HttpVerb.GET, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                resultado = JSON.Deserializa<T>(Response);
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível pesquisarRegFull<T> em Doc: " + id_doc + " URI: " + iUri, ex);
            }
            return resultado;
        }

        public string pesquisar(Pesquisa oPesquisa)
        {
            if (oPesquisa == null) {
                iUri = BaseUrl + "/" + BaseNome + "/doc";
            } else {
                iUri = BaseUrl + "/" + BaseNome + "/doc?$$=" + HttpUtility.UrlEncode(JSON.Serialize<Pesquisa>(oPesquisa));
                var blaa = iUri;
            }

            var resultado = "";
            try
            {
                var objRest = new REST(iUri, HttpVerb.GET, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                resultado = Response;

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível pesquisar em Doc URI: " + iUri, ex);
            }
            return resultado;
        }

        public string fieldRelacional(ulong id_doc, string field)
        {

            iUri = BaseUrl + "/" + BaseNome + "/doc/" + id_doc + field;

            var resultado = "";
            try
            {
                var objRest = new REST(iUri, HttpVerb.GET, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                resultado = Response;

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST fieldRelacional: Não foi possível pesquisar em Doc URI: " + iUri, ex);
            }
            return resultado;
        }

        public string pathGet(ulong id_doc, string path)
        {

            iUri = BaseUrl + "/" + BaseNome + "/doc/" + id_doc + "/" + HttpUtility.UrlEncode(path);

            var resultado = "";
            try
            {
                var objRest = new REST(iUri, HttpVerb.GET, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                resultado = Response;

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST pathGet: Não foi possível pesquisar em Reg URI: " + iUri, ex);
            }
            return resultado;
        }

        public string pathPost(ulong id_doc, string path, string value, string retorno)
        {

            iUri = BaseUrl + "/" + BaseNome + "/doc/" + id_doc + "/" + HttpUtility.UrlEncode(path);

            var resultado = "";
            try
            {
                var parametros = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(retorno)) parametros.Add("return", retorno);
                parametros.Add("value", value);
                var objRest = new REST(iUri, HttpVerb.POST, parametros) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                resultado = Response;

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST pathPost: Não foi possível pesquisar em Doc URI: " + iUri, ex);
            }
            return resultado;
        }

        public string pathPut(ulong id_doc, string path, string value, string retorno)
        {

            iUri = BaseUrl + "/" + BaseNome + "/doc/" + id_doc + "/" + HttpUtility.UrlEncode(path);

            var resultado = "";
            try
            {
                var parametros = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(retorno)) parametros.Add("return", retorno);
                parametros.Add("value", value);
                var objRest = new REST(iUri, HttpVerb.PUT, parametros) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                resultado = Response;

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST pathPut: Não foi possível pesquisar em Reg URI: " + iUri, ex);
            }
            return resultado;
        }

        public string pathDelete(ulong id_doc, string path, string retorno)
        {

            iUri = BaseUrl + "/" + BaseNome + "/doc/" + id_doc + "/" + HttpUtility.UrlEncode(path);
            var resultado = "";
            try
            {
                var parametros = new Dictionary<string, object>();
                if (!string.IsNullOrEmpty(retorno)) parametros.Add("return", retorno);
                var objRest = new REST(iUri, HttpVerb.DELETE, parametros) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                resultado = Response;

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST pathDelete: Não foi possível pesquisar em Doc URI: " + iUri, ex);
            }
            return resultado;
        }

        public UInt64 incluir(Dictionary<string, object> Parameters)
        {

            Params.CheckExistKeyInDictionary("value", Parameters);

            iUri = BaseUrl + "/" + BaseNome + "/doc";

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
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível incluir em Doc URI: " + iUri, ex);
            }
            return idNovo;
        }


        public bool alterar(UInt64 id_doc, Dictionary<string, object> parametros)
        {

            Params.CheckNotZeroOrNull("id_doc", id_doc);
            Params.CheckExistKeyInDictionary("value", parametros);

            iUri = BaseUrl + "/" + BaseNome + "/doc/" + id_doc;

            var resultado = false;

            try {
                var objRest = new REST(iUri, HttpVerb.PUT, parametros) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                if (Response.ToUpper() == "UPDATED") {
                    resultado = true;
                }
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível alterar em Doc: " + id_doc + " URI: " + iUri, ex);
            }

            return resultado;
        }

        public string OP(Pesquisa oPesquisa, Dictionary<string, object> parametros)
        {
            iUri = BaseUrl + "/" + BaseNome + "/doc?$$=" + HttpUtility.UrlEncode(JSON.Serialize<Pesquisa>(oPesquisa));

            string resultado;
            try {
                var objRest = new REST(iUri, HttpVerb.PUT, parametros) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                resultado = Response;
            } catch (Exception ex) {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível OP Lote de Registros:  URI: " + iUri, ex);
            }

            return resultado;
        }

        public bool excluir(UInt64 id_doc)
        {
            Params.CheckNotZeroOrNull("id_doc", id_doc);

            iUri = BaseUrl + "/" + BaseNome + "/doc/" + id_doc;

            var resultado = false;

            try
            {
                var parametros = new Dictionary<string, object> { };
                var objRest = new REST(iUri, HttpVerb.DELETE, parametros) { RequestTimeOut = TimeOut };

                preencheResponse(ref objRest);

                if (Response.ToUpper() == "DELETED") {
                    resultado = true;
                }

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível excluir em Doc: " + id_doc + " URI: " + iUri, ex);
            }

            return resultado;
        }

        public string excluir(Pesquisa oPesquisa)
        {
            if (oPesquisa == null) {
                iUri = BaseUrl + "/" + BaseNome + "/doc";
            } else {
                iUri = BaseUrl + "/" + BaseNome + "/doc?$$=" + HttpUtility.UrlEncode(JSON.Serialize<Pesquisa>(oPesquisa));
            }

            var resultado = "";
            try
            {
                var objRest = new REST(iUri, HttpVerb.DELETE, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                resultado = Response;

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível pesquisar em Doc URI: " + iUri, ex);
            }
            return resultado;
        }
   
    }
}
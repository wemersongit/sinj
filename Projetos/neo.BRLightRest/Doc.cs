using System;
using System.Collections.Generic;
using System.Web;
using util.BRLight;

namespace neo.BRLightREST
{
    public class Doc : neoBRLightREST
    {

        public Doc(string pBaseNome)
        {
            BaseUrl = BaseUrl;
            BaseNome = pBaseNome;
            Params.CheckNotNullOrEmpty("BaseNome", BaseNome);
            Params.CheckNotNullOrEmpty("BaseUrl", BaseUrl);
            if (TimeOut == 0)
            {
                TimeOut = 400000;
            }
        }

        public Doc(string pBaseNome, string pBaseUrl)
        {
            BaseUrl = pBaseUrl;
            BaseNome = pBaseNome;
            Params.CheckNotNullOrEmpty("BaseNome", BaseNome);
            Params.CheckNotNullOrEmpty("BaseUrl", BaseUrl);
            if (TimeOut == 0)
            {
                TimeOut = 400000;
            }
        }

        public string pesquisarDoc(string id_file)
        {
            Params.CheckNotNullOrEmpty("id_file", id_file);

            iUri = BaseUrl + "/" + BaseNome + "/file/" + id_file;

            string resultado;
            try
            {
                var objRest = new REST(iUri, HttpVerb.GET, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                resultado = Response;

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível pesquisar Doc:" + id_file + " URI: " + iUri, ex);
            }

            return resultado;
        }

        public File doc(string id_file)
        {
            Params.CheckNotNullOrEmpty("id_file", id_file);

            iUri = BaseUrl + "/" + BaseNome + "/file/" + id_file;

            File resultado;
            try
            {
                var objRest = new REST(iUri, HttpVerb.GET, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                resultado = JSON.Deserializa<File>(Response);

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível pesquisar doc:" + id_file + " URI: " + iUri, ex);
            }

            return resultado;
        }

        public string pesquisar(Pesquisa oPesquisa)
        {

            if (oPesquisa == null){
                iUri = BaseUrl + "/" + BaseNome + "/file";
            } else {
                iUri = BaseUrl + "/" + BaseNome + "/file?$$=" + HttpUtility.UrlEncode(JSON.Serialize<Pesquisa>(oPesquisa));
            }

            string resultado;
            try {

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

        public string incluir(Dictionary<string, object> parametros)
        {
            string chaveDoc;

            Params.CheckExistValueFileParameter("FileParameter", parametros);

            iUri = BaseUrl + "/" + BaseNome + "/file";

            try
            {
                var objRest = new REST(iUri, HttpVerb.POST, parametros) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                try{
                    chaveDoc = Response;
                } catch {
                    chaveDoc = "";
                }
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível incluir em Doc URI: " + iUri, ex);
            }
            return chaveDoc;
        }
      
        public bool alterarText(UInt64 id_file, Dictionary<string, object> parametros)
        {
            Params.CheckNotZeroOrNull("id_file", id_file);
            Params.CheckExistKeyInDictionary("texto_doc", parametros);

            iUri = BaseUrl + "/" + BaseNome + "/file/" + id_file + "/text";

            var resultado = false;
            try
            {
                var objRest = new REST(iUri, HttpVerb.PUT, parametros) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                if (Response.ToUpper() == "UPDATED") {
                    resultado = true;
                }

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível alterarText em Doc: " + id_file + " URI: " + iUri, ex);
            }

            return resultado;
        }

        public bool excluir(UInt64 id_file)
        {
            Params.CheckNotZeroOrNull("id_file", id_file);

            iUri = BaseUrl + "/" + BaseNome + "/file/" + id_file;

            var resultado = false;
            try
            {
                var parametros = new Dictionary<string, object>();
                parametros.Add("$method", "DELETE");
                var objRest = new REST(iUri, HttpVerb.POST, parametros) { RequestTimeOut = TimeOut };

                preencheResponse(ref objRest);

                if (Response.ToUpper() == "DELETED")
                {
                    resultado = true;
                }
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível excluir em Doc: " + id_file + " URI: " + iUri, ex);
            }

            return resultado;
        }

        public byte[] download(string id_file)
        {
           
            if (!string.IsNullOrEmpty(id_file))
            {
                byte[] resultado;
                iUri = BaseUrl + "/" + BaseNome + "/file/" + id_file + "/download";
                try
                {
                    var objRest = new REST(iUri, HttpVerb.GET, new Dictionary<string, object>())
                                      {RequestTimeOut = TimeOut};
                    resultado = objRest.GetResponseStream();
                }
                catch (Exception ex)
                {
                    throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível realizar download em Doc: " + id_file + " URI: " + iUri,ex);
                }
                return resultado;
            }
            else
            {
                return null;
            }
            
        }

        public byte[] download(UInt64 id_file, string disposition)
        {

            Params.CheckNotZeroOrNull("id_file", id_file);
            Params.CheckNotNullOrEmpty("disposition", disposition);

            if (!disposition.ToLower().Contains("attachment") || !disposition.ToLower().Contains("inline"))
            {
                throw new ParametroInvalidoException(disposition);
            }

            iUri = BaseUrl + "/" + BaseNome + "/file/" + id_file + "/download?disposition=" + disposition;

            byte[] resultado;

            try
            {
                var objRest = new REST(iUri, HttpVerb.GET, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
                resultado = objRest.GetResponseStream();
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível realizar download em Doc: " + id_file + " URI: " + iUri, ex);
            }

            return resultado;
        }
    }
}
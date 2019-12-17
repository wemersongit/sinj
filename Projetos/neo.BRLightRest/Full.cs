using System;
using System.Collections.Generic;
using util.BRLight;

namespace neo.BRLightREST
{
    public class Full : neoBRLightREST
    {

        public Full(string pBaseNome)
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

        public Full(string pBaseNome, string pBaseUrl)
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

        public string pesquisarRegFull(ulong id_doc)
        {

            Params.CheckNotZeroOrNull("id_doc", id_doc);

            iUri = BaseUrl + "/" + BaseNome + "/full/" + id_doc ;

            string resultado;
            try
            {
                var objRest = new REST(iUri, HttpVerb.GET, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
                preencheResponse(ref objRest);
                resultado = Response;

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("neoBRLightREST BASE: Não foi possível pesquisarRegFull em Reg: " + id_doc + " URI: " + iUri, ex);
            }

            return resultado;
        }
    }
}
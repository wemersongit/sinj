using System;
using util.BRLight;

namespace neo.BRLightREST
{
    public class neoBRLightREST
    {

        public int TimeOut { get; set; }

        public string BaseNome { get; set; }

        public string StatusResponse { get; internal set; }

        public string Response { get; internal set; }

        public string iUri { get; internal set; }

        private string _uri;

        public string BaseUrl
        {
            get {
                if (String.IsNullOrEmpty(_uri))
                    _uri = Config.ValorChave("URLBaseREST", true);
                return _uri;
            }
            set { _uri = value; }
        }

        internal void preencheResponse(ref REST oREST)
        {
            Response = oREST.GetResponse();
            StatusResponse = oREST.GetStatusCode();
        }

        internal void preencheResponse(string pResponse, string pStatusResponse)
        {
            Response = pResponse;
            StatusResponse = pStatusResponse;
        }

        public ErroOV Erro
        {

            get
            {
                if (string.IsNullOrEmpty(Response))
                {
                    return new ErroOV();
                } try {
                    var erroOv = new ErroOV();
                    if (Response.IndexOf("error_message") > -1) {
                        erroOv = JSON.Deserializa<ErroOV>(Response);
                    }
                    return erroOv;
                } catch {
                    return new ErroOV();
                        
                }
            }
        }
    }
}


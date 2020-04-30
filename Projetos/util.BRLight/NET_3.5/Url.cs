using System.Web;

namespace util.BRLight
{
    public class Url
    {
        /// <summary>
        /// Decodifica um parametro da urlReferrer
        /// </summary>
        /// <param name="request"></param>
        /// <param name="chave"></param>
        /// <returns></returns>
        public string QueryStringInRequestUrlReferrer(HttpRequest request, string chave)
        {
            var uri = request.UrlReferrer.OriginalString;
            string[] uriSplit = uri.Split('?');
            if (uriSplit.Length == 2)
            {
                string queryUri = uriSplit[1];
                if (queryUri.Contains(chave))
                {
                    string[] parametros = queryUri.Split('&');
                    foreach (string parametro in parametros)
                    {
                        string[] parametroSplit = parametro.Split('=');
                        if (parametroSplit.Length == 2)
                        {
                            if (parametroSplit[0] == chave)
                            {
                                return HttpUtility.UrlDecode(parametroSplit[1]);
                            }
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// Decodifica um parametro da url
        /// </summary>
        /// <param name="request"></param>
        /// <param name="chave"></param>
        /// <returns></returns>
        public string QueryStringInRequestUrl(HttpRequest request, string chave)
        {
            var uri = request.Url.OriginalString;
            string[] uriSplit = uri.Split('?');
            if (uriSplit.Length == 2)
            {
                string queryUri = uriSplit[1];
                if (queryUri.Contains(chave))
                {
                    string[] parametros = queryUri.Split('&');
                    foreach (string parametro in parametros)
                    {
                        string[] parametroSplit = parametro.Split('=');
                        if (parametroSplit.Length == 2)
                        {
                            if (parametroSplit[0] == chave)
                            {
                                return HttpUtility.UrlDecode(parametroSplit[1]);
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
using System;
using System.Collections.Generic;
using util.BRLight;

namespace neo.BRLightREST
{
    public class _command : neoBRLightREST
    {
       public _command()
        {
            BaseUrl = BaseUrl;
            Params.CheckNotNullOrEmpty("BaseUrl", BaseUrl);
            if (TimeOut == 0) TimeOut = 4000;
        }

       public _command( string pBaseUrl)
        {
            BaseUrl = pBaseUrl;
            Params.CheckNotNullOrEmpty("BaseUrl", BaseUrl);
            if (TimeOut == 0) TimeOut = 4000;
        }

       public string version()
       {
           string result;

           iUri = BaseUrl + "/_command/version ";

           try {
               var objRest = new REST(iUri, HttpVerb.POST, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
               preencheResponse(ref objRest);
               try {
                   result = Response;
               } catch {
                   result = "";
               }
           }
           catch (Exception ex)
           {
               throw new FalhaOperacaoException("neoBRLightREST _command: Não foi possível verificar a 'version': " + iUri, ex);
           }
           return result;
       }

       public string reset()
       {
           var result = "";

           iUri = BaseUrl + "/_command/reset ";

           try
           {
               var objRest = new REST(iUri, HttpVerb.POST, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
               preencheResponse(ref objRest);
               try
               {
                   result = Response;
               }
               catch
               {
                   result = "";
               }
           }
           catch (Exception ex)
           {
               throw new FalhaOperacaoException("neoBRLightREST _command: Não foi possível verificar a 'reset': " + iUri, ex);
           }
           return result;
       }

       public string rest_url()
       {
           var result = "";

           iUri = BaseUrl + "/_command/rest_url ";

           try
           {
               var objRest = new REST(iUri, HttpVerb.POST, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
               preencheResponse(ref objRest);
               try
               {
                   result = Response;
               }
               catch
               {
                   result = "";
               }
           }
           catch (Exception ex)
           {
               throw new FalhaOperacaoException("neoBRLightREST _command: Não foi possível verificar a 'rest_url': " + iUri, ex);
           }
           return result;
       }

       public string db_url()
       {
           var result = "";

           iUri = BaseUrl + "/_command/db_url ";

           try
           {
               var objRest = new REST(iUri, HttpVerb.POST, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
               preencheResponse(ref objRest);
               try
               {
                   result = Response;
               }
               catch
               {
                   result = "";
               }
           }
           catch (Exception ex)
           {
               throw new FalhaOperacaoException("neoBRLightREST _command: Não foi possível verificar a 'db_url': " + iUri, ex);
           }
           return result;
       }

       public string base_mem()
       {
           var result = "";

           iUri = BaseUrl + "/_command/base_mem ";

           try
           {
               var objRest = new REST(iUri, HttpVerb.POST, new Dictionary<string, object>()) { RequestTimeOut = TimeOut };
               preencheResponse(ref objRest);
               try
               {
                   result = Response;
               }
               catch
               {
                   result = "";
               }
           }
           catch (Exception ex)
           {
               throw new FalhaOperacaoException("neoBRLightREST _command: Não foi possível verificar a 'base_mem': " + iUri, ex);
           }
           return result;
       }

    }
}

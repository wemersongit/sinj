using System;
using System.Web;
  
namespace util.BRLight
{
    public static class Cookies {
		
        public static char KeyDelimiter = '_';

        public static bool CookiesSupported {
            get {
				bool cook = false;
				try {
			     	cook = true;
                } catch (Exception Ex) {
                    throw new Exception("CookiesSupported: ", Ex);
                }      
     
				return cook;
		    }      
     		
        }

        public static string ReadCookie(string cookieName) {
            try {
                  var httpCookie =  HttpContext.Current.Request.Cookies[cookieName];
                  return httpCookie != null ? HttpContext.Current.Server.HtmlEncode(httpCookie.Value).Trim() : string.Empty;

	         } catch (Exception Ex)
             {
                 throw new Exception("ReadCookie: ", Ex);
             }      
     
        }

        public static bool WriteCookie(string cookieName, string cookieValue, bool isHttpCookie) {
			bool resultado = false;
            try { 
                var aCookie = new HttpCookie(cookieName)
                                  {
                                      Value = cookieValue,
                                      HttpOnly = isHttpCookie
                                  };
				
                HttpContext.Current.Response.Cookies.Add(aCookie);
				resultado = true;

            }  catch ( Exception Ex )  {
                throw new Exception("WriteCookie: ", Ex);
            }

            return resultado;
        }

        public static bool WriteCookie(string cookieName, string cookieValue, bool isHttpCookie, DateTime cookieExpireDate) {
			bool resultado = false;
            try {
                var aCookie = new HttpCookie(cookieName)
                                  {
                                      Value = cookieValue,
                                      Expires = cookieExpireDate,
                                      HttpOnly = isHttpCookie
                                  };
				
                HttpContext.Current.Response.Cookies.Add(aCookie);
				resultado = true;

            }  catch ( Exception Ex )  {
                throw new Exception("WriteCookie: ", Ex);
            }
			
            return resultado;
        }

        public static void DeleteCookie(string cookieName) {
			
			try {
			    var aCookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1) };
                HttpContext.Current.Response.Cookies.Add(aCookie);
			    try {
                    RemoveCookie(cookieName);
			    } catch {
                    }
			} catch (Exception Ex) {
				throw new Exception("DeleteCookie: ", Ex);
			}
        }
        public static void RemoveCookie(string cookieName) {
            try {
                HttpContext.Current.Response.Cookies.Remove(cookieName);
            }
            catch (Exception Ex)
            {
                throw new Exception("RemoveCookie: ", Ex);
            }
        }

        public static void DeleteAllCookies() {
			
			try {
				
			   for (int i = 0; i <= HttpContext.Current.Request.Cookies.Count - 1; i++)  {
                    HttpContext.Current.Response.Cookies.Add(new HttpCookie( HttpContext.Current.Request.Cookies[i].Name) { Expires = DateTime.Now.AddDays(-1) });
               }
			} catch (Exception Ex ) {
				throw new Exception("DeleteAllCookies: ", Ex);
			}
			

        }

    }

}
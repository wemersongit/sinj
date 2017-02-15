using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Reindexacao
{
    /// <summary>
    /// Summary description for GetEs
    /// </summary>
    public class GetEs : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            try
            {
                var _url = context.Request["url_es"];
                var _verbo = context.Request["verbo_es"];
                var _body = context.Request["body_es"];
                var _st_conditional = context.Request["st_conditional"];
                var _keys = context.Request.Form.GetValues("key_es");
                var _values = context.Request.Form.GetValues("value_es");
                var dic = new Dictionary<string, object>();
                if (_keys != null)
                {
                    for (var i = 0; i < _keys.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(_keys[i]))
                        {
                            dic.Add(_keys[i], _values[i]);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(_url) && !string.IsNullOrEmpty(_verbo))
                {
                    switch (_verbo)
                    {
                        case "get":
                            sRetorno = new REST(_url, HttpVerb.GET, "").GetResponse();
                            break;
                        case "post":
                            if (dic.Count() > 0)
                            {
                                sRetorno = new REST(_url, HttpVerb.POST, dic).GetResponse();
                            }
                            else
                            {
                                sRetorno = new REST(_url, HttpVerb.POST, _body).GetResponse();
                            }
                            break;
                        case "put":
                            if (dic.Count() > 0)
                            {
                                if (!string.IsNullOrEmpty(_st_conditional))
                                {
                                    var sTimeOut = Config.ValorChave("ScriptTimeout");
                                    int iTimeOut = 0;
                                    if (sTimeOut != "-1" && int.TryParse(sTimeOut, out iTimeOut))
                                    {
                                        context.Server.ScriptTimeout = iTimeOut;
                                    }
                                    else
                                    {
                                        context.Server.ScriptTimeout = 7200;
                                    }

                                    var opResult = new neo.BRLightREST.opResult();
                                    ulong suc = 1;
                                    while(suc > 0)
                                    {
                                        sRetorno = new REST(_url, HttpVerb.PUT, dic).GetResponse();
                                        var json_op_result = JSON.Deserializa<neo.BRLightREST.opResult>(sRetorno);
                                        opResult.failure += json_op_result.failure;
                                        opResult.success += json_op_result.success;
                                        suc = json_op_result.success;
                                    }
                                    sRetorno = JSON.Serialize(opResult);
                                }
                                else
                                {
                                    sRetorno = new REST(_url, HttpVerb.PUT, dic).GetResponse();
                                }
                            }
                            else
                            {
                                sRetorno = new REST(_url, HttpVerb.PUT, _body).GetResponse();
                            }
                            break;
                        case "delete":
                            if (dic.Count() > 0)
                            {
                                sRetorno = new REST(_url, HttpVerb.DELETE, dic).GetResponse();
                            }
                            else
                            {
                                sRetorno = new REST(_url, HttpVerb.DELETE, _body).GetResponse();
                            }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                sRetorno = "{\"error_message\":\"" + Excecao.LerTodasMensagensDaExcecao(ex, false) + "\"}";
                context.Response.StatusCode = 500;
            }
            if (!JSON.IsJson(sRetorno))
            {
                sRetorno = "{\"return\":\"" + sRetorno + "\"}";
            }
            context.Response.Write(sRetorno);

            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
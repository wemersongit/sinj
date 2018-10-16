using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using util.BRLight;
using Newtonsoft.Json;
using TCDF.Sinj.Log;
using TCDF.Sinj.ES;

namespace TCDF.Sinj.Web.ashx.Reindexacao
{
    /// <summary>
    /// Summary description for Reindex
    /// </summary>
    public class Reindex : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            List<string> ids_erros = new List<string>();
            int ids_migrados = 0;
            try
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
                var _url_es_antigo = context.Request["url_es_antigo"];
                var _url_es_novo = context.Request["url_es_novo"];
                var _json_consulta = context.Request["json_consulta"];
                if (!string.IsNullOrEmpty(_url_es_antigo) && !string.IsNullOrEmpty(_url_es_novo))
                {
                    var _url_es_antigo_splited = _url_es_antigo.Split(':');
                    var host_es_antigo = _url_es_antigo_splited[0] + ":" + _url_es_antigo_splited[1] + ":9200";
                    var retorno_post = "";
                    var json_doc = "";
                    var scan_scroll = new ScanAndScroll<object>();
                    retorno_post = new REST(_url_es_antigo + "/_search?search_type=scan&scroll=10m&size=20", HttpVerb.POST, _json_consulta).GetResponse();
                    scan_scroll = JSON.Deserializa<ScanAndScroll<object>>(retorno_post);
                    while (true)
                    {
                        retorno_post = new REST(host_es_antigo + "/_search/scroll?scroll=10m&scroll_id=" + scan_scroll._scroll_id, HttpVerb.GET, "").GetResponse();
                        scan_scroll = JsonConvert.DeserializeObject<ScanAndScroll<object>>(retorno_post);
                        if (scan_scroll.hits.hits.Count == 0)
                        {
                            sRetorno = "{\"sucess_message\":\"Reindexação concluída. Quantidade de Registros migrados: " + ids_migrados + ". Ids que deram erro: " + JSON.Serialize<List<string>>(ids_erros) + "\"}";
                            break;
                        }
                        for (var i = 0; i < scan_scroll.hits.hits.Count; i++)
                        {
                            try
                            {
                                json_doc = JsonConvert.SerializeObject(scan_scroll.hits.hits[i]._source);
                                retorno_post = new REST(_url_es_novo + "/" + scan_scroll.hits.hits[i]._id, HttpVerb.POST, json_doc).GetResponse();
                                ids_migrados++;
                            }
                            catch (Exception ex)
                            {
                                var erro = new ErroRequest
                                {
                                    Pagina = context.Request.Path,
                                    RequestQueryString = context.Request.QueryString,
                                    MensagemDaExcecao = "Documento indexado: " + _url_es_novo + "/" + scan_scroll.hits.hits[i]._id + "<br/>Mensagem da Exceção: " + Excecao.LerTodasMensagensDaExcecao(ex, true),
                                    StackTrace = ex.StackTrace
                                };
                                LogErro.gravar_erro("REINDEXACAO", erro, "", "");
                                ids_erros.Add(scan_scroll.hits.hits[i]._id);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sRetorno = "{\"error_message\":\"" + Excecao.LerTodasMensagensDaExcecao(ex, false) + "\"}";
                context.Response.StatusCode = 500;
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

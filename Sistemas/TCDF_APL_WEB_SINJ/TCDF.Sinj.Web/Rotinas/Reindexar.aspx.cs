using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using util.BRLight;
using Newtonsoft.Json;

namespace TCDF.Sinj.Web.Rotinas
{
    public partial class Reindexar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Server.ScriptTimeout = 14400;
            //var retorno = "";
            //List<string> ids_erros = new List<string>();
            //int ids_migrados = 0;
            //StringBuilder id_doc_erro = new StringBuilder();
            //try
            //{
            //    var _url_es_antigo = "http://10.0.0.130:9200";
            //    var _url_es_novo = "http://10.0.0.130:9200";
            //    var _nm_index_novo = "sinj_index";
            //    var _nm_index_antigo = "sinj_index3";
            //    var _nm_type_novo = "diario";
            //    var _nm_type_antigo = "diario";
            //    var _js_consulta = "";
            //    var json_doc = "";
            //    var scan_scroll = new ScanAndScroll();
            //    var retorno_post = Post(_js_consulta, _url_es_antigo + "/" + _nm_index_antigo + "/" + _nm_type_antigo + "/_search?search_type=scan&scroll=10m&size=10");
            //    scan_scroll = JSON.Deserializa<ScanAndScroll>(retorno_post);
            //    while (true)
            //    {
            //        retorno_post = Get(_url_es_antigo + "/_search/scroll?scroll=10m&scroll_id=" + scan_scroll._scroll_id);
            //        scan_scroll = JsonConvert.DeserializeObject<ScanAndScroll>(retorno_post);
            //        if (scan_scroll.hits.hits.Length == 0)
            //        {
            //            retorno = "Reindexação concluída.";
            //            break;
            //        }
            //        for (var i = 0; i < scan_scroll.hits.hits.Length; i++)
            //        {
            //            try
            //            {
            //                json_doc = JsonConvert.SerializeObject(scan_scroll.hits.hits[i]._source);
            //                retorno_post = Post(json_doc, _url_es_novo + "/" + _nm_index_novo + "/" + _nm_type_novo + "/" + scan_scroll.hits.hits[i]._id);
            //                ids_migrados++;
            //            }
            //            catch (Exception ex)
            //            {
            //                ids_erros.Add(scan_scroll.hits.hits[i]._id);
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    retorno = Excecao.LerTodasMensagensDaExcecao(ex, false);
            //}
            //div_resultado.InnerHtml = retorno + ".<br/>Quantidade de Registros migrados: " + ids_migrados + ".<br/>Ids que deram erro: " + JSON.Serialize<List<string>>(ids_erros);
        }

        public string Post(string content, string uri)
        {
            return new REST(uri, HttpVerb.POST, content).GetResponse();
        }
        public string Get(string uri)
        {
            return new REST(uri, HttpVerb.GET, "").GetResponse();
        }
    }
    public class Metadata
    {
        public bool idx_exp { get; set; }
        public string password { get; set; }
        public string description { get; set; }
        public string color { get; set; }
        public long file_ext_time { get; set; }
        public string dt_base { get; set; }
        public string idx_exp_url { get; set; }
        public bool file_ext { get; set; }
        public long idx_exp_time { get; set; }
        public object model { get; set; }
        public ulong id_base { get; set; }
        public string name { get; set; }
    }
}

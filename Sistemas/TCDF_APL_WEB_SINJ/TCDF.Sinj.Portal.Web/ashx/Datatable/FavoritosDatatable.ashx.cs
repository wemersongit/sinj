using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.Log;
using util.BRLight;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using TCDF.Sinj.ES;
using TCDF.Sinj.AD;

namespace TCDF.Sinj.Portal.Web.ashx.Datatable
{
    /// <summary>
    /// Summary description for FavoritosPesquisaDatatable
    /// </summary>
    public class FavoritosDatatable : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sAction = "FAV.PES";
            var json_resultado = "";
            var _iDisplayLength = context.Request["iDisplayLength"];
            var _iDisplayStart = context.Request["iDisplayStart"];
            var _sEcho = context.Request.Params["sEcho"];
            try
            {

                object datatable_result = null;
                var _base = context.Request["b"];
                if (_base == "norma")
                {
                    var notifiquemeRn = new NotifiquemeRN();
                    var sessaoNotifiquemeOv = notifiquemeRn.LerSessaoNotifiquemeOv();
                    var notifiquemeOv = notifiquemeRn.Doc(sessaoNotifiquemeOv.email_usuario_push);
                    SentencaPesquisaFavoritosOV sentencaOv = new SentencaPesquisaFavoritosOV();
                    sentencaOv.@base = _base;
                    sentencaOv.favoritos = notifiquemeOv.favoritos.ToArray();
                    var query = new NormaBuscaEs().MontarBusca(sentencaOv).GetQuery();
                    Result<NormaOV> result_norma = new NormaAD().ConsultarEs(query);
                    datatable_result = new { aaData = result_norma.hits.hits, sEcho = _sEcho, offset = _iDisplayStart, iTotalRecords = _iDisplayLength, iTotalDisplayRecords = result_norma.hits.total };
                }
                json_resultado = Newtonsoft.Json.JsonConvert.SerializeObject(datatable_result);
            }
            catch (Exception ex)
            {
                json_resultado = "{ \"aaData\": [], \"sEcho\": \"" + _sEcho + "\", \"iTotalRecords\": \"" + _iDisplayLength + "\", \"iTotalDisplayRecords\": 0}";
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                var nm_usuario = "visitante";
                var nm_login_usuario = "visitante";

                SessaoNotifiquemeOV sessao_push = Util.LerSessaoPush();
                if (sessao_push != null)
                {
                    nm_usuario = sessao_push.nm_usuario_push;
                    nm_login_usuario = sessao_push.email_usuario_push;
                }
                LogErro.gravar_erro(sAction, erro, nm_usuario, nm_login_usuario);
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(json_resultado);
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

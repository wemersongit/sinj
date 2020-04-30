using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.Log;
using util.BRLight;
using TCDF.Sinj.RN;
using TCDF.Sinj.ES;
using TCDF.Sinj.AD;

namespace TCDF.Sinj.Web.ashx.Datatable
{
    /// <summary>
    /// Summary description for FavoritosDatatable
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
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (Config.ValorChave("Aplicacao") == "CADASTRO")
                {
                    sessao_usuario = Util.ValidarSessao();
                    //Não faz nada se o usuário não tiver sessão no portal pois essa pesquisa pode ser feita por qualquer um.
                }
                object datatable_result = null;
                var _base = context.Request["b"];
                if (_base == "norma")
                {
                    var notifiquemeRn = new NotifiquemeRN();
                    var sessaoNotifiquemeOv = notifiquemeRn.LerSessaoNotifiquemeOv();
                    var notifiquemeOv = notifiquemeRn.Doc(sessaoNotifiquemeOv.email_usuario_push);
                    SentencaPesquisaFavoritosOV sentencaOv = new SentencaPesquisaFavoritosOV
                    {
                        filtros = context.Request.Params.GetValues("filtro"),
                        iDisplayStart = Convert.ToUInt64(_iDisplayStart),
                        iDisplayLength = Convert.ToUInt64(_iDisplayLength),
                        @base = _base,
                        favoritos = notifiquemeOv.favoritos.ToArray()
                    };
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
                if (sessao_usuario != null)
                {
                    LogErro.gravar_erro(sAction, erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
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

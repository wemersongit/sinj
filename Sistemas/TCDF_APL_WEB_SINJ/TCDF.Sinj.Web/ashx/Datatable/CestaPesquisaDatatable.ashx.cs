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

namespace TCDF.Sinj.Web.ashx.Datatable
{
    /// <summary>
    /// Summary description for CestaPesquisaDatatable
    /// </summary>
    public class CestaPesquisaDatatable : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sAction = "";
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
                CestaRN cestaRn = new CestaRN();
                object datatable_result = null;
                var _base = context.Request["b"];
                SentencaPesquisaCestaOV sentencaOv = new SentencaPesquisaCestaOV();
                sentencaOv.@base = _base;
                sentencaOv.cesta = context.Request["cesta"];
                if (_base == "sinj_norma")
                {
                    sAction = "CST.NOR";
                    var query = new NormaBuscaEs().MontarBusca(sentencaOv).GetQuery();
                    Result<NormaOV> result_norma = new NormaAD().ConsultarEs(query);
                    datatable_result = new { aaData = result_norma.hits.hits, sEcho = _sEcho, offset = _iDisplayStart, iTotalRecords = _iDisplayLength, iTotalDisplayRecords = result_norma.hits.total };
                }
                else if (_base == "sinj_diario")
                {
                    sAction = "CST.DIO";
                    var query = new DiarioBuscaEs().MontarBusca(sentencaOv).GetQuery();
                    Result<DiarioOV> result_diario = new DiarioAD().ConsultarEs(query);
                    datatable_result = new { aaData = result_diario.hits.hits, sEcho = _sEcho, offset = _iDisplayStart, iTotalRecords = _iDisplayLength, iTotalDisplayRecords = result_diario.hits.total };
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using util.BRLight;
using TCDF.Sinj.OV;
using TCDF.Sinj.Log;
using neo.BRLightES;
using TCDF.Sinj.RN;

namespace TCDF.Sinj.Web.ashx.Datatable
{
    /// <summary>
    /// Summary description for HistoricoDePesquisaDatatable
    /// </summary>
    public class HistoricoDePesquisaDatatable : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var json_resultado = "";
            var _iDisplayStart = context.Request["iDisplayStart"];
            var _chave = context.Request["chave"];
            var _iDisplayLength = context.Request["iDisplayLength"];
            var _sEcho = context.Request.Params["sEcho"];
            var iSortCol = 0;
            int.TryParse(context.Request["iSortCol_0"], out iSortCol);
            var iSortDir = context.Request["sSortDir_0"];
            var _sColOrder = context.Request["mDataProp_" + iSortCol];

            try
            {
                var result = new HistoricoDePesquisaRN().ConsultarEs(context);
                var datatable_result = new { aaData = result.hits.hits, sEcho = _sEcho, offset = _iDisplayStart, iTotalRecords = _iDisplayLength, iTotalDisplayRecords = result.hits.total };
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
                LogErro.gravar_erro("HST.PES", erro, "","");
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
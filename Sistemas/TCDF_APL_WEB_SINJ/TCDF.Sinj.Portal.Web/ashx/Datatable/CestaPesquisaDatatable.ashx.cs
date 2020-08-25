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
    /// Summary description for CestaPesquisaDatatable
    /// </summary>
    public class CestaPesquisaDatatable : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sAction = "";
            var json_resultado = "";
            var _iDisplayLength = context.Request["iDisplayLength"];
            ulong iDisplayLength = 0;
            var _iDisplayStart = context.Request["iDisplayStart"];
            ulong iDisplayStart = 0;
            var _sEcho = context.Request.Params["sEcho"];

            if (!string.IsNullOrEmpty(_iDisplayStart))
            {
                iDisplayStart = ulong.Parse(_iDisplayStart);
            }

            if (!string.IsNullOrEmpty(_iDisplayLength))
            {
                iDisplayLength = ulong.Parse(_iDisplayLength);
            }

            try
            {

                CestaRN cestaRn = new CestaRN();
                object datatable_result = null;
                var _base = context.Request["b"];
                SentencaPesquisaCestaOV sentencaOv = new SentencaPesquisaCestaOV();
                sentencaOv.@base = _base;
                sentencaOv.cesta = context.Request["cesta"];
                sentencaOv.iDisplayLength = iDisplayLength;
                sentencaOv.iDisplayStart = iDisplayStart;
                //sentencaOv.sentencaOrdenamento = MontarOrdenamento(context);

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

        private SentencaOrdenamentoOV MontarOrdenamento(HttpContext context)
        {
            var sentencaOrdenamentoOv = new SentencaOrdenamentoOV();
            var _sSortCol = context.Request["iSortCol_0"];
            var iSortCol = 0;

            if (!string.IsNullOrEmpty(_sSortCol))
            {
                int.TryParse(_sSortCol, out iSortCol);
                sentencaOrdenamentoOv.sSortDir = context.Request["sSortDir_0"];
                sentencaOrdenamentoOv.sColOrder = context.Request["mDataProp_" + iSortCol];
            }

            return sentencaOrdenamentoOv;
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

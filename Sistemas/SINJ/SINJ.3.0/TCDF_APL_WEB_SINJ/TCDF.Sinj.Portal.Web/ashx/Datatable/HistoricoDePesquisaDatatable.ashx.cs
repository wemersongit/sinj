﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using util.BRLight;
using TCDF.Sinj.OV;
using TCDF.Sinj.Log;
using TCDF.Sinj.RN;

namespace TCDF.Sinj.Portal.Web.ashx.Datatable
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
                json_resultado = JSON.Serialize<object>(datatable_result);
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
                LogErro.gravar_erro("HST.PES", erro, nm_usuario, nm_login_usuario);
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
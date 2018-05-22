﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.ES;
using TCDF.Sinj.Log;
using util.BRLight;
using TCDF.Sinj.RN;
using TCDF.Sinj.AD;

namespace TCDF.Sinj.Portal.Web.ashx.Datatable
{
    /// <summary>
    /// Summary description for ResultadoDePesquisaDiarioDatatable
    /// </summary>
    public class ResultadoDePesquisaDiarioDatatable : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sAction = "";
            string sRetorno = "";
            string _outras_buscas = context.Request["outras_buscas"];
            string _relatorio = context.Request["relatorio"];
            string _bbusca = context.Request["bbusca"];

            var _iDisplayLength = context.Request["iDisplayLength"];
            ulong iDisplayLength = 0;
            var _iDisplayStart = context.Request["iDisplayStart"];
            ulong iDisplayStart = 0;
            var _sEcho = context.Request["sEcho"];


            string _tipo_pesquisa = context.Request["tipo_pesquisa"];
            string _sSearch = context.Request["sSearch"];
            string _sFiltrar = context.Request["filtrar"];
            string _all = context.Request["all"];

            var sentencaOrdenamento = MontarOrdenamento(context);

            try
            {
                var diarioRn = new DiarioRN();

                var diarioBuscaEs = new DiarioBuscaEs();

                var query = "";

                switch (_tipo_pesquisa)
                {
                    case "geral":
                        SentencaPesquisaGeralOV pesquisaGeral = new SentencaPesquisaGeralOV();
                        pesquisaGeral.all = context.Request["all"];
                        pesquisaGeral.filtros = context.Request.Params.GetValues("filtro");
                        pesquisaGeral.iDisplayStart = iDisplayStart;
                        pesquisaGeral.iDisplayLength = iDisplayLength;
                        pesquisaGeral.sentencaOrdenamento = sentencaOrdenamento;
                        query = diarioBuscaEs.MontarBusca(pesquisaGeral).GetQuery();
                        break;
                    case "notifiqueme":
                        SentencaPesquisaNotifiquemeDiarioOV pesquisaNotifiqueme = new SentencaPesquisaNotifiquemeDiarioOV();
                        pesquisaNotifiqueme.ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                        pesquisaNotifiqueme.filetext = context.Request["filetext"];
                        pesquisaNotifiqueme.in_exata = context.Request["in_exata"];
                        pesquisaNotifiqueme.iDisplayStart = iDisplayStart;
                        pesquisaNotifiqueme.iDisplayLength = iDisplayLength;
                        pesquisaNotifiqueme.sentencaOrdenamento = sentencaOrdenamento;
                        var buscaNotifiqueme = new DiarioBuscaEs().MontarBusca(pesquisaNotifiqueme);
                        query = buscaNotifiqueme.GetQuery();
                        break;
                    case "diario":
                        SentencaPesquisaDiretaDiarioOV pesquisaDireta = new SentencaPesquisaDiretaDiarioOV();
                        pesquisaDireta.filtros = context.Request.Params.GetValues("filtro");
                        pesquisaDireta.ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                        pesquisaDireta.ch_tipo_edicao = context.Request["ch_tipo_edicao"];
                        pesquisaDireta.nr_diario = context.Request["nr_diario"];
                        pesquisaDireta.secao_diario = context.Request.Form.GetValues("secao_diario");
                        pesquisaDireta.filetext = context.Request["filetext"];
                        pesquisaDireta.op_dt_assinatura = context.Request["op_dt_assinatura"];
                        pesquisaDireta.dt_assinatura = context.Request.Form.GetValues("dt_assinatura");
                        pesquisaDireta.iDisplayStart = iDisplayStart;
                        pesquisaDireta.iDisplayLength = iDisplayLength;
                        pesquisaDireta.sentencaOrdenamento = sentencaOrdenamento;
                        query = diarioBuscaEs.MontarBusca(pesquisaDireta).GetQuery();
                        break;
                }

                Result<DiarioOV> result_diario = new DiarioAD().ConsultarEs(query);
                sAction = Util.GetEnumDescription(AcoesDoUsuario.dio_pes);
                var datatable_result = new { aaData = result_diario.hits.hits, sEcho = _sEcho, offset = _iDisplayStart, iTotalRecords = _iDisplayLength, iTotalDisplayRecords = result_diario.hits.total, result_diario.aggregations };
                sRetorno = Newtonsoft.Json.JsonConvert.SerializeObject(datatable_result);

            }
            catch (Exception ex)
            {
                sRetorno = "{\"echo\":\"" + _sEcho + "\",\"iTotalRecords\":\"0\",\"iTotalDisplayRecords\":\"0\",\"aaData\":[]}";
                
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
            context.Response.Write(sRetorno);
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
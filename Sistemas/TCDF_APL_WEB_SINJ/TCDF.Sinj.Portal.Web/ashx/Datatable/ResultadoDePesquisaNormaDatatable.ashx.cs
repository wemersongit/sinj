using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using TCDF.Sinj.ES;
using TCDF.Sinj.AD;
using System.Text.RegularExpressions;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Portal.Web.ashx.Datatable
{
    /// <summary>
    /// Summary description for ResultadoDePesquisaNormaDatatable
    /// </summary>
    public class ResultadoDePesquisaNormaDatatable : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sAction = "";
            string sRetorno = "";

            var _iDisplayLength = context.Request["iDisplayLength"];
            ulong iDisplayLength=0;
            var _iDisplayStart = context.Request["iDisplayStart"];
            ulong iDisplayStart=0;
            var _sEcho = context.Request["sEcho"];

            string _tipo_pesquisa = context.Request["tipo_pesquisa"];


            if (!string.IsNullOrEmpty(_iDisplayStart))
            {
                iDisplayStart = ulong.Parse(_iDisplayStart);
            }
            if (!string.IsNullOrEmpty(_iDisplayLength))
            {
                iDisplayLength = ulong.Parse(_iDisplayLength);
            }

            var sentencaOrdenamento = MontarOrdenamento(context);

            try
            {

                var normaRn = new NormaRN();
                
                var utilNormaBuscaEs = new NormaBuscaEs();

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
                        query = utilNormaBuscaEs.MontarBusca(pesquisaGeral).GetQuery();
                        break;
                    case "norma":
                        SentencaPesquisaDiretaNormaOV pesquisaDireta = new SentencaPesquisaDiretaNormaOV();
                        pesquisaDireta.all = context.Request["all"];
                        pesquisaDireta.filtros = context.Request.Params.GetValues("filtro");
                        pesquisaDireta.ch_tipo_norma = context.Request["ch_tipo_norma"];
                        pesquisaDireta.nr_norma = context.Request["nr_norma"];
                        pesquisaDireta.norma_sem_numero = context.Request["norma_sem_numero"];
                        pesquisaDireta.ano_assinatura = context.Request["ano_assinatura"];
                        pesquisaDireta.dt_assinatura = context.Request["dt_assinatura"];
                        pesquisaDireta.ch_termos = context.Request.Params.GetValues("ch_termo");
                        pesquisaDireta.ch_orgao = context.Request["ch_orgao"];
                        pesquisaDireta.ch_hierarquia = context.Request["ch_hierarquia"];
                        pesquisaDireta.origem_por = context.Request["origem_por"];
                        pesquisaDireta.iDisplayStart = iDisplayStart;
                        pesquisaDireta.iDisplayLength = iDisplayLength;
                        pesquisaDireta.sentencaOrdenamento = sentencaOrdenamento;
                        query = utilNormaBuscaEs.MontarBusca(pesquisaDireta).GetQuery();
                        break;
                    case "avancada":
                        SentencaPesquisaAvancadaNormaOV pesquisaAvancada = new SentencaPesquisaAvancadaNormaOV();
                        pesquisaAvancada.ch_tipo_norma = context.Request.Params.GetValues("ch_tipo_norma");
                        pesquisaAvancada.argumentos = context.Request.Params.GetValues("argumento");
                        pesquisaAvancada.filtros = context.Request.Params.GetValues("filtro");
                        pesquisaAvancada.iDisplayStart = iDisplayStart;
                        pesquisaAvancada.iDisplayLength = iDisplayLength;
                        pesquisaAvancada.sentencaOrdenamento = sentencaOrdenamento;
                        query = utilNormaBuscaEs.MontarBusca(pesquisaAvancada).GetQuery();
                        break;
                }

                Result<NormaOV> result_norma = new NormaAD().ConsultarEs(query);

                sAction = Util.GetEnumDescription(AcoesDoUsuario.nor_pes);
                var datatable_result = new { aaData = result_norma.hits.hits, sEcho = _sEcho, offset = _iDisplayStart, iTotalRecords = _iDisplayLength, iTotalDisplayRecords = result_norma.hits.total, result_norma.aggregations };
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
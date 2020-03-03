using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using TCDF.Sinj.Log;
using util.BRLight;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Web.ashx.Datatable
{
    /// <summary>
    /// Summary description for DiarioDatatable
    /// </summary>
    public class DiarioDatatable : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var json_resultado = "";
            var _iDisplayLength = context.Request["iDisplayLength"];
            var _iDisplayStart = context.Request["iDisplayStart"];
            var _sEcho = context.Request.Params["sEcho"];
            var iSortCol = 0;
            int.TryParse(context.Request["iSortCol_0"], out iSortCol);
            var iSortDir = context.Request["sSortDir_0"];
            var _sColOrder = context.Request["mDataProp_" + iSortCol];
            var action = AcoesDoUsuario.dio_pes;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                Pesquisa pesquisa = new Pesquisa();
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);

                var lb = new LB();

                var results = lb.PesquisarDocs<DiarioOV>(context, sessao_usuario, "sinj_diario");

                var datatable_result = new { aaData = results.results, sEcho = _sEcho, offset = _iDisplayStart, iTotalRecords = _iDisplayLength, iTotalDisplayRecords = results.result_count };
                json_resultado = Newtonsoft.Json.JsonConvert.SerializeObject(datatable_result);
                var log_busca = new LogBuscar
                {
                    RegistrosPorPagina = _iDisplayLength,
                    RegistroInicial = _iDisplayStart,
                    RequestNumero = _sEcho,
                    RegistrosTotal = results.result_count.ToString(),
                    PesquisaLight = pesquisa
                };

                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_busca, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);

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
                    LogErro.gravar_erro(Util.GetEnumDescription(action), erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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

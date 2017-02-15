using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.RN;

namespace TCDF.Sinj.Portal.Web.ashx.Datatable
{
    /// <summary>
    /// Summary description for ResultadoDePesquisaDatatable
    /// </summary>
    public class ResultadoDePesquisaDatatable : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sAction = "";
            string sRetorno = "";
            string _exibir_total = context.Request["exibir_total"];
            string _relatorio = context.Request["relatorio"];
            string _bbusca = context.Request["bbusca"];

            var _iDisplayLength = context.Request["iDisplayLength"];
            var _iDisplayStart = context.Request["iDisplayStart"];
            var _sEcho = context.Request["sEcho"];
            try
            {

                var normaRn = new NormaRN();
                var diarioRn = new DiarioRN();

                if (_exibir_total == "1")
                {
                    if (_bbusca == "sinj_norma")
                    {
                        sRetorno = "{\"counts\":[{\"nm_base\":\"" + _bbusca + "\",\"count\":" + normaRn.PesquisarTotalEs(context) + "}]}";
                    }
                    else if (_bbusca == "sinj_diario")
                    {
                        sRetorno = "{\"counts\":[{\"nm_base\":\"" + _bbusca + "\",\"count\":" + diarioRn.PesquisarTotalEs(context) + "}]}";
                    }
                    else
                    {
                        sRetorno = "{\"counts\":[{\"nm_base\":\"sinj_norma\",\"count\":" + normaRn.PesquisarTotalEs(context) + "},{\"nm_base\":\"sinj_diario\",\"count\":" + diarioRn.PesquisarTotalEs(context) + "}]}";
                    }
                }
                else
                {

                    object datatable_result = null;
                    switch (_bbusca)
                    {
                        case "sinj_norma":
                            sAction = Util.GetEnumDescription(AcoesDoUsuario.nor_pes);
                            var result_norma = normaRn.ConsultarEs(context);
                            datatable_result = new { aaData = result_norma.hits.hits, sEcho = _sEcho, offset = _iDisplayStart, iTotalRecords = _iDisplayLength, iTotalDisplayRecords = result_norma.hits.total, result_norma.aggregations };
                            break;
                        case "sinj_diario":
                            sAction = Util.GetEnumDescription(AcoesDoUsuario.dio_pes);
                            var result_diario = diarioRn.ConsultarEs(context);
                            datatable_result = new { aaData = result_diario.hits.hits, sEcho = _sEcho, offset = _iDisplayStart, iTotalRecords = _iDisplayLength, iTotalDisplayRecords = result_diario.hits.total, result_diario.aggregations };
                            break;
                    }
                    sRetorno = Newtonsoft.Json.JsonConvert.SerializeObject(datatable_result);
                }

            }
            catch (Exception ex)
            {
                if (_exibir_total == "1")
                {
                    sRetorno = "{\"counts\":[{\"nm_base\":\"sinj_norma\",\"count\":{\"count\":0}},{\"nm_base\":\"sinj_diario\",\"count\":{\"count\":0}},{\"nm_base\":\"cesta\",\"count\":{\"count\":0}}]}";
                }
                else
                {
                    sRetorno = "{\"echo\":\"" + _sEcho + "\",\"iTotalRecords\":\"0\",\"iTotalDisplayRecords\":\"0\",\"aaData\":[]}";
                }
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

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
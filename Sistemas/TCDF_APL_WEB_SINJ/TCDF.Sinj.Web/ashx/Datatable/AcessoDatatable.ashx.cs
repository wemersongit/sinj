using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Datatable
{
    /// <summary>
    /// Summary description for AcessoDatatable
    /// </summary>
    public class AcessoDatatable : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var json_resultado = "";
            Pesquisa pesquisa = new Pesquisa();

            var _app = context.Request["app"];
            var _texto_livre = context.Request["texto_livre"];
            var _nm_login_user_acesso = context.Request["nm_login_user_acesso"];
            var _dt_acesso = context.Request["dt_acesso"];
            var _dt_acesso_fim = context.Request["dt_acesso_fim"];
            var _op_intervalo = context.Request["op_intervalo"];
            var _sSearch = context.Request["sSearch"];

            string iDisplayLength = context.Request["iDisplayLength"];
            string iDisplayStart = context.Request["iDisplayStart"];
            string sEcho = context.Request.Params["sEcho"];
            var iSortCol = int.Parse(context.Request["iSortCol_0"]);
            var iSortDir = context.Request["sSortDir_0"];
            var _sColOrder = context.Request["mDataProp_" + iSortCol].Replace("_metadata.", "");
            var action = AcoesDoUsuario.aud_ace;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                if (iDisplayLength != "-1")
                {
                    pesquisa.limit = iDisplayLength;
                    pesquisa.offset = iDisplayStart;
                }
                if (!string.IsNullOrEmpty(_sColOrder))
                {
                    if (("desc" == iSortDir))
                        pesquisa.order_by.desc = new[] { _sColOrder };
                    else
                        pesquisa.order_by.asc = new[] { _sColOrder };
                }
                if (!string.IsNullOrEmpty(_app))
                {
                    pesquisa.literal += (!string.IsNullOrEmpty(pesquisa.literal) ? " AND " : "") + "ds_login='" + (_app == "push" ? "SINJ.PUSH" : "SINJ.CAD") + "'";
                }
                if (!string.IsNullOrEmpty(_nm_login_user_acesso))
                {
                    pesquisa.literal += (!string.IsNullOrEmpty(pesquisa.literal) ? " AND " : "") + "nm_login_user_acesso='" + _nm_login_user_acesso + "'";
                }
                if (!string.IsNullOrEmpty(_dt_acesso))
                {
                    if (_op_intervalo == "intervalo" && !string.IsNullOrEmpty(_dt_acesso_fim))
                    {
                        pesquisa.literal += (!string.IsNullOrEmpty(pesquisa.literal) ? " AND " : "") + "dt_acesso::date>='" + _dt_acesso + "' AND dt_acesso::date<='" + _dt_acesso_fim + "'";
                    }
                    else
                    {
                        pesquisa.literal += (!string.IsNullOrEmpty(pesquisa.literal) ? " AND " : "") + "dt_acesso::date"+LB.ReplaceOperatorToQuery(_op_intervalo)+"'" + _dt_acesso + "'";
                    }
                }
                if (!string.IsNullOrEmpty(_texto_livre))
                {
                    pesquisa.literal += (!string.IsNullOrEmpty(pesquisa.literal) ? " AND " : "") + "Upper(document::text) like '%" + _texto_livre.ToUpper() + "%'";
                }
                if (!string.IsNullOrEmpty(_sSearch))
                {
                    pesquisa.literal += (!string.IsNullOrEmpty(pesquisa.literal) ? " AND " : "") + "Upper(document::text) like '%" + _sSearch.ToUpper() + "%'";
                }

                json_resultado = new Log.RN.log_acessoRN().jsonReg(pesquisa);

                json_resultado = json_resultado.Replace("\"results\": ", "\"aaData\":")
                      .Replace("\"offset\": ", "\"sEcho\": " + ((string.IsNullOrEmpty(sEcho)) ? "\"1\"" : sEcho) + ", \"offset\":")
                      .Replace("\"limit\": ", "\"iTotalRecords\": ")
                      .Replace("\"result_count\": ", "\"iTotalDisplayRecords\": ");

                var ind = json_resultado.IndexOf("\"iTotalDisplayRecords\": ") + "\"iTotalDisplayRecords\": ".Length;
                var Busca = new LogBuscar
                {
                    RegistrosPorPagina = iDisplayLength,
                    RegistroInicial = iDisplayStart,
                    RequestNumero = sEcho,
                    RegistrosTotal = json_resultado.Substring(ind, json_resultado.IndexOf(",", ind) - ind),
                    PesquisaLight = pesquisa
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ".PES", Busca, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (Exception ex)
            {
                json_resultado = "{ \"aaData\": [], \"sEcho\": " + sEcho + ", \"iTotalRecords\": \"" + iDisplayLength + "\", \"iTotalDisplayRecords\": 0}";
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                if (sessao_usuario != null)
                {
                    LogErro.gravar_erro(Util.GetEnumDescription(action) + ".PES", erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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

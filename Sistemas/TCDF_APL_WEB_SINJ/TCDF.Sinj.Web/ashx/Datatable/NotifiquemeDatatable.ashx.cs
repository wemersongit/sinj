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
    /// Summary description for NotifiquemeDatatable
    /// </summary>
    public class NotifiquemeDatatable : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var json_resultado = "";
            Pesquisa pesquisa = new Pesquisa();
            var _ch_tipo_norma = context.Request.Params["ch_tipo_norma"];
            var _nr_norma = context.Request.Params["nr_norma"];
            var _ch_orgao = context.Request.Params["ch_orgao"];
            var _dt_doc = context.Request.Params["dt_doc"];
            var _dt_doc_fim = context.Request["dt_doc_fim"];
            var _op_intervalo = context.Request["op_intervalo"];
            var _dt_last_up = context.Request.Params["dt_last_up"];
            var _dt_last_up_fim = context.Request["dt_last_up_fim"];
            var _op_intervalo_dt_last_up = context.Request["op_intervalo_dt_last_up"];
            var _email_usuario_push = context.Request.Params["email_usuario_push"];
            var _st_push = context.Request.Params["st_push"];
            var _sSearch = context.Request.Params["sSearch"];
            var _texto_livre = context.Request.Params["texto_livre"];

            string iDisplayLength = context.Request["iDisplayLength"];
            string iDisplayStart = context.Request["iDisplayStart"];
            string sEcho = context.Request.Params["sEcho"];
            var iSortCol = int.Parse(context.Request["iSortCol_0"]);
            var iSortDir = context.Request["sSortDir_0"];
            var _sColOrder = context.Request["mDataProp_" + iSortCol].Replace("_metadata.", "");
            var action = AcoesDoUsuario.aud_pus;
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
                if (!string.IsNullOrEmpty(_ch_tipo_norma))
                {
                    pesquisa.literal += (!string.IsNullOrEmpty(pesquisa.literal) ? " AND " : "") + "('"+_ch_tipo_norma+"'=any(ch_tipo_norma_monitorada) OR '"+_ch_tipo_norma+"'=any(ch_tipo_norma_criacao))";
                }
                if (!string.IsNullOrEmpty(_nr_norma))
                {
                    pesquisa.literal += (!string.IsNullOrEmpty(pesquisa.literal) ? " AND " : "") + "nr_norma='" + _nr_norma + "'";
                }
                if (!string.IsNullOrEmpty(_ch_orgao))
                {
                    pesquisa.literal += (!string.IsNullOrEmpty(pesquisa.literal) ? " AND " : "") + "('" + _ch_orgao + "'=any(ch_orgao_monitorada) OR '" + _ch_orgao + "'=any(ch_orgao_criacao))";
                }
                if (!string.IsNullOrEmpty(_dt_doc))
                {
                    if (_op_intervalo == "intervalo" && !string.IsNullOrEmpty(_dt_doc_fim))
                    {
                        pesquisa.literal += (!string.IsNullOrEmpty(pesquisa.literal) ? " AND " : "") + "dt_doc::date>='" + _dt_doc + "' AND dt_doc::date<='" + _dt_doc_fim + "'";
                    }
                    else
                    {
                        pesquisa.literal += (!string.IsNullOrEmpty(pesquisa.literal) ? " AND " : "") + "dt_doc::date" + LB.ReplaceOperatorToQuery(_op_intervalo) + "'" + _dt_doc + "'";
                    }
                }
                if (!string.IsNullOrEmpty(_dt_last_up))
                {
                    if (_op_intervalo == "intervalo" && !string.IsNullOrEmpty(_dt_last_up_fim))
                    {
                        pesquisa.literal += (!string.IsNullOrEmpty(pesquisa.literal) ? " AND " : "") + "dt_last_up::date>='" + _dt_last_up + "' AND dt_last_up::date<='" + _dt_last_up_fim + "'";
                    }
                    else
                    {
                        pesquisa.literal += (!string.IsNullOrEmpty(pesquisa.literal) ? " AND " : "") + "dt_last_up::date" + LB.ReplaceOperatorToQuery(_op_intervalo_dt_last_up) + "'" + _dt_last_up + "'";
                    }
                }
                if (!string.IsNullOrEmpty(_email_usuario_push))
                {
                    pesquisa.literal += (!string.IsNullOrEmpty(pesquisa.literal) ? " AND " : "") + "email_usuario_push='" + _email_usuario_push + "'";
                }
                if (!string.IsNullOrEmpty(_st_push))
                {
                    pesquisa.literal += (!string.IsNullOrEmpty(pesquisa.literal) ? " AND " : "") + "st_push=" + _st_push + "";
                }
                if (!string.IsNullOrEmpty(_texto_livre))
                {
                    pesquisa.literal += (!string.IsNullOrEmpty(pesquisa.literal) ? " AND " : "") + "Upper(document::text) like '%" + _texto_livre.ToUpper() + "%'";
                }
                if (!string.IsNullOrEmpty(_sSearch))
                {
                    pesquisa.literal += (!string.IsNullOrEmpty(pesquisa.literal) ? " AND " : "") + "Upper(document::text) like '%" + _sSearch.ToUpper() + "%'";
                }
                json_resultado = new NotifiquemeRN().JsonReg(pesquisa);
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

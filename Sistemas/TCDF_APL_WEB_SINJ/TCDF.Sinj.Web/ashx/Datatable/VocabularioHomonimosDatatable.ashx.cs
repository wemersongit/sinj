using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Datatable
{
    /// <summary>
    /// Summary description for VocabularioHomonimosDatatable
    /// </summary>
    public class VocabularioHomonimosDatatable : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var json_resultado = "";
            var vocabularioRn = new VocabularioRN();
            Pesquisa pesquisa = new Pesquisa();
            var _sSearch = context.Request.Params["sSearch"];

            string iDisplayLength = context.Request["iDisplayLength"];
            string iDisplayStart = context.Request["iDisplayStart"];
            string sEcho = context.Request.Params["sEcho"];
            var iSortCol = int.Parse(context.Request["iSortCol_0"]);
            var iSortDir = context.Request["sSortDir_0"];
            var _sColOrder = context.Request["mDataProp_" + iSortCol].Replace("_metadata.", "");
            var action = AcoesDoUsuario.voc_pes;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);

                ///Busca todos os termos e verifica duplicidade de nomes
                ///para então adicionar a chaves destes termos para uma nova pesquisa
                pesquisa.limit = null;
                pesquisa.select = new string[] { "nm_termo" };
                var result = vocabularioRn.Consultar(pesquisa);
                var query = "";
                if (!string.IsNullOrEmpty(_sSearch))
                {
                    query += "Upper(nm_termo) like '%" + _sSearch.ToUpper() + "%'";
                }

                var query_chaves = "";
                var count = 0;
                foreach (var termo in result.results)
                {
                    count = result.results.Count<VocabularioOV>(t => t.nm_termo.ToUpper() == termo.nm_termo.ToUpper());
                    if (count > 1)
                    {
                        if (!string.IsNullOrEmpty(termo.nm_termo) && query_chaves.IndexOf("='" + termo.nm_termo.ToUpper() + "'") < 0)
                        {
                            query_chaves += (query_chaves != "" ? " or " : "") + "Upper(nm_termo)='" + termo.nm_termo.ToUpper() + "'";
                        }
                    }
                }
                if (query_chaves != "")
                {
                    query = (query != "" ? query + " and (" + query_chaves + ")" : query_chaves);

                    ///Após selecionar as chaves dos termos homonimos
                    ///faz-se a pesquisa dos mesmos.
                    if (!string.IsNullOrEmpty(_sColOrder))
                    {
                        if (("desc" == iSortDir))
                            pesquisa.order_by.desc = new[] { _sColOrder };
                        else
                            pesquisa.order_by.asc = new[] { _sColOrder };
                    }
                    else
                    {
                        pesquisa.order_by.asc = new[] { "nm_termo" };
                    }
                    if (iDisplayLength != "-1")
                    {
                        pesquisa.limit = iDisplayLength;
                        pesquisa.offset = iDisplayStart;
                    }
                    pesquisa.select = null;
                    pesquisa.literal = query;
                    result = vocabularioRn.Consultar(pesquisa);

                    json_resultado = new VocabularioRN().JsonReg(pesquisa);
                    json_resultado = json_resultado.Replace("\"results\": ", "\"aaData\":")
                          .Replace("\"offset\": ", "\"sEcho\": " + ((string.IsNullOrEmpty(sEcho)) ? "\"1\"" : sEcho) + ", \"offset\":")
                          .Replace("\"limit\": ", "\"iTotalRecords\": ")
                          .Replace("\"result_count\": ", "\"iTotalDisplayRecords\": ");

                }
                else
                {
                    json_resultado = "{ \"aaData\": [], \"sEcho\": " + sEcho + ", \"iTotalRecords\": \"" + iDisplayLength + "\", \"iTotalDisplayRecords\": 0}";
                }

                var ind = json_resultado.IndexOf("\"iTotalDisplayRecords\": ") + "\"iTotalDisplayRecords\": ".Length;
                var Busca = new LogBuscar
                {
                    RegistrosPorPagina = iDisplayLength,
                    RegistroInicial = iDisplayStart,
                    RequestNumero = sEcho,
                    RegistrosTotal = json_resultado.Substring(ind, json_resultado.IndexOf(",", ind) - ind),
                    PesquisaLight = pesquisa
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), Busca,sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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

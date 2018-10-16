using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using TCDF.Sinj.Log;
using util.BRLight;
using TCDF.Sinj.RN;

namespace TCDF.Sinj.Web.ashx.Datatable
{
    /// <summary>
    /// Summary description for ArquivoDatatable
    /// </summary>
    public class ArquivoDatatable : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var json_resultado = "";
            Pesquisa pesquisa = new Pesquisa();
            pesquisa.limit = null;
            string _ch_arquivo_superior = context.Request["ch_arquivo_superior"];

            var _sSearch = context.Request["sSearch"];
            string iDisplayLength = context.Request["iDisplayLength"];
            string iDisplayStart = context.Request["iDisplayStart"];
            string sEcho = context.Request.Params["sEcho"];
            var iSortCol = int.Parse(context.Request["iSortCol_0"]);
            var iSortDir = context.Request["sSortDir_0"];
            var _sColOrder = context.Request["mDataProp_" + iSortCol].Replace("_metadata.", "");


            var action = AcoesDoUsuario.arq_pro;
            SessaoUsuarioOV sessao_usuario = null;

            try
            {
                sessao_usuario = Util.ValidarSessao();
                if (iDisplayLength != "-1")
                {
                    pesquisa.limit = iDisplayLength;
                    pesquisa.offset = iDisplayStart;
                }
                if (!string.IsNullOrEmpty(_sColOrder))
                {
                    if (("desc" == iSortDir))
                    {
                        pesquisa.order_by.asc = new[] { "nr_tipo_arquivo" };
                        pesquisa.order_by.desc = new[] { _sColOrder };
                    }
                    else
                    {
                        pesquisa.order_by.asc = new[] { "nr_tipo_arquivo", _sColOrder };
                    }
                }
                if (!string.IsNullOrEmpty(_ch_arquivo_superior))
                {
                    pesquisa.literal = "ch_arquivo_superior='" + _ch_arquivo_superior + "'";
                }
                if (!string.IsNullOrEmpty(_sSearch))
                {
                    pesquisa.literal += (!string.IsNullOrEmpty(pesquisa.literal) ? " AND " : "") + "Upper(nm_arquivo) like '%" + _sSearch.ToUpper() + "%'";
                }

                var oResultado = new SINJ_ArquivoRN().Consultar(pesquisa);
                var dict = new Dictionary<string, object>();
                dict.Add("aaData", oResultado.results);
                dict.Add("sEcho", ((string.IsNullOrEmpty(sEcho)) ? "\"1\"" : sEcho));
                dict.Add("iTotalRecords", oResultado.limit);
                dict.Add("iTotalDisplayRecords", oResultado.result_count);
                dict.Add("offset", oResultado.offset);
                json_resultado = Newtonsoft.Json.JsonConvert.SerializeObject(dict);

                var Busca = new LogBuscar
                {
                    RegistrosPorPagina = iDisplayLength,
                    RegistroInicial = iDisplayStart,
                    RequestNumero = sEcho,
                    RegistrosTotal = oResultado.result_count.ToString(),
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

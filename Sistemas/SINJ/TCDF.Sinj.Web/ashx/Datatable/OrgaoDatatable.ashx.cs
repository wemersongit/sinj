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
    /// Summary description for OrgaoDatatable
    /// </summary>
    public class OrgaoDatatable : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var json_resultado = "";
            Pesquisa pesquisa = new Pesquisa();
            string iDisplayLength = context.Request["iDisplayLength"];
            string iDisplayStart = context.Request["iDisplayStart"];
            string sEcho = context.Request.Params["sEcho"];
            var iSortCol = int.Parse(context.Request["iSortCol_0"]);
            var iSortDir = context.Request["sSortDir_0"];
            var _sColOrder = context.Request["mDataProp_" + iSortCol].Replace("_metadata.", "");
			
            var _nm_orgao = context.Request["nm_orgao"];
            var _ch_orgao = context.Request["ch_orgao"];
            var _sg_orgao = context.Request["sg_orgao"];
            var _orgao_cadastrador = context.Request["orgao_cadastrador"];
            var _id_ambito = context.Request["id_ambito"];
            var _st_orgao = context.Request["st_orgao"];
            var _dt_inicio_vigencia = context.Request["dt_inicio_vigencia"];
            var _dt_fim_vigencia = context.Request["dt_fim_vigencia"];
            var _op_inicio_vigencia = context.Request["op_inicio_vigencia"];
            var _op_fim_vigencia = context.Request["op_fim_vigencia"];
            var _livre = context.Request["livre"];

            var _sSearch = context.Request.Params["sSearch"];
            var _sSt_autoridade = context.Request.Params["st_autoridade"];


            var action = AcoesDoUsuario.org_pes;
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
                var query = "";
                if (!string.IsNullOrEmpty(_sSearch))
                {
                    //sQuery = string.Format("TRANSLATE(Upper(nm_orgao), 'áéíóúàèìòùãõâêîôôäëïöüçÁÉÍÓÚÀÈÌÒÙÃÕÂÊÎÔÛÄËÏÖÜÇ', 'aeiouaeiouaoaeiooaeioucAEIOUAEIOUAOAEIOOAEIOUC') like TRANSLATE('%{0}%', 'áéíóúàèìòùãõâêîôôäëïöüçÁÉÍÓÚÀÈÌÒÙÃÕÂÊÎÔÛÄËÏÖÜÇ', 'aeiouaeiouaoaeiooaeioucAEIOUAEIOUAOAEIOOAEIOUC') or Upper(sg_orgao) like'%" + _texto.ToUpper() + "%'", _texto.ToUpper());
                    query = string.Format("TRANSLATE(Upper(nm_orgao), 'áéíóúàèìòùãõâêîôôäëïöüçÁÉÍÓÚÀÈÌÒÙÃÕÂÊÎÔÛÄËÏÖÜÇ', 'aeiouaeiouaoaeiooaeioucAEIOUAEIOUAOAEIOOAEIOUC') like TRANSLATE('%{0}%', 'áéíóúàèìòùãõâêîôôäëïöüçÁÉÍÓÚÀÈÌÒÙÃÕÂÊÎÔÛÄËÏÖÜÇ', 'aeiouaeiouaoaeiooaeioucAEIOUAEIOUAOAEIOOAEIOUC') or Upper(sg_orgao) like'%" + _sSearch.ToUpper() + "%'", _sSearch.ToUpper());
                    //query = "Upper(nm_orgao) like '%" + _sSearch.ToUpper() + "%' or Upper(sg_orgao) like '%" + _sSearch.ToUpper() + "%'";
                }
                if (!string.IsNullOrEmpty(_sSt_autoridade))
                {
                    query += (query != "" ? " and " : "") + "st_autoridade=" + _sSt_autoridade;
                }
                if (!string.IsNullOrEmpty(_livre))
                {
                    query += (query != "" ? " and " : "") + "Upper(document::text) like '%" + _livre.ToUpper() + "%'";
                }
                if (!string.IsNullOrEmpty(_nm_orgao))
                {
                    query += (query != "" ? " and " : "") + "Upper(nm_orgao) like '%" + _nm_orgao.ToUpper() + "%'";
                }
                if (!string.IsNullOrEmpty(_sg_orgao))
                {
                    query += (query != "" ? " and " : "") + "Upper(sg_orgao) like '%" + _sg_orgao.ToUpper() + "%'";
                }
                if (!string.IsNullOrEmpty(_orgao_cadastrador))
                {
                    query += (query != "" ? " and " : "") + _orgao_cadastrador.ToUpper() + "=true";
                }
                if (!string.IsNullOrEmpty(_id_ambito))
                {
                    query += (query != "" ? " and " : "") + "id_ambito='" + _id_ambito + "'";
                }
                if (!string.IsNullOrEmpty(_st_orgao))
                {
                    query += (query != "" ? " and " : "") + "st_orgao=" + _st_orgao;
                }
                if (!string.IsNullOrEmpty(_ch_orgao))
                {
                    query += (query != "" ? " and " : "") + "ch_orgao=" + _ch_orgao;
                }
                if (!string.IsNullOrEmpty(_dt_inicio_vigencia))
                {
                    query += (query != "" ? " and " : "") + "CAST(dt_inicio_vigencia AS DATE)" + LB.ReplaceOperatorToQuery(_op_inicio_vigencia) + "'" + _dt_inicio_vigencia + "'";
                }
                if (!string.IsNullOrEmpty(_dt_fim_vigencia))
                {
                    query += (query != "" ? " and " : "") + "CAST(dt_fim_vigencia AS DATE)" + LB.ReplaceOperatorToQuery(_op_fim_vigencia) + "'" + _dt_fim_vigencia + "'";
                }
                pesquisa.literal = query;
                json_resultado = new OrgaoRN().JsonReg(pesquisa);
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
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), Busca, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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
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
    /// Summary description for VocabularioDatatable
    /// </summary>
    public class VocabularioDatatable : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var json_resultado = "";
            Pesquisa pesquisa = new Pesquisa();
            var _sSearch = context.Request.Params["sSearch"];

            var _sId_doc_ignored = context.Request.Params["id_doc_ignored"];

            //Esse parametro foi adicionado para tratamento de termos restaurados da migração bugada
            var _sCh_termo_ignoreds = context.Request.Params["ch_termo_ignoreds"];

            ulong id_doc_ignored = 0;
            var _ch_tipo_termo = context.Request.Params["ch_tipo_termo"];
            var _filtro_ch_tipo_termo = context.Request.Params["filtro_ch_tipo_termo"];
            var _ch_termo = context.Request.Params["ch_termo"];
            var _nm_termo = context.Request.Params["nm_termo"];
            var _letra = context.Request.Params["letra"];
            var _sSt_excluir = context.Request.Params["st_excluir"];
            var bSt_excluir = false;
            var _sSt_aprovado = context.Request.Params["st_aprovado"];
            var bSt_aprovado = false;
            var _sSt_ativo = context.Request.Params["st_ativo"];
            var bSt_ativo = false;
            var _sSt_restaurado = context.Request.Params["st_restaurado"];
            var bSt_restaraudo = false;
            var _sIn_nao_autorizado = context.Request.Params["in_nao_autorizado"];
            var bIn_nao_autorizado = false;

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
                if (!string.IsNullOrEmpty(_letra))
                {
                    query = "Upper(nm_termo) like '" + _letra.ToUpper() + "%'";
                }
                else if (!string.IsNullOrEmpty(_nm_termo))
                {
                    if (_nm_termo == "%")
                    {
                        if (!string.IsNullOrEmpty(_sSearch))
                        {
                            query = string.Format("TRANSLATE(Upper(nm_termo), 'áéíóúàèìòùãõâêîôôäëïöüçÁÉÍÓÚÀÈÌÒÙÃÕÂÊÎÔÛÄËÏÖÜÇ', 'aeiouaeiouaoaeiooaeioucAEIOUAEIOUAOAEIOOAEIOUC') like TRANSLATE('%{0}%', 'áéíóúàèìòùãõâêîôôäëïöüçÁÉÍÓÚÀÈÌÒÙÃÕÂÊÎÔÛÄËÏÖÜÇ', 'aeiouaeiouaoaeiooaeioucAEIOUAEIOUAOAEIOOAEIOUC')", _sSearch.ToUpper().Replace("'", "''"));
                        }
                        else
                        {
                            query = "Upper(nm_termo) like '%'";
                        }
                    }
                    else{
                        query = "Upper(nm_termo) like '%" + _nm_termo.ToUpper() + "%'";
                        if (!string.IsNullOrEmpty(_sSearch))
                        {
                            query += " and " + string.Format("TRANSLATE(Upper(nm_termo), 'áéíóúàèìòùãõâêîôôäëïöüçÁÉÍÓÚÀÈÌÒÙÃÕÂÊÎÔÛÄËÏÖÜÇ', 'aeiouaeiouaoaeiooaeioucAEIOUAEIOUAOAEIOOAEIOUC') like TRANSLATE('%{0}%', 'áéíóúàèìòùãõâêîôôäëïöüçÁÉÍÓÚÀÈÌÒÙÃÕÂÊÎÔÛÄËÏÖÜÇ', 'aeiouaeiouaoaeiooaeioucAEIOUAEIOUAOAEIOOAEIOUC')", _sSearch.ToUpper().Replace("'", "''"));
                        }
                    }
                    
                }
                else if (!string.IsNullOrEmpty(_sSearch))
                {
                    query += string.Format("TRANSLATE(Upper(nm_termo), 'áéíóúàèìòùãõâêîôôäëïöüçÁÉÍÓÚÀÈÌÒÙÃÕÂÊÎÔÛÄËÏÖÜÇ', 'aeiouaeiouaoaeiooaeioucAEIOUAEIOUAOAEIOOAEIOUC') like TRANSLATE('%{0}%', 'áéíóúàèìòùãõâêîôôäëïöüçÁÉÍÓÚÀÈÌÒÙÃÕÂÊÎÔÛÄËÏÖÜÇ', 'aeiouaeiouaoaeiooaeioucAEIOUAEIOUAOAEIOOAEIOUC')", _sSearch.ToUpper().Replace("'", "''"));
                }
                if (!string.IsNullOrEmpty(_sId_doc_ignored) && ulong.TryParse(_sId_doc_ignored, out id_doc_ignored))
                {
                    query += (query != "" ? " and " : "") + "id_doc<>" + id_doc_ignored;
                }
                if (!string.IsNullOrEmpty(_sCh_termo_ignoreds))
                {
                    var ch_doc_ignoreds = _sCh_termo_ignoreds.Split(',');
                    foreach(var ch in ch_doc_ignoreds){
                        query += (query != "" ? " and " : "") + "ch_termo<>'" + ch +"'";
                    }
                }

                if (!string.IsNullOrEmpty(_filtro_ch_tipo_termo))
                {
                    var aux_query = "";
                    foreach (var filtro in _filtro_ch_tipo_termo.Split(','))
                    {
                        aux_query += (aux_query != "" ? " or " : "") + "ch_tipo_termo='"+filtro+"'";
                    }
                    query += (query != "" ? " and " : "") + "(" + aux_query + ")";
                }
                else if (!string.IsNullOrEmpty(_ch_tipo_termo))
                {
                    query += (query != "" ? " and " : "") + "ch_tipo_termo='" + _ch_tipo_termo + "'";
                }

                if (!string.IsNullOrEmpty(_sSt_excluir) && bool.TryParse(_sSt_excluir, out bSt_excluir))
                {
                    query += (query != "" ? " and " : "") + "st_excluir=" + bSt_excluir;
                }
                if (!string.IsNullOrEmpty(_sSt_aprovado) && bool.TryParse(_sSt_aprovado, out bSt_aprovado))
                {
                    query += (query != "" ? " and " : "") + "st_aprovado=" + bSt_aprovado;
                }
                if (!string.IsNullOrEmpty(_sSt_ativo) && bool.TryParse(_sSt_ativo, out bSt_ativo))
                {
                    query += (query != "" ? " and " : "") + "st_ativo=" + bSt_ativo;
                }
                if (!string.IsNullOrEmpty(_sSt_restaurado) && bool.TryParse(_sSt_restaurado, out bSt_restaraudo))
                {
                    query += (query != "" ? " and " : "") + "st_restaurado=" + bSt_restaraudo;
                }
                if (!string.IsNullOrEmpty(_sIn_nao_autorizado) && bool.TryParse(_sIn_nao_autorizado, out bIn_nao_autorizado))
                {
                    query += (query != "" ? " and " : "") + "in_nao_autorizado=" + bIn_nao_autorizado;
                }
                if (!string.IsNullOrEmpty(_ch_termo))
                {
                    query += (query != "" ? " and " : "") + "ch_termo='" + _ch_termo + "'";
                }

                pesquisa.literal = query;
                json_resultado = new VocabularioRN().JsonReg(pesquisa);
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

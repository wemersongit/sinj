using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Datatable
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
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (Config.ValorChave("Aplicacao") == "CADASTRO")
                {
                    sessao_usuario = Util.ValidarSessao();
                    //Não faz nada se o usuário não tiver sessão no portal pois essa pesquisa pode ser feita por qualquer um.
                }
                var normaRn = new NormaRN();
                var diarioRn = new DiarioRN();

                if (_exibir_total == "1")
                {
                    if (_bbusca == "sinj_norma")
                    {
                        sRetorno = "{\"counts\":[{\"nm_base\":\"" + _bbusca + "\",\"ds_base\":\"Normas\",\"count\":" + normaRn.PesquisarTotalEs(context) + "}]}";
                    }
                    else if (_bbusca == "sinj_diario")
                    {
                        sRetorno = "{\"counts\":[{\"nm_base\":\"" + _bbusca + "\",\"ds_base\":\"Diários\",\"count\":" + diarioRn.PesquisarTotalEs(context) + "}]}";
                    }
                    else
                    {
                        sRetorno = "{\"counts\":[{\"nm_base\":\"sinj_norma\",\"ds_base\":\"Normas\",\"count\":" + normaRn.PesquisarTotalEs(context) + "},{\"nm_base\":\"sinj_diario\",\"ds_base\":\"Diários\",\"count\":" + diarioRn.PesquisarTotalEs(context) + "}]}";
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
                    sRetorno = "{\"counts\":[{\"nm_base\":\"sinj_norma\",\"ds_base\":\"Normas\",\"count\":{\"count\":0}},{\"nm_base\":\"sinj_diario\",\"ds_base\":\"Diários\",\"count\":{\"count\":0}}]}";
                }
                else
                {
                    sRetorno = "{\"echo\":" + _sEcho + ",\"iTotalRecords\":\"0\",\"iTotalDisplayRecords\":\"0\",\"aaData\":[]}";
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
                if (sessao_usuario != null)
                {
                    nm_usuario = sessao_usuario.nm_usuario;
                    nm_login_usuario = sessao_usuario.nm_login_usuario;
                    LogErro.gravar_erro(sAction, erro, nm_usuario, nm_login_usuario);
                }
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(sRetorno);
            context.Response.End();
        }

        ///// <summary>
        ///// Consulta no elasticsearch
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="sessao_usuario_ov"></param>
        ///// <returns></returns>
        //private string MontarConsulta(ref HttpContext context, ref SessaoUsuarioOV sessao_usuario_ov, string _bbusca)
        //{
        //    string _tipo_pesquisa = context.Request["tipo_pesquisa"];
        //    string _sSearch = context.Request["sSearch"];
        //    string _all = context.Request["all"];
        //    string _exibir_total = context.Request["exibir_total"];

        //    var sOrder = MontarOrdenamento(ref context, _bbusca);

        //    //Os campos que retornarão do ElasticSearch
        //    var partial_fields = "";
        //    if (_exibir_total != "1")
        //    {
        //        partial_fields = MontarPartialFields(ref context, _bbusca);
        //    }

        //    //Os campos onde a consulta textual será feita
        //    var fields = MontarCamposPesquisados(ref context, _bbusca);

        //    var fields_highlight = "";
        //    var highlight = "";

        //    var query = "";
        //    if (_tipo_pesquisa == "geral")
        //    {
        //        if (_exibir_total != "1")
        //        {
        //            fields_highlight = MontarHighlight(ref context, _bbusca);
        //        }
        //        if (!string.IsNullOrEmpty(_all))
        //        {
        //            //Escapar os caracteres reservados antes de montar a query
        //            query += new PortalRN().TratarCaracteresReservadosDoEs(_all);

        //            //MontarQueryRankemanto(ref context, ref fields, ref query, ref sessao_usuario_ov);

        //            query = "{\"query_string\":{\"fields\":[" + fields + "],\"query\":\"" + query;

        //            //Insere na consulta o filtro digitado no campo search do datatable
        //            if (!string.IsNullOrEmpty(_sSearch))
        //            {
        //                //Remove a acentuação porque a consulta com asterisco não funciona com acentuação
        //                util.BRLight.ManipulaTexto.RemoverAcentuacao(ref _sSearch);
        //                query += " AND _all:(" + new PortalRN().TratarCaracteresReservadosDoEs(_sSearch) + "*)";
        //            }

        //            //Aplicar filtro por unidade
        //            //MontarQueryFiltros(ref context, ref query);

        //            //Define como operador padrão o AND para preencher as lacunas da consulta, ou seja, "josé da silva" igual a "josé AND da AND silva"
        //            query += "\", \"default_operator\":\"AND\"}}";
        //        }
        //    }
        //    else if (_tipo_pesquisa == "norma")
        //    {
        //        var filtered = "";
        //        var filter = "";
        //        var query_textual = "";
        //        var _ch_tipo_norma = context.Request["ch_tipo_norma"];
        //        var _nr_norma = context.Request["nr_norma"];
        //        var _ano_assinatura = context.Request["ano_assinatura"];
        //        var _ch_orgao = context.Request["ch_orgao"];
        //        var _ch_termo = context.Request.Form.GetValues("ch_termo");
        //        if (!string.IsNullOrEmpty(_ch_tipo_norma))
        //        {
        //            filter += (!string.IsNullOrEmpty(filter) ? "," : "") + "{\"term\":{\"ch_tipo_norma\":{\"value\":\"" + _ch_tipo_norma + "\"}}}";
        //        }
        //        if (!string.IsNullOrEmpty(_nr_norma))
        //        {
        //            filter += (!string.IsNullOrEmpty(filter) ? "," : "") + "{\"term\":{\"nr_norma\":{\"value\":\"" + _nr_norma + "\"}}}";
        //        }
        //        if (!string.IsNullOrEmpty(_ano_assinatura))
        //        {
        //            filter += (!string.IsNullOrEmpty(filter) ? "," : "") + "{\"range\":{\"dt_assinatura\":{ \"gte\":\"01/01/" + _ano_assinatura + "\",\"lte\":\"31/12/" + _ano_assinatura + "\"}}}";
        //        }
        //        if (_ch_termo != null && _ch_termo.Length > 0)
        //        {
        //            for (var i = 0; i < _ch_termo.Length; i++)
        //            {
        //                filter += (!string.IsNullOrEmpty(filter) ? "," : "") + "{\"term\":{\"ch_termo\":{\"value\":\"" + _ch_termo[i] + "\"}}}";
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(_ch_orgao))
        //        {
        //            var _origem_por = context.Request["origem_por"];
        //            var chaves = "";
        //            if (string.IsNullOrEmpty(_origem_por))
        //            {
        //                _origem_por = "toda_a_hierarquia_em_qualquer_epoca";
        //            }
        //            var orgaos_hierarquia = new List<OrgaoOV>();
        //            var orgaos_cronologia = new List<OrgaoOV>();
        //            var orgaos_superiores = new List<OrgaoOV>();
        //            var orgaos_inferiores = new List<OrgaoOV>();
        //            switch (_origem_por)
        //            {
        //                case "toda_a_hierarquia_em_qualquer_epoca":
        //                    orgaos_hierarquia = new OrgaoRN().BuscarOrgaosDaHierarquia(_ch_orgao);
        //                    orgaos_cronologia = new OrgaoRN().BuscarOrgaosDaCronologia(_ch_orgao);
        //                    foreach (var orgao in orgaos_hierarquia)
        //                    {
        //                        chaves += (!string.IsNullOrEmpty(chaves) ? " OR " : "") + orgao.ch_orgao;
        //                    }
        //                    foreach (var orgao in orgaos_cronologia)
        //                    {
        //                        chaves += (!string.IsNullOrEmpty(chaves) ? " OR " : "") + orgao.ch_orgao;
        //                    }
        //                    break;
        //                case "hierarquia_superior":
        //                    var _ch_hierarquia = context.Request["ch_hierarquia"];
        //                    orgaos_superiores = new OrgaoRN().BuscarHierarquiaSuperior(_ch_hierarquia);
        //                    foreach (var orgao in orgaos_superiores)
        //                    {
        //                        chaves += (!string.IsNullOrEmpty(chaves) ? " OR " : "") + orgao.ch_orgao;
        //                    }
        //                    break;
        //                case "hierarquia_inferior":
        //                    orgaos_inferiores = new OrgaoRN().BuscarHierarquiaInferior(_ch_orgao);
        //                    foreach (var orgao in orgaos_inferiores)
        //                    {
        //                        chaves += (!string.IsNullOrEmpty(chaves) ? " OR " : "") + orgao.ch_orgao;
        //                    }
        //                    break;
        //                case "toda_a_hierarquia":
        //                    orgaos_hierarquia = new OrgaoRN().BuscarOrgaosDaHierarquia(_ch_orgao);
        //                    foreach (var orgao in orgaos_hierarquia)
        //                    {
        //                        chaves += (!string.IsNullOrEmpty(chaves) ? " OR " : "") + orgao.ch_orgao;
        //                    }
        //                    break;
        //                case "em_qualquer_epoca":
        //                    orgaos_cronologia = new OrgaoRN().BuscarOrgaosDaCronologia(_ch_orgao);
        //                    if (orgaos_cronologia.Count > 0)
        //                    {
        //                        foreach (var orgao in orgaos_cronologia)
        //                        {
        //                            chaves += (!string.IsNullOrEmpty(chaves) ? " OR " : "") + orgao.ch_orgao;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        chaves = _ch_orgao;
        //                    }
        //                    break;
        //                default:
        //                    chaves = _ch_orgao;
        //                    break;
        //            }
        //            filter += (!string.IsNullOrEmpty(filter) ? "," : "") + "{\"query\":{\"match\":{\"ch_orgao\":{\"query\":\"" + chaves + "\"}}}}";
        //        }
        //        if (!string.IsNullOrEmpty(_all))
        //        {
        //            var fields_split = fields.Replace("\"", "").Split(',');
        //            for (var i = 0; i < fields_split.Length; i++)
        //            {
        //                query_textual += (query_textual != "" ? " AND " : "") + "(" + fields_split[i] + ":" + new PortalRN().TratarCaracteresReservadosDoEsPesquisaAvancada(_all) + ")";
        //            }
        //            if (_exibir_total != "1")
        //            {
        //                fields_highlight += (fields_highlight != "" ? "," : "") + MontarHighlight(ref context, _bbusca);
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(_sSearch))
        //        {
        //            query_textual += (query_textual != "" ? " AND " : "") + "(_all:" + new PortalRN().TratarCaracteresReservadosDoEsPesquisaAvancada(_sSearch) + "*)";
        //        }
        //        if (query_textual != "")
        //        {
        //            filtered += "{\"query\":{\"query_string\":{\"query\":\"" + query_textual + "\", \"default_operator\":\"AND\"}}";
        //        }
        //        if (filtered != "" && filter != "")
        //        {
        //            query = "{\"filtered\":" + filtered + ",\"filter\":{\"and\":[" + filter + "]}}}";
        //        }
        //        else if (filtered != "")
        //        {
        //            query = "{\"filtered\":" + filtered + "}}";
        //        }
        //        else if (filter != "")
        //        {
        //            query = "{\"filtered\":{\"filter\":{\"and\":[" + filter + "]}}}";
        //        }
        //    }
        //    else if (_tipo_pesquisa == "diario")
        //    {
        //        var filtered = "";
        //        var filter = "";
        //        var query_textual = "";
        //        var _ch_tipo_fonte = context.Request["ch_tipo_fonte"];
        //        var _nr_diario = context.Request["nr_diario"];
        //        var _secao_diario = context.Request["secao_diario"];
        //        var _filetext = context.Request["filetext"];
        //        var _op_dt_assinatura = context.Request["op_dt_assinatura"];
        //        var _dt_assinatura = context.Request.Form.GetValues("dt_assinatura");
        //        if (!string.IsNullOrEmpty(_ch_tipo_fonte))
        //        {
        //            filter += (!string.IsNullOrEmpty(filter) ? "," : "") + "{\"term\":{\"ch_tipo_fonte\":{\"value\":\"" + _ch_tipo_fonte + "\"}}}";
        //        }
        //        if (!string.IsNullOrEmpty(_nr_diario))
        //        {
        //            filter += (!string.IsNullOrEmpty(filter) ? "," : "") + "{\"term\":{\"nr_diario\":{\"value\":\"" + _nr_diario + "\"}}}";
        //        }
        //        if (_dt_assinatura != null && _dt_assinatura.Length > 0 && !string.IsNullOrEmpty(_dt_assinatura[0]))
        //        {
        //            if (_op_dt_assinatura == "intervalo")
        //            {
        //                if (_dt_assinatura.Length == 2)
        //                {
        //                    filter += (!string.IsNullOrEmpty(filter) ? "," : "") + "{\"range\":{\"dt_assinatura\":{ \"gte\":\"" + _dt_assinatura[0] + "\",\"lte\":\"" + _dt_assinatura[1] + "\"}}}";
        //                }
        //            }
        //            else if (_op_dt_assinatura == "menor")
        //            {
        //                filter += (!string.IsNullOrEmpty(filter) ? "," : "") + "{\"range\":{\"dt_assinatura\":{\"lt\":\"" + _dt_assinatura[0] + "\"}}}";

        //            }
        //            else if (_op_dt_assinatura == "menorouigual")
        //            {
        //                filter += (!string.IsNullOrEmpty(filter) ? "," : "") + "{\"range\":{\"dt_assinatura\":{\"lte\":\"" + _dt_assinatura[0] + "\"}}}";

        //            }
        //            else if (_op_dt_assinatura == "maior")
        //            {
        //                filter += (!string.IsNullOrEmpty(filter) ? "," : "") + "{\"range\":{\"dt_assinatura\":{\"gt\":\"" + _dt_assinatura[0] + "\"}}}";

        //            }
        //            else if (_op_dt_assinatura == "maiorouigual")
        //            {
        //                filter += (!string.IsNullOrEmpty(filter) ? "," : "") + "{\"range\":{\"dt_assinatura\":{\"gte\":\"" + _dt_assinatura[0] + "\"}}}";

        //            }
        //            else if (_op_dt_assinatura == "diferente")
        //            {
        //                filter += (!string.IsNullOrEmpty(filter) ? "," : "") + "{\"not\":{\"term\":{\"dt_assinatura\":{ \"value\":\"" + _dt_assinatura[0] + "\"}}}}";
        //            }
        //            else
        //            {
        //                filter += (!string.IsNullOrEmpty(filter) ? "," : "") + "{\"term\":{\"dt_assinatura\":{\"value\":\"" + _dt_assinatura[0] + "\"}}}";
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(_all))
        //        {
        //            var fields_split = fields.Replace("\"", "").Split(',');
        //            for (var i = 0; i < fields_split.Length; i++)
        //            {
        //                query_textual += (query_textual != "" ? " AND " : "") + "(" + fields_split[i] + ":" + new PortalRN().TratarCaracteresReservadosDoEsPesquisaAvancada(_all) + ")";
        //            }
        //            if (_exibir_total != "1")
        //            {
        //                fields_highlight += (fields_highlight != "" ? "," : "") + MontarHighlight(ref context, _bbusca);
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(_sSearch))
        //        {
        //            query_textual += (query_textual != "" ? " AND " : "") + "(_all:" + new PortalRN().TratarCaracteresReservadosDoEsPesquisaAvancada(_sSearch) + "*)";
        //        }
        //        if (!string.IsNullOrEmpty(_secao_diario))
        //        {
        //            query_textual += (query_textual != "" ? " AND " : "") + "(secao_diario:" + new PortalRN().TratarCaracteresReservadosDoEs(_secao_diario) + ")";
        //            fields_highlight += (fields_highlight != "" ? "," : "") + "\"secao_diario\":{\"force_source\":true,\"number_of_fragments\":0}";
                    
        //        }
        //        if (!string.IsNullOrEmpty(_filetext))
        //        {
        //            query_textual += (query_textual != "" ? " AND " : "") + "(ar_diario.filetext:" + new PortalRN().TratarCaracteresReservadosDoEs(_filetext) + ")";
        //            if (highlight.IndexOf("ar_diario.filetext") == -1)
        //            {
        //                fields_highlight += (fields_highlight != "" ? "," : "") + "\"ar_diario.filetext\":{\"force_source\":true,\"number_of_fragments\":0}";
        //            }
        //        }
        //        if (query_textual != "")
        //        {
        //            filtered += "{\"query\":{\"query_string\":{\"query\":\"" + query_textual + "\", \"default_operator\":\"AND\"}}";
        //        }
        //        if (filtered != "" && filter != "")
        //        {
        //            query = "{\"filtered\":" + filtered + ",\"filter\":{\"and\":[" + filter + "]}}}";
        //        }
        //        else if (filtered != "")
        //        {
        //            query = "{\"filtered\":" + filtered + "}}";
        //        }
        //        else if (filter != "")
        //        {
        //            query = "{\"filtered\":{\"filter\":{\"and\":[" + filter + "]}}}";
        //        }
        //    }
        //    else if(_tipo_pesquisa == "cesta"){
        //        var _cesta = context.Request["cesta"];
        //        var _base = context.Request["b"];
        //        var aCesta = new string[0];
        //        var ids = "";
        //        if(!string.IsNullOrEmpty(_cesta)){
        //            aCesta = _cesta.Split(',');
        //        }
        //        foreach(var sCesta in aCesta){
        //            var sCesta_split = sCesta.Split('_');
        //            if (sCesta_split[0] == _base)
        //            {
        //                ids += (ids != "" ? "," : "") + sCesta_split[1];
        //            }
        //        }
        //        if (ids != "")
        //        {
        //            query = "{\"ids\":{\"values\":[" + ids + "]}}";
        //        }
        //    }
        //    else if (_tipo_pesquisa == "avancada")
        //    {
        //        var _argumento = context.Request.Params.GetValues("argumento");
        //        var query_textual = "";
        //        if (_argumento != null)
        //        {
        //            var b_all_fields_highlight = false;
        //            var filtered = "";
        //            var filter = "";
        //            var filtering = "";
        //            var filter_and = "";
        //            var filter_or = "";
        //            var argumento_split = new string[0];
        //            var _tipo_campo = "";
        //            var _ch_campo = "";
        //            var _ch_operador = "";
        //            var _ch_valor = "";
        //            var _conector = "";

        //            //guardam o conector do último argumento para concatenar com o proximo
        //            var aux_conector_es = "";
        //            foreach (var argumento in _argumento)
        //            {
        //                argumento_split = argumento.Split('#');
        //                if (argumento_split.Length == 8)
        //                {
        //                    _tipo_campo = argumento_split[0];
        //                    _ch_campo = argumento_split[1];
        //                    _ch_operador = argumento_split[3];
        //                    _ch_valor = argumento_split[5];
        //                    if (_tipo_campo == "autocomplete")
        //                    {
        //                        if (_ch_operador == "diferente")
        //                        {
        //                            query_textual += (query_textual != "" ? aux_conector_es : "") + "(" + _ch_campo + ":(NOT \\\"" + _ch_valor + "\\\"))";
        //                        }
        //                        else
        //                        {
        //                            query_textual += (query_textual != "" ? aux_conector_es : "") + "(" + _ch_campo + ":\\\"" + _ch_valor + "\\\")";
        //                        }
        //                        if (_exibir_total != "1")
        //                        {
        //                            fields_highlight += (fields_highlight != "" ? "," : "") + "\"" + _ch_campo + "\":{\"force_source\":true,\"number_of_fragments\":0}";
        //                        }
        //                        //filtering = "{\"query\":{\"query_string\":{\"query\":\"" + _ch_campo + ":\\\"" + _ch_valor + "\\\"\"}}}";
        //                    }
        //                    else if (_tipo_campo == "text")
        //                    {
        //                        var conector_textual = _ch_operador == "contem" ? " OR " : " ";
        //                        if (_ch_campo == "all")
        //                        {
        //                            var fields_split = fields.Replace("\"", "").Split(',');
        //                            if (_ch_operador == "diferente")
        //                            {
        //                                for (var i = 0; i < fields_split.Length; i++)
        //                                {
        //                                    query_textual += (query_textual != "" ? aux_conector_es : "") + "(" + fields_split[i] + ":(NOT " + new PortalRN().TratarCaracteresReservadosDoEsPesquisaAvancada(_ch_valor).Replace(" ", conector_textual) + "))";
        //                                }
        //                            }
        //                            else
        //                            {
        //                                for (var i = 0; i < fields_split.Length; i++)
        //                                {
        //                                    query_textual += (query_textual != "" ? aux_conector_es : "") + "(" + fields_split[i] + ":" + new PortalRN().TratarCaracteresReservadosDoEsPesquisaAvancada(_ch_valor).Replace(" ", conector_textual) + ")";
        //                                }
        //                            }
        //                            if (_exibir_total != "1")
        //                            {
        //                                if (!b_all_fields_highlight)
        //                                {
        //                                    fields_highlight += (fields_highlight != "" ? "," : "") + MontarHighlight(ref context, _bbusca);
        //                                    b_all_fields_highlight = true;
        //                                }
        //                                else if (highlight.IndexOf(_ch_campo) == -1)
        //                                {
        //                                    fields_highlight += (fields_highlight != "" ? "," : "") + "\"" + _ch_campo + "\":{\"force_source\":true,\"number_of_fragments\":0}";
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (_ch_operador == "diferente")
        //                            {
        //                                query_textual += (query_textual != "" ? aux_conector_es : "") + "(" + _ch_campo + ":(NOT " + new PortalRN().TratarCaracteresReservadosDoEsPesquisaAvancada(_ch_valor).Replace(" ", conector_textual) + "))";
        //                            }
        //                            else
        //                            {
        //                                query_textual += (query_textual != "" ? aux_conector_es : "") + "(" + _ch_campo + ":" + new PortalRN().TratarCaracteresReservadosDoEsPesquisaAvancada(_ch_valor).Replace(" ", conector_textual) + ")";
        //                            }
        //                            if (_exibir_total != "1")
        //                            {
        //                                fields_highlight += (fields_highlight != "" ? "," : "") + "\"" + _ch_campo + "\":{\"force_source\":true,\"number_of_fragments\":0}";
        //                            }
        //                        }
        //                    }
        //                    else if (_tipo_campo == "number" || _tipo_campo == "date")
        //                    {
        //                        filtering = new PortalRN().MontarArgumentoRange(_ch_campo, _ch_operador, _ch_valor);
        //                        if (_conector == "OU")
        //                        {
        //                            filter_or += (!string.IsNullOrEmpty(filter_or) ? "," : "") + filtering;
        //                        }
        //                        if (_conector == "NÃO")
        //                        {
        //                            filter_and += (!string.IsNullOrEmpty(filter_and) ? "," : "") + "{\"not\":" + filtering + "}";
        //                        }
        //                        else
        //                        {
        //                            filter_and += (!string.IsNullOrEmpty(filter_and) ? "," : "") + filtering;
        //                        }
        //                    }
        //                    else if (_tipo_campo == "datetime")
        //                    {
        //                        var ch_valor_splited = _ch_valor.Split(',');
        //                        if (ch_valor_splited.Length == 2)
        //                        {
        //                            for (var i = 0; i < ch_valor_splited.Length; i++)
        //                            {
        //                                var aux_valor_splited = ch_valor_splited[i].Split(' ');
        //                                if (aux_valor_splited.Length == 2 && aux_valor_splited[1] == "00:00:00")
        //                                {
        //                                    ch_valor_splited[i] = aux_valor_splited[0];
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            var aux_valor_splited = ch_valor_splited[0].Split(' ');
        //                            if (aux_valor_splited.Length == 2 && aux_valor_splited[1] == "00:00:00")
        //                            {
        //                                ch_valor_splited = new string[] { ch_valor_splited[0], aux_valor_splited[0] + " 23:59:59" };
        //                                _ch_operador = "intervalo";
        //                            }
        //                        }

        //                        _ch_valor = String.Join(",", ch_valor_splited);

        //                        filtering = new PortalRN().MontarArgumentoRange(_ch_campo, _ch_operador, _ch_valor);
        //                        if (_conector == "OU")
        //                        {
        //                            filter_or += (!string.IsNullOrEmpty(filter_or) ? "," : "") + filtering;
        //                        }
        //                        if (_conector == "NÃO")
        //                        {
        //                            filter_and += (!string.IsNullOrEmpty(filter_and) ? "," : "") + "{\"not\":" + filtering + "}";
        //                        }
        //                        else
        //                        {
        //                            filter_and += (!string.IsNullOrEmpty(filter_and) ? "," : "") + filtering;
        //                        }
        //                    }
        //                    _conector = argumento_split[7];
        //                    //substitui o conector (E/OU) pelo que é usado no ES (AND/OR)
        //                    aux_conector_es = _conector == "E" ? " AND " : _conector == "OU" ? " OR " : _conector == "NÃO" ? " NOT " : " ";
        //                }
        //            }
        //            //Insere na consulta o filtro digitado no campo search do datatable
        //            if (!string.IsNullOrEmpty(_sSearch))
        //            {
        //                //Remove a acentuação porque a consulta com asterisco não funciona com acentuação
        //                util.BRLight.ManipulaTexto.RemoverAcentuacao(ref _sSearch);
        //                query_textual += (query_textual != "" ? " AND " : "") + "_all:(" + new PortalRN().TratarCaracteresReservadosDoEs(_sSearch) + "*)";
        //            }
        //            if (query_textual != "")
        //            {
        //                filtered += "{\"query\":{\"query_string\":{\"query\":\"" + query_textual + "\", \"default_operator\":\"AND\"}}";
        //            }
        //            //if (fields_highlight != "")
        //            //{
        //            //    highlight = ",\"highlight\":{\"pre_tags\":[\"_pre_tag_highlight_\"],\"post_tags\":[\"_post_tag_highlight_\"],\"fields\":{" + fields_highlight + "}}";
        //            //}
        //            if (_bbusca == "sinj_norma")
        //            {
        //                var _ch_tipo_norma = context.Request["ch_tipo_norma"];
        //                if (!string.IsNullOrEmpty(_ch_tipo_norma) && (_ch_tipo_norma != "todas"))
        //                {
        //                    filter_and += (!string.IsNullOrEmpty(filter_and) ? "," : "") + "{\"term\":{\"ch_tipo_norma\":{\"value\":\"" + _ch_tipo_norma + "\"}}}";
        //                }
        //            }
        //            if (filter_and != "")
        //            {
        //                filter = "\"and\":[" + filter_and + "]";
        //            }
        //            if (filter_or != "")
        //            {
        //                filter += (!string.IsNullOrEmpty(filter) ? "," : "") + "\"or\":[" + filter_or + "]";
        //            }
        //            if (filtered != "" && filter != "")
        //            {
        //                query = "{\"filtered\":" + filtered + ",\"filter\":{" + filter + "}}}";
        //            }
        //            else if (filtered != "")
        //            {
        //                query = "{\"filtered\":" + filtered + "}}";
        //            }
        //            else if (filter != "")
        //            {
        //                query = "{\"filtered\":{\"filter\":{" + filter + "}}}";
        //            }
        //        }
        //    }
        //    if (query == "")
        //    {
        //        throw new Exception("Pesquisa vazia.");
        //    }
        //    if (fields_highlight != "" && _exibir_total != "1")
        //    {
        //        highlight = ",\"highlight\":{\"pre_tags\":[\"_pre_tag_highlight_\"],\"post_tags\":[\"_post_tag_highlight_\"],\"fields\":{" + fields_highlight + "}}";
        //    }
        //    query = "{\"query\":" + query + sOrder + partial_fields + highlight + "}";
        //    return query;
        //}

        /////// <summary>
        /////// Filtrar por unidade e arquivados
        /////// </summary>
        /////// <param name="context"></param>
        /////// <param name="query"></param>
        ////private void MontarQueryFiltros(ref HttpContext context, ref string query)
        ////{
        ////    string _cd_unidade = context.Request["cd_unidade"];
        ////    string _arquivados = context.Request["arquivados"];
        ////    string _bbusca = context.Request["bbusca"];
        ////    if (!string.IsNullOrEmpty(_cd_unidade))
        ////    {
        ////        var nm_campo = "";
        ////        var cd_unidades = new List<string>();
        ////        cd_unidades.Add(_cd_unidade);
        ////        var result_orgaos = new Manutencao.RN.OrgaoInternoRN().Consultar(new neo.BRLightREST.Pesquisa { limit = null, select = new[] { "cd_unidade" }, literal = "cd_un_numeradora='" + _cd_unidade + "'" });
        ////        foreach (var orgao in result_orgaos.results)
        ////        {
        ////            cd_unidades.Add(orgao.cd_unidade);
        ////        }
        ////        var vBuscaAux = "";
        ////        foreach (var cd_unidade in cd_unidades)
        ////        {
        ////            vBuscaAux += (vBuscaAux != "" ? " OR " : "") + cd_unidade;
        ////        }
        ////        if (vBuscaAux != "")
        ////        {
        ////            switch (_bbusca)
        ////            {
        ////                case "expedido":
        ////                    nm_campo = "id_orgao";
        ////                    break;
        ////                case "recebido":
        ////                    nm_campo = "cd_un_user_inclusao";
        ////                    break;
        ////                case "docs_pro":
        ////                    nm_campo = "ax_andamento_destino_id_orgao";
        ////                    break;
        ////            }
        ////            query += string.Format(" AND {0}:({1})", nm_campo, vBuscaAux);
        ////        }
        ////    }
        ////    if (_arquivados == "1")
        ////    {
        ////        if (_bbusca == "docs_pro")
        ////        {
        ////            query += " AND nm_st_documento:(Arquivado)";
        ////        }
        ////    }
        ////}

        /////// <summary>
        /////// Inserir na consulta os ids dos Órgaõs do usuário com impulsionador igual a 5 para o rankeamento por órgão
        /////// </summary>
        /////// <param name="context"></param>
        /////// <returns></returns>
        ////private void MontarQueryRankemanto(ref HttpContext context, ref string fields, ref string query, ref SessaoUsuarioOV sessao_usuario_ov)
        ////{
        ////    string _bbusca = context.Request["bbusca"];
        ////    string _sBoost = context.Request["boost"];
        ////    if (!string.IsNullOrEmpty(_sBoost))
        ////    {
        ////        if (_sBoost == "unidade_usuario")
        ////        {
        ////            var query_rankemanto = "";
        ////            foreach (var orgao in sessao_usuario_ov.orgaos)
        ////            {
        ////                query_rankemanto += (query_rankemanto != "" ? " OR " : "") + "(" + query + " " + orgao.id_gr_orgao + ")";
        ////            }
        ////            if (!string.IsNullOrEmpty(query_rankemanto))
        ////            {
        ////                switch (_bbusca)
        ////                {
        ////                    case "expedido":
        ////                        fields += ",\"ax_id_orgao^9\"";
        ////                        break;
        ////                    case "recebido":
        ////                        fields += ",\"cd_un_user_inclusao^9\"";
        ////                        break;
        ////                    case "docs_pro":
        ////                        fields += ",\"ax_andamento_destino_id_orgao^9\"";
        ////                        break;
        ////                }

        ////                query = "((" + query + ") OR " + query_rankemanto + ")";
        ////            }
        ////        }
        ////    }
        ////}

        ///// <summary>
        ///// Campos nos quais será feita a pesquisa textual
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string MontarCamposPesquisados(ref HttpContext context, string _bbusca)
        //{
        //    var fields = "";
        //    switch (_bbusca)
        //    {
        //        case "sinj_diario":
        //            fields = "\"nm_tipo_fonte^2\",\"nr_diario_text^2\",\"dt_assinatura_text^2\",\"filetext\"";
        //            break;
        //        case "sinj_norma":
        //            fields = "\"nm_tipo_norma^2\",\"nr_norma_text^2\",\"dt_assinatura_text^2\",\"dt_autuacao_text\",\"rankeamentos^3\",\"nm_situacao\",\"nm_ambito\",\"ds_apelido^2\",\"ds_ementa\",\"nm_orgao^2\",\"sg_orgao^2\",\"filetext\",\"nm_termo\"";
        //            break;
        //    }
        //    return fields;
        //}

        ///// <summary>
        ///// Campos que retornarão do elasticsearch
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string MontarPartialFields(ref HttpContext context, string _bbusca)
        //{
        //    var partial_fields = "";
        //    switch (_bbusca)
        //    {
        //        case "sinj_diario":
        //            partial_fields = ", \"partial_fields\":{\"partial\":{\"include\":[\"_metadata.id_doc\",\"nm_tipo_fonte\",\"nr_diario\",\"secao_diario\",\"dt_assinatura\",\"st_pendente\",\"ar_diario.id_file\"]}}";
        //            break;
        //        case "sinj_norma":
        //            partial_fields = ", \"partial_fields\":{\"partial\":{\"include\":[\"_metadata.id_doc\",\"origens.sg_orgao\",\"origens.ch_orgao\",\"ar_atualizado.id_file\",\"fontes.ar_fonte.id_file\",\"nm_tipo_norma\",\"nr_norma\",\"ch_norma\",\"dt_assinatura\",\"ds_ementa\",\"nm_situacao\"]}}";
        //            break;
        //        case "cesta":
        //            var _base = context.Request["b"];
        //            partial_fields = _base == "norma" ? ", \"partial_fields\":{\"partial\":{\"include\":[\"_metadata.id_doc\",\"origens.sg_orgao\",\"origens.ch_orgao\",\"ar_atualizado.id_file\",\"fontes.ar_fonte.id_file\",\"nm_tipo_norma\",\"nr_norma\",\"dt_assinatura\",\"ds_ementa\",\"nm_situacao\"]}}" :  _base == "diario" ? ", \"partial_fields\":{\"partial\":{\"include\":[\"_metadata.id_doc\",\"nm_tipo_fonte\",\"nr_diario\",\"secao_diario\",\"dt_assinatura\",\"st_pendente\",\"ar_diario.id_file\"]}}" : "";
        //            break;
        //    }
        //    return partial_fields;
        //}

        ///// <summary>
        ///// Campos que retornarão do elasticsearch
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string MontarHighlight(ref HttpContext context, string _bbusca)
        //{
        //    var highlight = "";
        //    switch (_bbusca)
        //    {
        //        case "sinj_diario":
        //            highlight = "\"nm_tipo_fonte\":{\"force_source\":true,\"number_of_fragments\":0},\"nr_diario_text\":{\"force_source\":true,\"number_of_fragments\":0},\"dt_assinatura_text\":{\"force_source\":true,\"number_of_fragments\":0}";
        //            break;
        //        case "sinj_norma":
        //            highlight = "\"nm_tipo_norma\":{\"force_source\":true,\"number_of_fragments\":0},\"nr_norma_text\":{\"force_source\":true,\"number_of_fragments\":0},\"dt_assinatura_text\":{\"force_source\":true,\"number_of_fragments\":0},\"dt_autuacao_text\":{\"force_source\":true,\"number_of_fragments\":0},\"nm_situacao\":{\"force_source\":true,\"number_of_fragments\":0},\"nm_ambito\":{\"force_source\":true,\"number_of_fragments\":0},\"ds_apelido\":{\"force_source\":true,\"number_of_fragments\":0},\"ds_ementa\":{\"force_source\":true,\"number_of_fragments\":0},\"nm_orgao\":{\"force_source\":true,\"number_of_fragments\":0},\"sg_orgao\":{\"force_source\":true,\"number_of_fragments\":0},\"ar_atualizado.filetext\":{\"force_source\":true,\"number_of_fragments\":0},\"ar_fonte.filetext\":{\"force_source\":true,\"number_of_fragments\":0},\"nm_termo\":{\"force_source\":true,\"number_of_fragments\":0}";
        //            break;
        //    }
        //    return highlight;
        //}

        ///// <summary>
        ///// Monta a url de consulta para pesquisa ou total
        ///// </summary>
        ///// <param name="context"></param>
        ///// <param name="_bbusca"></param>
        ///// <returns></returns>
        //private string MontarUrl(ref HttpContext context, string _bbusca)
        //{
        //    string _exibir_total = context.Request["exibir_total"];
        //    var _iDisplayLength = context.Request["iDisplayLength"];
        //    var _iDisplayStart = context.Request["iDisplayStart"];
        //    if (_bbusca == "cesta")
        //    {
        //        var _base = context.Request["b"];
        //        _bbusca = _base;
        //    }
        //    var url_es = new PortalRN().GetUrlEs(_bbusca);
        //    if (_exibir_total == "1")
        //    {
        //        url_es += "/_count";
        //    }
        //    else
        //    {
        //        url_es += "/_search";
        //        if (!string.IsNullOrEmpty(_iDisplayLength) && _iDisplayLength != "-1")
        //        {
        //            url_es += string.Format("?from={0}&size={1}", _iDisplayStart, _iDisplayLength);
        //        }
        //    }
        //    return url_es;
        //}

        ///// <summary>
        ///// Monta o ordenamento
        ///// </summary>
        ///// <param name="context"></param>
        ///// <returns></returns>
        //private string MontarOrdenamento(ref HttpContext context, string _bbusca)
        //{
        //    var sOrder = "";
        //    string _exibir_total = context.Request["exibir_total"];

        //    if (_exibir_total != "1")
        //    {
        //        var _sSortCol = context.Request["iSortCol_0"];
        //        var iSortCol = 0;
        //        var _sSortDir = "";
        //        var _sColOrder = "";
        //        _sSortCol = context.Request["iSortCol_0"];
        //        if (!string.IsNullOrEmpty(_sSortCol))
        //        {
        //            int.TryParse(_sSortCol, out iSortCol);
        //            _sSortDir = context.Request["sSortDir_0"];
        //            _sColOrder = context.Request["mDataProp_" + iSortCol];
        //        }
        //        //Quando relatorio esse parametro pode ser passado, o que define o ordemaneto da pesquisa, então  atribuo seus valores à _sColOrder e iSortDir
        //        var _order = context.Request["order"];
        //        if (!string.IsNullOrEmpty(_order))
        //        {
        //            var order_split = _order.Split(',');
        //            if (order_split.Length == 2)
        //            {
        //                _sColOrder = order_split[0];
        //                _sSortDir = order_split[1];
        //            }
        //        }
        //        sOrder = ",\"sort\":[";
        //        //Se não tiver _sSortCol então não foi selecionado nenhum ordenamento, log devemos ordenar por dt_doc que é o ordenamento padrão
        //        if (string.IsNullOrEmpty(_sColOrder))
        //        {
        //            sOrder += "\"_score\",{\"dt_assinatura_untouched\":{\"order\":\"desc\"}}";
        //        }
        //        else
        //        {
        //            if(_bbusca == "sinj_diario"){
        //                if (_sColOrder == "dt_assinatura")
        //                {
        //                    _sColOrder = "dt_assinatura_untouched";
        //                }
        //                else if (_sColOrder == "nr_diario")
        //                {
        //                    _sColOrder = "nr_diario_untouched";
        //                }
        //                if (("desc" == _sSortDir))
        //                {

        //                    sOrder += "{\"" + _sColOrder + "\":{\"order\":\"desc\"}}";
        //                }
        //                else
        //                {
        //                    sOrder += "{\"" + _sColOrder + "\":{\"order\":\"asc\"}}";
        //                }
        //            }
        //            else if (_bbusca == "sinj_norma")
        //            {
        //                if (_sColOrder == "dt_assinatura")
        //                {
        //                    _sColOrder = "dt_assinatura_untouched";
        //                }

        //                if (("desc" == _sSortDir))
        //                {
        //                    sOrder += "{\"" + _sColOrder + "\":{\"order\":\"desc\"}}";
        //                    if (_sColOrder == "nm_tipo_norma")
        //                    {
        //                        sOrder += ",{\"nr_norma_untouched\":{\"order\":\"desc\"}}";
        //                    }
        //                }
        //                else
        //                {
        //                    sOrder += "{\"" + _sColOrder + "\":{\"order\":\"asc\"}}";
        //                    if (_sColOrder == "nm_tipo_norma")
        //                    {
        //                        sOrder += ",{\"nr_norma_untouched\":{\"order\":\"asc\"}}";
        //                    }
        //                }
        //            }
        //            else if(_bbusca == "cesta"){
        //                if (_sColOrder == "dt_assinatura")
        //                {
        //                    _sColOrder = "dt_assinatura_untouched";
        //                }
        //                else if (_sColOrder == "nr_diario")
        //                {
        //                    _sColOrder = "nr_diario_untouched";
        //                }
        //                if (("desc" == _sSortDir))
        //                {
        //                    sOrder += "{\"" + _sColOrder + "\":{\"order\":\"desc\"}}";
        //                    if (_sColOrder == "nm_tipo_norma")
        //                    {
        //                        sOrder += ",{\"nr_norma_untouched\":{\"order\":\"desc\"}}";
        //                    }
        //                }
        //                else
        //                {
        //                    sOrder += "{\"" + _sColOrder + "\":{\"order\":\"asc\"}}";
        //                    if (_sColOrder == "nm_tipo_norma")
        //                    {
        //                        sOrder += ",{\"nr_norma_untouched\":{\"order\":\"asc\"}}";
        //                    }
        //                }
        //            }
        //        }
        //        sOrder += "]";
        //    }
        //    return sOrder;
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.ES;
using TCDF.Sinj.RN;
using util.BRLight;

namespace TCDF.Sinj.ES
{
    public class ESAd
    {
        private DocEs _docEs;

        public ESAd()
        {
            _docEs = new DocEs();
        }

        public string PesquisarTotal(HttpContext context, SessaoUsuarioOV sessao_usuario_ov, string _bbusca)
        {
            string url_es = "";
            string query = "";
            try
            {
                url_es = MontarUrl(context, _bbusca);
                query = MontarConsulta(context, sessao_usuario_ov, _bbusca);
                return _docEs.CountEs(query, url_es);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar Total. url_es: " + url_es + ". query: " + query, ex);
            }
        }

        public Result<T> PesquisarDocs<T>(string query, string url_es)
        {
            try
            {
                return _docEs.Pesquisar<T>(query, url_es);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar Docs. url_es: " + url_es + ". query: " + query, ex);
            }
        }

        public Result<T> PesquisarDocs<T>(HttpContext context, SessaoUsuarioOV sessao_usuario_ov, string _bbusca)
        {
            string url_es = "";
            string query = "";
            try
            {
                url_es = MontarUrl(context, _bbusca);
                query = MontarConsulta(context, sessao_usuario_ov, _bbusca);
                return _docEs.Pesquisar<T>(query, url_es);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar Docs. url_es: " + url_es + ". query: " + query, ex);
            }
        }

        public List<Resultados<T>> RelatorioDocs<T>(HttpContext context, SessaoUsuarioOV sessao_usuario_ov, string _bbusca)
        {
            var resultados = new List<Resultados<T>>();
            string url_es = "";
            string query = "";
            try
            {

                ulong limit_max = 1000;
                url_es = MontarUrl(context, _bbusca).Split('?')[0];
                query = MontarConsulta(context, sessao_usuario_ov, _bbusca);
                var slimit = Config.ValorChave("LimiteRelatorio");
                if(slimit != "-1"){
                    ulong.TryParse(slimit, out limit_max);
                }
                ulong from = 0;
                ulong size = 200;
                ulong total = 0;
                while (from <= total && from < limit_max)
                {
                    var result = _docEs.Pesquisar<T>(query, url_es + "?from=" + from + "&size=" + size);
                    total = result.hits.total;
                    from += size;
                    resultados.AddRange(result.hits.hits);
                }
                return resultados;

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao Pesquisar Docs para Relatório. url_es: " + url_es + ". query: " + query, ex);
            }
        }
        public string IncluirDoc(string json_doc, string uri)
        {
            return _docEs.IncluirDoc(json_doc, uri);
        }
        public string AtualizarDoc(string json_doc, string uri)
        {
            return _docEs.AtualizarDoc(json_doc, uri);
        }
        public string DeletarDoc(string json_doc, string uri)
        {
            return _docEs.DeletarDoc(json_doc, uri);
        }

        /// <summary>
        /// Consulta no elasticsearch
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sessao_usuario_ov"></param>
        /// <returns></returns>
        private string MontarConsulta(HttpContext context, SessaoUsuarioOV sessao_usuario_ov, string _bbusca)
        {
            string _tipo_pesquisa = context.Request["tipo_pesquisa"];
            string _sSearch = context.Request["sSearch"];
            string _sFiltrar = context.Request["filtrar"];
            string _all = context.Request["all"];
            string _exibir_total = context.Request["exibir_total"];

            var sOrder = MontarOrdenamento(context, _bbusca);

            //Os campos que retornarão do ElasticSearch
            var partial_fields = "";
            if (_exibir_total != "1")
            {
                partial_fields = MontarPartialFields(context, _bbusca);
            }

            //Os campos onde a consulta textual será feita
            var fields = "";

            var fields_highlight = "";
            var highlight = "";

            var query = "";
            var query_textual = "";
            var ids = "";
            if (_tipo_pesquisa == "geral")
            {
                fields = MontarCamposPesquisados(context, _bbusca);
                if (_exibir_total != "1")
                {
                    fields_highlight = MontarHighlight(context, _bbusca);
                }
                if (!string.IsNullOrEmpty(_all))
                {
                    //Escapar os caracteres reservados antes de montar a query
                    query_textual = _docEs.TratarCaracteresReservadosDoEs(_all);

                    //Insere na consulta o filtro digitado no campo search do datatable
                    if (!string.IsNullOrEmpty(_sSearch))
                    {
                        //Remove a acentuação porque a consulta com asterisco não funciona com acentuação
                        util.BRLight.ManipulaTexto.RemoverAcentuacao(ref _sSearch);
                        query_textual += " AND _all:(" + _docEs.TratarCaracteresReservadosDoEs(_sSearch) + "*)";
                    }

                    //Aplicar filtro por unidade
                    //MontarQueryFiltros(context, ref query_textual);

                }
            }
            else if (_tipo_pesquisa == "norma")
            {
                fields = MontarCamposPesquisados(context, _bbusca);
                var _ch_tipo_norma = context.Request["ch_tipo_norma"];
                var _nr_norma = context.Request["nr_norma"];
                var _norma_sem_numero = context.Request["norma_sem_numero"];
                var _ano_assinatura = context.Request["ano_assinatura"];
                var _ch_orgao = context.Request["ch_orgao"];
                var _ch_termo = context.Request.Form.GetValues("ch_termo");
                if (!string.IsNullOrEmpty(_ch_tipo_norma))
                {
                    query_textual += (query_textual != "" ? " AND " : "") + "(ch_tipo_norma:\\\"" + _ch_tipo_norma + "\\\")";
                }
                if (!string.IsNullOrEmpty(_ano_assinatura))
                {
                    query_textual += (query_textual != "" ? " AND " : "") + "(dt_assinatura:[01/01/" + _ano_assinatura + " TO 31/12/" + _ano_assinatura + "])";
                }
                if (_ch_termo != null && _ch_termo.Length > 0)
                {
                    query_textual += (query_textual != "" ? " AND " : "") + "(ch_termo:(\\\"" + string.Join("\\\" OR \\\"", _ch_termo) + "\\\"))";
                }
                if (!string.IsNullOrEmpty(_ch_orgao))
                {
                    var _origem_por = context.Request["origem_por"];
                    var chaves = "";
                    if (string.IsNullOrEmpty(_origem_por))
                    {
                        _origem_por = "toda_a_hierarquia_em_qualquer_epoca";
                    }
                    var orgaos_hierarquia = new List<OrgaoOV>();
                    var orgaos_cronologia = new List<OrgaoOV>();
                    var orgaos_superiores = new List<OrgaoOV>();
                    var orgaos_inferiores = new List<OrgaoOV>();
                    var _ch_hierarquia = context.Request["ch_hierarquia"];
                    switch (_origem_por)
                    {
                        case "toda_a_hierarquia_em_qualquer_epoca":
                            orgaos_hierarquia = new OrgaoRN().BuscarOrgaosDaHierarquia(_ch_orgao, _ch_hierarquia);
                            orgaos_cronologia = new OrgaoRN().BuscarOrgaosDaCronologia(_ch_orgao);
                            foreach (var orgao in orgaos_hierarquia)
                            {
                                chaves += (!string.IsNullOrEmpty(chaves) ? " OR " : "") + orgao.ch_orgao;
                            }
                            foreach (var orgao in orgaos_cronologia)
                            {
                                chaves += (!string.IsNullOrEmpty(chaves) ? " OR " : "") + orgao.ch_orgao;
                            }
                            break;
                        case "hierarquia_superior":
                            orgaos_superiores = new OrgaoRN().BuscarHierarquiaSuperior(_ch_hierarquia);
                            foreach (var orgao in orgaos_superiores)
                            {
                                chaves += (!string.IsNullOrEmpty(chaves) ? " OR " : "") + orgao.ch_orgao;
                            }
                            break;
                        case "hierarquia_inferior":
                            orgaos_inferiores = new OrgaoRN().BuscarHierarquiaInferior(_ch_orgao);
                            foreach (var orgao in orgaos_inferiores)
                            {
                                chaves += (!string.IsNullOrEmpty(chaves) ? " OR " : "") + orgao.ch_orgao;
                            }
                            break;
                        case "toda_a_hierarquia":
                            orgaos_hierarquia = new OrgaoRN().BuscarOrgaosDaHierarquia(_ch_orgao, _ch_hierarquia);
                            foreach (var orgao in orgaos_hierarquia)
                            {
                                chaves += (!string.IsNullOrEmpty(chaves) ? " OR " : "") + orgao.ch_orgao;
                            }
                            break;
                        case "em_qualquer_epoca":
                            orgaos_cronologia = new OrgaoRN().BuscarOrgaosDaCronologia(_ch_orgao);
                            if (orgaos_cronologia.Count > 0)
                            {
                                foreach (var orgao in orgaos_cronologia)
                                {
                                    chaves += (!string.IsNullOrEmpty(chaves) ? " OR " : "") + orgao.ch_orgao;
                                }
                            }
                            else
                            {
                                chaves = _ch_orgao;
                            }
                            break;
                        default:
                            chaves = _ch_orgao;
                            break;
                    }
                    query_textual += (query_textual != "" ? " AND " : "") + "(ch_orgao:(" + chaves + "))";
                }
                if (!string.IsNullOrEmpty(_all))
                {
                    var fields_split = fields.Replace("\"", "").Split(',');
                    var query_all = "";
                    for (var i = 0; i < fields_split.Length; i++)
                    {
                        var rankeamento = "";
                        if (fields_split[i].IndexOf('^') > -1)
                        {
                            rankeamento = fields_split[i].Substring(fields_split[i].IndexOf('^'));
                            fields_split[i] = fields_split[i].Substring(0, fields_split[i].IndexOf('^'));
                        }
                        query_all += (query_all != "" ? " OR " : "") + "(" + fields_split[i] + ":" + _docEs.TratarCaracteresReservadosDoEsPesquisaAvancada(_all) + ")" + rankeamento;
                    }
                    if (query_all != "")
                    {
                        query_textual += (query_textual != "" ? "AND (" + query_all + ")" : query_all);
                        //query_textual += " AND (" + query_all + ")";
                    }
                    if (_exibir_total != "1")
                    {
                        fields_highlight += (fields_highlight != "" ? "," : "") + MontarHighlight(context, _bbusca);
                    }
                }
                if (!string.IsNullOrEmpty(_nr_norma))
                {
                    query_textual += (query_textual != "" ? " AND " : "") + "(nr_norma:" + _nr_norma + ")";
                }
                else if (!string.IsNullOrEmpty(_norma_sem_numero))
                {
                    query_textual += (query_textual != "" ? " AND " : "") + "(nr_norma:'0')";
                }
                if (!string.IsNullOrEmpty(_sSearch))
                {
                    query_textual += (query_textual != "" ? " AND " : "") + "(_all:" + _docEs.TratarCaracteresReservadosDoEsPesquisaAvancada(_sSearch) + "*)";
                }
            }
            else if (_tipo_pesquisa == "diario")
            {
                fields = MontarCamposPesquisados(context, _bbusca);
                var _ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                var _nr_diario = context.Request["nr_diario"];
                var _secao_diario = context.Request["secao_diario"];
                var _filetext = context.Request["filetext"];
                var _op_dt_assinatura = context.Request["op_dt_assinatura"];
                var _dt_assinatura = context.Request.Form.GetValues("dt_assinatura");
                if (!string.IsNullOrEmpty(_nr_diario))
                {
                    query_textual += (query_textual != "" ? " AND " : "") + "(nr_diario:" + _nr_diario + ")";
                }
                if (!string.IsNullOrEmpty(_ch_tipo_fonte))
                {
                    query_textual += (query_textual != "" ? " AND " : "") + "(ch_tipo_fonte:" + _ch_tipo_fonte + ")";
                }
                if (_dt_assinatura != null && _dt_assinatura.Length > 0 && !string.IsNullOrEmpty(_dt_assinatura[0]))
                {
                    query_textual += (query_textual != "" ? " AND " : "") + _docEs.MontarArgumentoRangeQueryString("dt_assinatura", _op_dt_assinatura, string.Join(",", _dt_assinatura));
                }
                if (!string.IsNullOrEmpty(_sSearch))
                {
                    query_textual += (query_textual != "" ? " AND " : "") + "(_all:" + _docEs.TratarCaracteresReservadosDoEsPesquisaAvancada(_sSearch) + "*)";
                }
                if (!string.IsNullOrEmpty(_secao_diario))
                {
                    query_textual += (query_textual != "" ? " AND " : "") + "(secao_diario:" + _docEs.TratarCaracteresReservadosDoEs(_secao_diario) + ")";
                    fields_highlight += (fields_highlight != "" ? "," : "") + "\"secao_diario\":{\"number_of_fragments\":20,\"fragment_size\":1}";

                }
                if (!string.IsNullOrEmpty(_filetext))
                {
                    query_textual += (query_textual != "" ? " AND " : "") + "(ar_diario.filetext:" + _docEs.TratarCaracteresReservadosDoEs(_filetext) + ")";
                    if (highlight.IndexOf("ar_diario.filetext") == -1)
                    {
                        fields_highlight += (fields_highlight != "" ? "," : "") + "\"ar_diario.filetext\":{\"number_of_fragments\":20,\"fragment_size\":1}";
                    }
                }
            }
            else if (_tipo_pesquisa == "cesta")
            {
                var _cesta = context.Request["cesta"];
                var _base = context.Request["b"];
                var aCesta = new string[0];
                if (!string.IsNullOrEmpty(_cesta))
                {
                    aCesta = _cesta.Split(',');
                }
                foreach (var sCesta in aCesta)
                {
                    var sCesta_split = sCesta.Split('_');
                    if (sCesta_split.Length > 2)
                    {
                        for (var i = 1; i < sCesta_split.Length - 1; i++)
                        {
                            sCesta_split[0] += "_" + sCesta_split[i];
                        }
                    }
                    if (sCesta_split[0] == _base)
                    {
                        ids += (ids != "" ? "," : "") + sCesta_split.Last<string>();
                    }
                }
            }
            else if (_tipo_pesquisa == "favoritos")
            {
                var notifiquemeRn = new NotifiquemeRN();
                var sessaoNotifiquemeOv = notifiquemeRn.LerSessaoNotifiquemeOv();
                var notifiquemeOv = notifiquemeRn.Doc(sessaoNotifiquemeOv.email_usuario_push);
                var _base = context.Request["b"];
                string chaves = "";
                foreach (var favorito in notifiquemeOv.favoritos)
                {
                    var favorito_splited = favorito.Split('_');
                    if (favorito_splited[0] == _base)
                    {
                        chaves += (chaves != "" ? " OR " : "") + favorito_splited[1];
                    }
                }
                if (chaves != "")
                {
                    query_textual = "ch_norma:(" + chaves + ")";
                }
                else
                {
                    throw new Exception("Nenhum Favorito para pesquisar.");
                }
            }
            else if (_tipo_pesquisa == "avancada")
            {
                fields = MontarCamposPesquisados(context, _bbusca);
                var _argumento = context.Request.Form.GetValues("argumento");
                if (_argumento != null)
                {
                    var b_all_fields_highlight = false;
                    var argumento_split = new string[0];
                    var _tipo_campo = "";
                    var _ch_campo = "";
                    var _ch_operador = "";
                    var _ch_valor = "";
                    var _conector = "";

                    //guardam o conector do último argumento para concatenar com o proximo
                    var aux_conector_es = "";
                    foreach (var argumento in _argumento)
                    {
                        argumento_split = argumento.Split('#');
                        if (argumento_split.Length == 8)
                        {
                            _tipo_campo = argumento_split[0];
                            _ch_campo = argumento_split[1];
                            _ch_operador = argumento_split[3];
                            _ch_valor = argumento_split[5];
                            if (_tipo_campo == "autocomplete")
                            {
                                if (_ch_operador == "diferente")
                                {
                                    query_textual += (query_textual != "" ? aux_conector_es : "") + "(" + _ch_campo + ":(NOT \\\"" + _ch_valor + "\\\"))";
                                }
                                else
                                {
                                    query_textual += (query_textual != "" ? aux_conector_es : "") + "(" + _ch_campo + ":\\\"" + _ch_valor + "\\\")";
                                }
                                if (_exibir_total != "1")
                                {
                                    fields_highlight += (fields_highlight != "" ? "," : "") + "\"" + _ch_campo + "\":{\"number_of_fragments\":20,\"fragment_size\":1}";
                                }
                                //filtering = "{\"query\":{\"query_string\":{\"query\":\"" + _ch_campo + ":\\\"" + _ch_valor + "\\\"\"}}}";
                            }
                            else if (_tipo_campo == "text")
                            {
                                var conector_textual = _ch_operador == "contem" ? " OR " : " ";
                                if (_ch_campo == "all")
                                {
                                    var fields_split = fields.Replace("\"", "").Split(',');
                                    if (_ch_operador == "diferente")
                                    {
                                        for (var i = 0; i < fields_split.Length; i++)
                                        {
                                            if (fields_split[i].IndexOf('^') > -1)
                                            {
                                                fields_split[i] = fields_split[i].Substring(0, fields_split[i].IndexOf('^'));
                                            }
                                            query_textual += (i > 0 ? " OR " : (query_textual != "" ? aux_conector_es : "")) + "(" + fields_split[i] + ":(NOT " + _docEs.TratarCaracteresReservadosDoEsPesquisaAvancada(_ch_valor).Replace(" ", conector_textual) + "))";
                                        }
                                    }
                                    else
                                    {
                                        for (var i = 0; i < fields_split.Length; i++)
                                        {
                                            var rankeamento = "";
                                            if (fields_split[i].IndexOf('^') > -1)
                                            {
                                                rankeamento = fields_split[i].Substring(fields_split[i].IndexOf('^'));
                                                fields_split[i] = fields_split[i].Substring(0, fields_split[i].IndexOf('^'));
                                            }
                                            query_textual += (i > 0 ? " OR " : (query_textual != "" ? aux_conector_es : "")) + "(" + fields_split[i] + ":" + _docEs.TratarCaracteresReservadosDoEsPesquisaAvancada(_ch_valor).Replace(" ", conector_textual) + ")" + rankeamento;
                                        }
                                    }
                                    if (_exibir_total != "1")
                                    {
                                        if (!b_all_fields_highlight)
                                        {
                                            fields_highlight += (fields_highlight != "" ? "," : "") + MontarHighlight(context, _bbusca);
                                            b_all_fields_highlight = true;
                                        }
                                        else if (highlight.IndexOf(_ch_campo) == -1)
                                        {
                                            if (_ch_campo.IndexOf("filetext") > -1)
                                            {
                                                fields_highlight += (fields_highlight != "" ? "," : "") + "\"" + _ch_campo + "\":{\"number_of_fragments\":20, \"fragment_size\":1}";
                                            }
                                            fields_highlight += (fields_highlight != "" ? "," : "") + "\"" + _ch_campo + "\":{\"number_of_fragments\":20,\"fragment_size\":1}";
                                        }
                                    }
                                }
                                else
                                {
                                    if (_ch_operador == "diferente")
                                    {
                                        query_textual += (query_textual != "" ? aux_conector_es : "") + "(" + _ch_campo + ":(NOT " + _docEs.TratarCaracteresReservadosDoEsPesquisaAvancada(_ch_valor).Replace(" ", conector_textual) + "))";
                                    }
                                    else
                                    {
                                        query_textual += (query_textual != "" ? aux_conector_es : "") + "(" + _ch_campo + ":" + _docEs.TratarCaracteresReservadosDoEsPesquisaAvancada(_ch_valor).Replace(" ", conector_textual) + ")";
                                    }
                                    if (_exibir_total != "1")
                                    {
                                        fields_highlight += (fields_highlight != "" ? "," : "") + "\"" + _ch_campo + "\":{\"number_of_fragments\":20,\"fragment_size\":1}";
                                    }
                                }
                            }
                            else if (_tipo_campo == "number" || _tipo_campo == "date")
                            {
                                if(_bbusca == "sinj_norma"){
                                    if (_ch_campo == "ano_assinatura")
                                    {
                                        var ch_operador_aux = _ch_operador;
                                        _ch_campo = "dt_assinatura";
                                        if (_ch_operador == "intervalo")
                                        {
                                            var ch_valor = _ch_valor.Split(',');
                                            if (ch_valor.Length == 2)
                                            {
                                                _ch_valor = "01/01/" + ch_valor[0] + "," + "31/12/" + ch_valor[1];
                                            }
                                        }
                                        else
                                        {
                                            _ch_operador = "intervalo";
                                            _ch_valor = "01/01/" + _ch_valor + "," + "31/12/" + _ch_valor;
                                        }
                                        if (ch_operador_aux == "diferente")
                                        {
                                            query_textual += (query_textual != "" ? aux_conector_es : "") + "(\"NOT\":" + _docEs.MontarArgumentoRangeQueryString(_ch_campo, _ch_operador, _ch_valor) + ")";
                                        }
                                        else
                                        {
                                            query_textual += (query_textual != "" ? aux_conector_es : "") + _docEs.MontarArgumentoRangeQueryString(_ch_campo, _ch_operador, _ch_valor);
                                        }
                                    }
                                    else
                                    {
                                        query_textual += (query_textual != "" ? aux_conector_es : "") + _docEs.MontarArgumentoRangeQueryString(_ch_campo, _ch_operador, _ch_valor);
                                    }
                                }
                            }
                            else if (_tipo_campo == "datetime")
                            {
                                _ch_operador = _ch_operador.Replace("_", "");
                                var ch_valor_splited = _ch_valor.Split(',');
                                if (ch_valor_splited.Length == 2)
                                {
                                    if (ch_valor_splited[0].IndexOf(' ') < 0 && ch_valor_splited[1].IndexOf(' ') < 0)
                                    {
                                        ch_valor_splited[0] = "\\\"" + ch_valor_splited[0] + " 00:00:00\\\"";
                                        ch_valor_splited[1] = "\\\"" + ch_valor_splited[1] + " 23:59:59\\\"";
                                    }
                                }
                                else if (_ch_operador == "diferente")
                                {
                                    if (ch_valor_splited[0].IndexOf(' ') < 0)
                                    {
                                        ch_valor_splited = new string[] { "\\\"" + ch_valor_splited[0] + " 00:00:00\\\"", "\\\"" + ch_valor_splited[0] + " 23:59:59\\\"" };
                                        _ch_operador = "intervalo";
                                    }
                                    query_textual += (query_textual != "" ? aux_conector_es : "") + "(\"NOT\":" + _docEs.MontarArgumentoRangeQueryString(_ch_campo, _ch_operador, _ch_valor) + ")";
                                }
                                else
                                {
                                    if (ch_valor_splited[0].IndexOf(' ') < 0)
                                    {
                                        if (_ch_operador == "igual" || _ch_operador == "menor" || _ch_operador == "maiorouigual")
                                        {
                                            ch_valor_splited[0] = "\\\"" + ch_valor_splited[0] + " 00:00:00\\\"";
                                        }
                                        else if (_ch_operador == "maior" || _ch_operador == "menorouigual")
                                        {
                                            ch_valor_splited[0] = "\\\"" + ch_valor_splited[0] + " 23:59:59\\\"";
                                        }
                                    }
                                    var aux_valor_splited = ch_valor_splited[0].Split(' ');
                                    if (aux_valor_splited.Length == 2 && (aux_valor_splited[1] == "00:00:00" || aux_valor_splited[1] == "00:00:00\\\"") && _ch_operador == "igual")
                                    {
                                        ch_valor_splited = new string[] { "\\\"" + aux_valor_splited[0] + " 00:00:00\\\"", "\\\"" + aux_valor_splited[0] + " 23:59:59\\\"" };
                                        _ch_operador = "intervalo";
                                    }
                                }
                                _ch_valor = String.Join(",", ch_valor_splited);
                                query_textual += (query_textual != "" ? aux_conector_es : "") + _docEs.MontarArgumentoRangeQueryString(_ch_campo, _ch_operador, _ch_valor);
                            }
                            _conector = argumento_split[7];
                            //substitui o conector (E/OU) pelo que é usado no ES (AND/OR)
                            aux_conector_es = _conector == "E" ? " AND " : _conector == "OU" ? " OR " : _conector == "NÃO" ? " NOT " : " ";
                        }
                    }
                    //Insere na consulta o filtro digitado no campo search do datatable
                    if (!string.IsNullOrEmpty(_sSearch))
                    {
                        //Remove a acentuação porque a consulta com asterisco não funciona com acentuação
                        util.BRLight.ManipulaTexto.RemoverAcentuacao(ref _sSearch);
                        query_textual += (query_textual != "" ? " AND " : "") + "_all:(" + _docEs.TratarCaracteresReservadosDoEs(_sSearch) + "*)";
                    }
                    //if (fields_highlight != "")
                    //{
                    //    highlight = ",\"highlight\":{\"pre_tags\":[\"_pre_tag_highlight_\"],\"post_tags\":[\"_post_tag_highlight_\"],\"fields\":{" + fields_highlight + "}}";
                    //}
                    if (_bbusca == "sinj_norma")
                    {
                        var _ch_tipo_norma = context.Request["ch_tipo_norma"];
                        if (!string.IsNullOrEmpty(_ch_tipo_norma) && (_ch_tipo_norma != "todas"))
                        {
                            query_textual += (query_textual != "" ? " AND " : "") + "(ch_tipo_norma:\\\"" + _ch_tipo_norma + "\\\")";
                        }
                    }
                }
            }

            if (ids != "")
            {
                query = "{\"query\":{\"ids\":{\"values\":[" + ids + "]}}";
            }
            else
            {
                if (query_textual == "")
                {
                    query_textual = "*";
                }
                var query_filtro = MontarQueryFiltros(context);//O que esse cara pega do context???
                query = "{\"query\":{\"query_string\":{\"fields\":[" + fields + "],\"query\":\"" + query_textual 
                + (!string.IsNullOrEmpty(query_filtro) ? " AND (" + query_filtro + ")" : "") + "\", \"default_operator\":\"AND\"}}";

                if (fields_highlight != "" && _exibir_total != "1")
                {
                    highlight = ",\"highlight\":{\"pre_tags\":[\"\"],\"post_tags\":[\"\"],\"fields\":{" + fields_highlight + "}}";
                }
            }
            var bla = query + sOrder + partial_fields + highlight + "}";
            return query + sOrder + partial_fields + highlight + "}";
        }

        /// <summary>
        /// Campos nos quais será feita a pesquisa textual
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string MontarCamposPesquisados(HttpContext context, string _bbusca)
        {
            var fields = "";
            switch (_bbusca)
            {
                case "sinj_diario":
                    fields = "\"nm_tipo_fonte^2\",\"nr_diario_text^2\",\"dt_assinatura_text^2\",\"ar_diario.filetext\"";
                    break;
                case "sinj_norma":
                    fields = "\"nm_tipo_norma^2\",\"nr_norma^2\",\"dt_assinatura_text^2\",\"dt_autuacao_text\",\"rankeamentos^3\",\"nm_situacao\",\"nm_ambito\",\"nm_apelido^2\",\"ds_ementa\",\"nm_orgao^2\",\"sg_orgao^2\",\"ar_atualizado.filetext\",\"fontes.ar_fonte.filetext\",\"nm_termo\"";
                    break;
            }
            return fields;
        }

        /// <summary>
        /// Campos que retornarão do elasticsearch
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string MontarPartialFields(HttpContext context, string _bbusca)
        {
            var partial_fields = "";
            switch (_bbusca)
            {
                case "sinj_diario":
                    partial_fields = ", \"partial_fields\":{\"partial\":{\"include\":[\"_metadata.id_doc\",\"nm_tipo_fonte\",\"nr_diario\",\"cr_diario\",\"secao_diario\",\"dt_assinatura\",\"st_pendente\",\"ar_diario.id_file\",\"ar_diario.filesize\",\"dt_cadastro\",\"nm_login_usuario_cadastro\"]}}";
                    break;
                case "sinj_norma":
                    partial_fields = ", \"partial_fields\":{\"partial\":{\"include\":[\"_metadata.id_doc\",\"origens.sg_orgao\",\"origens.nm_orgao\",\"origens.ch_orgao\",\"ar_atualizado.id_file\",\"ar_atualizado.filesize\",\"ar_atualizado.mimetype\",\"ar_atualizado.filename\",\"nm_tipo_norma\",\"nr_norma\",\"ch_norma\",\"dt_assinatura\",\"ds_ementa\",\"nm_situacao\",\"vides\",\"nm_apelido\",\"ds_observacao\",\"autorias.nm_autoria\",\"nm_pessoa_fisica_e_juridica\",\"fontes\",\"indexacoes\",\"dt_cadastro\",\"nm_login_usuario_cadastro\"]}}";
                    break;
                case "cesta":
                    var _base = context.Request["b"];
                    partial_fields = _base == "sinj_norma" ? ", \"partial_fields\":{\"partial\":{\"include\":[\"_metadata.id_doc\",\"origens.sg_orgao\",\"origens.nm_orgao\",\"origens.ch_orgao\",\"ar_atualizado.id_file\",\"ar_atualizado.filesize\",\"ar_atualizado.mimetype\",\"nm_tipo_norma\",\"nr_norma\",\"dt_assinatura\",\"ds_ementa\",\"nm_situacao\",\"vides\",\"nm_apelido\",\"ds_observacao\",\"autorias.nm_autoria\",\"nm_pessoa_fisica_e_juridica\",\"fontes\",\"indexacoes\",\"dt_cadastro\",\"nm_login_usuario_cadastro\"]}}" : _base == "sinj_diario" ? ", \"partial_fields\":{\"partial\":{\"include\":[\"_metadata.id_doc\",\"nm_tipo_fonte\",\"nr_diario\",\"secao_diario\",\"dt_assinatura\",\"st_pendente\",\"ar_diario.id_file\",\"dt_cadastro\",\"nm_login_usuario_cadastro\"]}}" : "";
                    break;
            }
            return partial_fields;
        }

        /// <summary>
        /// Campos que retornarão do elasticsearch
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string MontarHighlight(HttpContext context, string _bbusca)
        {
            var highlight = "";
            switch (_bbusca)
            {
                case "sinj_diario":
                    highlight = "\"nm_tipo_fonte\":{\"number_of_fragments\":20,\"fragment_size\":1},\"nr_diario_text\":{\"number_of_fragments\":20,\"fragment_size\":1},\"dt_assinatura_text\":{\"number_of_fragments\":20,\"fragment_size\":1},\"ar_diario.filetext\":{\"number_of_fragments\":10, \"fragment_size\":100}";
                    break;
                case "sinj_norma":
                    highlight = "\"nm_tipo_norma\":{\"number_of_fragments\":20,\"fragment_size\":1},\"nr_norma\":{\"number_of_fragments\":20,\"fragment_size\":1},\"dt_assinatura_text\":{\"number_of_fragments\":20,\"fragment_size\":1},\"dt_autuacao_text\":{\"number_of_fragments\":20,\"fragment_size\":1},\"nm_situacao\":{\"number_of_fragments\":20,\"fragment_size\":1},\"nm_ambito\":{\"number_of_fragments\":20,\"fragment_size\":1},\"ds_apelido\":{\"number_of_fragments\":20,\"fragment_size\":1},\"ds_ementa\":{\"number_of_fragments\":20,\"fragment_size\":1},\"nm_orgao\":{\"number_of_fragments\":20,\"fragment_size\":1},\"sg_orgao\":{\"number_of_fragments\":20,\"fragment_size\":1},\"nm_termo\":{\"number_of_fragments\":20,\"fragment_size\":1},\"fontes.ar_fonte.filetext\":{\"number_of_fragments\":5, \"fragment_size\":60},\"ar_atualizado.filetext\":{\"number_of_fragments\":5, \"fragment_size\":60}";
                    break;
            }
            return highlight;
        }

        /// <summary>
        /// Monta a url de consulta para pesquisa ou total
        /// </summary>
        /// <param name="context"></param>
        /// <param name="_bbusca"></param>
        /// <returns></returns>
        public string MontarUrl(HttpContext context, string _bbusca)
        {
            string _exibir_total = context.Request["exibir_total"];
            var _iDisplayLength = context.Request["iDisplayLength"];
            var _iDisplayStart = context.Request["iDisplayStart"];
            if (_bbusca == "cesta")
            {
                var _base = context.Request["b"];
                _bbusca = _base;
            }
            var url_es = new DocEs().GetUrlEs(_bbusca);
            if (_exibir_total == "1")
            {
                url_es += "/_count";
            }
            else
            {
                url_es += "/_search";
                if (!string.IsNullOrEmpty(_iDisplayLength) && _iDisplayLength != "-1")
                {
                    url_es += string.Format("?from={0}&size={1}", _iDisplayStart, _iDisplayLength);
                }
            }
            return url_es;
        }

        /// <summary>
        /// Monta o ordenamento
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string MontarOrdenamento(HttpContext context, string _bbusca)
        {
            var sOrder = "";
            string _exibir_total = context.Request["exibir_total"];

            if (_exibir_total != "1")
            {
                var _sSortCol = context.Request["iSortCol_0"];
                var iSortCol = 0;
                var _sSortDir = "";
                var _sColOrder = "";
                _sSortCol = context.Request["iSortCol_0"];
                if (!string.IsNullOrEmpty(_sSortCol))
                {
                    int.TryParse(_sSortCol, out iSortCol);
                    _sSortDir = context.Request["sSortDir_0"];
                    _sColOrder = context.Request["mDataProp_" + iSortCol];
                }
                //Quando relatorio esse parametro pode ser passado, o que define o ordemaneto da pesquisa, então  atribuo seus valores à _sColOrder e iSortDir
                var _order = context.Request["order"];
                if (!string.IsNullOrEmpty(_order))
                {
                    var order_split = _order.Split(',');
                    if (order_split.Length == 2)
                    {
                        _sColOrder = order_split[0];
                        _sSortDir = order_split[1];
                    }
                }
                sOrder = ",\"sort\":[";
                //Se não tiver _sSortCol então não foi selecionado nenhum ordenamento, log devemos ordenar por dt_doc que é o ordenamento padrão
                if (string.IsNullOrEmpty(_sColOrder))
                {
                    sOrder += "\"_score\",{\"dt_assinatura_untouched\":{\"order\":\"desc\"}}";
                    if (_bbusca == "sinj_diario")
                    {
                        var _ds_norma = context.Request["ds_norma"];
                        if (!string.IsNullOrEmpty((_ds_norma)))
                        {
                            sOrder = ",\"sort\":[{\"secao_diario\":{\"order\":\"asc\"}}";
                        }
                        else
                        {
                            sOrder += ",{\"secao_diario\":{\"order\":\"asc\"}}";
                        }
                    }
                }
                else
                {
                    if (_bbusca == "sinj_diario")
                    {
                        if (_sColOrder == "dt_assinatura")
                        {
                            _sColOrder = "dt_assinatura_untouched";
                        }
                        else if (_sColOrder == "nr_diario")
                        {
                            _sColOrder = "nr_diario_untouched";
                        }
                        if (("desc" == _sSortDir))
                        {

                            sOrder += "{\"" + _sColOrder + "\":{\"order\":\"desc\"}}";
                        }
                        else
                        {
                            sOrder += "{\"" + _sColOrder + "\":{\"order\":\"asc\"}}";
                        }
                    }
                    else if (_bbusca == "sinj_norma")
                    {
                        if (_sColOrder == "dt_assinatura")
                        {
                            _sColOrder = "dt_assinatura_untouched";
                        }

                        if (("desc" == _sSortDir))
                        {
                            if (_sColOrder == "nm_tipo_norma")
                            {
                                sOrder += "{\"nm_tipo_norma_untouched\":{\"order\":\"desc\"}},{\"nr_norma_untouched\":{\"order\":\"desc\"}}";
                            }
                            else
                            {
                                sOrder += "{\"" + _sColOrder + "\":{\"order\":\"desc\"}}";
                            }
                        }
                        else
                        {
                            if (_sColOrder == "nm_tipo_norma")
                            {
                                sOrder += "{\"nm_tipo_norma_untouched\":{\"order\":\"asc\"}},{\"nr_norma_untouched\":{\"order\":\"asc\"}}";
                            }
                            else
                            {
                                sOrder += "{\"" + _sColOrder + "\":{\"order\":\"asc\"}}";
                            }
                        }
                    }
                    else if (_bbusca == "cesta")
                    {
                        if (_sColOrder == "dt_assinatura")
                        {
                            _sColOrder = "dt_assinatura_untouched";
                        }
                        else if (_sColOrder == "nr_diario")
                        {
                            _sColOrder = "nr_diario_untouched";
                        }
                        if (("desc" == _sSortDir))
                        {
                            if (_sColOrder == "nm_tipo_norma")
                            {
                                sOrder += "{\"nm_tipo_norma_untouched\":{\"order\":\"desc\"}},{\"nr_norma_untouched\":{\"order\":\"desc\"}}";
                            }
                            else
                            {
                                sOrder += "{\"" + _sColOrder + "\":{\"order\":\"desc\"}}";
                            }
                        }
                        else
                        {
                            if (_sColOrder == "nm_tipo_norma")
                            {
                                sOrder += "{\"nm_tipo_norma_untouched\":{\"order\":\"asc\"}},{\"nr_norma_untouched\":{\"order\":\"asc\"}}";
                            }
                            else
                            {
                                sOrder += "{\"" + _sColOrder + "\":{\"order\":\"asc\"}}";
                            }
                        }
                    }
                }
                sOrder += "]";
            }
            return sOrder;
        }

        public string MontarQueryFiltros(HttpContext context)
	    {
		    string filtrar = context.Request["filtrar"];//ver aqui
            string query = "";
            if(filtrar == "norma"){
                string _ch_tipo_norma = context.Request["ch_tipo_norma"];
                if(!string.IsNullOrEmpty(_ch_tipo_norma)){
                    query += "ch_tipo_norma:("+_ch_tipo_norma+")";
                }
                string _ch_orgao = context.Request["ch_orgao"];
                if(!string.IsNullOrEmpty(_ch_orgao)){
                    query += (!string.IsNullOrEmpty(query) ? " AND " : "") + "ch_orgao:(" + _ch_orgao + ")";
                }
                string _ch_situacao = context.Request["ch_situacao"];
                if (!string.IsNullOrEmpty(_ch_situacao))
                {
                    query += (!string.IsNullOrEmpty(query) ? " AND " : "") + "ch_situacao:(" + _ch_situacao + ")";
                }
                string _ano = context.Request["ano"];
                if (!string.IsNullOrEmpty(_ano))
                {
                    query += (!string.IsNullOrEmpty(query) ? " AND " : "") + "dt_assinatura:[01/01/" + _ano + " TO 31/12/" + _ano + "]";
                }
            }
            else if (filtrar == "diario")
            {
                string _ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                if (!string.IsNullOrEmpty(_ch_tipo_fonte))
                {
                    query += "ch_tipo_fonte:(" + _ch_tipo_fonte + ")";
                }
                string _ano = context.Request["ano"];
                if (!string.IsNullOrEmpty(_ano))
                {
                    string _mes = context.Request["mes"];
                    if (!string.IsNullOrEmpty(_mes))
                    {
                        string _dia = context.Request["dia"];
                        if (!string.IsNullOrEmpty(_dia))
                        {
                            query += (!string.IsNullOrEmpty(query) ? " AND " : "") + "dt_assinatura:(\\\"" + _dia + "/" + _mes + "/" + _ano + "\\\")";
                        }
                        else
                        {
                            query += (!string.IsNullOrEmpty(query) ? " AND " : "") + "dt_assinatura:[01/" + _mes + "/" + _ano + " TO 31/" + _mes + "/" + _ano + "]";
                        }
                    }
                    else
                    {
                        query += (!string.IsNullOrEmpty(query) ? " AND " : "") + "dt_assinatura:[01/01/" + _ano + " TO 31/12/" + _ano + "]";
                    }
                }
            }
            else
            {
                string[] _filtros = context.Request.Params.GetValues("filtro");
                if (_filtros != null)
                {
                    foreach (var _filtro in _filtros)
                    {


                        if (_filtro.IndexOf("ano_") == 0)
                        {
                            string[] _ano = _filtro.Split(':');
                            query += (!string.IsNullOrEmpty(query) ? " AND " : "") + "dt_assinatura:[01/01/" + _ano[1] + " TO 31/12/" + _ano[1] + "]";
                        }
                        else
                        {
                            query += (!string.IsNullOrEmpty(query) ? " AND " : "") + _filtro;
                        }
                    }
                }
            }
		    return query;
	    }
    }
}

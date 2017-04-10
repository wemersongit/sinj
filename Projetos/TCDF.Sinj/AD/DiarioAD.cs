//using BRLight.ElasticSearch;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using util.BRLight;
using System.Collections.Generic;
using System;
using System.Web;
using neo.BRLightES;

namespace TCDF.Sinj.AD
{
    public class DiarioAD
    {
        private DocEs _docEs;
        private AcessoAD<DiarioOV> _acessoAd;
        private string _nm_base;

        public DiarioAD()
        {
            _nm_base = util.BRLight.Util.GetVariavel("NmBaseDiario", true);
            _acessoAd = new AcessoAD<DiarioOV>(_nm_base);
            _docEs = new DocEs();
        }

        internal Results<DiarioOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        internal DiarioOV Doc(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

        internal DiarioOV Doc(string ch_diario)
        {
            Pesquisa query = new Pesquisa();
            query.limit = "1";
            query.offset = "0";
            query.literal = string.Format("ch_diario='{0}'", ch_diario);
            var result = Consultar(query);
            if (result.result_count > 1)
            {
                throw new Exception("Foi verificado mais de um registro com a mesma chave.");
            }
            if (result.result_count > 0)
            {
                return result.results[0];
            }
            throw new Exception("Nenhum registro foi encontrado, é possível que tenha sido excluído.");
        }

        internal string JsonReg(ulong id_doc)
        {
            return _acessoAd.jsonReg(id_doc);
        }

        internal string JsonReg(Pesquisa query)
        {
            return _acessoAd.jsonReg(query);
        }

        internal string PathPut(ulong id_doc, string path, string value, string retorno)
        {
            return _acessoAd.pathPut(id_doc, path, value, retorno);
        }

        /// <summary>
        /// Inclui um diario e retorna o id_doc
        /// </summary>
        /// <param name="diarioOv"></param>
        /// <returns></returns>
        internal ulong Incluir(DiarioOV diarioOv)
        {
            try
            {
                return _acessoAd.Incluir(diarioOv);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("duplicate key") > -1 || ex.Message.IndexOf("duplicar valor da chave") > -1)
                {
                    throw new DocDuplicateKeyException("Registro duplicado!!!");
                }
                throw ex;
            }
        }
		
        internal bool Atualizar(ulong id_doc, DiarioOV diarioOv)
        {
            try
            {
                return _acessoAd.Alterar(id_doc, diarioOv);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("duplicate key") > -1 || ex.Message.IndexOf("duplicar valor da chave") > -1)
                {
                    throw new DocDuplicateKeyException("Registro já existente na base de dados!!!");
                }
                throw ex;
            }
        }

        internal bool Excluir(ulong id_doc)
        {
            return _acessoAd.Excluir(id_doc);
        }

        #region Arquivo

        public string GetDoc(string id_file)
        {
            var doc = new Doc(_nm_base);
            return doc.pesquisarDoc(id_file);
        }

        public string AnexarArquivo(FileParameter fileParameter)
        {
            string resultado;
            try
            {
                var doc = new Doc(_nm_base);
                var dicionario = new Dictionary<string, object>();
                dicionario.Add("file", fileParameter);
                resultado = doc.incluir(dicionario);
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("Não foi possível anexar o arquivo", ex);
            }
            return resultado;
        }

        public byte[] Download(string id_file)
        {
            var doc = new Doc(_nm_base);
            return doc.download(id_file);
        }

        #endregion

        #region ES

        /// <summary>
        /// Monta o ordenamento
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string MontarOrdenamento(HttpContext context)
        {
            var sOrder = "";
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

            sOrder = ",\"sort\":[";
            //Se não tiver _sSortCol então não foi selecionado nenhum ordenamento, logo devemos ordenar por dt_doc que é o ordenamento padrão
            if (string.IsNullOrEmpty(_sColOrder))
            {
                sOrder += "\"_score\",{\"dt_assinatura_untouched\":{\"order\":\"desc\"}}";
                //se a listagem de diários foi requisitada para visualizar os diários da publicação de um ato deve-se ordenar pela seção (somente)
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
            else
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
            sOrder += "]";
            return sOrder;
        }

        /// <summary>
        /// Campos que retornarão do elasticsearch
        /// </summary>
        /// <returns></returns>
        private string MontarPartialFields()
        {
            return ", \"partial_fields\":{\"partial\":{\"include\":[\"_metadata.id_doc\",\"ch_diario\",\"nm_tipo_fonte\",\"nm_tipo_edicao\",\"nm_diferencial_edicao\",\"nr_diario\",\"cr_diario\",\"secao_diario\",\"dt_assinatura\",\"st_pendente\",\"nm_diferencial_suplemento\",\"st_suplemento\",\"ar_diario.id_file\",\"ar_diario.filesize\",\"ar_diario.filename\",\"arquivos.arquivo_diario.id_file\",\"arquivos.arquivo_diario.filesize\",\"arquivos.arquivo_diario.filename\",\"arquivos.ds_arquivo\"]}}";
        }

        /// <summary>
        /// Campos nos quais será feita a pesquisa textual
        /// </summary>
        /// <returns></returns>
        private string MontarCamposPesquisados()
        {
            return "\"nm_tipo_fonte^2\",\"nm_tipo_edicao^2\",\"nr_diario_text^2\",\"dt_assinatura_text^2\",\"ar_diario.filetext\",\"arquivos.arquivo_diario.filetext\"";
        }

        /// <summary>
        /// Campos que retornarão do elasticsearch
        /// </summary>
        /// <returns></returns>
        private string MontarHighlight()
        {
            return "\"nm_tipo_fonte\":{\"number_of_fragments\":20,\"fragment_size\":1},\"nr_diario_text\":{\"number_of_fragments\":20,\"fragment_size\":1},\"dt_assinatura_text\":{\"number_of_fragments\":20,\"fragment_size\":1},\"arquivos.arquivo_diario.filetext\":{\"number_of_fragments\":8, \"fragment_size\":150, \"no_match_size\":300}";
        }

        /// <summary>
        /// Consulta no elasticsearch
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sessao_usuario_ov"></param>
        /// <returns></returns>
        private string MontarConsulta(HttpContext context)
        {
            string _tipo_pesquisa = context.Request["tipo_pesquisa"];
            string _sSearch = context.Request["sSearch"];
            string _sFiltrar = context.Request["filtrar"];
            string _all = context.Request["all"];
            string _exibir_total = context.Request["exibir_total"];
            var aggs = "";

            //Os campos que retornarão do ElasticSearch
            var partial_fields = "";
            var sOrder = "";
            if (_exibir_total != "1")
            {
                sOrder = MontarOrdenamento(context);
                partial_fields = MontarPartialFields();
                aggs = ",\"aggs\":{\"nm_tipo_fonte_keyword\":{\"terms\":{\"field\":\"nm_tipo_fonte_keyword\",\"order\":{\"_count\":\"desc\"},\"size\":20}},\"ano_assinatura\":{\"date_histogram\":{\"field\":\"dt_assinatura\",\"interval\":\"year\",\"format\":\"yyyy\",\"order\":{\"_key\":\"desc\"}}}}";
                //aggs = ",\"aggs\":{\"agg_terms\":{\"terms\":{\"field\":\"nr_ano\",\"order\":{\"_term\":\"desc\"},\"size\":30}},\"agg_terms_tipo\":{\"terms\":{\"field\":\"ch_tipo_fonte\",\"order\":{\"_count\":\"desc\"},\"size\":30}}}";
            }

            //Os campos onde a consulta textual será feita
            var fields = "";

            var fields_highlight = "";
            var highlight = "";

            var query = "";
            var query_textual = "";
            var query_filtered = "";
            var filters = new List<string>();
            var ids = "";
            if (_tipo_pesquisa == "geral")
            {
                fields = MontarCamposPesquisados();
                if (_exibir_total != "1")
                {
                    fields_highlight = MontarHighlight();
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
                    if (fields_highlight != "" && _exibir_total != "1")
                    {
                        highlight = ",\"highlight\":{\"pre_tags\":[\"_pre_tag_highlight_\"],\"post_tags\":[\"_post_tag_highlight_\"],\"fields\":{" + fields_highlight + "}}";
                    }
                }
            }
            else if (_tipo_pesquisa == "diario")
            {
                var _ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                var _ch_tipo_edicao = context.Request["ch_tipo_edicao"];
                var _nr_diario = context.Request["nr_diario"];
                var _secao_diario = context.Request.Form.GetValues("secao_diario");
                var _filetext = context.Request["filetext"];
                var _op_dt_assinatura = context.Request["op_dt_assinatura"];
                var _dt_assinatura = context.Request.Form.GetValues("dt_assinatura");
                if (!string.IsNullOrEmpty(_filetext))
                {
                    query_filtered = _docEs.TratarCaracteresReservadosDoEs(_filetext);
                    if (!string.IsNullOrEmpty(_ch_tipo_fonte))
                    {
                        filters.Add("{\"term\":{\"ch_tipo_fonte\":\"" + _ch_tipo_fonte + "\"}}");
                    }
                    if (!string.IsNullOrEmpty(_ch_tipo_edicao))
                    {
                        filters.Add("{\"term\":{\"ch_tipo_edicao\":\"" + _ch_tipo_edicao + "\"}}");
                    }
                    if (!string.IsNullOrEmpty(_nr_diario))
                    {
                        filters.Add("{\"query\":{\"query_string\":{\"query\":\"nr_diario:" + _nr_diario + "\"}}}");
                    }
                    if (_dt_assinatura != null && _dt_assinatura.Length > 0)
                    {
                        if (!string.IsNullOrEmpty(_dt_assinatura[0]))
                        {
                            filters.Add("{\"range\":{\"dt_assinatura\":{\"gte\":\"" + _dt_assinatura[0] + "\"}}}");
                        }
                        if (!string.IsNullOrEmpty(_dt_assinatura[1]))
                        {
                            filters.Add("{\"range\":{\"dt_assinatura\":{\"lte\":\"" + _dt_assinatura[1] + "\"}}}");
                        }
                    }
                    if (_secao_diario != null && _secao_diario.Length > 0)
                    {
                        var sSecao = "";
                        for (var i = 0; i < _secao_diario.Length; i++)
                        {
                            sSecao += (sSecao != "" ? (i < (_secao_diario.Length - 1) ? ", " : " e ") : "") + _secao_diario[i];
                        }
                        filters.Add("{\"query\":{\"query_string\":{\"query\":\"secao_diario:\\\"" + sSecao + "\\\"}}}");
                    }
                    if (!string.IsNullOrEmpty(_sSearch))
                    {
                        query_filtered += (query_filtered != "" ? " AND " : "") + "(" + _docEs.TratarCaracteresReservadosDoEsPesquisaAvancada(_sSearch) + "*)";
                    }
                    if (_exibir_total != "1")
                    {
                        fields_highlight += (fields_highlight != "" ? "," : "") + "\"arquivos.arquivo_diario.filetext\":{\"number_of_fragments\":8,\"fragment_size\":150, \"no_match_size\":300}";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(_nr_diario))
                    {
                        query_textual += (query_textual != "" ? " AND " : "") + "(nr_diario:" + _nr_diario + ")";
                    }
                    if (!string.IsNullOrEmpty(_ch_tipo_fonte))
                    {
                        query_textual += (query_textual != "" ? " AND " : "") + "(ch_tipo_fonte:" + _ch_tipo_fonte + ")";
                    }
                    if (!string.IsNullOrEmpty(_ch_tipo_edicao))
                    {
                        query_textual += (query_textual != "" ? " AND " : "") + "(ch_tipo_edicao:" + _ch_tipo_edicao + ")";
                    }
                    if (_dt_assinatura != null && _dt_assinatura.Length > 0 && !string.IsNullOrEmpty(_dt_assinatura[0]))
                    {
                        query_textual += (query_textual != "" ? " AND " : "") + _docEs.MontarArgumentoRangeQueryString("dt_assinatura", _op_dt_assinatura, string.Join(",", _dt_assinatura));
                    }
                    if (!string.IsNullOrEmpty(_sSearch))
                    {
                        query_textual += (query_textual != "" ? " AND " : "") + "(_all:" + _docEs.TratarCaracteresReservadosDoEsPesquisaAvancada(_sSearch) + "*)";
                    }
                    if (_secao_diario != null && _secao_diario.Length > 0)
                    {
                        var sSecao = "";
                        for (var i = 0; i < _secao_diario.Length; i++)
                        {
                            sSecao += (sSecao != "" ? (i < (_secao_diario.Length - 1) ? ", " : " e ") : "") + _secao_diario[i];
                        }
                        query_textual += (query_textual != "" ? " AND " : "") + "(secao_diario:\\\"" + sSecao + "\\\")";
                    }
                }
            }
            else if (_tipo_pesquisa == "diretorio_diario")
            {
                fields = "";
                sOrder = ",\"sort\":[{\"dt_assinatura_untouched\":{\"order\":\"desc\"}},{\"secao_diario\":{\"order\":\"asc\"}}]";
                var _ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                var _ano = context.Request["ano"];
                var _mes = context.Request["mes"];
                if (!string.IsNullOrEmpty(_ch_tipo_fonte))
                {
                    query_textual += (query_textual != "" ? " AND " : "") + "(ch_tipo_fonte:" + _ch_tipo_fonte + ")";
                }
                if (!string.IsNullOrEmpty(_ano) && !string.IsNullOrEmpty(_mes))
                {
                    query_textual += (query_textual != "" ? " AND " : "") + _docEs.MontarArgumentoRangeQueryString("dt_assinatura", "maiorouigual", "01/" + _mes + "/" + _ano);
                    query_textual += (query_textual != "" ? " AND " : "") + _docEs.MontarArgumentoRangeQueryString("dt_assinatura", "menor", "01/" + (_mes == "12" ? "01" : (int.Parse(_mes) + 1).ToString()) + "/" + (_mes == "12" ? (int.Parse(_ano) + 1).ToString() : _ano ));
                }
            }
            else if (_tipo_pesquisa == "texto_diario")
            {
                var _ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                var _filetext = context.Request["filetext"];
                var _intervalo = context.Request["intervalo"];
                var _dt_assinatura_inicio = context.Request["dt_assinatura_inicio"];
                var _dt_assinatura_termino = context.Request["dt_assinatura_termino"];
                var _ano = context.Request["ano"];
                if (!string.IsNullOrEmpty(_filetext))
                {
                    //query_textual += (query_textual != "" ? " AND " : "") + "(ar_diario.filetext:" + _docEs.TratarCaracteresReservadosDoEs(_filetext) + ")";
                    query_filtered = _docEs.TratarCaracteresReservadosDoEs(_filetext);
                    fields_highlight = "\"arquivos.arquivo_diario.filetext\":{\"number_of_fragments\":5,\"fragment_size\":300,\"no_match_size\":300}";
                }

                if (!string.IsNullOrEmpty(_ch_tipo_fonte))
                {
                    //query_textual += (query_textual != "" ? " AND " : "") + "(ch_tipo_fonte:" + _ch_tipo_fonte + ")";
                    filters.Add("{\"term\":{\"ch_tipo_fonte\":\""+_ch_tipo_fonte+"\"}}");
                }
                if (_intervalo == "1")
                {
                    if (!string.IsNullOrEmpty(_dt_assinatura_inicio))
                    {
                        //query_textual += (query_textual != "" ? " AND " : "") + _docEs.MontarArgumentoRangeQueryString("dt_assinatura", "maiorouigual", _dt_assinatura_inicio);
                        filters.Add("{\"range\":{\"dt_assinatura\":{\"gte\":\"" + _dt_assinatura_inicio + "\"}}}");
                    }
                    if (!string.IsNullOrEmpty(_dt_assinatura_termino))
                    {
                        //query_textual += (query_textual != "" ? " AND " : "") + _docEs.MontarArgumentoRangeQueryString("dt_assinatura", "menorouigual", _dt_assinatura_termino);
                        filters.Add("{\"range\":{\"dt_assinatura\":{\"lte\":\"" + _dt_assinatura_termino + "\"}}}");
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(_dt_assinatura_inicio))
                    {
                        filters.Add("{\"term\":{\"dt_assinatura\":\"" + _dt_assinatura_inicio + "\"}}");
                    }
                }
                if (!string.IsNullOrEmpty(_ano))
                {
                    filters.Add("{\"range\":{\"dt_assinatura\":{\"gte\":\"01/01/" + _ano + "\",\"lte\":\"31/12/" + _ano + "\"}}}");
                }
                if (!string.IsNullOrEmpty(_sSearch))
                {
                    query_filtered += (query_filtered != "" ? " AND " : "") + "(" + _docEs.TratarCaracteresReservadosDoEsPesquisaAvancada(_sSearch) + "*)";
                }
            }
            if (!string.IsNullOrEmpty(query_filtered) || filters.Count > 0)
            {
                string[] _filtros = context.Request.Params.GetValues("filtro");
                if (_filtros != null)
                {
                    foreach (var _filtro in _filtros)
                    {
                        filters.Add("{\"query\":{\"query_string\":{\"query\":\""+_filtro+"\"}}}");
                    }
                }
                var filters_and = "";
                foreach (var filter in filters)
                {
                    filters_and += (filters_and != "" ? "," : "") + filter;
                }
                if (!string.IsNullOrEmpty(filters_and))
                {
                    filters_and = ",\"filter\":{\"and\":[" + filters_and + "]}";
                }
                if (!string.IsNullOrEmpty(fields_highlight))
                {
                    highlight = ",\"highlight\":{\"pre_tags\":[\"_pre_tag_highlight_\"],\"post_tags\":[\"_post_tag_highlight_\"],\"fields\":{" + fields_highlight + "}}";
                }
                query = "{\"query\":{\"filtered\":{\"query\":{\"query_string\":{\"fields\":[\"arquivos.arquivo_diario.filetext\"],\"query\":\"" + query_filtered + "\", \"default_operator\":\"AND\"}}" + filters_and + "}}";
            }
            else if (string.IsNullOrEmpty(query_textual))
            {
                query_textual = "*";
            }
            if (!string.IsNullOrEmpty(query_textual))
            {
                if (fields_highlight != "" && _exibir_total != "1" && string.IsNullOrEmpty(highlight))
                {
                    highlight = ",\"highlight\":{\"pre_tags\":[\"\"],\"post_tags\":[\"\"],\"fields\":{" + fields_highlight + "}}";
                }
                var query_filtro = MontarQueryFiltros(context);
                fields = fields != "" ? "\"fields\":[" + fields + "]," : "";
                query = "{\"query\":{\"query_string\":{" + fields + "\"query\":\"" + query_textual + (!string.IsNullOrEmpty(query_filtro) ? " AND (" + query_filtro + ")" : "") + "\", \"default_operator\":\"AND\"}}";
            }
            //query = "{\"query\":{\"query_string\":{\"fields\":[\"ar_diario.filetext\"],\"query\":\"" + query_textual + "\", \"default_operator\":\"AND\"}}";
            //query = "{\"query\":{\"filtered\":{\"query\":{\"query_string\":{\"fields\":[\"ar_diario.filetext\"],\"query\":\"" + query_textual + "\", \"default_operator\":\"AND\"}},\"filter\":{\"and\":["+JSON.Serialize<List<string>>(filter)+"]}}";
            return query + sOrder + partial_fields + highlight + aggs + "}";
        }

        public string MontarQueryFiltros(HttpContext context)
        {
            string query = "";
            string _ch_tipo_fonte = context.Request["ch_tipo_fonte_filtro"];
            if (!string.IsNullOrEmpty(_ch_tipo_fonte))
            {
                query += "ch_tipo_fonte:(" + _ch_tipo_fonte + ")";
            }
            string _ano = context.Request["ano_filtro"];
            if (!string.IsNullOrEmpty(_ano))
            {
                string _mes = context.Request["mes_filtro"];
                if (!string.IsNullOrEmpty(_mes))
                {
                    string _dia = context.Request["dia_filtro"];
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
            string[] _filtros = context.Request.Params.GetValues("filtro");
            if (_filtros != null)
            {
                foreach (var _filtro in _filtros)
                {
                    query += (!string.IsNullOrEmpty(query) ? " AND " : "") + _filtro;
                }
            }
            return query;
        }

        /// <summary>
        /// Monta a url de consulta para pesquisa ou total
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string MontarUrl(HttpContext context)
        {
            string _exibir_total = context.Request["exibir_total"];
            var _iDisplayLength = context.Request["iDisplayLength"];
            var _iDisplayStart = context.Request["iDisplayStart"];
            var url_es = new DocEs().GetUrlEs(_nm_base);
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

        public Result<DiarioOV> ConsultarEs(HttpContext context)
        {
            var query = "";
            var url_es = MontarUrl(context);
            query = MontarConsulta(context);
            return new ES().PesquisarDocs<DiarioOV>(query, url_es);
        }

        public string PesquisarTotalEs(HttpContext context)
        {
            string url_es = "";
            string query = "";
            try
            {
                string _tipo_pesquisa = context.Request["tipo_pesquisa"];
                url_es = MontarUrl(context);
                query = MontarConsulta(context);
                return _docEs.CountEs(query, url_es);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar Total. url_es: " + url_es + ". query: " + query, ex);
            }
        }

        #endregion
    }
}
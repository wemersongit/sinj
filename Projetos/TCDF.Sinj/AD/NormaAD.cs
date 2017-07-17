using neo.BRLightREST;
using TCDF.Sinj.OV;
using util.BRLight;
using System.Collections.Generic;
using System;
using neo.BRLightES;
using System.Web;
using TCDF.Sinj.RN;

namespace TCDF.Sinj.AD
{
    public class NormaAD
    {
        private DocEs _docEs;
        private AcessoAD<NormaOV> _acessoAd;
        private string _nm_base;

        public NormaAD()
        {
            _nm_base = util.BRLight.Util.GetVariavel("NmBaseNorma", true);
            _acessoAd = new AcessoAD<NormaOV>(_nm_base);
            _docEs = new DocEs();
        }

        internal Results<NormaOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        internal NormaOV Doc(ulong id_doc)
        {
            return _acessoAd.ConsultarReg(id_doc);
        }

        internal NormaOV Doc(string ch_norma)
        {
            Pesquisa query = new Pesquisa();
            query.limit = "1";
            query.offset = "0";
            query.literal = string.Format("ch_norma='{0}'", ch_norma);
            var result = Consultar(query);
            if (result.result_count > 1)
            {
                throw new Exception("Foi verificado mais de um registro com a mesma chave.");
            }
            if (result.result_count > 0)
            {
                return result.results[0];
            }
            throw new DocNotFoundException("Nenhum registro foi encontrado, é possível que tenha sido excluído.");
        }

        internal string JsonReg(Pesquisa query)
        {
            return _acessoAd.jsonReg(query);
        }

        internal string JsonReg(ulong id_doc)
        {
            return _acessoAd.jsonReg(id_doc);
        }

        internal string PathPut(ulong id_doc, string path, string value, string retorno)
        {
            return _acessoAd.pathPut(id_doc, path, value, retorno);
        }

        internal string PathPut<T>(Pesquisa pesquisa, List<opMode<T>> listopMode)
        {
            return new AcessoAD<T>(_nm_base).OP(pesquisa, listopMode);
        }

        /// <summary>
        /// Inclui uma norma e retorna o id_doc
        /// </summary>
        /// <param name="normaOv"></param>
        /// <returns></returns>
        internal ulong Incluir(NormaOV normaOv)
        {
            try
            {
                return _acessoAd.Incluir(normaOv);
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

		internal bool Atualizar(ulong id_doc, NormaOV normaOv)
        {
            try
            {
                return _acessoAd.Alterar(id_doc, normaOv);
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
            string _exibir_total = context.Request["exibir_total"];

            if (_exibir_total != "1")
            {
                var _sSortCol = context.Request["iSortCol_0"];
                var iSortCol = 0;
                var _sSortDir = "";
                var _sColOrder = "";
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
                }
                else
                {
                    if (_sColOrder == "dt_assinatura")
                    {
                        _sColOrder = "dt_assinatura_untouched";
                    }

                    if ("desc" == _sSortDir)
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
                sOrder += "]";
            }
            return sOrder;
        }

        /// <summary>
        /// Campos que retornarão do elasticsearch
        /// </summary>
        /// <returns></returns>
        private string MontarPartialFields()
        {
            return ", \"partial_fields\":{\"partial\":{\"include\":[\"_metadata.id_doc\",\"origens.sg_orgao\",\"origens.nm_orgao\",\"origens.ch_orgao\",\"ar_atualizado.id_file\",\"ar_atualizado.filesize\",\"ar_atualizado.mimetype\",\"ar_atualizado.filename\",\"fontes.nm_tipo_publicacao\",\"fontes.ar_fonte.id_file\",\"fontes.ar_fonte.filesize\",\"fontes.ar_fonte.filename\",\"fontes.ar_fonte.mimetype\",\"nm_tipo_norma\",\"nr_norma\",\"ch_norma\",\"dt_assinatura\",\"ds_ementa\",\"nm_situacao\"]}}";
        }

        /// <summary>
        /// Campos nos quais será feita a pesquisa textual
        /// </summary>
        /// <returns></returns>
        private string MontarCamposPesquisados()
        {
            return "\"nm_tipo_norma^2\",\"nr_norma^2\",\"nr_norma_numeric^2\",\"dt_assinatura_text^2\",\"dt_autuacao_text\",\"rankeamentos^3\",\"nm_situacao\",\"nm_ambito\",\"nm_apelido^2\",\"ds_ementa^3\",\"nm_orgao^2\",\"sg_orgao^2\",\"ar_atualizado.filetext\",\"fontes.ar_fonte.filetext\",\"nm_termo^3\"";
        }

        /// <summary>
        /// Campos que retornarão do elasticsearch
        /// </summary>
        /// <returns></returns>
        private string MontarHighlight()
        {
            return "\"nm_tipo_norma\":{\"number_of_fragments\":20,\"fragment_size\":1},\"nr_norma\":{\"number_of_fragments\":20,\"fragment_size\":1},\"dt_assinatura_text\":{\"number_of_fragments\":20,\"fragment_size\":1},\"dt_autuacao_text\":{\"number_of_fragments\":20,\"fragment_size\":1},\"nm_situacao\":{\"number_of_fragments\":20,\"fragment_size\":1},\"nm_ambito\":{\"number_of_fragments\":20,\"fragment_size\":1},\"ds_apelido\":{\"number_of_fragments\":20,\"fragment_size\":1},\"ds_ementa\":{\"number_of_fragments\":20,\"fragment_size\":1},\"nm_orgao\":{\"number_of_fragments\":20,\"fragment_size\":1},\"sg_orgao\":{\"number_of_fragments\":20,\"fragment_size\":1},\"nm_termo\":{\"number_of_fragments\":20,\"fragment_size\":1},\"fontes.ar_fonte.filetext\":{\"number_of_fragments\":5, \"fragment_size\":60},\"ar_atualizado.filetext\":{\"number_of_fragments\":5, \"fragment_size\":60}";
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

            //Os campos que retornarão do ElasticSearch
            var partial_fields = "";
            var sOrder = "";
            var aggs = "";
            if (_exibir_total != "1")
            {
                sOrder = MontarOrdenamento(context);
                partial_fields = MontarPartialFields();
                aggs = ",\"aggs\":{\"sg_orgao_keyword\":{\"terms\":{\"field\":\"sg_orgao_keyword\",\"order\":{\"_count\":\"desc\"},\"size\":20}}, \"nm_tipo_norma_keyword\":{\"terms\":{\"field\":\"nm_tipo_norma_keyword\",\"order\":{\"_count\":\"desc\"},\"size\":20}}, \"nm_situacao_keyword\":{\"terms\":{\"field\":\"nm_situacao_keyword\",\"order\":{\"_count\":\"desc\"},\"size\":20}},\"ano_assinatura\":{\"date_histogram\":{\"field\":\"dt_assinatura\",\"interval\":\"year\",\"format\":\"yyyy\",\"order\":{\"_key\":\"desc\"}}}}";
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

                }
            }
            else if (_tipo_pesquisa == "norma")
            {
                fields = MontarCamposPesquisados();
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
                        fields_highlight += (fields_highlight != "" ? "," : "") + MontarHighlight();
                    }
                }
                if (!string.IsNullOrEmpty(_nr_norma))
                {
                    query_textual += (query_textual != "" ? " AND " : "") + "(nr_norma:(" + _nr_norma + ") OR nr_norma_numeric:("+_nr_norma+"))";
                }
                else if (!string.IsNullOrEmpty(_norma_sem_numero))
                {
                    query_textual += (query_textual != "" ? " AND " : "") + "(nr_norma:0)";
                }
                if (!string.IsNullOrEmpty(_sSearch))
                {
                    query_textual += (query_textual != "" ? " AND " : "") + "(_all:" + _docEs.TratarCaracteresReservadosDoEsPesquisaAvancada(_sSearch) + "*)";
                }
            }
            else if (_tipo_pesquisa == "avancada")
            {
                fields = MontarCamposPesquisados();
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
                                            fields_highlight += (fields_highlight != "" ? "," : "") + MontarHighlight();
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
                                _ch_operador = _ch_operador.Replace("_","");
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
                    var _ch_tipo_norma = context.Request["ch_tipo_norma"];
                    if (!string.IsNullOrEmpty(_ch_tipo_norma) && (_ch_tipo_norma != "todas"))
                    {
                        query_textual += (query_textual != "" ? " AND " : "") + "(ch_tipo_norma:\\\"" + _ch_tipo_norma + "\\\")";
                    }
                }
            }
            if (query_textual == "")
            {
                query_textual = "*";
            }
            var query_filtro = MontarQueryFiltros(context);
            query = "{\"query\":{\"query_string\":{\"fields\":[" + fields + "],\"query\":\"" + query_textual + (!string.IsNullOrEmpty(query_filtro) ? " AND (" + query_filtro + ")" : "") + "\", \"default_operator\":\"AND\"}}";
            if (fields_highlight != "" && _exibir_total != "1")
            {
                highlight = ",\"highlight\":{\"pre_tags\":[\"\"],\"post_tags\":[\"\"],\"fields\":{" + fields_highlight + "}}";
            }
            return query + sOrder + partial_fields + highlight + aggs + "}";
        }

        public string MontarQueryFiltros(HttpContext context)
        {
            string query = "";
            string _ch_tipo_norma = context.Request["ch_tipo_norma_filtro"];
            if (!string.IsNullOrEmpty(_ch_tipo_norma))
            {
                query += "ch_tipo_norma:(" + _ch_tipo_norma + ")";
            }
            string _ch_orgao = context.Request["ch_orgao_filtro"];
            if (!string.IsNullOrEmpty(_ch_orgao))
            {
                query += (!string.IsNullOrEmpty(query) ? " AND " : "") + "ch_orgao:(" + _ch_orgao + ")";
            }
            string _ch_situacao = context.Request["ch_situacao_filtro"];
            if (!string.IsNullOrEmpty(_ch_situacao))
            {
                query += (!string.IsNullOrEmpty(query) ? " AND " : "") + "ch_situacao:(" + _ch_situacao + ")";
            }
            string _ano = context.Request["ano_filtro"];
            if (!string.IsNullOrEmpty(_ano))
            {
                query += (!string.IsNullOrEmpty(query) ? " AND " : "") + "dt_assinatura:[01/01/" + _ano + " TO 31/12/" + _ano + "]";
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

        public Result<NormaOV> ConsultarEs(HttpContext context)
        {
            var query = MontarConsulta(context);
            var url_es = MontarUrl(context);
            try
            {
                return _docEs.Pesquisar<NormaOV>(query, url_es);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar Docs. url_es: " + url_es + ". query: " + query, ex);
            }
        }

        public string PesquisarTotalEs(HttpContext context)
        {
            string url_es = "";
            string query = "";
            try
            {
                query = MontarConsulta(context);
                url_es = MontarUrl(context);
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
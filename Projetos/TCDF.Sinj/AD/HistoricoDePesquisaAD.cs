using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using util.BRLight;
using TCDF.Sinj.ES;
using System.Web;

namespace TCDF.Sinj.AD
{
    class HistoricoDePesquisaAD
    {
        private DocEs _docEs;
        private AcessoAD<HistoricoDePesquisaOV> _acessoAd;
        private string _nm_base;

        public HistoricoDePesquisaAD()
        {
            _nm_base = util.BRLight.Util.GetVariavel("NmBaseLogsSearch", true);
            _acessoAd = new AcessoAD<HistoricoDePesquisaOV>(_nm_base);
            _docEs = new DocEs();
        }

        internal Results<HistoricoDePesquisaOV> Consultar(Pesquisa query)
        {
            return _acessoAd.Consultar(query);
        }

        internal ulong Incluir(HistoricoDePesquisaOV HistoricoDePesquisaOv)
        {
            return _acessoAd.Incluir(HistoricoDePesquisaOv);
        }

        internal bool Atualizar(ulong id_doc, HistoricoDePesquisaOV historicoDePesquisaOv)
        {
            try
            {
                return _acessoAd.Alterar(id_doc, historicoDePesquisaOv);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && (ex.InnerException.Message.IndexOf("duplicate key") > -1 || ex.InnerException.Message.IndexOf("duplicar valor da chave") > -1))
                {
                    throw new DocDuplicateKeyException("Registro já existente na base de dados!!!");
                }
                throw ex;
            }
        }

        #region ES

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
            if (!string.IsNullOrEmpty(_sColOrder))
            {
                sOrder = ",\"sort\":[";
                if ("desc" == _sSortDir)
                {
                    sOrder += "{\"" + _sColOrder + "\":{\"order\":\"desc\"}},";
                }
                else
                {
                    sOrder += "{\"" + _sColOrder + "\":{\"order\":\"asc\"}},";
                }
                sOrder = sOrder.Substring(0, sOrder.Length - 1) + "]";
            }
            return sOrder;
        }/// <summary>
        /// Monta a url de consulta para pesquisa ou total
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string MontarUrl(HttpContext context)
        {
            string _exibir_total = context.Request["exibir_total"];
            var _iDisplayLength = context.Request["iDisplayLength"];
            var _iDisplayStart = context.Request["iDisplayStart"];
            var _tipo_pesquisa = context.Request["tipo_pesquisa"];
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

        private string MontarConsulta(HttpContext context)
        {
            var query = "";
            var _tipo_pesquisa = context.Request["tipo_pesquisa"];
            var _dt_historico = context.Request["dt_historico"];
            var _dt_historico_fim = context.Request["dt_historico_fim"];
            var _op_intervalo = context.Request["op_intervalo"];
            
            if (_tipo_pesquisa == "estatistica")
            {
                var _nm_agg = context.Request["nm_agg"];
                var _agrupar_dt = context.Request["agrupar_dt"];
                var _size = context.Request["size"];
                if (string.IsNullOrEmpty(_size))
                {
                    _size = "0";
                }
                if (!string.IsNullOrEmpty(_nm_agg))
                {
                    var query_string = "";
                    if (!string.IsNullOrEmpty(_dt_historico) && !string.IsNullOrEmpty(_op_intervalo))
                    {
                        if (_op_intervalo == "intervalo" && !string.IsNullOrEmpty(_dt_historico_fim))
                        {
                            _dt_historico = "\\\"" + _dt_historico + " 00:00:00\\\",\\\"" + _dt_historico_fim + " 23:59:59\\\"";
                        }
                        else if (_op_intervalo == "menor" || _op_intervalo == "maiorouigual")
                        {
                            _dt_historico = "\\\"" + _dt_historico + " 00:00:00\\\"";
                        }
                        else if (_op_intervalo == "maior" || _op_intervalo == "menorouigual")
                        {
                            _dt_historico = "\\\"" + _dt_historico + " 23:59:59\\\"";
                        }
                        else if (_op_intervalo == "igual")
                        {
                            _op_intervalo = "intervalo";
                            _dt_historico = "\\\"" + _dt_historico + " 00:00:00\\\",\\\"" + _dt_historico + " 23:59:59\\\"";
                        }

                        query_string = "\"query\":{\"query_string\":{\"query\":\"" + _docEs.MontarArgumentoRangeQueryString("dt_historico", _op_intervalo, _dt_historico) + "\"}},";
                    }
                    switch (_nm_agg)
                    {
                        case "agg_pesquisas":
                            query = "{" + query_string + "\"from\":0,\"size\":1,\"sort\":[{\"dt_doc\":{\"order\":\"asc\"}}], \"aggs\":{" +
                                "\"agg_pesquisas\":{\"terms\":{\"field\":\"ds_historico_keyword\",\"size\":" + _size + ",\"order\":{\"agg_sum\":\"desc\"}},\"aggs\":{\"agg_sum\":{\"sum\":{\"field\":\"contador\"}}}}" +
                            "}}";
                            break;
                        case "agg_termos":
                            query = "{" + query_string + "\"from\":0,\"size\":1,\"sort\":[{\"dt_doc\":{\"order\":\"asc\"}}], \"aggs\":{" +
                                "\"agg_termos\":{\"terms\":{\"field\":\"valor_keyword\",\"size\":" + _size + ",\"order\":{\"agg_sum\":\"desc\"}},\"aggs\":{\"agg_sum\":{\"sum\":{\"field\":\"contador\"}}}}" +
                            "}}";
                            break;
                        case "agg_tipos":
                            query = "{" + query_string + "\"from\":0,\"size\":1,\"sort\":[{\"dt_doc\":{\"order\":\"asc\"}}], \"aggs\":{" +
                                "\"agg_tipos\":{\"terms\":{\"field\":\"nm_tipo_pesquisa_keyword\",\"size\":" + _size + ",\"order\":{\"agg_sum\":\"desc\"}},\"aggs\":{\"agg_sum\":{\"sum\":{\"field\":\"contador\"}}}}" +
                            "}}";
                            break;
                        case "agg_dt_historico":
                            var interval_format = "\"interval\":\"1d\",\"format\":\"dd/MM/yyyy\"";
                            if (_agrupar_dt == "mes")
                            {
                                interval_format = "\"interval\":\"1M\",\"format\":\"MM/yyyy\"";
                            }
                            else if (_agrupar_dt == "ano")
                            {
                                interval_format = "\"interval\":\"1y\",\"format\":\"yyyy\"";
                            }
                            query = "{" + query_string + "\"from\":0,\"size\":1,\"sort\":[{\"dt_doc\":{\"order\":\"asc\"}}], \"aggs\":{" +
                                "\"agg_dt_historico\":{\"date_histogram\":{\"field\":\"dt_historico\", " + interval_format + ",\"order\" : { \"_key\" : \"desc\" }},\"aggs\":{\"agg_sum\":{\"sum\":{\"field\":\"contador\"}}}}" +
                            "}}";
                            break;
                    }
                }
                else
                {
                    query = "{\"from\":0,\"size\":1,\"sort\":[{\"dt_doc\":{\"order\":\"asc\"}}]," +
                        "\"aggs\":{" +
                            "\"agg_pesquisas\":{\"terms\":{\"field\":\"ds_historico_keyword\",\"size\":" + _size + ",\"order\":{\"agg_sum\":\"desc\"}},\"aggs\":{\"agg_sum\":{\"sum\":{\"field\":\"contador\"}}}}," +
                            "\"agg_termos\":{\"terms\":{\"field\":\"valor_keyword\",\"size\":" + _size + ",\"order\":{\"agg_sum\":\"desc\"}},\"aggs\":{\"agg_sum\":{\"sum\":{\"field\":\"contador\"}}}}," +
                            "\"agg_dt_historico\":{\"date_histogram\":{\"field\":\"dt_historico\",\"interval\":\"1d\",\"format\":\"dd/MM/yyyy\",\"order\" : { \"_key\" : \"desc\" }},\"aggs\":{\"agg_sum\":{\"sum\":{\"field\":\"contador\"}}}}," +
                            "\"agg_tipos\":{\"terms\":{\"field\":\"nm_tipo_pesquisa_keyword\",\"size\":" + _size + ",\"order\":{\"agg_sum\":\"desc\"}},\"aggs\":{\"agg_sum\":{\"sum\":{\"field\":\"contador\"}}}}" +
                        "}}";
                }
            }
            else
            {
                var _chave = context.Request["chave"];
                var _termo = context.Request["termo"];
                var sOrder = MontarOrdenamento(context);

                if (!string.IsNullOrEmpty(_chave))
                {
                    query = "ch_usuario:" + _chave + "";
                }
                else if (!string.IsNullOrEmpty(_termo) || !string.IsNullOrEmpty(_dt_historico))
                {
                    if (!string.IsNullOrEmpty(_termo))
                    {
                        query = "argumentos.valor:(" + _termo + ")";
                    }
                    if (!string.IsNullOrEmpty(_dt_historico))
                    {
                        if (_op_intervalo == "intervalo" && !string.IsNullOrEmpty(_dt_historico_fim))
                        {
                            _dt_historico = "\\\"" + _dt_historico + " 00:00:00\\\",\\\"" + _dt_historico_fim + " 23:59:59\\\"";
                        }
                        else if (_op_intervalo == "menor" || _op_intervalo == "maiorouigual")
                        {
                            _dt_historico = "\\\"" + _dt_historico + " 00:00:00\\\"";
                        }
                        else if (_op_intervalo == "maior" || _op_intervalo == "menorouigual")
                        {
                            _dt_historico = "\\\"" + _dt_historico + " 23:59:59\\\"";
                        }
                        else if (_op_intervalo == "igual")
                        {
                            _op_intervalo = "intervalo";
                            _dt_historico = "\\\"" + _dt_historico + " 00:00:00\\\",\\\"" + _dt_historico + " 23:59:59\\\"";
                        }
                        query = (!string.IsNullOrEmpty(_termo) ? " AND " : "") + _docEs.MontarArgumentoRangeQueryString("dt_historico", _op_intervalo, _dt_historico);
                        //query = (!string.IsNullOrEmpty(_termo) ? " AND " : "") + "dt_historico:[\\\"" + _dt_historico + " 00:00:00\\\" TO \\\"" + _dt_historico + " 23:59:59\\\"]";
                    }
                }
                if (query == "")
                {
                    query = "*";
                }
                query = "{\"query\":{\"query_string\":{\"query\":\"" + query + "\"}}" + sOrder + "}";
            }
            return query;
        }

        public Result<HistoricoDePesquisaOV> ConsultarEs(HttpContext context)
        {
            var query = MontarConsulta(context);
            var url_es = MontarUrl(context);
            try
            {
                return _docEs.Pesquisar<HistoricoDePesquisaOV>(query, url_es);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar Docs. url_es: " + url_es + ". query: " + query, ex);
            }
        }

        #endregion
    }
}

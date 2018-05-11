using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.OV;
using System.Web;

namespace TCDF.Sinj.ES
{
    public class DiarioBuscaEs
    {
        public BuscaGeralEs MontarBusca(SentencaPesquisaGeralOV sentencaOv)
        {
            var buscaGeral = new BuscaGeralEs();
            if (!string.IsNullOrEmpty(sentencaOv.all))
            {
                sentencaOv.all = new DocEs().TratarCaracteresReservadosDoEs(sentencaOv.all);
                if (!string.IsNullOrEmpty(sentencaOv.all))
                {
                    buscaGeral.searchValue = sentencaOv.all;
                }
            }
            string _sSearch = sentencaOv.search;
            if (!string.IsNullOrEmpty(_sSearch))
            {
                util.BRLight.ManipulaTexto.RemoverAcentuacao(ref _sSearch);
                sentencaOv.search = new DocEs().TratarCaracteresReservadosDoEs(_sSearch);
                if (!string.IsNullOrEmpty(sentencaOv.search))
                {
                    buscaGeral.searchFilter = "((" + sentencaOv.search + ") OR (" + sentencaOv.search + "*))";
                }
            }

            if (!sentencaOv.isCount)
            {
                buscaGeral.order = MontarOrdenamento(sentencaOv.sentencaOrdenamento);
                buscaGeral.sourceInclude = MontarSourceInclude();
                buscaGeral.aggregation = MontarAggregation();
                buscaGeral.highlight = MontarHighlight();
                buscaGeral.highlight.preTag = "_pre_tag_highlight_";
                buscaGeral.highlight.postTag = "_post_tag_highlight_";
                buscaGeral.from = sentencaOv.iDisplayStart;
                buscaGeral.size = sentencaOv.iDisplayLength;
            }
            buscaGeral.fields = MontarSearchableFields();
            FilterQueryString filterQueryString;
            if (sentencaOv.filtros != null)
            {
                string[] filtroSplited;
                foreach (var _filtro in sentencaOv.filtros)
                {
                    filtroSplited = _filtro.Split(':');
                    filterQueryString = new FilterQueryString() { name = filtroSplited[0], value = filtroSplited[1] };
                    if (filterQueryString.name.IndexOf("ano_") == 0)
                    {
                        filterQueryString.isYear = true;
                    }
                    buscaGeral.filtersToQueryString.Add(filterQueryString);
                }
            }

            return buscaGeral;
        }

        public BuscaDiretaEs MontarBusca(SentencaPesquisaDiretaDiarioOV sentencaOv)
        {
            var buscaDireta = new BuscaDiretaEs();

            //var _ch_tipo_fonte = context.Request["ch_tipo_fonte"];
            //var _ch_tipo_edicao = context.Request["ch_tipo_edicao"];
            //var _nr_diario = context.Request["nr_diario"];
            //var _secao_diario = context.Request.Form.GetValues("secao_diario");
            //var _filetext = context.Request["filetext"];
            //var _op_dt_assinatura = context.Request["op_dt_assinatura"];
            //var _dt_assinatura = context.Request.Form.GetValues("dt_assinatura");

            if (!string.IsNullOrEmpty(sentencaOv.filetext))
            {
                sentencaOv.filetext = new DocEs().TratarCaracteresReservadosDoEs(sentencaOv.filetext);
                if (!string.IsNullOrEmpty(sentencaOv.filetext))
                {
                    buscaDireta.searchValue = sentencaOv.filetext;
                    buscaDireta.fields.Add(new SearchableField() { name = "arquivos.arquivo_diario.filetext" });
                }
            }
            string _sSearch = sentencaOv.search;
            if (!string.IsNullOrEmpty(_sSearch))
            {
                util.BRLight.ManipulaTexto.RemoverAcentuacao(ref _sSearch);
                sentencaOv.search = new DocEs().TratarCaracteresReservadosDoEs(_sSearch);

                if (!string.IsNullOrEmpty(sentencaOv.search))
                {
                    buscaDireta.searchFilter = "((" + sentencaOv.search + ") OR (" + sentencaOv.search + "*))";
                }
            }
            if (!sentencaOv.isCount)
            {
                buscaDireta.order = MontarOrdenamento(sentencaOv.sentencaOrdenamento);
                buscaDireta.sourceInclude = MontarSourceInclude();
                buscaDireta.aggregation = MontarAggregation();
                buscaDireta.highlight = MontarHighlight();
                buscaDireta.highlight.preTag = "_pre_tag_highlight_";
                buscaDireta.highlight.postTag = "_post_tag_highlight_";
                buscaDireta.from = sentencaOv.iDisplayStart;
                buscaDireta.size = sentencaOv.iDisplayLength;
            }
            var dictionaryQueryHighlight = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(sentencaOv.ch_tipo_fonte))
            {
                buscaDireta.filtersToQueryFiltered.Add(
                    new FilterQueryFiltered()
                    {
                        name = "ch_tipo_fonte",
                        value = sentencaOv.ch_tipo_fonte,
                        type = TypeFilter.term
                    }
                );
            }
            if (!string.IsNullOrEmpty(sentencaOv.ch_tipo_edicao))
            {
                buscaDireta.filtersToQueryFiltered.Add(
                    new FilterQueryFiltered()
                    {
                        name = "ch_tipo_edicao",
                        value = sentencaOv.ch_tipo_edicao,
                        type = TypeFilter.term
                    }
                );
            }
            if (!string.IsNullOrEmpty(sentencaOv.nr_diario))
            {
                buscaDireta.filtersToQueryFiltered.Add(
                    new FilterQueryFiltered()
                    {
                        name = "nr_diario",
                        @operator = TypeOperator.equal,
                        value = sentencaOv.nr_diario
                    }
                );
                if (!sentencaOv.isCount)
                {
                    dictionaryQueryHighlight.Add("nr_diario", sentencaOv.nr_diario);
                }
            }
            if (sentencaOv.dt_assinatura != null && sentencaOv.dt_assinatura.Length > 0 && !string.IsNullOrEmpty(sentencaOv.dt_assinatura[0]))
            {
                var operador = new UtilBuscaEs().GetOperador(sentencaOv.op_dt_assinatura);
                if (sentencaOv.op_dt_assinatura.Equals("intervalo") && sentencaOv.dt_assinatura.Length == 2)
                {
                    buscaDireta.filtersToQueryFiltered.Add(
                        new FilterQueryFiltered()
                        {
                            name = "dt_assinatura",
                            value = sentencaOv.dt_assinatura[0],
                            @operator = TypeOperator.gte,
                            type = TypeFilter.date
                        }
                    );
                    buscaDireta.filtersToQueryFiltered.Add(
                        new FilterQueryFiltered()
                        {
                            name = "dt_assinatura",
                            value = sentencaOv.dt_assinatura[1],
                            @operator = TypeOperator.lte,
                            type = TypeFilter.date
                        }
                    );
                }
                else
                {
                    buscaDireta.filtersToQueryFiltered.Add(
                        new FilterQueryFiltered()
                        {
                            name = "dt_assinatura",
                            value = sentencaOv.dt_assinatura[0],
                            @operator = new UtilBuscaEs().GetOperador(sentencaOv.op_dt_assinatura),
                            type = TypeFilter.date
                        }
                    );

                    if (sentencaOv.op_dt_assinatura.Equals("igual") && !sentencaOv.isCount)
                    {
                        dictionaryQueryHighlight.Add("dt_assinatura", sentencaOv.dt_assinatura[0]);
                    }
                }
            }
            if (sentencaOv.secao_diario != null && sentencaOv.secao_diario.Length > 0)
            {
                var sSecao = "";
                for (var i = 0; i < sentencaOv.secao_diario.Length; i++)
                {
                    sSecao += (sSecao != "" ? (i < (sentencaOv.secao_diario.Length - 1) ? ", " : " e ") : "") + sentencaOv.secao_diario[i];
                }
                buscaDireta.filtersToQueryFiltered.Add(
                    new FilterQueryFiltered()
                    {
                        name = "secao_diario",
                        @operator = TypeOperator.equal,
                        value = sSecao
                    }
                );
                if (!sentencaOv.isCount)
                {
                    dictionaryQueryHighlight.Add("secao_diario", sSecao);
                }
            }

            if (sentencaOv.filtros != null)
            {
                string[] filtroSplited;
                foreach (var _filtro in sentencaOv.filtros)
                {
                    filtroSplited = _filtro.Split(':');
                    if (filtroSplited[0].IndexOf("ano_") == 0)
                    {
                        buscaDireta.filtersToQueryFiltered.Add(
                            new FilterQueryFiltered()
                            {
                                name = filtroSplited[0].Replace("ano_", "dt_"),
                                value = filtroSplited[1],
                                @operator = TypeOperator.equal,
                                type = TypeFilter.year
                            }
                        );
                    }
                    else
                    {
                        buscaDireta.filtersToQueryFiltered.Add(
                            new FilterQueryFiltered()
                            {
                                name = filtroSplited[0],
                                @operator = TypeOperator.equal,
                                value = filtroSplited[1]
                            }
                        );
                    }
                }
            }

            if (!sentencaOv.isCount)
            {
                foreach (var key in dictionaryQueryHighlight.Keys)
                {
                    var index = buscaDireta.highlight.fields.FindIndex(fh => fh.name == key);
                    if (index > -1)
                    {
                        buscaDireta.highlight.fields[index].query = dictionaryQueryHighlight[key];
                    }
                    else
                    {
                        buscaDireta.highlight.fields.Add(new FieldHighlight() { name = key, query = dictionaryQueryHighlight[key] });
                    }
                }
            }
            return buscaDireta;
        }

        public BuscaDiretaEs MontarBusca(SentencaPesquisaNotifiquemeDiarioOV sentencaOv)
        {
            var buscaDireta = new BuscaDiretaEs();

            //var _ch_tipo_fonte = context.Request["ch_tipo_fonte"];
            //var _filetext = context.Request["filetext"];
            //var _in_exata = context.Request["in_exata"];

            var textoConsultado = sentencaOv.filetext.Replace("\"", "");
            if (textoConsultado.Contains(" "))
            {
                if (sentencaOv.in_exata == "1")
                {
                    buscaDireta.proximitySearchValue = 5;
                }
                buscaDireta.searchValue = textoConsultado;
            }
            if (!string.IsNullOrEmpty(sentencaOv.ch_tipo_fonte))
            {
                buscaDireta.filtersToQueryFiltered.Add(
                    new FilterQueryFiltered()
                    {
                        name = "ch_tipo_fonte",
                        value = sentencaOv.ch_tipo_fonte,
                        type = TypeFilter.term
                    }
                );
            }
            else
            {
                var filterFontes = new FilterQueryFiltered();
                filterFontes.filtersToQueryFiltered.Add(
                    new FilterQueryFiltered()
                    {
                        name = "ch_tipo_fonte",
                        value = "1",
                        type = TypeFilter.term,
                        connector = TypeConnector.OR
                    }
                );
                filterFontes.filtersToQueryFiltered.Add(
                    new FilterQueryFiltered()
                    {
                        name = "ch_tipo_fonte",
                        value = "4",
                        type = TypeFilter.term,
                        connector = TypeConnector.OR
                    }
                );
                filterFontes.filtersToQueryFiltered.Add(
                    new FilterQueryFiltered()
                    {
                        name = "ch_tipo_fonte",
                        value = "11",
                        type = TypeFilter.term,
                        connector = TypeConnector.OR
                    }
                );
                buscaDireta.filtersToQueryFiltered.Add(filterFontes);
            }

            string _sSearch = sentencaOv.search;
            if (!string.IsNullOrEmpty(_sSearch))
            {
                util.BRLight.ManipulaTexto.RemoverAcentuacao(ref _sSearch);
                sentencaOv.search = new DocEs().TratarCaracteresReservadosDoEs(_sSearch);

                if (!string.IsNullOrEmpty(sentencaOv.search))
                {
                    buscaDireta.searchFilter = "((" + sentencaOv.search + ") OR (" + sentencaOv.search + "*))";
                }
            }
            if (!sentencaOv.isCount)
            {
                buscaDireta.order = MontarOrdenamento(sentencaOv.sentencaOrdenamento);
                buscaDireta.sourceInclude = MontarSourceInclude();
                buscaDireta.aggregation = MontarAggregation();
                buscaDireta.highlight.fields.Add(new FieldHighlight() { name = "arquivos.arquivo_diario.filetext", fragments = 8, fragmentSize = 150, noMatchSize = 300 });
                buscaDireta.highlight.fields.Add(new FieldHighlight() { name = "ar_diario.filetext", fragments = 8, fragmentSize = 150, noMatchSize = 300 });
                buscaDireta.from = sentencaOv.iDisplayStart;
                buscaDireta.size = sentencaOv.iDisplayLength;
            }
            return buscaDireta;
        }

        public BuscaDiretaEs MontarBusca(SentencaPesquisaDiretorioDiarioOV sentencaOv)
        {
            var buscaDireta = new BuscaDiretaEs();

            //var _ch_tipo_fonte = context.Request["ch_tipo_fonte"];
            //var _ano = context.Request["ano"];
            //var _mes = context.Request["mes"];

            if (!string.IsNullOrEmpty(sentencaOv.ch_tipo_fonte))
            {
                buscaDireta.filtersToQueryFiltered.Add(
                    new FilterQueryFiltered()
                    {
                        name = "ch_tipo_fonte",
                        value = sentencaOv.ch_tipo_fonte,
                        type = TypeFilter.term
                    }
                );
            }
            if (!string.IsNullOrEmpty(sentencaOv.ano) && !string.IsNullOrEmpty(sentencaOv.mes))
            {
                buscaDireta.filtersToQueryFiltered.Add(
                        new FilterQueryFiltered()
                        {
                            name = "dt_assinatura",
                            value = "01/" + sentencaOv.mes + "/" + sentencaOv.ano,
                            @operator = TypeOperator.gte,
                            type = TypeFilter.date
                        }
                    );
                buscaDireta.filtersToQueryFiltered.Add(
                    new FilterQueryFiltered()
                    {
                        name = "dt_assinatura",
                        value = "01/" + (sentencaOv.mes == "12" ? "01" : (int.Parse(sentencaOv.mes) + 1).ToString()) + "/" + (sentencaOv.mes == "12" ? (int.Parse(sentencaOv.ano) + 1).ToString() : sentencaOv.ano),
                        @operator = TypeOperator.lt,
                        type = TypeFilter.date
                    }
                );
            }
            if (!sentencaOv.isCount)
            {
                buscaDireta.order.Add(new Ordenamento() { campo = "dt_assinatura_untouched", ordem = Ordem.desc });
                buscaDireta.order.Add(new Ordenamento() { campo = "secao_diario", ordem = Ordem.asc });
                buscaDireta.sourceInclude = MontarSourceInclude();
                buscaDireta.from = sentencaOv.iDisplayStart;
                buscaDireta.size = sentencaOv.iDisplayLength;
            }
            return buscaDireta;
        }

        public BuscaDiretaEs MontarBusca(SentencaPesquisaTextoDiarioOV sentencaOv)
        {
            var buscaDireta = new BuscaDiretaEs();

            //var _ch_tipo_fonte = context.Request["ch_tipo_fonte"];
            //var _filetext = context.Request["filetext"];
            //var _intervalo = context.Request["intervalo"];
            //var _dt_assinatura_inicio = context.Request["dt_assinatura_inicio"];
            //var _dt_assinatura_termino = context.Request["dt_assinatura_termino"];
            //var _ano = context.Request["ano"];

            if (!string.IsNullOrEmpty(sentencaOv.filetext))
            {
                sentencaOv.filetext = new DocEs().TratarCaracteresReservadosDoEs(sentencaOv.filetext);
                if (!string.IsNullOrEmpty(sentencaOv.filetext))
                {
                    buscaDireta.searchValue = sentencaOv.filetext;
                    buscaDireta.fields.Add(new SearchableField() { name = "arquivos.arquivo_diario.filetext" });
                    buscaDireta.highlight.preTag = "_pre_tag_highlight_";
                    buscaDireta.highlight.postTag = "_post_tag_highlight_";
                    buscaDireta.highlight.fields.Add(new FieldHighlight() { name = "arquivos.arquivo_diario.filetext", fragments = 5, fragmentSize = 300, noMatchSize = 300 });
                }
            }
            string _sSearch = sentencaOv.search;
            if (!string.IsNullOrEmpty(_sSearch))
            {
                util.BRLight.ManipulaTexto.RemoverAcentuacao(ref _sSearch);
                sentencaOv.search = new DocEs().TratarCaracteresReservadosDoEs(_sSearch);

                if (!string.IsNullOrEmpty(sentencaOv.search))
                {
                    buscaDireta.searchFilter = "((" + sentencaOv.search + ") OR (" + sentencaOv.search + "*))";
                }
            }
            buscaDireta.order = MontarOrdenamento(sentencaOv.sentencaOrdenamento);
            buscaDireta.sourceInclude = MontarSourceInclude();
            buscaDireta.aggregation = MontarAggregation();


            if (!string.IsNullOrEmpty(sentencaOv.ch_tipo_fonte))
            {
                buscaDireta.filtersToQueryFiltered.Add(
                    new FilterQueryFiltered()
                    {
                        name = "ch_tipo_fonte",
                        value = sentencaOv.ch_tipo_fonte,
                        type = TypeFilter.term
                    }
                );
            }
            if (sentencaOv.intervalo.Equals("1"))
            {
                if (!string.IsNullOrEmpty(sentencaOv.dt_assinatura_inicio))
                {
                    buscaDireta.filtersToQueryFiltered.Add(
                        new FilterQueryFiltered()
                        {
                            name = "dt_assinatura",
                            value = sentencaOv.dt_assinatura_inicio,
                            @operator = TypeOperator.gte,
                            type = TypeFilter.date
                        }
                    );
                }
                if (!string.IsNullOrEmpty(sentencaOv.dt_assinatura_termino))
                {
                    buscaDireta.filtersToQueryFiltered.Add(
                        new FilterQueryFiltered()
                        {
                            name = "dt_assinatura",
                            value = sentencaOv.dt_assinatura_termino,
                            @operator = TypeOperator.lte,
                            type = TypeFilter.date
                        }
                    );
                }
            }
            else
            {
                buscaDireta.filtersToQueryFiltered.Add(
                    new FilterQueryFiltered()
                    {
                        name = "dt_assinatura",
                        value = sentencaOv.dt_assinatura_inicio,
                        type = TypeFilter.term
                    }
                );
            }
            if (!string.IsNullOrEmpty(sentencaOv.ano))
            {
                if (!string.IsNullOrEmpty(sentencaOv.dt_assinatura_inicio))
                {
                    buscaDireta.filtersToQueryFiltered.Add(
                        new FilterQueryFiltered()
                        {
                            name = "dt_assinatura",
                            value = "01/01/" + sentencaOv.ano,
                            @operator = TypeOperator.gte,
                            type = TypeFilter.date
                        }
                    );
                }
                if (!string.IsNullOrEmpty(sentencaOv.dt_assinatura_termino))
                {
                    buscaDireta.filtersToQueryFiltered.Add(
                        new FilterQueryFiltered()
                        {
                            name = "dt_assinatura",
                            value = "31/12/" + sentencaOv.ano,
                            @operator = TypeOperator.lte,
                            type = TypeFilter.date
                        }
                    );
                }
            }
            return buscaDireta;
        }


        public BuscaByIdEs MontarBusca(SentencaPesquisaCestaOV sentencaOv)
        {
            var buscaIds = new BuscaByIdEs();

            if (!sentencaOv.isCount)
            {
                buscaIds.order = MontarOrdenamento(sentencaOv.sentencaOrdenamento);
                buscaIds.sourceInclude = MontarSourceInclude();
                buscaIds.from = sentencaOv.iDisplayStart;
                buscaIds.size = sentencaOv.iDisplayLength;
            }

            var aCesta = new string[0];
            if (!string.IsNullOrEmpty(sentencaOv.cesta))
            {
                aCesta = sentencaOv.cesta.Split(',');
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
                if (sCesta_split[0] == sentencaOv.@base)
                {
                    buscaIds.ids.Add(sCesta_split.Last<string>());
                }
            }
            return buscaIds;
        }

        /// <summary>
        /// Define os campos, e a relevancia deles, que serão pesquisados quando a o valor buscado é em todos os campos
        /// </summary>
        /// <returns></returns>
        private List<SearchableField> MontarSearchableFields()
        {
            List<SearchableField> fields = new List<SearchableField>();

            fields.Add(new SearchableField() { name = "nm_tipo_fonte", boost = 2 });
            fields.Add(new SearchableField() { name = "nm_tipo_edicao", boost = 2 });
            fields.Add(new SearchableField() { name = "nr_diario_text", boost = 2 });
            fields.Add(new SearchableField() { name = "dt_assinatura_text", boost = 2 });
            fields.Add(new SearchableField() { name = "ar_diario.filetext" });
            fields.Add(new SearchableField() { name = "arquivos.arquivo_diario.filetext" });

            return fields;
        }

        /// <summary>
        /// Define os campos que serão agrupados (quantidade de registros para cada valor existente em cada campo) para a criação dos filtros de pesquisa
        /// </summary>
        /// <returns></returns>
        private List<Agg> MontarAggregation()
        {
            List<Agg> aggregation = new List<Agg>();
            aggregation.Add(new Agg() { field = "nm_tipo_fonte_keyword", name = "nm_tipo_fonte_keyword" });
            aggregation.Add(new Agg() { field = "dt_assinatura", name = "ano_assinatura", interval = "year", format = "yyyy", type = TypeAggregation.date_histogram, ordenamento = new Ordenamento() { campo = "_key", ordem = Ordem.desc }, size = 0 });
            return aggregation;
        }

        /// <summary>
        /// Define o ordenamento do resultado da pesquisa
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private List<Ordenamento> MontarOrdenamento(SentencaOrdenamentoOV sentencaOrdenamento)
        {
            //var _sSortCol = context.Request["iSortCol_0"];
            //var iSortCol = 0;
            //var _sSortDir = "";
            //var _sColOrder = "";
            //if (!string.IsNullOrEmpty(_sSortCol))
            //{
            //    int.TryParse(_sSortCol, out iSortCol);
            //    _sSortDir = context.Request["sSortDir_0"];
            //    _sColOrder = context.Request["mDataProp_" + iSortCol];
            //}
            ////Quando relatorio esse parametro pode ser passado, o que define o ordemaneto da pesquisa, então  atribuo seus valores à _sColOrder e iSortDir
            //var _order = context.Request["order"];
            //if (!string.IsNullOrEmpty(_order))
            //{
            //    var order_split = _order.Split(',');
            //    if (order_split.Length == 2)
            //    {
            //        _sColOrder = order_split[0];
            //        _sSortDir = order_split[1];
            //    }
            //}
            var order = new List<Ordenamento>();
            //Se não tiver _sSortCol então não foi selecionado nenhum ordenamento, log devemos ordenar por dt_doc que é o ordenamento padrão
            if (string.IsNullOrEmpty(sentencaOrdenamento.sColOrder))
            {
                order.Add(new Ordenamento() { campo = "_score", ordem = Ordem.desc });
                order.Add(new Ordenamento() { campo = "dt_assinatura_untouched", ordem = Ordem.desc });
                order.Add(new Ordenamento() { campo = "secao_diario", ordem = Ordem.asc });
            }
            else
            {
                if (sentencaOrdenamento.sColOrder == "dt_assinatura")
                {
                    sentencaOrdenamento.sColOrder = "dt_assinatura_untouched";
                }
                else if (sentencaOrdenamento.sColOrder == "nr_diario")
                {
                    sentencaOrdenamento.sColOrder = "nr_diario_untouched";
                }

                if ("desc" == sentencaOrdenamento.sSortDir)
                {
                    order.Add(new Ordenamento() { campo = sentencaOrdenamento.sColOrder, ordem = Ordem.desc });
                }
                else
                {
                    order.Add(new Ordenamento() { campo = sentencaOrdenamento.sColOrder, ordem = Ordem.asc });
                }

            }
            return order;
        }

        /// <summary>
        /// Define os campos que retornarão na pesquisa
        /// </summary>
        /// <returns></returns>
        private List<string> MontarSourceInclude()
        {
            List<string> sourceInclude = new List<string>();
            sourceInclude.Add("_metadata.id_doc");
            sourceInclude.Add("ch_diario");
            sourceInclude.Add("nm_tipo_fonte");
            sourceInclude.Add("nm_tipo_edicao");
            sourceInclude.Add("nm_diferencial_edicao");
            sourceInclude.Add("nr_diario");
            sourceInclude.Add("cr_diario");
            sourceInclude.Add("secao_diario");
            sourceInclude.Add("dt_assinatura");
            sourceInclude.Add("st_pendente");
            sourceInclude.Add("nm_diferencial_suplemento");
            sourceInclude.Add("st_suplemento");
            sourceInclude.Add("ar_diario.id_file");
            sourceInclude.Add("ar_diario.filesize");
            sourceInclude.Add("ar_diario.filename");
            sourceInclude.Add("arquivos.arquivo_diario.id_file");
            sourceInclude.Add("arquivos.arquivo_diario.filesize");
            sourceInclude.Add("arquivos.arquivo_diario.filename");
            sourceInclude.Add("arquivos.ds_arquivo");
            return sourceInclude;
        }

        private Highlight MontarHighlight(string query = "")
        {
            Highlight highlight = new Highlight();
            highlight.fields.Add(new FieldHighlight() { query = query, name = "nm_tipo_fonte" });
            highlight.fields.Add(new FieldHighlight() { query = query, name = "nr_diario_text" });
            highlight.fields.Add(new FieldHighlight() { query = query, name = "dt_assinatura_text" });
            highlight.fields.Add(new FieldHighlight() { query = query, name = "arquivos.arquivo_diario.filetext", fragments = 8, fragmentSize = 150, noMatchSize = 300 });
            highlight.fields.Add(new FieldHighlight() { query = query, name = "ar_diario.filetext", fragments = 8, fragmentSize = 150, noMatchSize = 300 });
            return highlight;
        }
    }
}

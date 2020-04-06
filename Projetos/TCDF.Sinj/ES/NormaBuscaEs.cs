using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.OV;
using System.Web;
using TCDF.Sinj.AD;
using TCDF.Sinj.RN;

namespace TCDF.Sinj.ES
{
    public class NormaBuscaEs
    {
        public BuscaDiretaEs MontarBusca(SentencaPesquisaAvancadaNormaOV sentencaOv)
        {
            var buscaAvancada = new BuscaDiretaEs();
            string _sSearch = sentencaOv.search;
            if (!string.IsNullOrEmpty(_sSearch))
            {
                util.BRLight.ManipulaTexto.RemoverAcentuacao(ref _sSearch);
                sentencaOv.search = new DocEs().TratarCaracteresReservadosDoEs(_sSearch);
                if (!string.IsNullOrEmpty(sentencaOv.search))
                {
                    buscaAvancada.searchFilter = "((" + sentencaOv.search + ") OR (" + sentencaOv.search + "*))";
                }
            }
            if (!sentencaOv.isCount)
            {
                buscaAvancada.order = MontarOrdenamento(sentencaOv.sentencaOrdenamento);
                buscaAvancada.sourceInclude = MontarSourceInclude();
                buscaAvancada.aggregation = MontarAggregation();
                buscaAvancada.from = sentencaOv.iDisplayStart;
                buscaAvancada.size = sentencaOv.iDisplayLength;
            }
            buscaAvancada.fields = MontarSearchableFields();

            if (sentencaOv.ch_tipo_norma != null)
            {
                var filterIntervalo = new FilterQueryFiltered()
                {
                    connector = TypeConnector.AND
                };
                
                foreach (var ch in sentencaOv.ch_tipo_norma)
                {
                    if (ch.Equals("todas")) continue;

                    filterIntervalo.filtersToQueryFiltered.Add(
                        new FilterQueryFiltered()
                        {
                            name = "ch_tipo_norma",
                            value = ch,
                            @operator = TypeOperator.equal,
                            connector = TypeConnector.OR
                        }
                    );
                }
                if (filterIntervalo.filtersToQueryFiltered.Count > 0)
                {
                    buscaAvancada.filtersToQueryFiltered.Add(filterIntervalo);
                }
            }

            if (sentencaOv.argumentos != null)
            {
                var queryHighlight = new Dictionary<string, string>();
                var argumento_split = new string[0];
                var _tipo_campo = "";
                var _ch_campo = "";
                var _ch_operador = "";
                var _ch_valor = "";
                var aux_conector = "";
                //guarda o conector do último argumento para concatenar com o proximo
                foreach (var argumento in sentencaOv.argumentos)
                {
                    //Define o conector com o último da lista
                    TypeConnector connector = aux_conector.Equals("NÃO") ? TypeConnector.NOT : aux_conector.Equals("OU") ? TypeConnector.OR : TypeConnector.AND;

                    argumento_split = argumento.Split('#');
                    aux_conector = argumento_split[7];
                    
                    if (argumento_split.Length == 8)
                    {
                        _tipo_campo = argumento_split[0];
                        _ch_campo = argumento_split[1];
                        _ch_operador = argumento_split[3];
                        _ch_valor = argumento_split[5];
                        var operador = new UtilBuscaEs().GetOperador(_ch_operador);
                        if (_tipo_campo == "autocomplete")
                        {
                            buscaAvancada.filtersToQueryFiltered.Add(
                                new FilterQueryFiltered()
                                {
                                    name = _ch_campo,
                                    value = _ch_valor,
                                    @operator = operador,
                                    connector = connector
                                }
                            );
                        }
                        else if (_tipo_campo == "text")
                        {
                            _ch_valor = new DocEs().TratarCaracteresReservadosDoEs(_ch_valor);
                            if (_ch_campo == "all")
                            {
                                buscaAvancada.filtersToQueryFiltered.Add(
                                    new FilterQueryFiltered()
                                    {
                                        isAllFields = true,
                                        value = _ch_valor,
                                        connector = connector,
                                        @operator = operador
                                    }
                                );
                            }
                            else
                            {
                                buscaAvancada.filtersToQueryFiltered.Add(
                                    new FilterQueryFiltered()
                                    {
                                        name = _ch_campo,
                                        value = _ch_valor,
                                        connector = connector,
                                        @operator = operador
                                    }
                                );
                            }

                            if (!sentencaOv.isCount)
                            {
                                if (queryHighlight.ContainsKey(_ch_campo))
                                {
                                    queryHighlight[_ch_campo] = queryHighlight[_ch_campo] + " OR " + string.Format("({0})", operador.Equals(TypeOperator.equal) ? "\\\"" + _ch_valor + "\\\"" : _ch_valor);
                                }
                                else
                                {
                                    queryHighlight.Add(_ch_campo, string.Format("({0})", operador.Equals(TypeOperator.equal) ? "\\\"" + _ch_valor + "\\\"" : _ch_valor));
                                }
                            }
                        }
                        else if (_tipo_campo == "number" || _tipo_campo == "date")
                        {
                            if (_ch_campo == "dt_assinatura")//foi atualizado o nome do parametro de "ano_assinatura" para "dt_assinatura" para que entrasse na condição by: Wemerson
                            {
                                if (_ch_operador == "intervalo")
                                {
                                    var ch_valor = _ch_valor.Split(',');
                                    if (ch_valor.Length == 2)
                                    {
                                        var filterIntervalo = new FilterQueryFiltered()
                                        {
                                            connector = connector
                                        };
                                        filterIntervalo.filtersToQueryFiltered.Add(
                                            new FilterQueryFiltered()
                                            {
                                                name = "dt_assinatura",
                                                value = ch_valor[0],
                                                connector = TypeConnector.AND,
                                                @operator = TypeOperator.gte,
                                                type = TypeFilter.year
                                            }
                                        );
                                        filterIntervalo.filtersToQueryFiltered.Add(
                                            new FilterQueryFiltered()
                                            {
                                                name = "dt_assinatura",
                                                value = ch_valor[1],
                                                connector = TypeConnector.AND,
                                                @operator = TypeOperator.lte,
                                                type = TypeFilter.year
                                            }
                                        );
                                        buscaAvancada.filtersToQueryFiltered.Add(filterIntervalo);
                                    }
                                }
                                else
                                {
                                    buscaAvancada.filtersToQueryFiltered.Add(
                                        new FilterQueryFiltered()
                                        {
                                            name = "dt_assinatura",
                                            value = _ch_valor,
                                            connector = connector,
                                            @operator = operador,
                                            type = TypeFilter.year
                                        }
                                    );
                                }
                            }
                            else
                            {
                                buscaAvancada.filtersToQueryFiltered.Add(
                                    new FilterQueryFiltered()
                                    {
                                        name = _ch_campo,
                                        value = _ch_valor,
                                        connector = connector,
                                        @operator = operador,
                                        type = TypeFilter.num
                                    }
                                );
                            }
                        }
                        else if (_tipo_campo == "datetime")
                        {
                            var ch_valor_splited = _ch_valor.Split(',');
                            if (_ch_operador == "intervalo" || _ch_operador == "igual" || _ch_operador == "diferente")
                            {
                                if (ch_valor_splited.Length == 2)
                                {
                                    if (ch_valor_splited[0].IndexOf(' ') < 0 && ch_valor_splited[1].IndexOf(' ') < 0)
                                    {
                                        ch_valor_splited[0] += " 00:00:00";
                                        ch_valor_splited[1] += " 23:59:59";
                                    }
                                }
                                else
                                {
                                    if (ch_valor_splited[0].IndexOf(' ') < 0)
                                    {
                                        ch_valor_splited = new string[] { ch_valor_splited[0] + " 00:00:00", ch_valor_splited[0] + " 23:59:59" };
                                    }
                                }
                                var filterIntervalo = new FilterQueryFiltered()
                                {
                                    connector = connector
                                };
                                filterIntervalo.filtersToQueryFiltered.Add(
                                    new FilterQueryFiltered()
                                    {
                                        name = _ch_campo,
                                        value = ch_valor_splited[0],
                                        connector = TypeConnector.AND,
                                        @operator = TypeOperator.gte,
                                        type = TypeFilter.datetime
                                    }
                                );
                                filterIntervalo.filtersToQueryFiltered.Add(
                                    new FilterQueryFiltered()
                                    {
                                        name = _ch_campo,
                                        value = ch_valor_splited[1],
                                        connector = TypeConnector.AND,
                                        @operator = TypeOperator.lte,
                                        type = TypeFilter.datetime
                                    }
                                );
                                buscaAvancada.filtersToQueryFiltered.Add(filterIntervalo);
                            }
                            else
                            {
                                if (ch_valor_splited[0].IndexOf(' ') < 0)
                                {
                                    if (_ch_operador == "menor" || _ch_operador == "maiorouigual")
                                    {
                                        ch_valor_splited[0] += " 00:00:00";
                                    }
                                    else if (_ch_operador == "maior" || _ch_operador == "menorouigual")
                                    {
                                        ch_valor_splited[0] += " 23:59:59";
                                    }
                                }
                                buscaAvancada.filtersToQueryFiltered.Add(
                                    new FilterQueryFiltered()
                                    {
                                        name = _ch_campo,
                                        value = ch_valor_splited[0],
                                        connector = connector,
                                        @operator = operador,
                                        type = TypeFilter.datetime
                                    }
                                );
                            }
                        }
                    }
                }

                if (!sentencaOv.isCount)
                {
                    if (!string.IsNullOrEmpty(buscaAvancada.searchFilter))
                    {
                        if (queryHighlight.ContainsKey("_all"))
                        {
                            queryHighlight["_all"] = queryHighlight["_all"] + " OR " + string.Format("{0}", buscaAvancada.searchFilter);
                        }
                        else
                        {
                            queryHighlight.Add("_all", buscaAvancada.searchFilter);
                        }
                    }
                    if (queryHighlight.ContainsKey("_all"))
                    {
                        buscaAvancada.highlight = MontarHighlight(queryHighlight["_all"]);
                    }
                }

                new UtilBuscaEs().MontarFiltroBuscaDireta(sentencaOv.filtros, buscaAvancada);
                if (!sentencaOv.isCount)
                {
                    foreach (var item in queryHighlight)
                    {
                        if (item.Key.Equals("_all")) continue;
                        if (buscaAvancada.highlight.fields.Count<FieldHighlight>(f => f.name == item.Key) > 0)
                        {
                            var index = buscaAvancada.highlight.fields.FindIndex(f => f.name == item.Key);
                            buscaAvancada.highlight.fields[index].query = buscaAvancada.highlight.fields[index].query + " OR (" + item.Value + ")";
                        }
                        else
                        {
                            buscaAvancada.highlight.fields.Add(new FieldHighlight() { query = item.Value, name = item.Key });
                        }
                    }
                }
            }
            return buscaAvancada;
        }

        public BuscaGeralEs MontarBusca(SentencaPesquisaGeralOV sentencaOv)
        {
            var buscaGeral = new BuscaGeralEs();
            if (!string.IsNullOrEmpty(sentencaOv.all))
            {
                var auxAll = "";
                if (sentencaOv.all.IndexOf("\"") < 0)
                {
                    auxAll = "\\\"" + sentencaOv.all + "\\\"";
                }
                sentencaOv.all = new DocEs().TratarCaracteresReservadosDoEs(sentencaOv.all);
                if (!string.IsNullOrEmpty(sentencaOv.all))
                {
                    if (auxAll != "")
                    {
                        sentencaOv.all = "((" + sentencaOv.all + ") OR (" + auxAll + "))";
                    }
                    buscaGeral.searchValue = sentencaOv.all;
                }
            }
            if (!string.IsNullOrEmpty(sentencaOv.search))
            {
                var sSearch = sentencaOv.search;
                util.BRLight.ManipulaTexto.RemoverAcentuacao(ref sSearch);
                sentencaOv.search = new DocEs().TratarCaracteresReservadosDoEs(sSearch);
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
                buscaGeral.from = sentencaOv.iDisplayStart;
                buscaGeral.size = sentencaOv.iDisplayLength;
            }

            buscaGeral.fields = MontarSearchableFields();

            new UtilBuscaEs().MontarFiltroBuscaGeral(sentencaOv.filtros, buscaGeral);

            return buscaGeral;
        }

        //Pendente de publicacao
        public BuscaPendenteDePublicacaoEs MontarBusca(SentencaPesquisaPendenteDePublicacaoOV sentencaOv)
        {
            var buscaPendenteDeEnvioEs = new BuscaPendenteDePublicacaoEs();
            if (!string.IsNullOrEmpty(sentencaOv.all))
            {
                var auxAll = "";
                if (sentencaOv.all.IndexOf("\"") < 0)
                {
                    auxAll = "\\\"" + sentencaOv.all + "\\\"";
                }
                sentencaOv.all = new DocEs().TratarCaracteresReservadosDoEs(sentencaOv.all);
                if (!string.IsNullOrEmpty(sentencaOv.all))
                {
                    if (auxAll != "")
                    {
                        sentencaOv.all = "((" + sentencaOv.all + ") OR (" + auxAll + "))";
                    }
                    buscaPendenteDeEnvioEs.searchValue = sentencaOv.all;
                }
            }
            if (!string.IsNullOrEmpty(sentencaOv.search))
            {
                var sSearch = sentencaOv.search;
                util.BRLight.ManipulaTexto.RemoverAcentuacao(ref sSearch);
                sentencaOv.search = new DocEs().TratarCaracteresReservadosDoEs(sSearch);
                if (!string.IsNullOrEmpty(sentencaOv.search))
                {
                    buscaPendenteDeEnvioEs.searchFilter = "((" + sentencaOv.search + ") OR (" + sentencaOv.search + "*))";
                }
            }
            if (!sentencaOv.isCount)
            {
                buscaPendenteDeEnvioEs.order = MontarOrdenamento(sentencaOv.sentencaOrdenamento);
                buscaPendenteDeEnvioEs.sourceInclude = MontarSourceInclude();
                buscaPendenteDeEnvioEs.aggregation = MontarAggregation();
                buscaPendenteDeEnvioEs.highlight = MontarHighlight();
                buscaPendenteDeEnvioEs.from = sentencaOv.iDisplayStart;
                buscaPendenteDeEnvioEs.size = sentencaOv.iDisplayLength;
            }

            buscaPendenteDeEnvioEs.fields = MontarSearchableFields();

            new UtilBuscaPendenteDePublicacaoEs().MontarFiltroBuscaGeral(sentencaOv.filtros, buscaPendenteDeEnvioEs);

            return buscaPendenteDeEnvioEs;
        }


        public BuscaDiretaEs MontarBusca(SentencaPesquisaDiretaNormaOV sentencaOv)
        {
            var buscaDireta = new BuscaDiretaEs();
            if (!string.IsNullOrEmpty(sentencaOv.all))
            {
                sentencaOv.all = new DocEs().TratarCaracteresReservadosDoEs(sentencaOv.all);
                if (!string.IsNullOrEmpty(sentencaOv.all))
                {
                    buscaDireta.searchValue = sentencaOv.all;
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
                buscaDireta.from = sentencaOv.iDisplayStart;
                buscaDireta.size = sentencaOv.iDisplayLength;
            }
            buscaDireta.fields = MontarSearchableFields();
            var dictionaryQueryHighlight = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(sentencaOv.ch_tipo_norma))
            {
                buscaDireta.filtersToQueryFiltered.Add(
                    new FilterQueryFiltered()
                    {
                        name = "ch_tipo_norma",
                        value = sentencaOv.ch_tipo_norma,
                        @operator = TypeOperator.equal
                    }
                );
            }
            if (!string.IsNullOrEmpty(sentencaOv.st_habilita_pesquisa))
            {
                buscaDireta.filtersToQueryFiltered.Add(
                    new FilterQueryFiltered()
                    {
                        name = "st_habilita_pesquisa",
                        value = sentencaOv.st_habilita_pesquisa,
                        @operator = TypeOperator.equal
                    }
                );
            }
            if (!string.IsNullOrEmpty(sentencaOv.ano_assinatura))
            {
                buscaDireta.filtersToQueryFiltered.Add(
                    new FilterQueryFiltered()
                    {
                        name = "dt_assinatura",
                        value = sentencaOv.ano_assinatura,
                        @operator = TypeOperator.equal,
                        type = TypeFilter.year
                    }
                );
                if (!sentencaOv.isCount)
                {
                    dictionaryQueryHighlight.Add("dt_assinatura_text", sentencaOv.ano_assinatura);
                }
            }
            if (sentencaOv.ch_termos != null && sentencaOv.ch_termos.Length > 0)
            {
                var filter = new FilterQueryFiltered();
                foreach (var ch_termo in sentencaOv.ch_termos)
                {
                    filter.filtersToQueryFiltered.Add(
                        new FilterQueryFiltered()
                        {
                            name = "ch_termo",
                            value = ch_termo,
                            @operator = TypeOperator.equal,
                            connector = TypeConnector.OR
                        }
                    );
                }
                buscaDireta.filtersToQueryFiltered.Add(filter);
            }
            if (!string.IsNullOrEmpty(sentencaOv.ch_orgao))
            {
                var filterOrgao = new FilterQueryFiltered();
                if (string.IsNullOrEmpty(sentencaOv.origem_por))
                {
                    sentencaOv.origem_por = "toda_a_hierarquia_em_qualquer_epoca1";
                }
                var orgaos = new List<OrgaoOV>();
                switch (sentencaOv.origem_por)
                {
                    case "toda_a_hierarquia_em_qualquer_epoca1":
                        orgaos = new OrgaoRN().BuscarTodaHierarquiaEmQualquerEpoca(sentencaOv.ch_orgao, sentencaOv.ch_hierarquia);
                        break;
                    case "hierarquia_superior":
                        orgaos = new OrgaoRN().BuscarHierarquiaSuperior(sentencaOv.ch_hierarquia);
                        break;
                    case "hierarquia_inferior":
                        orgaos = new OrgaoRN().BuscarHierarquiaInferior(sentencaOv.ch_orgao);
                        break;
                    case "toda_a_hierarquia":
                        orgaos = new OrgaoRN().BuscarOrgaosDaHierarquia(sentencaOv.ch_orgao, sentencaOv.ch_hierarquia);
                        break;
                    case "em_qualquer_epoca":
                        orgaos = new OrgaoRN().BuscarOrgaosDaCronologia(sentencaOv.ch_orgao);
                        break;
                }
                if (orgaos != null && orgaos.Count > 0)
                {
                    foreach (var orgao in orgaos)
                    {
                        filterOrgao.filtersToQueryFiltered.Add(
                            new FilterQueryFiltered()
                            {
                                name = "ch_orgao",
                                value = orgao.ch_orgao,
                                @operator = TypeOperator.equal,
                                connector = TypeConnector.OR
                            }
                        );
                    }
                    buscaDireta.filtersToQueryFiltered.Add(filterOrgao);
                }
                else
                {
                    buscaDireta.filtersToQueryFiltered.Add(
                        new FilterQueryFiltered()
                        {
                            name = "ch_orgao",
                            value = sentencaOv.ch_orgao,
                            @operator = TypeOperator.equal
                        }
                    );
                }
            }
            if (!string.IsNullOrEmpty(sentencaOv.nr_norma))
            {
                buscaDireta.filtersToQueryFiltered.Add(
                    new FilterQueryFiltered()
                    {
                        names = new List<string>() { "nr_norma", "nr_norma_numeric" },
                        value = sentencaOv.nr_norma
                    }
                );
                if (!sentencaOv.isCount)
                {
                    dictionaryQueryHighlight.Add("nr_norma", sentencaOv.nr_norma);
                }
            }
            else if (sentencaOv.norma_sem_numero == "true")
            {
                var filterSemNumero = new FilterQueryFiltered();
                filterSemNumero.filtersToQueryFiltered.Add(
                    new FilterQueryFiltered()
                    {
                        name = "nr_norma",
                        value = "0"
                    }
                );
                filterSemNumero.filtersToQueryFiltered.Add(
                    new FilterQueryFiltered()
                    {
                        name = "nr_norma",
                        type = TypeFilter.missing
                    }
                );
            }

            new UtilBuscaEs().MontarFiltroBuscaDireta(sentencaOv.filtros, buscaDireta);

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

        public BuscaGeralEs MontarBusca(SentencaPesquisaFavoritosOV sentencaOv)
        {
            var buscaGeral = new BuscaGeralEs();

            //var notifiquemeRn = new NotifiquemeRN();
            //var sessaoNotifiquemeOv = notifiquemeRn.LerSessaoNotifiquemeOv();
            //var notifiquemeOv = notifiquemeRn.Doc(sessaoNotifiquemeOv.email_usuario_push);
            string chaves = "";
            foreach (var favorito in sentencaOv.favoritos)
            {
                var favorito_splited = favorito.Split('_');
                if (favorito_splited[0] == sentencaOv.@base)
                {
                    chaves += (chaves != "" ? " OR " : "") + favorito_splited[1];
                }
            }
            if (chaves != "")
            {
                buscaGeral.searchValue = "ch_norma:(" + chaves + ")";
            }
            if (!sentencaOv.isCount)
            {
                buscaGeral.sourceInclude = MontarSourceInclude();
                buscaGeral.from = sentencaOv.iDisplayStart;
                buscaGeral.size = sentencaOv.iDisplayLength;
            }

            return buscaGeral;
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

            fields.Add(new SearchableField() { name = "nm_tipo_norma", boost = 70 });
            fields.Add(new SearchableField() { name = "nr_norma", boost = 2 });
            fields.Add(new SearchableField() { name = "dt_assinatura_text", boost = 2 });
            fields.Add(new SearchableField() { name = "rankeamentos", boost = 3 });
            fields.Add(new SearchableField() { name = "nm_apelido", boost = 2 });
            fields.Add(new SearchableField() { name = "ds_ementa", boost = 3 });
            fields.Add(new SearchableField() { name = "nm_orgao", boost = 2 });
            fields.Add(new SearchableField() { name = "sg_orgao", boost = 2 });
            fields.Add(new SearchableField() { name = "nm_termo", boost = 3 });
            fields.Add(new SearchableField() { name = "nm_pessoa_fisica_e_juridica", boost = 2 });
            fields.Add(new SearchableField() { name = "ds_observacao", boost = 2 });
            fields.Add(new SearchableField() { name = "dt_autuacao_text" });
            fields.Add(new SearchableField() { name = "nm_situacao" });
            fields.Add(new SearchableField() { name = "nm_ambito" });
            fields.Add(new SearchableField() { name = "ar_atualizado.filetext" });
            fields.Add(new SearchableField() { name = "fontes.ar_fonte.filetext" });
            fields.Add(new SearchableField() { name = "st_habilita_pesquisa", boost = 3 });

            return fields;
        }

        /// <summary>
        /// Define os campos que serão agrupados (quantidade de registros para cada valor existente em cada campo) para a criação dos filtros de pesquisa
        /// </summary>
        /// <returns></returns>
        private List<Agg> MontarAggregation()
        {
            List<Agg> aggregation = new List<Agg>();
            aggregation.Add(new Agg() { field = "sg_orgao_keyword", name = "sg_orgao_keyword" });
            aggregation.Add(new Agg() { field = "nm_tipo_norma_keyword", name = "nm_tipo_norma_keyword" });
            aggregation.Add(new Agg() { field = "nm_situacao_keyword", name = "nm_situacao_keyword" });
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
            //var _sSortCol = pesquisaGeral.iSortCol_0;
            //var iSortCol = 0;
            //var _sSortDir = "";
            //var _sColOrder = "";
            //if (!string.IsNullOrEmpty(_sSortCol))
            //{
            //    int.TryParse(_sSortCol, out iSortCol);
            //    _sSortDir = context.Request["sSortDir_0"];
            //    _sColOrder = context.Request["mDataProp_" + iSortCol];
            //}
            //Quando relatorio esse parametro pode ser passado, o que define o ordemaneto da pesquisa, então  atribuo seus valores à _sColOrder e iSortDir
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
            //throw new NotImplementedException("fazer a lógica de _order.Split(',') na camada da aplicacao");

            var order = new List<Ordenamento>();
            //Se não tiver _sSortCol então não foi selecionado nenhum ordenamento, log devemos ordenar por dt_doc que é o ordenamento padrão
            if (string.IsNullOrEmpty(sentencaOrdenamento.sColOrder))
            {
                order.Add(new Ordenamento() { campo = "_score", ordem = Ordem.desc });
                order.Add(new Ordenamento() { campo = "dt_assinatura_untouched", ordem = Ordem.desc });
            }
            else
            {
                if (sentencaOrdenamento.sColOrder == "dt_assinatura")
                {
                    sentencaOrdenamento.sColOrder = "dt_assinatura_untouched";
                }

                if ("desc" == sentencaOrdenamento.sSortDir)
                {
                    if (sentencaOrdenamento.sColOrder == "nm_tipo_norma")
                    {
                        order.Add(new Ordenamento() { campo = "nm_tipo_norma_untouched", ordem = Ordem.desc });
                        order.Add(new Ordenamento() { campo = "nr_norma_untouched", ordem = Ordem.desc });
                    }
                    else
                    {
                        order.Add(new Ordenamento() { campo = sentencaOrdenamento.sColOrder, ordem = Ordem.desc });
                    }
                }
                else
                {
                    if (sentencaOrdenamento.sColOrder == "nm_tipo_norma")
                    {
                        order.Add(new Ordenamento() { campo = "nm_tipo_norma_untouched", ordem = Ordem.asc });
                        order.Add(new Ordenamento() { campo = "nr_norma_untouched", ordem = Ordem.asc });
                    }
                    else
                    {
                        order.Add(new Ordenamento() { campo = sentencaOrdenamento.sColOrder, ordem = Ordem.asc });
                    }
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
            sourceInclude.Add("origens.nm_orgao");
            sourceInclude.Add("origens.ch_orgao");
            sourceInclude.Add("origens.sg_orgao");
            sourceInclude.Add("ar_atualizado.id_file");
            sourceInclude.Add("ar_atualizado.filesize");
            sourceInclude.Add("ar_atualizado.mimetype");
            sourceInclude.Add("ar_atualizado.filename");
            sourceInclude.Add("fontes.nm_tipo_publicacao");
            sourceInclude.Add("fontes.ar_fonte.id_file");
            sourceInclude.Add("fontes.ar_fonte.filesize");
            sourceInclude.Add("fontes.ar_fonte.filename");
            sourceInclude.Add("fontes.ar_fonte.mimetype");
            sourceInclude.Add("nm_tipo_norma");
            sourceInclude.Add("nr_norma");
            sourceInclude.Add("ch_norma");
            sourceInclude.Add("dt_assinatura");
            sourceInclude.Add("ds_ementa");
            sourceInclude.Add("nm_situacao");
            sourceInclude.Add("dt_inicio_vigencia");
            sourceInclude.Add("st_vacatio_legis");
            sourceInclude.Add("ds_vacatio_legis");
            sourceInclude.Add("st_habilita_pesquisa");
            sourceInclude.Add("st_habilita_email");
            return sourceInclude;
        }
        
        private Highlight MontarHighlight(string query = "")
        {
            Highlight highlight = new Highlight();
            highlight.fields.Add(new FieldHighlight() { query = query, name = "nm_tipo_norma" });
            highlight.fields.Add(new FieldHighlight() { query = query, name = "nr_norma" });
            highlight.fields.Add(new FieldHighlight() { query = query, name = "dt_assinatura_text" });
            highlight.fields.Add(new FieldHighlight() { query = query, name = "dt_autuacao_text" });
            highlight.fields.Add(new FieldHighlight() { query = query, name = "nm_situacao" });
            highlight.fields.Add(new FieldHighlight() { query = query, name = "nm_ambito" });
            highlight.fields.Add(new FieldHighlight() { query = query, name = "ds_apelido" });
            highlight.fields.Add(new FieldHighlight() { query = query, name = "ds_ementa" });
            highlight.fields.Add(new FieldHighlight() { query = query, name = "nm_orgao" });
            highlight.fields.Add(new FieldHighlight() { query = query, name = "sg_orgao" });
            highlight.fields.Add(new FieldHighlight() { query = query, name = "nm_termo" });
            highlight.fields.Add(new FieldHighlight() { query = query, name = "ds_observacao" });
            highlight.fields.Add(new FieldHighlight() { query = query, name = "nm_pessoa_fisica_e_juridica" });
            highlight.fields.Add(new FieldHighlight() { query = query, name = "fontes.ar_fonte.filetext" });
            highlight.fields.Add(new FieldHighlight() { query = query, name = "ar_atualizado.filetext" });
            return highlight;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using util.BRLight;
using TCDF.Sinj.ES;

namespace TCDF.Sinj.ES
{
    public class BuscaEs
    {
        public BuscaEs()
        {
            fields = new List<SearchableField>();
            aggregation = new List<Agg>();
            order = new List<Ordenamento>();
            sourceInclude = new List<string>();
            sourceExclude = new List<string>();
            ids = new List<string>();
            highlight = new Highlight();
            filtersToQueryString = new List<FilterQueryString>();
            filtersToQueryFiltered = new List<FilterQueryFiltered>();
        }
        public ulong from { get; set; }
        public ulong size { get; set; }
        public string searchValue { get; set; }
        public int proximitySearchValue { get; set; }
        public string searchFilter { get; set; }
        public bool isSuggestSearches { get; set; }
        public List<Ordenamento> order { get; set; }
        public List<string> ids { get; set; }
        public List<Agg> aggregation { get; set; }
        public List<string> sourceInclude { get; set; }
        public List<string> sourceExclude { get; set; }
        public List<SearchableField> fields { get; set; }
        public Highlight highlight { get; set; }
        public List<FilterQueryString> filtersToQueryString { get; set; }
        public List<FilterQueryFiltered> filtersToQueryFiltered { get; set; }

        protected string GetFromAndSize()
        {
            var sFromAndSize = "";
            if (from > 0)
            {
                sFromAndSize = "\"from\": " + from;
            }
            if (size > 0)
            {
                sFromAndSize += (sFromAndSize != "" ? ", " : "") + "\"size\": " + size;
            }
            return sFromAndSize;
        }

        protected string GetSearchableFields()
        {
            var sFields = "";
            if (fields != null && fields.Count > 0)
            {
                foreach (var field in fields)
                {
                    sFields += (sFields != "" ? ", " : "") + "\"" + field.name + (field.boost > 0 ? "^" + field.boost : "") + "\"";
                }
            }
            return sFields;
        }

        protected string GetSort()
        {
            var sort = "";
            if (order != null && order.Count > 0)
            {
                foreach (var ord in order)
                {
                    switch (ord.ordem)
                    {
                        case Ordem.asc:
                            sort += (sort != "" ? ", " : "") + string.Format("{{\"{0}\": {{\"order\": \"asc\"}}}}", ord.campo);
                            break;
                        case Ordem.desc:
                            sort += (sort != "" ? ", " : "") + string.Format("{{\"{0}\": {{\"order\": \"desc\"}}}}", ord.campo);
                            break;
                        default:
                            sort += (sort != "" ? ", " : "") + string.Format("\"{0}\"", ord.campo);
                            break;
                    }
                }
                sort = "\"sort\":[" + sort + "]";
            }
            return sort;
        }

        protected string GetSource()
        {
            var source = "";
            if (sourceInclude != null && sourceInclude.Count > 0)
            {
                source = "\"include\": " + JSON.Serialize<List<string>>(sourceInclude);
            }
            if (sourceExclude != null && sourceExclude.Count > 0)
            {
                source += (source != "" ? ", " : "") + "\"exclude\": " + JSON.Serialize<List<string>>(sourceExclude);
            }
            if (source != "")
            {
                source = "\"_source\":{"+source+"}";
            }
            return source;
        }

        protected string GetAggregation()
        {
            var sAgg = "";
            if (aggregation != null && aggregation.Count > 0)
            {
                foreach(var agg in aggregation){
                    switch(agg.type){
                        case TypeAggregation.date_histogram:
                            sAgg += (sAgg != "" ? ", " : "") + string.Format("\"{0}\": {{\"date_histogram\": {{\"field\":\"{1}\", \"interval\": \"{2}\", \"format\": \"{3}\"", agg.name, agg.field, agg.interval, agg.format);
                            break;
                        default:
                            sAgg += (sAgg != "" ? ", " : "") + string.Format("\"{0}\": {{\"terms\": {{\"field\":\"{1}\", \"size\":{2}", agg.name, agg.field, agg.size);
                            break;
                        
                    }
                    switch (agg.ordenamento.ordem)
                    {
                        case Ordem.asc:
                            sAgg += string.Format(", \"order\": {{\"{0}\":\"asc\"}}}}}}", agg.ordenamento.campo);
                            break;
                        default:
                            sAgg += string.Format(", \"order\": {{\"{0}\":\"desc\"}}}}}}", agg.ordenamento.campo);
                            break;
                    }
                }
                sAgg = "\"aggs\":{" + sAgg + "}";
            }
            return sAgg;
        }

        protected string GetHighlight()
        {
            var sHighlight = "";
            if (highlight != null && highlight.fields != null && highlight.fields.Count() > 0)
            {
                foreach (var field in highlight.fields)
                {
                    if (!string.IsNullOrEmpty(field.query))
                    {
                        sHighlight += (sHighlight != "" ? ", " : "") + string.Format("\"{0}\": {{\"number_of_fragments\":{1}, \"fragment_size\":{2}, \"highlight_query\":{{\"match\":{{\"{0}\":\"{3}\"}}}}, \"no_match_size\": {4}}}", field.name, field.fragments, field.fragmentSize, field.query, field.noMatchSize);
                    }
                    else
                    {
                        sHighlight += (sHighlight != "" ? ", " : "") + string.Format("\"{0}\": {{\"number_of_fragments\":{1}, \"fragment_size\":{2}, \"no_match_size\": {3}}}", field.name, field.fragments, field.fragmentSize, field.noMatchSize);
                    }
                }
                sHighlight = "\"highlight\":{\"pre_tags\":[\"" + highlight.preTag + "\"],\"post_tags\":[\"" + highlight.postTag + "\"],\"fields\": {" + sHighlight + "}}";
            }
            return sHighlight;
        }

        protected string GetFiltersToQueryString()
        {
            var sFilter = "";
            var sFilterOr = "";

            if (filtersToQueryString != null && filtersToQueryString.Count > 0)
            {
                foreach (var filter in filtersToQueryString)
                {
                    if (filter.values != null && filter.values.Length > 0)
                    {
                        sFilterOr = "";
                        foreach (var value in filter.values)
                        {
                            sFilterOr += (sFilterOr != "" ? " OR " : "");
                        }
                        sFilter += (sFilter != "" ? " AND " : "") + filter.name + ":(\\\"" + sFilterOr + "\\\")";
                    }
                    else
                    {
                        sFilter += (sFilter != "" ? ", " : "") + string.Format("{0}:{1}", filter.name, (filter.isYear ? "[01/01/" + filter.value + " TO 31/12/" + filter.value + "]" : "(\\\"" + filter.value + "\\\")"));
                    }
                }
            }
            return sFilter;
        }

        protected string GetFiltersToQueryFilteredRecursive(List<FilterQueryFiltered> filtersToQueryFiltered)
        {
            var sFilter = "";
            var sAndFilter = "";
            var sOrFilter = "";
            var sNotFilter = "";
            if (filtersToQueryFiltered != null && filtersToQueryFiltered.Count > 0)
            {
                foreach (var filter in filtersToQueryFiltered)
                {
                    sFilter = "";
                    if (!string.IsNullOrEmpty(filter.value))
                    {
                        switch (filter.type)
                        {
                            case TypeFilter.datetime:
                            case TypeFilter.date:
                            case TypeFilter.num:
                                sFilter = GetRangeFilter(filter);
                                break;
                            case TypeFilter.year:
                                sFilter = GetYearFilter(filter);
                                break;
                            case TypeFilter.text:
                                if (filter.isAllFields)
                                {
                                    sFilter = string.Format("{{\"query\":{{\"query_string\":{{\"query\":\"{0}\", \"default_operator\":\"AND\", \"fields\":[{1}]}}}}}}", (filter.@operator.Equals(TypeOperator.equal) ? "\\\"" + filter.value + "\\\"" + (filter.proximity > 0 ? "~" + filter.proximity : "") : filter.value), GetSearchableFields());
                                }
                                else if(filter.names != null && filter.names.Count > 0)
                                {
                                    foreach (var name in filter.names)
                                    {
                                        sFilter += (sFilter != "" ? " OR " : "") + name + ":(" + (filter.@operator.Equals(TypeOperator.equal) ? "\\\"" + filter.value + "\\\"" + (filter.proximity > 0 ? "~" + filter.proximity : "") : filter.value) + ")";
                                    }
                                    sFilter = string.Format("{{\"query\":{{\"query_string\":{{\"query\":\"{0}\", \"default_operator\":\"AND\"}}}}}}", sFilter);
                                }
                                else
                                {
                                    sFilter = string.Format("{{\"query\":{{\"query_string\":{{\"query\":\"{0}:({1})\", \"default_operator\":\"AND\"}}}}}}", filter.name, (filter.@operator.Equals(TypeOperator.equal) ? "\\\"" + filter.value + "\\\"" + (filter.proximity > 0 ? "~" + filter.proximity : "") : filter.value));
                                }
                                break;
                            case TypeFilter.term:
                                sFilter = string.Format("{{\"term\":{{\"{0}\":\"{1}\"}}}}", filter.name, filter.value);
                                break;
                        }
                    }
                    else if(filter.type.Equals(TypeFilter.missing)){
                        sFilter = string.Format("{{\"missing\":{{\"field\":\"{0}\"}}}}", filter.name);
                    }
                    else if (filter.filtersToQueryFiltered != null && filter.filtersToQueryFiltered.Count() > 0)
                    {
                        sFilter = GetFiltersToQueryFilteredRecursive(filter.filtersToQueryFiltered);
                    }
                    if (sFilter != "")
                    {
                        switch (filter.connector)
                        {
                            case TypeConnector.AND:
                                sAndFilter += (sAndFilter != "" ? ", " : "") + sFilter;
                                break;
                            case TypeConnector.OR:
                                sOrFilter += (sOrFilter != "" ? ", " : "") + sFilter;
                                break;
                            case TypeConnector.NOT:
                                sNotFilter += (sNotFilter != "" ? ", " : "") + sFilter;
                                break;
                        }
                    }
                }
            }
            sFilter = "";
            if (sAndFilter != "")
            {
                sFilter = string.Format("{{\"and\":[{0}]}}", sAndFilter);
            }
            if (sOrFilter != "")
            {
                sFilter += (sFilter != "" ? ", " : "") + string.Format("{{\"or\":[{0}]}}", sOrFilter);
            }
            if (sNotFilter != "")
            {
                sFilter += (sFilter != "" ? ", " : "") + string.Format("{{\"not\":{{\"filter\":{{\"and\":[{0}]}}}}}}", sNotFilter);
            }
            return sFilter;
        }

        private string GetRangeFilter(FilterQueryFiltered filter)
        {
            //Condição inserida para que possa receber os parametros de data no formato dd/mm/aaaa
            var sFilter = "";
            if (filter.@operator.Equals(TypeOperator.equal) && filter.value.Length == 10) { 
                sFilter = string.Format("{{\"term\":{{\"{0}\":\"{1}\"}}}}", filter.name, filter.value);
            } else { 
                switch (filter.@operator){
                    case TypeOperator.equal:
                        sFilter = string.Format("{{\"term\":{{\"01/01/{0}\":\"31/12/{1}\"}}}}", filter.name, filter.value);
                        break;
                    case TypeOperator.lt:
                        sFilter = string.Format("{{\"range\":{{\"{0}\":{\"lt\":\"{1}\"}}}}}}", filter.name, filter.value);
                        break;
                    case TypeOperator.lte:
                        sFilter = string.Format("{{\"range\":{{\"{0}\":{{\"lte\":\"{1}\"}}}}}}", filter.name, filter.value);
                        break;
                    case TypeOperator.gt:
                        sFilter = string.Format("{{\"range\":{{\"{0}\":{{\"gt\":\"{1}\"}}}}}}", filter.name, filter.value);
                        break;
                    case TypeOperator.gte:
                        sFilter = string.Format("{{\"range\":{{\"{0}\":{{\"gte\":\"{1}\"}}}}}}", filter.name, filter.value);
                        break;
                    case TypeOperator.not:
                        sFilter = string.Format("{{\"not\":{{\"term\":{{\"{0}\":\"{1}\"}}}}}}", filter.name, filter.value);
                        break;
                }
            }

            return sFilter;
        }

        private string GetYearFilter(FilterQueryFiltered filter) {
            //Console.WriteLine("Equals: " + (filter.@operator.Equals(TypeOperator.equal) && filter.value.Length == 10));
            //Console.WriteLine("TypeOperator: " + filter.@operator.Equals(TypeOperator.equal));
            //Console.WriteLine("Length: " + filter.@operator.Equals(filter.value.Length == 10));
            //Console.WriteLine("Length2:  " + filter.value.Length);


            var sFilter = "";
            //Condição inserida para que possa receber os parametros de data no formato dd/mm/aaaa
            if (filter.@operator.Equals(TypeOperator.equal) && filter.value.Length == 10)
            {
                sFilter = string.Format("{{\"term\":{{\"{0}\":\"{1}\"}}}}", filter.name, filter.value);
            }
            else
            {
                switch (filter.@operator)
                {
                    case TypeOperator.equal:
                        sFilter = string.Format("{{\"range\":{{\"{0}\":{{\"gte\":\"01/01/{1}\",\"lte\":\"31/12/{1}\"}}}}}}", filter.name, filter.value);
                        break;
                    case TypeOperator.lt:
                        sFilter = string.Format("{{\"range\":{{\"{0}\":{{\"lt\":\"{1}\"}}}}}}", filter.name, filter.value);
                        break;
                    case TypeOperator.lte:
                        sFilter = string.Format("{{\"range\":{{\"{0}\":{{\"lte\":\"{1}\"}}}}}}", filter.name, filter.value);
                        break;
                    case TypeOperator.gt:
                        sFilter = string.Format("{{\"range\":{{\"{0}\":{{\"gt\":\"{1}\"}}}}}}", filter.name, filter.value);
                        break;
                    case TypeOperator.gte:
                        sFilter = string.Format("{{\"range\":{{\"{0}\":{{\"gte\":\"{1}\"}}}}}}", filter.name, filter.value);
                        break;
                    case TypeOperator.not:
                        sFilter = string.Format("{{\"not\":{{\"range\":{{\"{0}\":{{\"gte\":\"{1}\",\"lte\":\"{1}\"}}}}}}}}", filter.name, filter.value);
                        break;
                }
            }

            return sFilter;
        }
        
    }

    //Pendente de publicacao
    public class BuscaPendenteDePublicacaoEs : BuscaEs
    {
        public string GetQuery()
        {
            var docEs = new DocEs();
            var sQuery = "";
            var sFilters = GetFiltersToQueryString();

            var sSort = GetSort();
            var sSource = GetSource();
            var sAgg = GetAggregation();
            var sHighlight = GetHighlight();
            var sFromAndSize = GetFromAndSize();

            var fields = GetSearchableFields();



            if (!string.IsNullOrEmpty(searchValue))
            {
                sQuery = searchValue;
            }
            else
            {
                sQuery = "*";
            }
            if (!string.IsNullOrEmpty(searchFilter))
            {
                sQuery += string.Format(" AND ({0})", searchFilter);
            }

            sQuery = "{\"query\":{\"query_string\":{\"fields\":[" + fields + "],\"query\":\"" + sQuery + (!string.IsNullOrEmpty(sFilters) ? " AND (" + sFilters + ")" : "") + "\", \"default_operator\":\"AND\"}}" + (sHighlight != "" ? ", " + sHighlight : "") + (sSort != "" ? ", " + sSort : "") + (sSource != "" ? ", " + sSource : "") + (sAgg != "" ? ", " + sAgg : "") + (sFromAndSize != "" ? ", " + sFromAndSize : "") + "}";


            return sQuery;
        }
    }


    public class BuscaGeralEs : BuscaEs
    {
        public string GetQuery()
        {
            var docEs = new DocEs();
            var sQuery = "";
            var sFilters = GetFiltersToQueryString();

            var sSort = GetSort();
            var sSource = GetSource();
            var sAgg = GetAggregation();
            var sHighlight = GetHighlight();
            var sFromAndSize = GetFromAndSize();

            var fields = GetSearchableFields();
            
            
            
            if (!string.IsNullOrEmpty(searchValue))
            {
                sQuery = searchValue;
                sQuery += "AND(st_habilita_pesquisa=true))";
            }
            else
            {
                sQuery = "st_habilita_pesquisa=true";
            }
            if (!string.IsNullOrEmpty(searchFilter))
            {
                sQuery += string.Format(" AND ({0})", searchFilter);
            }

            sQuery = "{\"query\":{\"query_string\":{\"fields\":[" + fields + "],\"query\":\"" + sQuery + (!string.IsNullOrEmpty(sFilters) ? " AND (" + sFilters + ")" : "") + "\", \"default_operator\":\"AND\"}}" + (sHighlight != "" ? ", " + sHighlight : "") + (sSort != "" ? ", " + sSort : "") + (sSource != "" ? ", " + sSource : "") + (sAgg != "" ? ", " + sAgg : "") + (sFromAndSize != "" ? ", " + sFromAndSize : "") + "}";


            return sQuery;
        }
    }

    public class BuscaDiretaEs : BuscaEs
    {
        
        public string GetQuery()
        {
            var sQuery = "";

            var sSort = GetSort();
            var sSource = GetSource();
            var sAgg = GetAggregation();
            var sHighlight = GetHighlight();
            var sFromAndSize = GetFromAndSize();
            

            var sFilter = GetFiltersToQueryFilteredRecursive(filtersToQueryFiltered);

            var sQueryString = "";
            var sQueryFiltered = "";
            if (!string.IsNullOrEmpty(searchValue))
            {
                sQueryString = searchValue;
            }
            if(!string.IsNullOrEmpty(searchFilter)){
                sQueryString = (sQueryString != "" ? "(" + sQueryString + ") AND (" + searchFilter + ")" : searchFilter);
            }
            
            if (!string.IsNullOrEmpty(sQueryString))
            {
                sQueryFiltered = string.Format("\"query\":{{\"query_string\":{{\"query\":\"{0}\", \"default_operator\":\"AND\", \"fields\":[{1}]}}}}", sQueryString, GetSearchableFields());
            }
            if (sFilter != "")
            {
                sQueryFiltered += (sQueryFiltered != "" ? ", " : "") + string.Format("\"filter\":{{\"and\":[{0}]}}", sFilter);
            }
            sQuery = string.Format("{{\"query\": {{\"filtered\":{{{0}}}}}{1}{2}{3}{4}{5}}}", sQueryFiltered, (sHighlight != "" ? "," + sHighlight : ""), (sSort != "" ? "," + sSort : ""), (sAgg != "" ? "," + sAgg : ""), (sSource != "" ? "," + sSource : ""), (sFromAndSize != "" ? ", " + sFromAndSize : ""));

            return sQuery;
        }

    }

    public class BuscaByIdEs : BuscaEs
    {

        public string GetQuery()
        {
            var sQueryId = "";
            var sSort = GetSort();
            var sSource = GetSource();
            var sFromAndSize = GetFromAndSize();

            if (ids != null && ids.Count > 0)
            {
                sQueryId += "\"ids\":{\"values\":" + JSON.Serialize<List<string>>(ids) + "}";
            }

            sQueryId = string.Format("{{\"query\": {{{0}}}{1}{2}{3}}}", sQueryId, (sSort != "" ? "," + sSort : ""), (sSource != "" ? "," + sSource : ""), (sFromAndSize != "" ? ", " + sFromAndSize : ""));

            return sQueryId;
        }

    }

    public class Agg
    {
        public Agg()
        {
            type = TypeAggregation.terms;
            size = 1000;//Quantidade de itens da lista de filtro na barra lateral esquerda pos pesquisa by Wemerson
            ordenamento = new Ordenamento() { campo = "_count", ordem = Ordem.desc };
        }
        public string name { get; set; }
        public string field { get; set; }
        public TypeAggregation type { get; set; }
        public string interval { get; set; }
        public string format { get; set; }
        public Ordenamento ordenamento { get; set; }
        public int size { get; set; }
    }

    public class Ordenamento
    {
        public string campo { get; set; }
        public Ordem ordem { get; set; }
    }

    public class SearchableField {
        public string name { get; set; }
        public int boost { get; set; }
    }

    public class Highlight
    {
        public Highlight()
        {
            fields = new List<FieldHighlight>();
            preTag = "";
            postTag = "";
        }
        public string preTag { get; set; }
        public string postTag { get; set; }
        public List<FieldHighlight> fields { get; set; }
    }

    public class FieldHighlight {
        public FieldHighlight()
        {
            fragments = 20;
            fragmentSize = 1;
            noMatchSize = 0;
        }
        public string name { get; set; }
        public int fragments { get; set; }
        public int fragmentSize { get; set; }
        public int noMatchSize { get; set; }
        public string query { get; set; }
    }

    public class FilterQueryString {
        public string name { get; set; }
        public bool isYear { get; set; }
        public string value { get; set; }
        public string[] values { get; set; }
    }

    public class FilterQueryFiltered
    {
        public FilterQueryFiltered()
        {
            filtersToQueryFiltered = new List<FilterQueryFiltered>();
            connector = TypeConnector.AND;
            type = TypeFilter.text;
            @operator = TypeOperator.contains;
            names = new List<string>();
        }
        public string name { get; set; }
        public List<string> names { get; set; }
        public bool isAllFields { get; set; }
        public TypeFilter type { get; set; }
        public TypeConnector connector { get; set; }
        public TypeOperator @operator { get; set; }
        public string value { get; set; }
        public int proximity { get; set; }
        public List<FilterQueryFiltered> filtersToQueryFiltered { get; set; }
    }

    public enum TypeOperator
    {
        equal,
        contains,
        lt,
        lte,
        gt,
        gte,
        not
    }

    public enum TypeFilter
    {
        datetime,
        date,
        year,
        text,
        num,
        missing,
        term
    }

    public enum TypeConnector{
        AND,
        OR,
        NOT
    }

    public enum TypeAggregation {
        terms,
        date_histogram
    }

    public enum Ordem
    {
        asc,
        desc
    }
}

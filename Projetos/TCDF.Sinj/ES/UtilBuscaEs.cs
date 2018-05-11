using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.OV;
using System.Web;

namespace TCDF.Sinj.ES
{
    public class UtilBuscaEs
    {
        public TypeOperator GetOperador(string _ch_operador)
        {
            _ch_operador = _ch_operador.Replace("_", "").ToLower();
            if (_ch_operador == "menor")
            {
                return TypeOperator.lt;

            }
            else if (_ch_operador == "menorouigual")
            {
                return TypeOperator.lte;

            }
            else if (_ch_operador == "maior")
            {
                return TypeOperator.gt;

            }
            else if (_ch_operador == "maiorouigual")
            {
                return TypeOperator.gte;

            }
            else if (_ch_operador == "diferente")
            {
                return TypeOperator.not;
            }
            else if (_ch_operador == "contem")
            {
                return TypeOperator.contains;
            }
            else
            {
                return TypeOperator.equal;
            }
        }

        public void MontarFiltroBuscaDireta(string[] filtros, BuscaDiretaEs buscaDireta)
        {
            if (filtros != null)
            {
                string[] filtroSplited;
                foreach (var _filtro in filtros)
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
        }

        public void MontarFiltroBuscaGeral(string[] filtros, BuscaGeralEs buscaGeral)
        {
            FilterQueryString filterQueryString;
            if (filtros != null)
            {
                string[] filtroSplited;
                foreach (var _filtro in filtros)
                {
                    filtroSplited = _filtro.Split(':');
                    filterQueryString = new FilterQueryString() { name = filtroSplited[0], value = filtroSplited[1] };
                    if (filterQueryString.name.IndexOf("ano_") == 0)
                    {
                        filterQueryString.isYear = true;
                        filterQueryString.name = filterQueryString.name.Replace("ano_", "dt_");
                    }
                    buscaGeral.filtersToQueryString.Add(filterQueryString);
                }
            }
        }
    }
}

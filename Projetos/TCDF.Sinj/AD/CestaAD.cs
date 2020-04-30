using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using TCDF.Sinj.ES;

namespace TCDF.Sinj.AD
{
    public class CestaAD
    {
        private DocEs _docEs;
        public CestaAD()
        {
            _docEs = new DocEs();
        }

        public string MontarOrdenamento(HttpContext context)
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
            return sOrder;
        }
        
        /// <summary>
        /// Campos que retornarão do elasticsearch
        /// </summary>
        /// <returns></returns>
        private string MontarPartialFields(string bbusca)
        {
            var fields = "";
            if (bbusca == "sinj_norma")
            {
                fields = ", \"partial_fields\":{\"partial\":{\"include\":[\"_metadata.id_doc\",\"origens.sg_orgao\",\"origens.ch_orgao\",\"origens.nm_orgao\",\"ar_atualizado.id_file\",\"ar_atualizado.filesize\",\"fontes.ar_fonte.id_file\",\"fontes.ar_fonte.filesize\",\"nm_tipo_norma\",\"nr_norma\",\"ch_norma\",\"dt_assinatura\",\"ds_ementa\",\"nm_situacao\"]}}";
            }
            else if (bbusca == "sinj_diario")
            {
                fields = ", \"partial_fields\":{\"partial\":{\"include\":[\"_metadata.id_doc\",\"nm_tipo_fonte\",\"nm_tipo_edicao\",\"nm_diferencial_edicao\",\"nr_diario\",\"cr_diario\",\"secao_diario\",\"dt_assinatura\",\"st_pendente\",\"nm_diferencial_suplemento\",\"st_suplemento\",\"ar_diario.id_file\",\"ar_diario.filesize\",\"arquivos.arquivo_diario.id_file\",\"arquivos.arquivo_diario.filesize\",\"arquivos.ds_arquivo\"]}}";
            }
            return fields;
        }

        public string MontarConsulta(HttpContext context)
        {
            var ids = "";

            var sOrder = MontarOrdenamento(context);

            var _cesta = context.Request["cesta"];
            var _base = context.Request["b"];
            var partial_fields = MontarPartialFields(_base);
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
            return "{\"query\":{\"ids\":{\"values\":[" + ids + "]}}" + sOrder + partial_fields + "}";
        }

        /// <summary>
        /// Monta a url de consulta para pesquisa ou total
        /// </summary>
        /// <param name="context"></param>
        /// <param name="_bbusca"></param>
        /// <returns></returns>
        public string MontarUrl(HttpContext context)
        {
            string _exibir_total = context.Request["exibir_total"];
            var _iDisplayLength = context.Request["iDisplayLength"];
            var _iDisplayStart = context.Request["iDisplayStart"];

            var _base = context.Request["b"];

            var url_es = new DocEs().GetUrlEs(_base);

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

        public Result<T> ConsultarEs<T>(HttpContext context)
        {
            var query = "";
            var url_es = MontarUrl(context);
            query = MontarConsulta(context);
            return new ESAd().PesquisarDocs<T>(query, url_es);
        }

        public string PesquisarTotalEs(HttpContext context)
        {
            string url_es = "";
            string query = "";
            try
            {
                url_es = MontarUrl(context);
                query = MontarConsulta(context);
                return _docEs.CountEs(query, url_es);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao pesquisar Total. url_es: " + url_es + ". query: " + query, ex);
            }
        }
    }
}

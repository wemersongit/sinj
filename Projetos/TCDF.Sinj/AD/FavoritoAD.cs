﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.ES;
using System.Web;
using TCDF.Sinj.RN;

namespace TCDF.Sinj.AD
{
    public class FavoritoAD
    {
        private DocEs _docEs;
        public FavoritoAD()
        {
            _docEs = new DocEs();
        }

        /// <summary>
        /// Campos que retornarão do elasticsearch
        /// </summary>
        /// <returns></returns>
        private string MontarPartialFields()
        {
            return ", \"partial_fields\":{\"partial\":{\"include\":[\"_metadata.id_doc\",\"origens.sg_orgao\",\"origens.ch_orgao\",\"origens.nm_orgao\",\"ar_atualizado.id_file\",\"ar_atualizado.filesize\",\"fontes.ar_fonte.id_file\",\"fontes.ar_fonte.filesize\",\"nm_tipo_norma\",\"nr_norma\",\"ch_norma\",\"dt_assinatura\",\"ds_ementa\",\"nm_situacao\"]}}";
        }

        public string MontarConsulta(HttpContext context)
        {
            string query = "";
            var partial_fields = MontarPartialFields();
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
                query = "ch_norma:(" + chaves + ")";
            }
            else
            {
                throw new Exception("Nenhum Favorito para pesquisar.");
            }
            return "{\"query\":{\"query_string\":{\"query\":\"" + query + "\"}}"+partial_fields+"}";
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
            var _bbusca = "";
            if (_base == "norma")
            {
                _bbusca = "sinj_norma";
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

        public Result<T> ConsultarEs<T>(HttpContext context)
        {
            var query = "";
            var url_es = MontarUrl(context);
            query = MontarConsulta(context);
            return new ES.ESAd().PesquisarDocs<T>(query, url_es);
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

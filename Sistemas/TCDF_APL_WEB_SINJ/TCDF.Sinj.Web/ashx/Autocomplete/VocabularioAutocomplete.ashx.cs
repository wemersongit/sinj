using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Web.ashx.Autocomplete
{
    /// <summary>
    /// Summary description for VocabularioAutocomplete
    /// </summary>
    public class VocabularioAutocomplete : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";
            
            var _texto = context.Request["texto"];
            var _offset = context.Request["offset"];
            var _limit = context.Request["limit"];
            var _sIn_lista = context.Request["in_lista"];
            var _ch_tipo_termo = context.Request["ch_tipo_termo"];
            var _ch_termo_ignore = context.Request["ch_termo_ignore"];
            var _ch_termo_ignoreds = context.Request["ch_termo_ignoreds"];
            
            var _inicio_sem_coringa = context.Request["inicio_sem_coringa"];
            var _sCad = context.Request["cad"];
            var bCad = false;
            bool.TryParse(_sCad, out bCad);
            var _sPes = context.Request["pes"];
            var bPes = false;
            bool.TryParse(_sPes, out bPes);
            var bIn_lista = false;

            var query = new Pesquisa();
            string sQuery = "";

            if (_limit != "-1" && !string.IsNullOrEmpty(_limit))
            {
                query.limit = _limit;
                query.offset = _offset;
            }
            else
            {
                query.limit = "30";
            }
            if (!string.IsNullOrEmpty(_texto))
            {
                if (_texto != "...")
                {
//					sQuery = "Upper(nm_termo) like'" + _texto.ToUpper() + "%'";
                    sQuery = string.Format("TRANSLATE(Upper(nm_termo), 'áéíóúàèìòùãõâêîôôäëïöüçÁÉÍÓÚÀÈÌÒÙÃÕÂÊÎÔÛÄËÏÖÜÇ', 'aeiouaeiouaoaeiooaeioucAEIOUAEIOUAOAEIOOAEIOUC') like TRANSLATE('{0}{1}%', 'áéíóúàèìòùãõâêîôôäëïöüçÁÉÍÓÚÀÈÌÒÙÃÕÂÊÎÔÛÄËÏÖÜÇ', 'aeiouaeiouaoaeiooaeioucAEIOUAEIOUAOAEIOOAEIOUC')", _inicio_sem_coringa == "1" ? "" : "%", _texto.ToUpper().Replace("'", "''"));
				}
            }
            if (!string.IsNullOrEmpty(_sIn_lista) && bool.TryParse(_sIn_lista, out bIn_lista))
            {
                sQuery += (sQuery != "" ? " and " : "") + "in_lista=" + bIn_lista;
            }
            if (!string.IsNullOrEmpty(_ch_tipo_termo))
            {
                sQuery += (sQuery != "" ? " and " : "") + "ch_tipo_termo='" + _ch_tipo_termo + "'";
            }
            if (!string.IsNullOrEmpty(_ch_termo_ignore))
            {
                sQuery += (sQuery != "" ? " and " : "") + "ch_termo!='" + _ch_termo_ignore + "'";
            }
            if (!string.IsNullOrEmpty(_ch_termo_ignoreds))
            {
                var chaves_ignoradas = _ch_termo_ignoreds.Split(',');
                foreach(var chave_ignorada in chaves_ignoradas){
                    sQuery += (sQuery != "" ? " and " : "") + "ch_termo!='" + chave_ignorada + "'";
                }
            }
            if (bCad)
            {
                sQuery += (sQuery != "" ? " and " : "") + "st_ativo=true";
            }

            query.literal = sQuery;
            query.order_by.asc = new[] { "nm_termo" };
            context.Response.Clear();

            try
            {
                var result = new VocabularioRN().JsonReg(query);
                var result_detalhado = JSON.Deserializa<Results<VocabularioDetalhado>>(result);
                if (bCad)
                {
                    VocabularioDetalhado termo_detalhado;
                    foreach (var termo in result_detalhado.results)
					{
						termo_detalhado = new VocabularioDetalhado();
						termo_detalhado.ch_tipo_termo = termo.ch_tipo_termo;
						termo.ds_autocomplete = termo.nm_termo + " ("+termo_detalhado.nm_tipo_termo+")";
                        termo_detalhado = new VocabularioDetalhado();
                        if (termo.in_nao_autorizado)
                        {
                            termo_detalhado = new VocabularioDetalhado();
                            termo_detalhado.ch_tipo_termo = termo.ch_tipo_termo;
                            termo.ds_autocomplete = "<span class='ds_autocomplete'>Não use:</span><br/>&nbsp;&nbsp;&nbsp;" + termo.nm_termo + " (" + termo_detalhado.nm_tipo_termo + ")<br/><span class='ds_autocomplete'>Use:</span><br/>&nbsp;&nbsp;&nbsp;" + termo.nm_termo_use + " (" + termo_detalhado.nm_tipo_termo + ")";
                            termo.ch_termo = termo.ch_termo_use;
                            termo.nm_termo = termo.nm_termo_use;
                        }
                    }
                }
                else if (bPes)
                {
                    var termos = new List<VocabularioDetalhado>();
                    VocabularioDetalhado termo_detalhado;
                    foreach (var termo in result_detalhado.results)
					{
                        termo.ds_autocomplete = termo.nm_termo;
                        if (termo.in_nao_autorizado)
                        {
                            termo_detalhado = new VocabularioDetalhado();
                            termo_detalhado.ds_autocomplete = "<span class='ds_autocomplete'>Não use:</span><br/>&nbsp;&nbsp;&nbsp;" + termo.nm_termo + "<br/><span class='ds_autocomplete'>Use:</span><br/>&nbsp;&nbsp;&nbsp;" + termo.nm_termo_use;
                            termo_detalhado.ch_termo = termo.ch_termo_use;
                            termo_detalhado.nm_termo = termo.nm_termo_use;
                            termos.Add(termo_detalhado);
                        }
                    }
                    result_detalhado.results.AddRange(termos);
                }
                else
                {
                    VocabularioDetalhado termo_detalhado;
                    foreach (var termo in result_detalhado.results)
                    {
                        termo_detalhado = new VocabularioDetalhado();
                        termo_detalhado.ch_tipo_termo = termo.ch_tipo_termo;
                        termo.ds_autocomplete = termo.nm_termo + " ("+termo_detalhado.nm_tipo_termo+")";
                    }
                }
                sRetorno=JSON.Serialize<Results<VocabularioDetalhado>>(result_detalhado);
            }
            catch (Exception Ex)
            {
                var retorno = new
                {
                    responseText = Excecao.LerInnerException(Ex, true),
                    statusText = "Erro interno do Servidor!!!",
                    status = 500,
                    url = context.Request.Url.PathAndQuery.ToString(),
                    ErroCallBack = true
                };

                sRetorno = JSON.Serialize<object>(retorno);
            }
            context.Response.ContentType = "application/json";
            context.Response.Write(sRetorno);
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}

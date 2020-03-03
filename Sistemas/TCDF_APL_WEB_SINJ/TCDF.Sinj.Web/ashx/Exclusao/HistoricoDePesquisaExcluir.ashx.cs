using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.ES;

namespace TCDF.Sinj.Web.ashx.Exclusao
{
    /// <summary>
    /// Summary description for HistoricoDePesquisaExcluir
    /// </summary>
    public class HistoricoDePesquisaExcluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var _id_doc = context.Request["id"];
            var _chave = context.Request["chave"];
            try
            {
                if (!string.IsNullOrEmpty(_id_doc))
                {
                    new ESAd().DeletarDoc("", Config.ValorChave("URLElasticSearchHistoricoDePesquisa", true) + "/" + _id_doc);
                }
                else if (!string.IsNullOrEmpty(_chave))
                {
                    new ESAd().DeletarDoc("{\"query\":{\"query_string\":{\"query\":\"chave:" + _chave + "\"}}}", Config.ValorChave("URLElasticSearchHistoricoDePesquisa", true) + "/_query");
                }
                sRetorno = "{\"success_message\":\"Histórico excluído com sucesso\"}";
            }
            catch (Exception ex)
            {
                var sErro = Excecao.LerTodasMensagensDaExcecao(ex, false);
                sRetorno = "{\"error_message\":\"" + sErro + "\"}";
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                LogErro.gravar_erro("HST.EXC", erro,"","");

            }
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

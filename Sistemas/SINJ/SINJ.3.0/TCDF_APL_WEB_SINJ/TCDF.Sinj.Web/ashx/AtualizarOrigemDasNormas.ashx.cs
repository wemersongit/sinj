using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx
{
    /// <summary>
    /// Ao se editar um órgao, as normas que possuem esse órgão como origem podem ficar com informações desatualizadas.
    /// Essa página é executada para que todas as normas cadastradas com certa origem tenham essa informação atualizada.
    /// </summary>
    public class AtualizarOrigemDasNormas : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            Pesquisa pesquisa = new Pesquisa();
            var normaRn = new NormaRN();

            var _ch_orgao = context.Request["ch_orgao"];
            var _nm_orgao = context.Request["nm_orgao"];
            var _sg_orgao = context.Request["sg_orgao"];

            try
            {
                if (!string.IsNullOrEmpty(_ch_orgao))
                {
                    var query = "'" + _ch_orgao + "' = any(ch_orgao)";
                    pesquisa.literal = query;
                    var results_norma = normaRn.Consultar(pesquisa);
                    var count_normas_alteradas = 0;

                    foreach (var norma in results_norma.results)
                    {
                        var alteracao = false;
                        foreach (var origem in norma.origens)
                        {
                            if (_ch_orgao == origem.ch_orgao && (_nm_orgao != origem.nm_orgao || _sg_orgao != origem.sg_orgao))
                            {
                                origem.nm_orgao = _nm_orgao;
                                origem.sg_orgao = _sg_orgao;
                                alteracao = true;
                            }
                        }
                        if (alteracao)
                        {
                            normaRn.PathPut(norma._metadata.id_doc, "origens", JSON.Serialize(norma.origens), "");
                            //normaRn.Atualizar(norma._metadata.id_doc, norma);
                            count_normas_alteradas++;
                        }
                    }

                    if (count_normas_alteradas > 0)
                    {
                        sRetorno = "{\"count_normas_alteradas\":" + count_normas_alteradas + "}";
                    }
                    else if (count_normas_alteradas == 0)
                    {
                        sRetorno = "{\"nenhuma\": \"nenhuma\"}";
                    }
                }
            }
            catch (Exception ex)
            {
                sRetorno = Excecao.LerTodasMensagensDaExcecao(ex, false);
                context.Response.StatusCode = 500;
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
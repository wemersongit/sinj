using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Portal.Web.ashx.Consulta
{
    public class OrigemDaNormaConsulta : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";

            var _ch_orgao = context.Request["ch_orgao"];

            try
            {
                var orgaoOv = new OrgaoRN().Doc(_ch_orgao);
                var ch_hierarquia = orgaoOv.ch_hierarquia;
                var split_ch_hierarquia = ch_hierarquia.Split('.');
                foreach (var ch_orgao in split_ch_hierarquia)
                {
                    if (!string.IsNullOrEmpty(ch_orgao))
                    {
                        var orgaoHierarquiaOv = new OrgaoRN().Doc(ch_orgao);
                        sRetorno += (sRetorno != "" ? ">" : "") + orgaoHierarquiaOv.nm_orgao;
                    }
                }
                sRetorno = "{\"ds_origem\":\"" + sRetorno + "\"}";
            }
            catch (Exception ex)
            {
                sRetorno = "{\"error_message\": \"Ocorreu erro um erro na consulta de órgão.\"}";
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                LogErro.gravar_erro("PORTAL_NOR.PES", erro, "visitante", "visitante");
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

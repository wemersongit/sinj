using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using System.Text;
using TCDF.Sinj.OV;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Consulta
{
    /// <summary>
    /// Summary description for ValidarInativacaoHierarquiaInferior
    /// </summary>
    public class ValidarInativacaoHierarquiaInferior : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {

            var sRetorno = "";
            Pesquisa pesquisa = new Pesquisa();
            var normaRn = new NormaRN();
            var orgaoRn = new OrgaoRN();

            var _ch_orgao = context.Request["ch_orgao"];
            var _dt_fim_vigencia = context.Request["dt_fim_vigencia"];
            var lista_validacao_erros = new List<ValidacaoFilhos>();
            var filhos_inativos = 0;

            if (!string.IsNullOrEmpty(_ch_orgao))
            {
                try
                {
                    var orgaos_inferiores = orgaoRn.BuscarHierarquiaInferior(_ch_orgao);
                    foreach (var filho in orgaos_inferiores)
                    {
                        if (filho.ch_orgao == _ch_orgao)
                        {
                            continue;
                        }
                        if (filho.st_orgao != false)
                        {
                            filho.dt_fim_vigencia = (Convert.ToDateTime(_dt_fim_vigencia)).ToString("dd'/'MM'/'yyyy");
                            orgaoRn.ValidarFilhos(filho, ref lista_validacao_erros);
                        }
                        else 
                        {
                            filhos_inativos++; 
                        }
                    }
                    if (orgaos_inferiores.Count == 1 || (orgaos_inferiores.Count - 1) == filhos_inativos)
                    {
                        sRetorno = "{\"sucesso\":\"nenhum\"}";
                    }
                    else if (orgaos_inferiores.Count > 1)
                    {
                        if (lista_validacao_erros.Count > 0)
                        {
                            sRetorno = "{\"lista_validacao_erros\": " + JSON.Serialize(lista_validacao_erros) + "}";
                        }
                        else
                        {
                            sRetorno = "{\"sucesso\": \"todos\"}";
                        }
                    }
                }
                catch (Exception ex)
                {
                    sRetorno = Excecao.LerTodasMensagensDaExcecao(ex, false);
                    context.Response.StatusCode = 500;
                }
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

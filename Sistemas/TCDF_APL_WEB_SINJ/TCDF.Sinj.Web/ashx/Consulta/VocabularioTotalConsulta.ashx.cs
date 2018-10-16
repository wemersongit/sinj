using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Web.ashx.Consulta
{
    /// <summary>
    /// Summary description for VocabularioTotalConsulta
    /// </summary>
    public class VocabularioTotalConsulta : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var vocabularioRn = new VocabularioRN();
            var action = AcoesDoUsuario.voc_pes;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                Pesquisa pesquisa = new Pesquisa();

                ulong total = 0;
                pesquisa.select = new string[0];
                pesquisa.literal = "ch_tipo_termo='DE'";
                pesquisa.limit = "1";
                var result = vocabularioRn.Consultar(pesquisa);
                sRetorno = "{\"de\":" + result.result_count;
                total += result.result_count;

                pesquisa.literal = "ch_tipo_termo='ES'";
                result = vocabularioRn.Consultar(pesquisa);
                sRetorno += ",\"es\":" + result.result_count;
                total += result.result_count;

                pesquisa.literal = "ch_tipo_termo='AU'";
                result = vocabularioRn.Consultar(pesquisa);
                sRetorno += ",\"au\":" + result.result_count;
                total += result.result_count;

                pesquisa.literal = "ch_tipo_termo='LA'";
                result = vocabularioRn.Consultar(pesquisa);
                sRetorno += ",\"la\":" + result.result_count;
                total += result.result_count;

                sRetorno += ",\"total\":"+total+"}";
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocNotFoundException || ex is SessionExpiredException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\"}";
                }
                else
                {
                    sRetorno = Excecao.LerTodasMensagensDaExcecao(ex, false);
                    context.Response.StatusCode = 500;
                }
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                if (sessao_usuario != null)
                {
                    LogErro.gravar_erro(Util.GetEnumDescription(action), erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
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

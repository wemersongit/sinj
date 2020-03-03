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
    /// Summary description for VocabularioHomonimosConsulta
    /// </summary>
    public class VocabularioHomonimosConsulta : IHttpHandler
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
                pesquisa.limit = null;
                pesquisa.select = new string[] { "nm_termo" };
                var result = vocabularioRn.Consultar(pesquisa);
                List<TermoHomonimo> termos_homonimos = new List<TermoHomonimo>();
                var count = 0;
                foreach(var termo in result.results){
                    count = result.results.Count<VocabularioOV>(t => t.nm_termo.ToUpper() == termo.nm_termo.ToUpper());
                    if(count > 1){
                        if (termos_homonimos.Count<TermoHomonimo>(th => th.nm_termo.ToUpper() == termo.nm_termo.ToUpper() && th.nr_total == count) < 1)
                        {
                            termos_homonimos.Add(new TermoHomonimo { nm_termo = termo.nm_termo, nr_total = count });
                        }
                    }
                }
                sRetorno = "{\"termos_homonimos\":" + JSON.Serialize<List<TermoHomonimo>>(termos_homonimos.OrderBy(th => th.nm_termo).ToList()) + "}";
                LogRelatorio log_relatorio = new LogRelatorio{Pesquisa = JSON.Serialize<Pesquisa>(pesquisa)};
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_relatorio, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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

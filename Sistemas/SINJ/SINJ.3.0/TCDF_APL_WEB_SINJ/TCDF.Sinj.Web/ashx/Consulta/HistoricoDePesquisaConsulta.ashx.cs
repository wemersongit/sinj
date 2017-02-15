using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Web.ashx.Consulta
{
    /// <summary>
    /// Summary description for HistoricoDePesquisaConsulta
    /// </summary>
    public class HistoricoDePesquisaConsulta : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var action = AcoesDoUsuario.aud_pes;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                var result = new HistoricoDePesquisaRN().ConsultarEs(context);
                sRetorno = Newtonsoft.Json.JsonConvert.SerializeObject(result);
                var Busca = new LogBuscar
                {
                    RegistrosPorPagina = "0",
                    RegistroInicial = "0",
                    RequestNumero = "0",
                    RegistrosTotal = "0"
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ".EST", Busca, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);

            }
            catch (Exception ex)
            {
                sRetorno = "{ \"aggregations\": null}";
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                LogErro.gravar_erro(Util.GetEnumDescription(action) + ".EST", erro, "", "");
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Consulta
{
    /// <summary>
    /// Summary description for DiarioConsulta
    /// </summary>
    public class DiarioConsulta : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var _ch_tipo_fonte = context.Request["ch_tipo_fonte"];
            var _dt_assinatura = context.Request["dt_assinatura"];
            var action = AcoesDoUsuario.dio_pes;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);

                Util.rejeitarInject(_ch_tipo_fonte);
                util.BRLight.Params.CheckNotNullOrEmpty("Tipo de Fonte", _ch_tipo_fonte);
                util.BRLight.Params.CheckNotNullOrEmpty("Data de Publicação", _dt_assinatura);
                
                Pesquisa pesquisa = new Pesquisa();
                pesquisa.literal = "ch_tipo_fonte='"+_ch_tipo_fonte+"' AND dt_assinatura='"+_dt_assinatura+"'";
                pesquisa.order_by.asc = new string[]{ "nr_diario", "secao_diario" };

                sRetorno = new DiarioRN().JsonReg(pesquisa);

                var ind_count = sRetorno.IndexOf("\"result_count\": ") + "\"result_count\": ".Length;
                var ind_chaves = sRetorno.IndexOf("}", ind_count);
                var ind_virgula = sRetorno.IndexOf(",", ind_count);
                var log_busca = new LogBuscar
                {
                    RegistrosTotal = sRetorno.Substring(ind_count, (ind_chaves > 0 ? ind_chaves : ind_virgula) - ind_count),
                    PesquisaLight = pesquisa
                };

                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_busca, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);

            }
            catch (Exception ex)
            {
                sRetorno = "{\"error_message\":\"" + Excecao.LerTodasMensagensDaExcecao(ex, true) + "\"}";
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

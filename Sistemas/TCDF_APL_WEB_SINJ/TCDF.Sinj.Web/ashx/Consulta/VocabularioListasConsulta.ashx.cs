using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.Log;
using util.BRLight;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using neo.BRLightREST;

namespace TCDF.Sinj.Web.ashx.Consulta
{
    /// <summary>
    /// Summary description for VocabularioListasConsulta
    /// </summary>
    public class VocabularioListasConsulta : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var _ch_termo = context.Request["ch_termo"];
            var _ch_lista = context.Request["ch_lista"];
            var vocabularioRn = new VocabularioRN();
            var action = AcoesDoUsuario.voc_vis;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                Pesquisa pesquisa = new Pesquisa();
                var query = "ch_tipo_termo='LA' and " + (!string.IsNullOrEmpty(_ch_lista) ? "ch_lista_superior='" + _ch_lista + "'" : "ch_lista_superior is null" );
                pesquisa.literal = query;
                pesquisa.limit = null;
                var termosOv = vocabularioRn.Consultar(pesquisa).results;
                var sTermos = JSON.Serialize<List<VocabularioOV>>(termosOv);
                var termos_detalhados = JSON.Deserializa<List<VocabularioDetalhado>>(sTermos);
                VocabularioDetalhado termo = null;
                if (!string.IsNullOrEmpty(_ch_termo))
                {
                    termo = BuscarTermoNaListaRecursivo(_ch_termo);
                }
                sRetorno = "{\"termos\": " + JSON.Serialize<List<VocabularioDetalhado>>(termos_detalhados) + ", \"termo_selecionado\":" + JSON.Serialize<VocabularioDetalhado>(termo) + "}";
                var log_visualizar = new LogVisualizar
                {
                    id_doc = 0,
                    ch_doc = _ch_termo
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_visualizar, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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



        private VocabularioDetalhado BuscarTermoNaListaRecursivo(string ch_termo)
        {
            var termo = new VocabularioRN().Doc(ch_termo);
            var sTermo = JSON.Serialize<VocabularioOV>(termo);
            var termo_detalhado = JSON.Deserializa<VocabularioDetalhado>(sTermo);
            if (!string.IsNullOrEmpty(termo_detalhado.ch_lista_superior))
            {
                termo_detalhado.lista_pai = BuscarTermoNaListaRecursivo(termo_detalhado.ch_lista_superior);
            }
            return termo_detalhado;
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

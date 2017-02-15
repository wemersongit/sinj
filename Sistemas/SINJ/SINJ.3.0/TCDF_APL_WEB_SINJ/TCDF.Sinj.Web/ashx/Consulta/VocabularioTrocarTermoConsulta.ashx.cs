using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Consulta
{
    /// <summary>
    /// Summary description for VocabularioTrocarTermoConsulta
    /// </summary>
    public class VocabularioTrocarTermoConsulta : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";

            var _id_doc_termo_antigo = context.Request["id_doc_termo_antigo"];
            var _ch_termos_antigos = context.Request["ch_termos_antigos"];
            ulong id_doc_termo_antigo = 0;
            var _id_doc_termo_novo = context.Request["id_doc_termo_novo"];
            ulong id_doc_termo_novo = 0;
            VocabularioOV termo_antigo = null;
            List<VocabularioOV> termos_antigos = new List<VocabularioOV>(); ;
            VocabularioOV termo_novo = null;
            var action = AcoesDoUsuario.voc_ger;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                if (!string.IsNullOrEmpty(_id_doc_termo_antigo) && ulong.TryParse(_id_doc_termo_antigo, out id_doc_termo_antigo))
                {
                    var termoOV = new VocabularioRN().JsonReg(id_doc_termo_antigo);
                    termo_antigo = JSON.Deserializa<VocabularioDetalhado>(termoOV);

                    var log_visualizar = new LogVisualizar
                    {
                        id_doc = id_doc_termo_antigo,
                        ch_doc = null
                    };
                    LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_visualizar, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
                else if (!string.IsNullOrEmpty(_ch_termos_antigos))
                {
                    var ch_termos_antigos = _ch_termos_antigos.Split(',');
                    Pesquisa pesquisa = new Pesquisa();
                    pesquisa.limit = null;
                    pesquisa.literal = "";
                    foreach(var ch in ch_termos_antigos){
                        pesquisa.literal += (pesquisa.literal != "" ? " OR " : "") + "ch_termo='" + ch + "'";
                    }
                    if (pesquisa.literal != "")
                    {
                        var result = new VocabularioRN().Consultar(pesquisa);
                        termos_antigos.AddRange(result.results);
                    }
                }
                if (!string.IsNullOrEmpty(_id_doc_termo_novo) && ulong.TryParse(_id_doc_termo_novo, out id_doc_termo_novo))
                {
                    var termoOV = new VocabularioRN().JsonReg(id_doc_termo_novo);
                    termo_novo = JSON.Deserializa<VocabularioDetalhado>(termoOV);

                    var log_visualizar = new LogVisualizar
                    {
                        id_doc = id_doc_termo_novo,
                        ch_doc = null
                    };
                    LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_visualizar,sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
                var retorno = new { termo_antigo = termo_antigo, termo_novo = termo_novo, termos_antigos = termos_antigos };
                sRetorno = JSON.Serialize<object>(retorno);
            }
            catch (Exception ex)
            {
                sRetorno = "{\"error_message\": \"Ocorreu erro um erro na consulta da troca de termo.\"}";
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
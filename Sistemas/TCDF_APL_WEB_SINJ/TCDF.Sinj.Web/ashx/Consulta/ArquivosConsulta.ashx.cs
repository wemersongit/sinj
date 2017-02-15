using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Consulta
{
    /// <summary>
    /// Summary description for ArquivosConsulta
    /// </summary>
    public class ArquivosConsulta : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "{}";

            string _ch_doc_raiz = context.Request["ch_arquivo_raiz"];
            string _id_doc = context.Request["id_doc"];

            context.Response.Clear();

            var action = AcoesDoUsuario.arq_pro;
            SessaoUsuarioOV sessao_usuario = null;

            var query = new Pesquisa();
            try
            {
                sessao_usuario = Util.ValidarSessao();
                if (!string.IsNullOrEmpty(_ch_doc_raiz))
                {
                    var bShared = false;
                    if (_ch_doc_raiz == "meus_arquivos")
                    {
                        _ch_doc_raiz = sessao_usuario.nm_login_usuario;
                        bShared = true;
                    }
                    else if (_ch_doc_raiz == "arquivos_orgao_cadastrador")
                    {
                        _ch_doc_raiz = sessao_usuario.orgao_cadastrador.nm_orgao_cadastrador;
                        bShared = true;
                    }
                    query.limit = null;
                    query.literal = "nr_nivel_arquivo<=1 AND (ch_arquivo_superior='" + _ch_doc_raiz + "'" + (bShared ? " OR ch_arquivo_superior='000shared'" : "") + ")";
                    query.order_by.asc = new string[] { "nr_tipo_arquivo", "ch_arquivo" };

                    var oResult = new SINJ_ArquivoRN().Consultar(query);
                    sRetorno = JSON.Serialize<Results<SINJ_ArquivoOV>>(oResult);
                    
                }
                else if (!string.IsNullOrEmpty(_id_doc))
                {
                    var id_doc = ulong.Parse(_id_doc);
                    var oResult = new SINJ_ArquivoRN().Doc(id_doc);
                    sRetorno = JSON.Serialize<SINJ_ArquivoOV>(oResult);
                }

                var ind_count = sRetorno.IndexOf("\"result_count\": ") + "\"result_count\": ".Length;
                var ind_chaves = sRetorno.IndexOf("}", ind_count);
                var ind_virgula = sRetorno.IndexOf(",", ind_count);
                var busca = new LogBuscar
                {
                    RegistrosTotal = sRetorno.Substring(ind_count, (ind_chaves > 0 ? ind_chaves : ind_virgula) - ind_count),
                    PesquisaLight = query
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), busca, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (Exception ex)
            {
                sRetorno = "{\"error_message\": \"Ocorreu erro um erro na consulta do(s) arquivo(s).\"}";
                var erro = new ErroRequest
                {
                    Pagina = context.Request.Path,
                    RequestQueryString = context.Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                if (sessao_usuario != null)
                {
                    LogErro.gravar_erro(Util.GetEnumDescription(action)+".PES", erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Exclusao
{
    /// <summary>
    /// Summary description for ArquivoDel
    /// </summary>
    public class ArquivoExcluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var _ch_arquivo = context.Request["ch_arquivo"];
            var _nr_nivel_arquivo = context.Request["nr_nivel_arquivo"];
            var _nr_tipo_arquivo = context.Request["nr_tipo_arquivo"];

            var action = AcoesDoUsuario.arq_pro;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (!string.IsNullOrEmpty(_ch_arquivo))
                {
                    sessao_usuario = Util.ValidarSessao();

                    var query = new Pesquisa();
                    var arquivoRn = new SINJ_ArquivoRN();
                    if (_nr_tipo_arquivo == "0")
                    {
                        query.literal = "(ch_arquivo='" + _ch_arquivo + "') OR (ch_arquivo like '" + _ch_arquivo + "/%')";
                    }
                    else
                    {
                        query.literal = "ch_arquivo='" + _ch_arquivo + "'";
                    }

                    query.limit = null;
                    var result = arquivoRn.Consultar(query);
                    List<SINJ_ArquivoOV> arquivos_excluidos = new List<SINJ_ArquivoOV>();
                    List<SINJ_ArquivoOV> arquivos_nao_excluidos = new List<SINJ_ArquivoOV>();

                    foreach (var arquivoOv in result.results)
                    {

                        var excluidoOv = new ExcluidoOV();
                        excluidoOv.id_doc_excluido = arquivoOv._metadata.id_doc;
                        excluidoOv.ds_justificativa = "Arquivo Excluído";
                        excluidoOv.dt_exclusao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                        excluidoOv.nm_base_excluido = util.BRLight.Util.GetVariavel("NmBaseArquivo", true);
                        excluidoOv.nm_login_usuario_exclusao = sessao_usuario.nm_login_usuario;
                        excluidoOv.json_doc_excluido = JSON.Serialize<SINJ_ArquivoOV>(arquivoOv);
                        if (arquivoRn.Excluir(arquivoOv._metadata.id_doc))
                        {
                            arquivos_nao_excluidos.Add(arquivoOv);
                        }
                        else
                        {
                            new ExcluidoRN().Incluir(excluidoOv);
                            arquivos_excluidos.Add(arquivoOv);
                            var log_excluir = new LogExcluir
                            {
                                id_doc = arquivoOv._metadata.id_doc,
                                nm_base = util.BRLight.Util.GetVariavel("NmBaseArquivo", true)
                            };
                            LogOperacao.gravar_operacao(Util.GetEnumDescription(action)+".EXC", log_excluir, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                        }
                    }
                    sRetorno = "{\"excluido\":true, \"arquivos_excluidos\":" + JSON.Serialize<List<SINJ_ArquivoOV>>(arquivos_excluidos) + ", \"arquivos_nao_excluidos\":" + JSON.Serialize<List<SINJ_ArquivoOV>>(arquivos_nao_excluidos) + ", \"success_message\":\"Excluído com Sucesso.\"}";
                    
                }
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocDependenciesException || ex is SessionExpiredException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\", \"ch_arquivo\":\"" + _ch_arquivo + "\"}";
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

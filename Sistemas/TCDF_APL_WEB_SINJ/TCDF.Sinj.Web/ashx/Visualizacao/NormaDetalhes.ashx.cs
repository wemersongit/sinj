using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.Log;
using System.Text.RegularExpressions;

namespace TCDF.Sinj.Web.ashx.Visualizacao
{
    /// <summary>
    /// Summary description for NormaDetalhes
    /// </summary>
    public class NormaDetalhes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Write(GetNormaDetalhes(context));
            context.Response.End();
        }

        public string GetNormaDetalhes(HttpContext context)
        {
            var sRetorno = "";
            var _id_doc = context.Request["id_doc"];
            var _ch_norma = context.Request["id_norma"];
            ulong id_doc = 0;
            var normaRn = new NormaRN();
            NormaDetalhada normaDetalhada = null;
            NormaOV normaOv = null;
            var action = AcoesDoUsuario.nor_vis;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                if (ulong.TryParse(_id_doc, out id_doc))
                {
                    normaOv = normaRn.Doc(id_doc);
                }
                else if (!string.IsNullOrEmpty(_ch_norma))
                {
                    Util.rejeitarInject(_ch_norma);
                    normaOv = normaRn.Doc(_ch_norma);
                }
                else
                {
                    throw new ParametroInvalidoException("Não foi passado parametro para a busca.");
                }
                if (normaOv != null)
                {
                    var sNorma = JSON.Serialize<NormaOV>(normaOv);
                    normaDetalhada = JSON.Deserializa<NormaDetalhada>(sNorma);
                    normaDetalhada.origensOv = new List<OrgaoOV>();
                    foreach (var origem in normaDetalhada.origens)
                    {
                        normaDetalhada.origensOv.Add(new OrgaoRN().Doc(origem.ch_orgao));
                    }
                    var tipoDeNormaOv = new TipoDeNormaRN().Doc(normaDetalhada.ch_tipo_norma);
                    var sTipoDeNormaOv = JSON.Serialize<TipoDeNormaOV>(tipoDeNormaOv);
                    normaDetalhada.tipoDeNorma = JSON.Deserializa<TipoDeNorma>(sTipoDeNormaOv);
                    normaDetalhada.ds_ementa = Regex.Replace(normaDetalhada.ds_ementa, "\\<[^\\>]*\\>", string.Empty);
                    normaDetalhada.ds_observacao = Regex.Replace(normaDetalhada.ds_observacao, "\\<[^\\>]*\\>", string.Empty);
                    normaDetalhada.dt_controle_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                    sRetorno = JSON.Serialize<NormaDetalhada>(normaDetalhada);
                }
                else
                {
                    sRetorno = "{\"error_message\":\"Norma não encontrada.\"}";
                }
                var log_visualizar = new LogVisualizar
                {
                    id_doc = id_doc,
                    ch_doc = _ch_norma
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_visualizar, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (Exception ex)
            {
                if (ex is PermissionException)
                {
                    sRetorno = "{\"error_type\": \"Unauthorized\", \"error_message\": \"" + ex.Message + "\", \"ch_doc_error\":\"" + _ch_norma + "\", \"id_doc_error\":\"" + _id_doc + "\"}";
                }
                else if (ex is DocNotFoundException)
                {
                    sRetorno = "{\"error_type\": \"NotFound\", \"error_message\": \"" + ex.Message + "\", \"ch_doc_error\":\"" + _ch_norma + "\", \"id_doc_error\":\"" + _id_doc + "\"}";
                }
                else if (ex is SessionExpiredException)
                {
                    sRetorno = "{\"error_type\": \"SessionExpired\", \"error_message\": \"" + ex.Message + "\", \"ch_doc_error\":\"" + _ch_norma + "\", \"id_doc_error\":\"" + _id_doc + "\"}";
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
            return sRetorno;
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

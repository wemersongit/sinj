using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.Log;
using System.Text.RegularExpressions;

namespace TCDF.Sinj.Portal.Web.ashx.Visualizacao
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
            var action = "PORTAL_NOR.VIS";
            try
            {
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
                    sRetorno = JSON.Serialize<NormaDetalhada>(normaDetalhada);
                }
                else
                {
                    sRetorno = "{\"error_message\":\"Norma não encontrada.\"}";
                }
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocNotFoundException || ex is SessionExpiredException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\", \"ch_doc_error\":\"" + _ch_norma + "\", \"id_doc_error\":\"" + _id_doc + "\"}";
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
                LogErro.gravar_erro(action, erro, "visitante", "visitante");
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

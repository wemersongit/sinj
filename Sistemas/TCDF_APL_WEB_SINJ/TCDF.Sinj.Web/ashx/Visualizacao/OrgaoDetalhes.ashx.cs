using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.Log;
using util.BRLight;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using neo.BRLightREST;

namespace TCDF.Sinj.Web.ashx.Visualizacao
{
    /// <summary>
    /// Summary description for OrgaoDetalhes
    /// </summary>
    public class OrgaoDetalhes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            var _id_doc = context.Request["id_doc"];
            var _ch_orgao = context.Request["ch_orgao"];
            ulong id_doc = 0;
            var orgaoRn = new OrgaoRN();
            OrgaoDetalhado orgao = null;
            OrgaoOV orgaoOv = null;
            var action = AcoesDoUsuario.org_vis;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                if (ulong.TryParse(_id_doc, out id_doc))
                {
                    orgaoOv = orgaoRn.Doc(id_doc);
                }
                else if (!string.IsNullOrEmpty(_ch_orgao))
                {
                    orgaoOv = orgaoRn.Doc(_ch_orgao);
                }
                else
                {
                    throw new ParametroInvalidoException("Não foi passado parametro para a busca.");
                }
                if (orgaoOv != null)
                {
                    var sOrgaoOv = JSON.Serialize<OrgaoOV>(orgaoOv);
                    orgao = JSON.Deserializa<OrgaoDetalhado>(sOrgaoOv);
                    if (!string.IsNullOrEmpty(orgao.ch_orgao_pai))
                    {
                        var orgao_superior_ov = orgaoRn.Doc(orgao.ch_orgao_pai);
                        var s_orgao_superior_ov = JSON.Serialize<OrgaoOV>(orgao_superior_ov);
                        orgao.set_orgao_superior = JSON.Deserializa<OrgaoDetalhado>(s_orgao_superior_ov);
                    }
                    if (orgao.ch_orgao_anterior != null && orgao.ch_orgao_anterior.Length > 0)
                    {
                        var orgaos_anteriores = orgaoRn.BuscarOrgaosAnteriores(orgao.ch_orgao_anterior);
                        if (orgaos_anteriores != null && orgaos_anteriores.Count > 0)
                        {
                            var s_orgaos_anteriores = JSON.Serialize<List<OrgaoOV>>(orgaos_anteriores);
                            orgao.set_orgaos_anteriores = JSON.Deserializa<List<OrgaoDetalhado>>(s_orgaos_anteriores);
                        }
                    }
                    var orgaos_filhos = orgaoRn.BuscarOrgaosFilhos(orgao.ch_orgao);
                    if (orgaos_filhos != null && orgaos_filhos.Count > 0)
                    {
                        var s_orgaos_filhos = JSON.Serialize<List<OrgaoOV>>(orgaos_filhos);
                        orgao.set_orgaos_inferiores = JSON.Deserializa<List<OrgaoDetalhado>>(s_orgaos_filhos);
                    }
                    var orgaos_posteriores = orgaoRn.BuscarOrgaosPosteriores(orgao.ch_orgao);
                    if(orgaos_posteriores != null && orgaos_posteriores.Count > 0){
                        var s_orgaos_posteriores = JSON.Serialize<List<OrgaoOV>>(orgaos_posteriores);
                        orgao.set_orgaos_posteriores = JSON.Deserializa<List<OrgaoDetalhado>>(s_orgaos_posteriores);
                    }
                    sRetorno = JSON.Serialize<OrgaoDetalhado>(orgao);
                }
                else
                {
                    sRetorno = "{\"error_message\":\"Órgão não encontrado.\"}";
                }
                var log_visualizar = new LogVisualizar
                {
                    id_doc = id_doc,
                    ch_doc = _ch_orgao
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_visualizar, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocNotFoundException || ex is SessionExpiredException)
                {
                    sRetorno = "{\"error_message\": " + ex.Message + ", \"id_doc_error\":" + _id_doc + "}";
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

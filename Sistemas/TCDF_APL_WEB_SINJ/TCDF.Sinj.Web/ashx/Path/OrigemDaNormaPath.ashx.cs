using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Path
{
    /// <summary>
    /// Summary description for OrigemDaNormaPath
    /// </summary>
    public class OrigemDaNormaPath : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            Pesquisa pesquisa = new Pesquisa();

            var _id_doc = context.Request["id_doc"];
            ulong id_doc = 0;
            var action = AcoesDoUsuario.org_edt;
            SessaoUsuarioOV sessao_usuario = null;

            try
            {
                if (!string.IsNullOrEmpty(_id_doc) && ulong.TryParse(_id_doc, out id_doc))
                {
                    var orgaoOv = new OrgaoRN().Doc(id_doc);
                    sessao_usuario = Util.ValidarSessao();
                    Util.ValidarUsuario(sessao_usuario, action);
                    sRetorno = JSON.Serialize<object>(AtualizarNormasQueUsamOOrgao(orgaoOv));
                }
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocDuplicateKeyException || ex is SessionExpiredException || ex is DocValidacaoException)
                {
                    sRetorno = "{\"error_message\": \"" + ex.Message + "\", \"id_doc_error\":" + _id_doc + "}";
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

        private object AtualizarNormasQueUsamOOrgao(OrgaoOV orgaoOv)
        {
            var normaRn = new NormaRN();
            var orgaoRn = new OrgaoRN();
            ulong sucesso = 0;
            ulong falha = 0;
            ulong offset = 0;
            ulong total_de_normas = 0;
            List<object> retorno_dos_filhos = new List<object>();
            try
            {
                var opResult = new opResult { success = 1, failure = 1 };
                var result_normas = normaRn.Consultar(new Pesquisa { literal = "'" + orgaoOv.ch_orgao + "'=any(ch_orgao)", limit = "1", order_by = new Order_By { asc = new string[] { "id_doc" } } });
                total_de_normas = result_normas.result_count;
                while (offset < total_de_normas)
                {
                    try
                    {
                        var retorno = normaRn.PathPut<object>(new Pesquisa { literal = "'" + orgaoOv.ch_orgao + "'=any(ch_orgao)", limit = "200", offset = offset.ToString(), order_by = new Order_By { asc = new string[] { "id_doc" } } }, new List<opMode<object>> { new opMode<object> { path = "origens/*", fn = "attr_equals", mode = "update", args = new object[] { "ch_orgao", orgaoOv.ch_orgao, new { sg_orgao = orgaoOv.sg_hierarquia, nm_orgao = orgaoOv.nm_hierarquia } } } });
                        opResult = JSON.Deserializa<opResult>(retorno);
                        sucesso += opResult.success;
                        falha += opResult.failure;
                        offset += 200;
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
                var orgaos_filhos = orgaoRn.BuscarOrgaosFilhos(orgaoOv.ch_orgao);
                foreach (var orgao_filho in orgaos_filhos)
                {
                    var retorno_do_filho = AtualizarNormasQueUsamOOrgao(orgao_filho);
                    if (retorno_do_filho != "")
                    {
                        retorno_dos_filhos.Add(AtualizarNormasQueUsamOOrgao(orgao_filho));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro na atualização das normas que usao o Órgão de ID="+orgaoOv._metadata.id_doc+".",ex);
            }
            return new {sg_orgao= orgaoOv.sg_orgao ,total= total_de_normas,sucesso=sucesso,erro=falha,filhos=retorno_dos_filhos};
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

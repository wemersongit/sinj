using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;

namespace TCDF.Sinj.Web.ashx.Path
{
    /// <summary>
    /// Summary description for VocabularioPath
    /// </summary>
    public class VocabularioPath : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";
            var _id_doc = context.Request["id_doc"];
            var _path = context.Request["path"];
            var _value = context.Request["value"];
            ulong id_doc = 0;
            AcoesDoUsuario action = AcoesDoUsuario.voc_edt;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (!string.IsNullOrEmpty(_id_doc) && ulong.TryParse(_id_doc, out id_doc) && !string.IsNullOrEmpty(_path))
                {
                    VocabularioRN vocabularioRn = new VocabularioRN();
                    if (_path == "st_ativo" || _path == "st_aprovado" || _path == "st_excluir")
                    {
                        action = AcoesDoUsuario.voc_ger;
                        var value = false;
                        if (string.IsNullOrEmpty(_value) || !bool.TryParse(_value, out value))
                        {
                            throw new Exception("Parametros inválidos.");
                        }
                    }
                    sessao_usuario = Util.ValidarSessao();
                    Util.ValidarUsuario(sessao_usuario, action);
                    if (vocabularioRn.PathPut(id_doc, _path, _value, null) == "UPDATED")
                    {
                        sRetorno = "{\"id_doc_success\":" + id_doc + ",\"path_update\":true}";
                    }
                    else
                    {
                        throw new Exception("Erro ao atualizar registro. id_doc:" + _id_doc);
                    }
                }
                else
                {
                    throw new Exception("Parametros inválidos.");
                }
                var log_putpath = new LogPutPath<VocabularioOV>
                {
                    id_doc = id_doc,
                    path = _path,
                    value = _value
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_putpath, id_doc, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocDuplicateKeyException || ex is SessionExpiredException)
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

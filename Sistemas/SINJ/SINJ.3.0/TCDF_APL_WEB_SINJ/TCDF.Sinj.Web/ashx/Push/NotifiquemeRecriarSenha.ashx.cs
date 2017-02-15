using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Web.ashx.Push
{
    /// <summary>
    /// Summary description for NotifiquemeRecriarSenha
    /// </summary>
    public class NotifiquemeRecriarSenha : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";
            var _recriar = context.Request["recriar"];
            var _id_doc = "";
            ulong id_doc = 0;
            var _token = "";
            var _senha_usuario_push = context.Request.Form.GetValues("senha_usuario_push");
            var action = AcoesDoUsuario.pus_edt;
            if (!string.IsNullOrEmpty(_recriar) && _senha_usuario_push.Length > 0)
            {
                var notifiquemeRn = new NotifiquemeRN();
                try
                {
                    _id_doc = _recriar.Substring(0, _recriar.IndexOf('A'));
                    _token = _recriar.Substring(_recriar.IndexOf('A') + 1);
                    var tokenOv = new Token().Doc(_token);
                    if (tokenOv == null)
                    {
                        throw new DocNotFoundException("Solicitação Expirou. Tente Solicitar novamente");
                    }
                    if (_id_doc != "" && ulong.TryParse(_id_doc, out id_doc))
                    {
                        if (_senha_usuario_push.Length == 2)
                        {
                            if (_senha_usuario_push[0] != _senha_usuario_push[1])
                            {
                                throw new DocValidacaoException("Senha Inválida. Confirme a Senha");
                            }
                            if (notifiquemeRn.PathPut(id_doc, "senha_usuario_push", Criptografia.CalcularHashMD5(_senha_usuario_push[0], true), null) == "UPDATED")
                            {
                                sRetorno = "{\"id_doc_success\":" + id_doc + "}";
                                new Token().Delete(tokenOv._metadata.id_doc);
                            }
                            else
                            {
                                sRetorno = "{\"error_message\": \"Ocorreu um erro ao recriar a senha.\"}";
                            }
                        }
                    }
                    else
                    {
                        throw new DocValidacaoException("Senha Inválida. Confirme a Senha");
                    }
                    var log_editar = new LogPutPath<NotifiquemeOV>
                    {
                        id_doc = id_doc,
                        path = "senha_usuario_push",
                        value = Criptografia.CalcularHashMD5(_senha_usuario_push[0], true)
                    };
                    LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_editar, id_doc, "", "");
                }
                catch (Exception ex)
                {
                    if (ex is DocValidacaoException || ex is DocNotFoundException)
                    {
                        ///ex is DocNotFoundException então vou redirecionar para limpar o token da url
                        sRetorno = "{\"error_message\": \"" + ex.Message + "\", \"DocNotFoundException\":"+(ex is DocNotFoundException)+"}";
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
                    LogErro.gravar_erro("recriar_senha_push", erro, "", "");
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
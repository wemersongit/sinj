using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Web.ashx.Cadastro
{
    /// <summary>
    /// Summary description for AutoriaEditar
    /// </summary>
    public class AutoriaEditar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno;
            var _id_doc = context.Request["id_doc"];
            AutoriaOV autoriaOv = null;
            ulong id_doc = 0;
            var action = AcoesDoUsuario.aut_edt;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (!string.IsNullOrEmpty(_id_doc) && ulong.TryParse(_id_doc, out id_doc))
                {
                    sessao_usuario = Util.ValidarSessao();
                    Util.ValidarUsuario(sessao_usuario, action);
                    var _nm_autoria = context.Request["nm_autoria"];

                    AutoriaRN autoriaRn = new AutoriaRN();
                    autoriaOv = autoriaRn.Doc(id_doc);

                    if (autoriaOv.nm_autoria == _nm_autoria)
                    {
                        throw new Exception("Nenhuma alteração foi feita. id_doc:" + id_doc);
                    }
                    autoriaOv.nm_autoria = _nm_autoria;

                    autoriaOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                    if (autoriaRn.Atualizar(id_doc, autoriaOv))
                    {
                        sRetorno = "{\"id_doc_success\":" + id_doc + ",\"update\":true}";
                    }
                    else
                    {
                        throw new Exception("Erro ao atualizar registro. id_doc:" + id_doc);
                    }
                }
                else
                {
                    throw new Exception("Erro ao atualizar registro. id_doc:" + _id_doc);
                }
                var log_atualizar = new LogAlterar<AutoriaOV>
                {
                    id_doc = id_doc,
                    registro = autoriaOv
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_atualizar, id_doc, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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
                LogErro.gravar_erro(Util.GetEnumDescription(action), erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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

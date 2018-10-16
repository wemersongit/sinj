using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Cadastro
{
    /// <summary>
    /// Summary description for TipoDePublicacaoEditar
    /// </summary>
    public class TipoDePublicacaoEditar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno;
            var _id_doc = context.Request["id_doc"];
            TipoDePublicacaoOV tipoDePublicacaoOv = null;
            ulong id_doc = 0;
            var action = AcoesDoUsuario.tdp_edt;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (!string.IsNullOrEmpty(_id_doc) && ulong.TryParse(_id_doc, out id_doc))
                {
                    sessao_usuario = Util.ValidarSessao();
                    Util.ValidarUsuario(sessao_usuario, action);
                    var _nm_tipo_publicacao = context.Request["nm_tipo_publicacao"];
                    var _ds_tipo_publicacao = context.Request["ds_tipo_publicacao"];

                    TipoDePublicacaoRN tipoDePublicacaoRn = new TipoDePublicacaoRN();
                    tipoDePublicacaoOv = tipoDePublicacaoRn.Doc(id_doc);
                    tipoDePublicacaoOv.nm_tipo_publicacao = _nm_tipo_publicacao;
                    tipoDePublicacaoOv.ds_tipo_publicacao = _ds_tipo_publicacao;

                    tipoDePublicacaoOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                    if (tipoDePublicacaoRn.Atualizar(id_doc, tipoDePublicacaoOv))
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
                var log_atualizar = new LogAlterar<TipoDePublicacaoOV>
                {
                    id_doc = id_doc,
                    registro = tipoDePublicacaoOv
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_atualizar, id_doc, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
            }
            catch (Exception ex)
            {
                if (ex is PermissionException || ex is DocDuplicateKeyException || ex is SessionExpiredException || ex is DocValidacaoException)
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

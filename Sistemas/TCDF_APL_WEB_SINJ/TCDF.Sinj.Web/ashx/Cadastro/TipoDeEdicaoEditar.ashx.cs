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
    /// Summary description for EdicaoEditar
    /// </summary>
    public class TipoDeEdicaoEditar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno;
            var _id_doc = context.Request["id_doc"];
            TipoDeEdicaoOV tipoDeEdicaoOv = null;
            ulong id_doc = 0;
            var action = AcoesDoUsuario.tdf_edt;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (!string.IsNullOrEmpty(_id_doc) && ulong.TryParse(_id_doc, out id_doc))
                {
                    sessao_usuario = Util.ValidarSessao();
                    Util.ValidarUsuario(sessao_usuario, action);
                    var _nm_tipo_edicao = context.Request["nm_tipo_edicao"];
                    var _ds_tipo_edicao = context.Request["ds_tipo_edicao"];
                    var _st_edicao = context.Request["st_edicao"];
                    var st_edicao = false;
                    bool.TryParse(_st_edicao, out st_edicao);

                    TipoDeEdicaoRN tipoDeEdicaoRn = new TipoDeEdicaoRN();
                    tipoDeEdicaoOv = tipoDeEdicaoRn.Doc(id_doc);

                    if (tipoDeEdicaoOv.nm_tipo_edicao == _nm_tipo_edicao && tipoDeEdicaoOv.ds_tipo_edicao == _ds_tipo_edicao && tipoDeEdicaoOv.st_edicao == st_edicao)
                    {
                        throw new Exception("Nenhuma alteração foi feita. id_doc:" + id_doc);
                    }
                    tipoDeEdicaoOv.nm_tipo_edicao = _nm_tipo_edicao;
                    tipoDeEdicaoOv.ds_tipo_edicao = _ds_tipo_edicao;
                    tipoDeEdicaoOv.st_edicao = st_edicao;

                    tipoDeEdicaoOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                    if (tipoDeEdicaoRn.Atualizar(id_doc, tipoDeEdicaoOv))
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
                var log_atualizar = new LogAlterar<TipoDeEdicaoOV>
                {
                    id_doc = id_doc,
                    registro = tipoDeEdicaoOv
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

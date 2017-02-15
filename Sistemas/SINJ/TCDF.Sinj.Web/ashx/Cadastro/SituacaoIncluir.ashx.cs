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
    /// Summary description for SituacaoIncluir
    /// </summary>
    public class SituacaoIncluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno;
            SituacaoOV situacaoOv = null;
            var action = AcoesDoUsuario.sit_inc;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                var _nm_situacao = context.Request["nm_situacao"];
                var _ds_situacao = context.Request["ds_situacao"];
                var _nr_peso_situacao = context.Request["nr_peso_situacao"];
                var nr_peso_situacao = 0;
                int.TryParse(_nr_peso_situacao, out nr_peso_situacao);

                situacaoOv = new SituacaoOV();

                situacaoOv.nm_situacao = _nm_situacao;
                situacaoOv.ds_situacao = _ds_situacao;
                situacaoOv.nr_peso_situacao = nr_peso_situacao;

                situacaoOv.nm_login_usuario_cadastro = sessao_usuario.nm_login_usuario;
                situacaoOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                var id_doc = new SituacaoRN().Incluir(situacaoOv);
                if (id_doc > 0)
                {
                    sRetorno = "{\"id_doc_success\":" + id_doc + "}";
                }
                else
                {
                    throw new Exception("Erro ao incluir registro.");
                }
                var log_incluir = new LogIncluir<SituacaoOV>
                {
                    registro = situacaoOv
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), log_incluir, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
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
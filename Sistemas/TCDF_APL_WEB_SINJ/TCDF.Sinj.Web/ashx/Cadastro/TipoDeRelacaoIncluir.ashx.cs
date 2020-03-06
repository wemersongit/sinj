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
    /// Summary description for TipoDeRelacaoIncluir
    /// </summary>
    public class TipoDeRelacaoIncluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno;
            TipoDeRelacaoOV tipoDeRelacaoOv = null;
            var action = AcoesDoUsuario.tdr_inc;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                var _nm_tipo_relacao = context.Request["nm_tipo_relacao"];
                var _ds_tipo_relacao = context.Request["ds_tipo_relacao"];
                var _ds_texto_para_alterador = context.Request["ds_texto_para_alterador"];
                var _ds_texto_para_alterado = context.Request["ds_texto_para_alterado"];
                var _nr_importancia = context.Request["nr_importancia"];
                var nr_importancia = 0;
                int.TryParse(_nr_importancia, out nr_importancia);
                var _in_relacao_de_acao = context.Request["_in_relacao_de_acao"];
                var _in_selecionavel = context.Request["in_selecionavel"];
                tipoDeRelacaoOv = new TipoDeRelacaoOV();

                tipoDeRelacaoOv.nm_tipo_relacao = _nm_tipo_relacao;
                tipoDeRelacaoOv.ds_tipo_relacao = _ds_tipo_relacao;
                tipoDeRelacaoOv.ds_texto_para_alterador = _ds_texto_para_alterador;
                tipoDeRelacaoOv.ds_texto_para_alterado = _ds_texto_para_alterado;
                tipoDeRelacaoOv.nr_importancia = nr_importancia;
                tipoDeRelacaoOv.in_relacao_de_acao = _in_relacao_de_acao == "1";
                tipoDeRelacaoOv.in_selecionavel = _in_selecionavel == "1";

                tipoDeRelacaoOv.nm_login_usuario_cadastro = sessao_usuario.nm_login_usuario;
                tipoDeRelacaoOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                var id_doc = new TipoDeRelacaoRN().Incluir(tipoDeRelacaoOv);
                if (id_doc > 0)
                {
                    sRetorno = "{\"id_doc_success\":" + id_doc + "}";
                }
                else
                {
                    throw new Exception("Erro ao incluir registro.");
                }
                var log_incluir = new LogIncluir<TipoDeRelacaoOV>
                {
                    registro = tipoDeRelacaoOv
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

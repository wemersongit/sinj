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
    /// Summary description for TipoDeRelacaoEditar
    /// </summary>
    public class TipoDeRelacaoEditar : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno;
            var _id_doc = context.Request["id_doc"];
            TipoDeRelacaoOV tipoDeRelacaoOv = null;
            ulong id_doc = 0;
            var action = AcoesDoUsuario.tdr_edt;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (!string.IsNullOrEmpty(_id_doc) && ulong.TryParse(_id_doc, out id_doc))
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
                    var _in_relacao_de_acao = context.Request["in_relacao_de_acao"];
                    var in_relacao_de_acao = false;
                    bool.TryParse(_in_relacao_de_acao, out in_relacao_de_acao);


                    TipoDeRelacaoRN tipoDeRelacaoRn = new TipoDeRelacaoRN();
                    tipoDeRelacaoOv = tipoDeRelacaoRn.Doc(id_doc);

                    tipoDeRelacaoOv.nm_tipo_relacao = _nm_tipo_relacao;
                    tipoDeRelacaoOv.ds_tipo_relacao = _ds_tipo_relacao;
                    tipoDeRelacaoOv.ds_texto_para_alterador = _ds_texto_para_alterador;
                    tipoDeRelacaoOv.ds_texto_para_alterado = _ds_texto_para_alterado;
                    tipoDeRelacaoOv.nr_importancia = nr_importancia;
                    tipoDeRelacaoOv.in_relacao_de_acao = in_relacao_de_acao;

                    tipoDeRelacaoOv.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = sessao_usuario.nm_login_usuario });
                    if (tipoDeRelacaoRn.Atualizar(id_doc, tipoDeRelacaoOv))
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
                var log_atualizar = new LogAlterar<TipoDeRelacaoOV>
                {
                    id_doc = id_doc,
                    registro = tipoDeRelacaoOv
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
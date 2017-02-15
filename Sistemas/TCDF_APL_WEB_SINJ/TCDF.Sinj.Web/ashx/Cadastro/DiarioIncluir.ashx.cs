using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.RN;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Cadastro
{
    /// <summary>
    /// Summary description for DiarioIncluir
    /// </summary>
    public class DiarioIncluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            var sRetorno = "";
            DiarioOV diarioOv = null;
            var action = AcoesDoUsuario.dio_inc;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);
                var _ch_tipo_fonte = context.Request["ch_tipo_fonte"];
                var _nm_tipo_fonte = context.Request["nm_tipo_fonte"];
                var _ch_tipo_edicao = context.Request["ch_tipo_edicao"];
                var _nm_tipo_edicao = context.Request["nm_tipo_edicao"];
                var _nm_diferencial_edicao = context.Request["nm_diferencial_edicao"];
                var _nr_diario = context.Request["nr_diario"];
                int nr_diario = 0;
                if (!int.TryParse(_nr_diario, out nr_diario))
                {
                    throw new DocValidacaoException("Número inválido.");
                }
                var _cr_diario = context.Request["cr_diario"];
                var _secoes_diario = context.Request.Form.GetValues("secao_diario");
                var _dt_assinatura = context.Request["dt_assinatura"];


                var _st_suplemento = context.Request["st_suplemento"];
                var st_suplemento = false;
                var _nm_diferencial_suplemento = context.Request["nm_diferencial_suplemento"];

				var _st_pendente = context.Request["st_pendente"];
                var st_pendente = false;
                var _ds_pendencia = context.Request["ds_pendencia"];
				
                var _nr_arquivos = context.Request.Form.GetValues("nr_arquivos");
					
                DiarioRN diarioRn = new DiarioRN();
                diarioOv = new DiarioOV();
                diarioOv.ch_tipo_fonte = _ch_tipo_fonte;
                diarioOv.nm_tipo_fonte = _nm_tipo_fonte;
                diarioOv.ch_tipo_edicao = _ch_tipo_edicao;
                diarioOv.nm_tipo_edicao = _nm_tipo_edicao;
                diarioOv.nm_diferencial_edicao = _nm_diferencial_edicao;
                diarioOv.nr_diario = nr_diario;
                diarioOv.cr_diario = _cr_diario;
				diarioOv.secao_diario = "";
                if (_secoes_diario != null)
                {
                    for (var i = 0; i < _secoes_diario.Length; i++ )
                    {
                        diarioOv.secao_diario += (diarioOv.secao_diario != "" ? (i < (_secoes_diario.Length - 1) ? ", " : " e ") : "") + _secoes_diario[i];
                    }
                }
				diarioOv.dt_assinatura = _dt_assinatura;

                bool.TryParse(_st_suplemento, out st_suplemento);
                diarioOv.st_suplemento = st_suplemento;
                if (st_suplemento)
                {
                    diarioOv.nm_diferencial_suplemento = _nm_diferencial_suplemento;
                }
                else
                {
                    diarioOv.nm_diferencial_suplemento = "";
                }

                bool.TryParse(_st_pendente, out st_pendente);
                diarioOv.st_pendente = st_pendente;
                if (st_pendente)
                {
                    diarioOv.ds_pendencia = _ds_pendencia;
                }
                else
                {
                    diarioOv.ds_pendencia = "";
                }

                if(_nr_arquivos != null){
                    var sArquivo = "";
                    var ds_arquivo = "";
                    foreach(var nr_arquivo in _nr_arquivos){
                        sArquivo = context.Request["json_arquivo_diario_" + nr_arquivo];
                        ds_arquivo = context.Request["ds_arquivo_diario_" + nr_arquivo];
                        diarioOv.arquivos.Add(new ArquivoDiario {
                            arquivo_diario = JSON.Deserializa<neo.BRLightREST.ArquivoOV>(sArquivo),
                            ds_arquivo = ds_arquivo
                        });
                    }
                }
                

                diarioOv.nm_login_usuario_cadastro = sessao_usuario.nm_login_usuario;
                diarioOv.dt_cadastro = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss");
                var id_doc = diarioRn.Incluir(diarioOv);
                if (id_doc > 0)
                {
                    sRetorno = "{\"id_doc_success\":" + id_doc + "}";
                }
                else
                {
                    sRetorno = "{\"error_message\": \"Erro ao cadastrar diário.\" }";
                    throw new Exception("Erro ao incluir novo diário.");
                }
                var log_incluir = new LogIncluir<DiarioOV>
                {
                    registro = diarioOv
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
                var erro = new ErroRequest {
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
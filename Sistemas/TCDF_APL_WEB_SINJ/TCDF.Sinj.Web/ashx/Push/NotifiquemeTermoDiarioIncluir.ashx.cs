using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web.ashx.Push
{
    /// <summary>
    /// Summary description for NotifiquemeTermoDiarioIncluir
    /// </summary>
    public class NotifiquemeTermoDiarioIncluir : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string sRetorno = "";
            var _ch_tipo_fonte_diario_monitorado = context.Request["ch_tipo_fonte_diario_monitorado"];
            var _nm_tipo_fonte_diario_monitorado = context.Request["nm_tipo_fonte_diario_monitorado"];
            var _ds_termo_diario_monitorado = context.Request["ds_termo_diario_monitorado"];
            var _in_exata_diario_monitorado = context.Request["in_exata_diario_monitorado"];
            ulong id_push = 0;
            var notifiquemeOv = new NotifiquemeOV();
            var action = AcoesDoUsuario.pus_edt;
            SessaoNotifiquemeOV sessaoNotifiquemeOv = null;
            try
            {
                if (!string.IsNullOrEmpty(_ds_termo_diario_monitorado))
                {
                    var termos = _ds_termo_diario_monitorado.Split(' ');
                    if (termos.Count() == 1 && termos[0].Length <= 3)
                    {
                        throw new DocValidacaoException("O valor informado é uma palavra muito curta que, além de irrelevante, pode ocasionar notificações excessivas ao seu e-mail.");
                    }

                    var notifiquemeRn = new NotifiquemeRN();
                    sessaoNotifiquemeOv = notifiquemeRn.LerSessaoNotifiquemeOv();
                    notifiquemeOv = notifiquemeRn.Doc(sessaoNotifiquemeOv.email_usuario_push);
                    id_push = notifiquemeOv._metadata.id_doc;
                    if (notifiquemeOv.termos_diarios_monitorados.Count<TermoDiarioMonitoradoPushOV>(t => t.ds_termo_diario_monitorado.Equals(_ds_termo_diario_monitorado, StringComparison.InvariantCultureIgnoreCase) && t.ch_termo_diario_monitorado == _ch_tipo_fonte_diario_monitorado) <= 0)
                    {
                        TermoDiarioMonitoradoPushOV termoMonitorado = new TermoDiarioMonitoradoPushOV
                        {
                            ch_termo_diario_monitorado = Guid.NewGuid().ToString("N"),
                            ch_tipo_fonte_diario_monitorado = _ch_tipo_fonte_diario_monitorado,
                            nm_tipo_fonte_diario_monitorado = _nm_tipo_fonte_diario_monitorado,
                            ds_termo_diario_monitorado = _ds_termo_diario_monitorado,
                            dt_cadastro_termo_diario_monitorado = DateTime.Now.ToString("dd'/'MM'/'yyyy"),
                            st_termo_diario_monitorado = true,
                            in_exata_diario_monitorado = _in_exata_diario_monitorado == "1"
                        };
                        notifiquemeOv.termos_diarios_monitorados.Add(termoMonitorado);
                        if (notifiquemeRn.Atualizar(id_push, notifiquemeOv))
                        {
                            notifiquemeOv.senha_usuario_push = null;
                            sRetorno = JSON.Serialize<NotifiquemeOV>(notifiquemeOv);
                        }
                        else
                        {
                            throw new Exception("Erro ao adicionar termo para monitorar diário. termo:" + _ds_termo_diario_monitorado);
                        }
                    }
                    else
                    {
                        throw new DocDuplicateKeyException("Não é possível salvar essa informação porque ela está duplicada.");
                    }
                }
                else
                {
                    throw new Exception("Erro ao adicionar termo para monitorar diário. termo:" + _ds_termo_diario_monitorado);
                }
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
                if (sessaoNotifiquemeOv != null)
                {
                    LogErro.gravar_erro(Util.GetEnumDescription(action) + ".DIARIO.ADD", erro, sessaoNotifiquemeOv.nm_usuario_push, sessaoNotifiquemeOv.email_usuario_push);
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

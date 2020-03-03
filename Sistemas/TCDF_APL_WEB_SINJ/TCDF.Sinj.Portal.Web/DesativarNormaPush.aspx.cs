using System;
using System.Web;
using System.Web.UI;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;
using System.Collections.Generic;

namespace TCDF.Sinj.Portal.Web
{
    public partial class DesativarNormaPush : System.Web.UI.Page
    {
        protected bool _ok = false;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Util.ehReplica())
            {
                Response.Redirect("./", true);
            }

            string sRetorno = "";
            var _email_usuario_push = Request["email_usuario_push"];
            var _ch_norma_monitorada = Request["ch_norma_monitorada"];
            var _ch_criacao_norma_monitorada = Request["ch_criacao_norma_monitorada"];
            var _ch_termo_diario_monitorado = Request["ch_termo_diario_monitorado"];
            ulong id_push = 0;
            var notifiquemeOv = new NotifiquemeOV();
            try
            {
                if (!string.IsNullOrEmpty(_ch_norma_monitorada))
                {
                    var notifiquemeRn = new NotifiquemeRN();
                    notifiquemeOv = notifiquemeRn.Doc(_email_usuario_push);
                    id_push = notifiquemeOv._metadata.id_doc;
                    if (notifiquemeOv.normas_monitoradas.RemoveAll(ch => ch.ch_norma_monitorada == _ch_norma_monitorada) > 0)
                    {
                        if (notifiquemeRn.Atualizar(id_push, notifiquemeOv))
                        {
                            notifiquemeRn.AtualizarSessao(notifiquemeOv);
                            sRetorno = "Notificação removida com sucesso.";
                            _ok = true;
                        }
                        else
                        {
                            throw new Exception("Erro ao remover critério do monitoramento. Código do erro: " + id_push + "#" + _ch_norma_monitorada);
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(_ch_criacao_norma_monitorada))
                {
                    var notifiquemeRn = new NotifiquemeRN();
                    notifiquemeOv = notifiquemeRn.Doc(_email_usuario_push);
                    id_push = notifiquemeOv._metadata.id_doc;
                    if (!string.IsNullOrEmpty(_ch_criacao_norma_monitorada))
                    {
                        if (notifiquemeOv.criacao_normas_monitoradas.RemoveAll(n => n.ch_criacao_norma_monitorada == _ch_criacao_norma_monitorada) > 0)
                        {
                            if (notifiquemeRn.Atualizar(id_push, notifiquemeOv))
                            {
                                sRetorno = "Notificação removida com sucesso.";
                                _ok = true;
                            }
                            else
                            {
                                throw new Exception("Erro ao remover critério do monitoramento. Código do erro: " + id_push + "#" + _ch_criacao_norma_monitorada);
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(_ch_termo_diario_monitorado))
                {
                    var notifiquemeRn = new NotifiquemeRN();
                    notifiquemeOv = notifiquemeRn.Doc(_email_usuario_push);
                    id_push = notifiquemeOv._metadata.id_doc;
                    if (notifiquemeOv.termos_diarios_monitorados.RemoveAll(ch => ch.ch_termo_diario_monitorado == _ch_termo_diario_monitorado) > 0)
                    {
                        if (notifiquemeRn.Atualizar(id_push, notifiquemeOv))
                        {
                            sRetorno = "Notificação removida com sucesso.";
                            _ok = true;
                        }
                        else
                        {
                            throw new Exception("Erro ao remover critério do monitoramento. Código do erro: " + id_push + "#" + _ch_termo_diario_monitorado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sRetorno = ex.Message;
            }
            label_retorno.InnerHtml = sRetorno;


        }
    }
}

using System;
using System.Web;
using System.Web.UI;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;
using System.Collections.Generic;

namespace TCDF.Sinj.Web
{
	
	public partial class DesativarNormaPush : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			string sRetorno = "";
			var _email_usuario_push = Request ["email_usuario_push"];
			var _ch_norma = Request["ch_norma"];
			var _criacao_normas_monitoradas = Request ["criacao_normas_monitoradas"];
			ulong id_push = 0;
			var notifiquemeOv = new NotifiquemeOV();
			try
			{
				if (!string.IsNullOrEmpty(_ch_norma))
				{
					var notifiquemeRn = new NotifiquemeRN();
					notifiquemeOv = notifiquemeRn.Doc(_email_usuario_push);
					id_push = notifiquemeOv._metadata.id_doc;
					if(notifiquemeOv.normas_monitoradas.RemoveAll(ch => ch.ch_norma_monitorada == _ch_norma) == 1)
					{
						var retornoPath = notifiquemeRn.PathPut(id_push, "normas_monitoradas", JSON.Serialize<List<NormaMonitoradaPushOV>>(notifiquemeOv.normas_monitoradas), null);
						if (retornoPath == "UPDATED")
						{
							notifiquemeRn.AtualizarSessao(notifiquemeOv);
							sRetorno = "Notificação removida com sucesso.";
						}
						else
						{
							throw new Exception("Erro ao remover monitoramento da norma.");
						}
					}
				}
				else if (!string.IsNullOrEmpty(_criacao_normas_monitoradas))
				{
					var notifiquemeRn = new NotifiquemeRN();
					notifiquemeOv = notifiquemeRn.Doc(_email_usuario_push);
					id_push = notifiquemeOv._metadata.id_doc;
					int j = 0;
					if (!string.IsNullOrEmpty(_criacao_normas_monitoradas) && int.TryParse(_criacao_normas_monitoradas, out j))
					{
						notifiquemeOv.criacao_normas_monitoradas.RemoveAt(j);
						var retornoPath = notifiquemeRn.PathPut(id_push, "criacao_normas_monitoradas", JSON.Serialize<List<CriacaoDeNormaMonitoradaPushOV>>(notifiquemeOv.criacao_normas_monitoradas), null);
						if (retornoPath == "UPDATED")
						{
							sRetorno = "Notificação removida com sucesso.";
						}
						else
						{
							throw new Exception("Erro ao remover critério do monitoramento. id_push:" + id_push);
						}
					}
				}
			}
			catch(Exception ex){
				sRetorno = ex.Message;
			}
			div_retorno.InnerHtml = sRetorno;
		}
	}
}


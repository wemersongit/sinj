using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.AD;
using TCDF.Sinj.OV;
using neo.BRLightSession;
using util.BRLight;
using neo.BRLightSession.OV;
using neo.BRLightREST;

namespace TCDF.Sinj.RN
{
	public class NotifiquemeRN
	{
		private string nm_cookie;
		private string nm_cookie_look;
		private NotifiquemeAD _pushAd;

		public NotifiquemeRN()
		{
			_pushAd = new NotifiquemeAD();
			nm_cookie = Config.ValorChave("NmCookiePush");
			nm_cookie_look = Config.ValorChave("NmCookiePushLook");
			
		}

		public NotifiquemeRN(bool start_session)
		{
			_pushAd = new NotifiquemeAD();
			nm_cookie = Config.ValorChave("NmCookiePush");
			nm_cookie_look = Config.ValorChave("NmCookiePushLook");
		}

		public Results<NotifiquemeOV> Consultar(Pesquisa query)
		{
			return _pushAd.Consultar(query);
		}

		public string PathPost(ulong id_doc, string path, string value, string retorno)
		{
			return _pushAd.PathPost(id_doc, path, value, retorno);
		}

		public string PathDelete(ulong id_doc, string path, string retorno)
		{
			return _pushAd.PathDelete(id_doc, path, retorno);
		}

		public string PathPut(ulong id_doc, string path, string value, string retorno)
		{
			return _pushAd.PathPut(id_doc, path, value, retorno);
		}

		public NotifiquemeOV Doc(ulong id_doc)
		{
			return _pushAd.Doc(id_doc);
		}

		public NotifiquemeOV Doc(string email_usuario_push)
		{
			return _pushAd.Doc(email_usuario_push);
		}

        public string JsonReg(Pesquisa query)
        {
            return _pushAd.JsonReg(query);
        }

		public ulong Incluir(NotifiquemeOV pushOv)
		{
			Validar(pushOv);
			pushOv.st_push = true;
			return _pushAd.Incluir(pushOv);
		}

		public bool Atualizar(ulong id_doc, NotifiquemeOV pushOv)
		{
			Validar(pushOv);
			return _pushAd.Atualizar(id_doc, pushOv);
		}

		private void Validar(NotifiquemeOV pushOv)
		{
			if (string.IsNullOrEmpty(pushOv.email_usuario_push) || !util.BRLight.ValidaDados.Email(pushOv.email_usuario_push))
			{
				throw new DocValidacaoException("Email inválido.");
			}
			if (string.IsNullOrEmpty(pushOv.nm_usuario_push))
			{
				throw new DocValidacaoException("Nome inválido.");
			}
			if (string.IsNullOrEmpty(pushOv.senha_usuario_push) || pushOv.senha_usuario_push.Length < 6)
			{
				throw new DocValidacaoException("Senha inválida.");
			}
		}

        public SessaoNotifiquemeOV CriarSessao(NotifiquemeOV notifiquemeOv, bool manterAtiva)
		{
			var sessaoNotifiquemeOv = new SessaoNotifiquemeOV();
			FazerParseSessaoUsuarioOV(notifiquemeOv, sessaoNotifiquemeOv);
			Session _session = new Session(nm_cookie, nm_cookie_look);
			_session.Create(manterAtiva);

			var success = _session.Post<SessaoNotifiquemeOV>("pushlogin", sessaoNotifiquemeOv);
			if (success)
			{
				sessaoNotifiquemeOv.sessao_id = _session.db_id;
				sessaoNotifiquemeOv.sessao_chave = _session.db_chave;
				success = _session.Put<SessaoNotifiquemeOV>("pushlogin", sessaoNotifiquemeOv);
                if (success)
                {
                    return sessaoNotifiquemeOv;
                }
			}
			return null;
		}

		public bool AtualizarSessao(NotifiquemeOV notifiquemeOv)
		{
			Session _session = new Session(nm_cookie, nm_cookie_look);
			var sessaoNotifiquemeOv = LerSessaoNotifiquemeOv();
			FazerParseSessaoUsuarioOV(notifiquemeOv, sessaoNotifiquemeOv);
			return _session.Put<SessaoNotifiquemeOV>("pushlogin", sessaoNotifiquemeOv);
		}

		public void FazerParseSessaoUsuarioOV(NotifiquemeOV notifiquemeOv, SessaoNotifiquemeOV sessaoNotifiquemeOv)
		{
			sessaoNotifiquemeOv.id_doc = notifiquemeOv._metadata.id_doc;
			sessaoNotifiquemeOv.nm_usuario_push = notifiquemeOv.nm_usuario_push;
            sessaoNotifiquemeOv.email_usuario_push = notifiquemeOv.email_usuario_push;
            sessaoNotifiquemeOv.ch_normas_monitoradas.Clear();
            foreach (var norma_monitorada in notifiquemeOv.normas_monitoradas)
            {
                sessaoNotifiquemeOv.ch_normas_monitoradas.Add(norma_monitorada.ch_norma_monitorada);
            }
            sessaoNotifiquemeOv.favoritos = notifiquemeOv.favoritos.ToList<string>();
		}

		public SessionOV LerSessao()
		{
			Session _session = new Session(nm_cookie, nm_cookie_look);
			return _session.Get<SessionOV>("pushlogin");
		}

		public SessaoNotifiquemeOV LerSessaoNotifiquemeOv()
		{
			try
			{
				var sessao = LerSessao();
				if (sessao != null)
				{
					return JSON.Deserializa<SessaoNotifiquemeOV>(sessao.ds_valor);
				}
				throw new DocNotFoundException("LerSessaoNotifiquemeOv sessao=null.");
			}
			catch
			{
				return null;
			}
		}

		public void Finalizar()
		{
			Session _session = new Session(nm_cookie, nm_cookie_look);
			_session.Delete("pushlogin");
			Cookies.DeleteCookie(nm_cookie);
			Cookies.DeleteCookie(nm_cookie_look);
		}

		public bool ChecarSessaoAtiva()
		{
			try
			{
				var sessao = LerSessao();
				if (sessao != null)
				{
					var sessaoNotifiquemeOv = JSON.Deserializa<SessaoNotifiquemeOV>(sessao.ds_valor);
					if (!string.IsNullOrEmpty(sessaoNotifiquemeOv.email_usuario_push))
					{
						return true;
					}
				}
			}
			catch
			{
				return false;
			}
			return false;
		}
	}
}

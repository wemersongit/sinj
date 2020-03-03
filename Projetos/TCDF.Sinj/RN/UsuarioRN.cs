using neo.BRLightREST;
using TCDF.Sinj.AD;
using TCDF.Sinj.OV;
using System;

namespace TCDF.Sinj.RN
{
	public class UsuarioRN
	{
		private UsuarioAD _usuarioAd;

		public UsuarioRN()
		{
			_usuarioAd = new UsuarioAD();
		}

		public Results<UsuarioOV> Consultar(Pesquisa query)
		{
			return _usuarioAd.Consultar(query);
		}

		public UsuarioOV Doc(string nm_login_usuario)
		{
			return _usuarioAd.Doc(nm_login_usuario);
		}

		public UsuarioOV Doc(ulong id_doc)
		{
			return _usuarioAd.Doc(id_doc);
		}

		public string PathPut(ulong id_doc, string path, string value, string retorno)
		{
			return _usuarioAd.PathPut(id_doc, path, value, retorno);
		}

		public string JsonReg(ulong id_doc)
		{
			return _usuarioAd.JsonReg(id_doc);
		}

		public string JsonReg(Pesquisa query)
		{
			return _usuarioAd.JsonReg(query);
		}

		public ulong Incluir(UsuarioOV usuarioOv)
		{
			Validar(usuarioOv);
			return _usuarioAd.Incluir(usuarioOv);
		}

		public bool Atualizar(ulong id_doc, UsuarioOV usuarioOv)
		{
			Validar(usuarioOv);
			return _usuarioAd.Atualizar(id_doc, usuarioOv);
		}

		public bool Excluir(ulong id_doc)
		{
			var usuarioOv = Doc(id_doc);
			ValidarDepencias(usuarioOv);
			return _usuarioAd.Excluir(id_doc);
		}

		public void ValidarDepencias(UsuarioOV usuarioOv)
        {
            Pesquisa query = new Pesquisa();
            query.literal = string.Format("nm_login_usuario_cadastro='{0}' or '{0}'=any(nm_login_usuario_alteracao)", usuarioOv.nm_login_usuario);
            if (new AutoriaRN().Consultar(query).results.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por uma ou mais autorias.");
            }
            if (new DiarioRN().Consultar(query).results.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por um ou mais diários.");
            }
            if (new InteressadoRN().Consultar(query).results.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por um ou mais interessados.");
            }
            if (new NormaRN().Consultar(query).results.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por uma ou mais normas.");
            }
            if (new OrgaoRN().Consultar(query).results.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por um ou mais órgãos.");
            }
            if (new ProcuradorRN().Consultar(query).results.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por um ou mais Procuradores.");
            }
            if (new RelatorRN().Consultar(query).results.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por um ou mais Relatores.");
            }
            if (new RequerenteRN().Consultar(query).results.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por um ou mais Requerentes.");
            }
            if (new RequeridoRN().Consultar(query).results.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por um ou mais Requeridos.");
            }
            if (new SituacaoRN().Consultar(query).results.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por um ou mais Situações.");
            }
            if (new TipoDeFonteRN().Consultar(query).results.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por um ou mais Tipos de Fonte.");
            }
            if (new TipoDeNormaRN().Consultar(query).results.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por um ou mais Tipos de Norma.");
            }
            if (new TipoDePublicacaoRN().Consultar(query).results.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por um ou mais Tipos de Publicação.");
            }
            if (new TipoDeRelacaoRN().Consultar(query).results.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por um ou mais Tipos de Relação.");
            }
            if (new UsuarioRN().Consultar(query).results.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por um ou mais Usuários.");
            }
            if (new VocabularioRN().Consultar(query).results.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por um ou mais Termos.");
            }
		}

		private void Validar(UsuarioOV usuarioOv)
		{
			if (string.IsNullOrEmpty(usuarioOv.nm_login_usuario) || usuarioOv.nm_login_usuario.Length < 5)
			{
				throw new DocValidacaoException("Login inv&aacute;lido.");
			}
			if (string.IsNullOrEmpty(usuarioOv.nm_usuario))
			{
				throw new DocValidacaoException("Nome inv&aacute;lido.");
			}
			if (string.IsNullOrEmpty(usuarioOv.senha_usuario) || usuarioOv.senha_usuario.Length < 6)
			{
                throw new DocValidacaoException("Senha inv&aacute;lida.");
			}
			if (usuarioOv.grupos == null || usuarioOv.grupos.Count <= 0)
			{
                throw new DocValidacaoException("Grupo inv&aacute;lido.");
			}
		}

		public bool ValidarPermissao(string nm_login_usuario, string ch_grupo)
		{
			var usuario = Doc(nm_login_usuario);
			if(usuario != null){
				if (usuario.grupos.IndexOf(ch_grupo) > -1)
				{
					return true;
				}
			}
			return false;
		}
	}
}

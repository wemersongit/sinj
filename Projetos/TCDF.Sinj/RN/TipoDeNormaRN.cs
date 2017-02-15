using neo.BRLightREST;
using TCDF.Sinj.AD;
using TCDF.Sinj.OV;
using System;

namespace TCDF.Sinj.RN
{
	public class TipoDeNormaRN
	{
		private TipoDeNormaAD _tipoDeNormaAd;

		public TipoDeNormaRN()
		{
			_tipoDeNormaAd = new TipoDeNormaAD();
		}

		public Results<TipoDeNormaOV> Consultar(Pesquisa query)
		{
			return _tipoDeNormaAd.Consultar(query);
		}

		public TipoDeNormaOV Doc(ulong id_doc)
		{
			var tipoDeNormaOV = _tipoDeNormaAd.Doc(id_doc);
			if (tipoDeNormaOV == null)
			{
				throw new DocNotFoundException("Registro não Encontrado.");
			}
			return tipoDeNormaOV;
		}

		public TipoDeNormaOV Doc(string ch_tipo_norma)
		{
			return _tipoDeNormaAd.Doc(ch_tipo_norma);
		}

		public string JsonReg(ulong id_doc)
		{
			return _tipoDeNormaAd.JsonReg(id_doc);
		}

		public string JsonReg(Pesquisa query)
		{
			return _tipoDeNormaAd.JsonReg(query);
		}

		public ulong Incluir(TipoDeNormaOV tipoDeNormaOV)
		{
			tipoDeNormaOV.ch_tipo_norma = Guid.NewGuid().ToString("N");
			return _tipoDeNormaAd.Incluir(tipoDeNormaOV);
		}

		public bool Atualizar(ulong id_doc, TipoDeNormaOV tipoDeNormaOV)
		{
			Validar(tipoDeNormaOV);
			return _tipoDeNormaAd.Atualizar(id_doc, tipoDeNormaOV);
		}

		public bool Excluir(ulong id_doc)
		{
			var tipoDeNormaOv = Doc(id_doc);
			ValidarDepencias(tipoDeNormaOv);
			return _tipoDeNormaAd.Excluir(id_doc);
		}

		public void ValidarDepencias(TipoDeNormaOV tipoDeNormaOV)
		{
            var normas = new NormaRN().BuscarNormasDoTipoDeNorma(tipoDeNormaOV.ch_tipo_norma);
            if (normas.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por uma ou mais normas.");
            }
		}

		private void Validar(TipoDeNormaOV tipoDeNormaOV)
		{
			if (string.IsNullOrEmpty(tipoDeNormaOV.nm_tipo_norma))
			{
				throw new DocValidacaoException("Nome inválido.");
			}
		}

	}
}
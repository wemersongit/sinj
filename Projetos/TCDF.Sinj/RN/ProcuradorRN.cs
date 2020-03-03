using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.AD;
using TCDF.Sinj.OV;
using neo.BRLightREST;

namespace TCDF.Sinj.RN
{
	public class ProcuradorRN
	{
		private ProcuradorAD _procuradorAd;

		public ProcuradorRN()
		{
			_procuradorAd = new ProcuradorAD();
		}

		public Results<ProcuradorOV> Consultar(Pesquisa query)
		{
			return _procuradorAd.Consultar(query);
		}

		public ProcuradorOV Doc(ulong id_doc)
		{
			var procuradorOv = _procuradorAd.Doc(id_doc);
			if (procuradorOv == null)
			{
				throw new DocNotFoundException("Registro não Encontrado.");
			}
			return procuradorOv;
		}

		public ProcuradorOV Doc(string ch_tipo_relacao)
		{
			return _procuradorAd.Doc(ch_tipo_relacao);
		}

		public string JsonReg(ulong id_doc)
		{
			return _procuradorAd.JsonReg(id_doc);
		}

		public string JsonReg(Pesquisa query)
		{
			return _procuradorAd.JsonReg(query);
		}

		public ulong Incluir(ProcuradorOV procuradorOv)
		{
			procuradorOv.ch_procurador = Guid.NewGuid().ToString("N");
			return _procuradorAd.Incluir(procuradorOv);
		}

		public bool Atualizar(ulong id_doc, ProcuradorOV procuradorOv)
		{
			Validar(procuradorOv);
			return _procuradorAd.Atualizar(id_doc, procuradorOv);
		}

		public bool Excluir(ulong id_doc)
		{
			var procuradorOv = Doc(id_doc);
			ValidarDepencias(procuradorOv);
			return _procuradorAd.Excluir(id_doc);
		}

		public void ValidarDepencias(ProcuradorOV procuradorOv)
		{
            var normas = new NormaRN().BuscarNormasDoProcurador(procuradorOv.ch_procurador);
            if (normas.Count > 0)
            {
                throw new DocDependenciesException("Erro de dependência. O Registro está sendo usado por uma ou mais normas.");
            }
		}

		private void Validar(ProcuradorOV procuradorOv)
		{
			if (string.IsNullOrEmpty(procuradorOv.nm_procurador))
			{
				throw new DocValidacaoException("Nome inválido.");
			}

		}
	}
}

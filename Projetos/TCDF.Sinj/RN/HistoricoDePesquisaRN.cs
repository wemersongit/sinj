using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TCDF.Sinj.AD;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using TCDF.Sinj.ESUtil;
using System.Web;

namespace TCDF.Sinj.RN
{
    public class HistoricoDePesquisaRN
    {
        private DocEs _docEs;
        private HistoricoDePesquisaAD _historicoDePesquisaAd;
        private string _nm_base;

        public HistoricoDePesquisaRN()
        {
            _historicoDePesquisaAd = new HistoricoDePesquisaAD();
            _docEs = new DocEs();
            _nm_base = util.BRLight.Util.GetVariavel("NmBaseLogsSearch", true);
        }

        public Results<HistoricoDePesquisaOV> Consultar(Pesquisa query)
        {
            return _historicoDePesquisaAd.Consultar(query);
        }

        public void Incluir(HistoricoDePesquisaOV historicoDePesquisaOv)
        {
            historicoDePesquisaOv.ch_consulta = historicoDePesquisaOv.ch_usuario + "_" + historicoDePesquisaOv.ds_historico;
            Pesquisa query = new Pesquisa();
            query.literal = "ch_consulta='" + historicoDePesquisaOv.ch_consulta + "'";
            query.select = new string[] { "id_doc" };
            var results = Consultar(query);
            if (results.results.Count > 0)
            {
                Atualizar(results.results[0]._metadata.id_doc, historicoDePesquisaOv);
            }
            else
            {
                _historicoDePesquisaAd.Incluir(historicoDePesquisaOv);
            }
        }

        public bool Atualizar(ulong id_doc, HistoricoDePesquisaOV historicoDePesquisaOv)
        {
            return _historicoDePesquisaAd.Atualizar(id_doc, historicoDePesquisaOv);
        }

        public Result<HistoricoDePesquisaOV> ConsultarEs(HttpContext context)
        {
            return _historicoDePesquisaAd.ConsultarEs(context);
        }

    }
}

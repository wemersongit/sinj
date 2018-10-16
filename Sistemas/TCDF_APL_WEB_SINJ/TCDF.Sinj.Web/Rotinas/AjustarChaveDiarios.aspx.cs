using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using TCDF.Sinj.Log;
using System.Text;

namespace TCDF.Sinj.Web.Rotinas
{
    public partial class AjustarChaveDiarios : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Server.ScriptTimeout = 14400;
            //StringBuilder id_doc_erro = new StringBuilder();
            //ulong offset = 0;
            //ulong total = 1;
            //var diarioRn = new DiarioRN();
            //while (offset < total)
            //{
            //    var result = diarioRn.Consultar(new Pesquisa { offset = offset.ToString(), limit = "50", select = new string[] { "id_doc", "ch_tipo_fonte", "dt_assinatura", "nr_diario", "cr_diario", "secao_diario" } });
            //    total = result.result_count;
            //    offset += 50;
            //    foreach(var diario in result.results){
            //        diarioRn.GerarChaveDoDiario(diario);
            //        if (diarioRn.PathPut(diario._metadata.id_doc, "ch_para_nao_duplicacao", diario.ch_para_nao_duplicacao, null) != "UPDATED")
            //        {
            //            id_doc_erro.Append("<br/>" + diario._metadata.id_doc);
            //        }
            //    }
            //}
            
            //div_resultado.InnerHtml = "Os seguinte registros não foram atualizados:" + id_doc_erro.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Web.Rotinas
{
    public partial class AtualizarNormas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Server.ScriptTimeout = 14400;
            //StringBuilder id_doc_erro = new StringBuilder();
            //var normaRn = new NormaRN();
            //var result = normaRn.Consultar(new Pesquisa { limit = null, literal = "dt_last_up::timestamp > '05/06/2015 00:00:00'", select= new string[]{"id_doc","ds_observacao"} });
            //foreach (var norma in result.results)
            //{
            //    if (normaRn.PathPut(norma._metadata.id_doc, "ds_observacao", norma.ds_observacao, null) != "UPDATED")
            //    {
            //        id_doc_erro.Append("<br/>" + norma._metadata.id_doc);
            //    }
            //}

            //div_resultado.InnerHtml = "Os seguinte registros não foram atualizados:" + id_doc_erro.ToString();
            
            //var normaRn = new NormaRN();
            //ulong offset = 0;
            //ulong total = 1;
            //Server.ScriptTimeout = 14400;
            //StringBuilder id_doc_erro = new StringBuilder();
            //try
            //{
            //    while (offset < total)
            //    {
            //        var result = normaRn.Consultar(new Pesquisa { offset = offset.ToString(), limit = "50", select = new string[] { "id_doc", "nr_norma" }});
            //        total = result.result_count;
            //        offset += 50;
            //        foreach (var norma in result.results)
            //        {
            //            if (normaRn.PathPut(norma._metadata.id_doc, "nr_norma", norma.nr_norma, null) != "UPDATED")
            //            {
            //                id_doc_erro.Append("<br/>" + norma._metadata.id_doc);
            //            }
            //        }
            //    }
            //    div_resultado.InnerHtml = "Os seguinte registros não foram atualizados:" + id_doc_erro.ToString();
            //}
            //catch (Exception ex) {
            
            //}
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using util.BRLight;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Web.Rotinas
{
    public partial class DeletarDtIdxNormas : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Server.ScriptTimeout = 14400;
            //StringBuilder id_doc_erro = new StringBuilder();
            //ulong offset = 0;
            //ulong total = 1;
            //var normaRn = new NormaRN();
            //while (offset < total)
            //{
            //    var result = normaRn.Consultar(new Pesquisa { offset = offset.ToString(), limit = "50", select = new string[] { "id_doc", "procuradores_responsaveis" } });
            //    total = result.result_count;
            //    offset += 50;
            //    foreach (var norma in result.results)
            //    {
            //        try
            //        {
            //            if (normaRn.PathPut(norma._metadata.id_doc, "procuradores_responsaveis", JSON.Serialize<List<Procurador>>(norma.procuradores_responsaveis), null) != "UPDATED")
            //            {
            //                id_doc_erro.Append("<br/>" + norma._metadata.id_doc);
            //            }
            //        }
            //        catch
            //        {
            //            id_doc_erro.Append("<br/>" + norma._metadata.id_doc);
            //        }
            //    }
            //}
            //div_resultado.InnerHtml = "Os seguinte registros não foram atualizados:" + id_doc_erro.ToString();
        }
    }
}
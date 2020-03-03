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
    public partial class AtualizarFontes : System.Web.UI.Page
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
            //    var result = normaRn.Consultar(new Pesquisa { offset = offset.ToString(), limit = "50", select = new string[] { "id_doc", "fontes" }, literal = "'93b915003f8b422c8857bbb6748db9b2' = any(ch_tipo_fonte)" });
            //    total = result.result_count;
            //    offset += 50;
            //    foreach (var norma in result.results)
            //    {
            //        foreach (var fonte in norma.fontes) {
            //            if (fonte.ch_tipo_fonte == "93b915003f8b422c8857bbb6748db9b2")
            //            {
            //                fonte.ch_tipo_fonte = "1";
            //                fonte.nm_tipo_fonte = "DODF";
            //            }
            //        }
            //        if (normaRn.PathPut(norma._metadata.id_doc, "fontes", JSON.Serialize<List<Fonte>>(norma.fontes), null) != "UPDATED")
            //        {
            //            id_doc_erro.Append("<br/>" + norma._metadata.id_doc);
            //        }
            //    }
            //}
            
            //div_resultado.InnerHtml = "Os seguinte registros não foram atualizados:" + id_doc_erro.ToString();
        }
    }
}

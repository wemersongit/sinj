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
    public partial class AjustarChaveCronologia : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Server.ScriptTimeout = 14400;
            //StringBuilder id_doc_erro = new StringBuilder();
            //ulong offset = 0;
            //ulong total = 1;
            //var orgaoRn = new DiarioRN();
            //while (offset < total)
            //{
            //    var result = orgaoRn.Consultar(new Pesquisa { offset = offset.ToString(), limit = "50", select = new string[] {  } });
            //    total = result.result_count;
            //    offset += 50;
            //    foreach (var orgao in result.results)
            //    {
            //        //if (orgaoRn.PathPut(orgao._metadata.id_doc, "ch_para_nao_duplicacao", orgao.ch_para_nao_duplicacao, null) != "UPDATED")
            //        //{
            //        //    id_doc_erro.Append("<br/>" + orgao._metadata.id_doc);
            //        //}
            //    }
            //}

            //div_resultado.InnerHtml = "Os seguinte registros não foram atualizados:" + id_doc_erro.ToString();
        }
    }
}
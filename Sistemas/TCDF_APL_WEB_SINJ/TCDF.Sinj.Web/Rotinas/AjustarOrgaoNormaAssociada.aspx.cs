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
    public partial class AjustarOrgaoNormaAssociada : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Server.ScriptTimeout = 14400;
            //StringBuilder id_doc_erro = new StringBuilder();
            //var orgaoRn = new OrgaoRN();
            //var result = orgaoRn.Consultar(new Pesquisa { limit = null });
            //foreach (var orgao in result.results)
            //{
            //    if (orgao.norma != null && !string.IsNullOrEmpty(orgao.norma.ch_norma))
            //    {
            //        orgao.ch_norma_inicio_vigencia = orgao.norma.ch_norma;
            //        orgao.ds_norma_inicio_vigencia = orgao.norma.ds_norma;
            //        orgao.norma = new NormaAssociada();
            //        try
            //        {
            //            if (orgaoRn.Atualizar(orgao._metadata.id_doc, orgao))
            //            {
            //                id_doc_erro.Append("<br/>" + orgao._metadata.id_doc);
            //            }
            //        }
            //        catch
            //        {
            //            id_doc_erro.Append("<br/>" + orgao._metadata.id_doc);
            //        }
            //    }
            //}

            //div_resultado.InnerHtml = "Os seguinte registros não foram atualizados:" + id_doc_erro.ToString();
        }
    }
}

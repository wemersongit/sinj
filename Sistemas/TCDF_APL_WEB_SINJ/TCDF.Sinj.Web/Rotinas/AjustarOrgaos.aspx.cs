using System;
using System.Text;
using neo.BRLightREST;
using TCDF.Sinj.RN;

namespace TCDF.Sinj.Web.Rotinas
{
    public partial class AjustarOrgaos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    var orgaoRn = new OrgaoRN();
            //    var pesquisa = new Pesquisa();
            //    pesquisa.limit = null;
            //    var lista_orgaos = orgaoRn.Consultar(pesquisa);
            //    StringBuilder orgaos_sucesso = new StringBuilder();
            //    StringBuilder orgaos_erro = new StringBuilder();
            //    ulong count_sucesso = 0;

            //    foreach (var orgao in lista_orgaos.results)
            //    {
            //        if (orgaoRn.Atualizar(orgao._metadata.id_doc, orgao, false))
            //        {
            //            orgaos_sucesso.Append("<br/>" + orgao._metadata.id_doc);
            //            count_sucesso++;
            //        }
            //        else
            //        {
            //            orgaos_erro.Append("<br/>" + orgao._metadata.id_doc);
            //        }
            //    }
            //    div_resultado.InnerHtml = "Os seguinte registros foram atualizados:" + orgaos_sucesso + "<br/><br/>Os seguintes registros NÃO foram atualizados:" + orgaos_erro;
            //}
            //catch (Exception ex)
            //{
            //    div_resultado.InnerHtml = ex.ToString();
            //}
        }
    }
}

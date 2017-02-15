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
    public partial class AtualizarTiposDeNorma : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Server.ScriptTimeout = 14400;
            //StringBuilder id_doc_erro = new StringBuilder();
            //var tipoDeNormaRn = new TipoDeNormaRN();
            //var result = tipoDeNormaRn.Consultar(new Pesquisa { limit = null });
            //foreach (var tipoDeNorma in result.results)
            //{
            //    tipoDeNorma.alteracoes.Add(new AlteracaoOV { dt_alteracao = DateTime.Now.ToString("dd'/'MM'/'yyyy HH:mm:ss"), nm_login_usuario_alteracao = "usrseplag"});
            //    if (!tipoDeNormaRn.Atualizar(tipoDeNorma._metadata.id_doc, tipoDeNorma))
            //    {
            //        id_doc_erro.Append("<br/>" + tipoDeNorma._metadata.id_doc);
            //    }
            //}

            //div_resultado.InnerHtml = "Os seguinte registros não foram atualizados:" + id_doc_erro.ToString();
        }
    }
}
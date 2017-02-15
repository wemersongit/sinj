using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TCDF.Sinj.Portal.Web
{
    public partial class ResultadoDePesquisa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var tipo_norma = Request["nm_tipo_norma"];
            var keywords = "sinj, decreto, lei, resolução, portaria, distrito federal";
            if(!string.IsNullOrEmpty(tipo_norma) && keywords.IndexOf(tipo_norma.ToLower()) == -1){
                keywords += ", " + tipo_norma;
            }
            this.Header.Keywords = keywords;

        }
    }
}
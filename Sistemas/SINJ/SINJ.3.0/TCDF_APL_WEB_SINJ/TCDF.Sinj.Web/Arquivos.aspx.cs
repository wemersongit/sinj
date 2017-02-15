using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TCDF.Sinj.Web
{
    public partial class Arquivos : System.Web.UI.Page
    {
        protected string _diretorio { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            _diretorio = Request["diretorio"];
            if(string.IsNullOrEmpty(_diretorio)){
                _diretorio = "arquivos_orgao_cadastrador";
            }
        }
    }
}
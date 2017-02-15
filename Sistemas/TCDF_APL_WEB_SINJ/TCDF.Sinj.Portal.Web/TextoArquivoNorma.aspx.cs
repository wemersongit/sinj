using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.RN;
using util.BRLight;
using neo.BRLightREST;
using TCDF.Sinj.OV;
using System.Text.RegularExpressions;
using System.Net;
using System.Web.UI.HtmlControls;

namespace TCDF.Sinj.Portal.Web
{
	public partial class TextoArquivoNorma : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			var _id_file = Request["id_file"];
			try
			{
				if (!string.IsNullOrEmpty(_id_file))
                {
                    var normaRn = new NormaRN();
                    var json_doc = normaRn.GetDoc(_id_file);

					if (json_doc.IndexOf("\"status\": 500") > -1)
					{
						throw new Exception("Erro ao obter texto do arquivo.");
                    }
                    if (json_doc.IndexOf("\"status\": 404") > -1)
                    {
                        throw new Exception("Arquivo não encontrado.");
                    }

					if (json_doc.IndexOf("\"filetext\": null") > -1)
					{
						throw new Exception("O texto do arquivo não foi extraído.");
					}
					var doc_full = JSON.Deserializa<ArquivoFullOV>(json_doc);
                    if (doc_full.mimetype.IndexOf("/htm") > -1)
                    {
                        var texto = Regex.Replace(doc_full.filetext, "\\<[^\\>]*\\>", string.Empty);
                        div_texto.InnerText = WebUtility.HtmlDecode(texto);
                    }
                    else
                    {
                        div_texto.InnerText = doc_full.filetext;
                    }
                    if (doc_full.id_doc != null && doc_full.id_doc != 0)
                    {
                        var norma = normaRn.Doc(doc_full.id_doc);
                        var ds_norma = norma.nm_tipo_norma + (!string.IsNullOrEmpty(norma.nr_norma) ? " Nº " + norma.nr_norma : "") + " de " + norma.dt_assinatura;
                        Page.Title = ds_norma;
                        HtmlMeta html_meta_keywords = new HtmlMeta();
                        html_meta_keywords.Name = "keywords";
                        html_meta_keywords.Content = "sinj, distrito, federal, df," + norma.nm_tipo_norma;
                        HtmlMeta html_meta_description = new HtmlMeta();
                        html_meta_description.Name = "description";
                        html_meta_description.Content = "Arquivo de " + ds_norma + " disponibilizado pelo SINJ.";
                        placeHolderHeader.Controls.Add(html_meta_keywords);
                        placeHolderHeader.Controls.Add(html_meta_description);
                    }
				}
			}
			catch (Exception Ex)
			{
                div_texto.InnerHtml = util.BRLight.Excecao.LerInnerException(Ex, true) + "<br/><br/>Nossa equipe resolverá o problema, você pode tentar mais tarde ou entrar em contato conosco.";
				a_print.Visible = false;
			}
		}
	}
}
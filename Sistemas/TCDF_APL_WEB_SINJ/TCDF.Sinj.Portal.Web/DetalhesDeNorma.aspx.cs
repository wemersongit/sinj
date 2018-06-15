using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using util.BRLight;
using System.Web.UI.HtmlControls;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Portal.Web
{
    public partial class DetalhesDeNorma : System.Web.UI.Page
    {
        public String json_norma;

        protected void Page_Load(object sender, EventArgs e)
        {
            var _id_norma = Request["id_norma"];
            var _id_doc = Request["id_doc"];
            NormaOV normaOv = null;
            NormaDetalhada normaDetalhada = null;

            try
            {
                if (!string.IsNullOrEmpty(_id_norma))
                {
                    normaOv = new NormaRN().Doc(_id_norma);
                }
                else if (!string.IsNullOrEmpty(_id_doc))
                {
                    normaOv = new NormaRN().Doc(ulong.Parse(_id_doc));
                }
                if (normaOv != null)
                {
                    // Meta tags
                    var ds_norma = normaOv.getDescricaoDaNorma();
                    Page.Title = ds_norma;
                    this.Header.Keywords = "sinj, distrito, federal, df," + normaOv.nm_tipo_norma;
                    this.Header.Description = normaOv.nm_tipo_norma + (!string.IsNullOrEmpty(normaOv.nr_norma) ? " Nº " + normaOv.nr_norma : "") + " publicada em " + normaOv.dt_assinatura + " por " + normaOv.origens[0].nm_orgao + ". " + normaOv.ds_ementa;

                    // Tags Open Graph para Facebook e Linkedin
                    HtmlMeta html_meta_fb_title = new HtmlMeta();
                    html_meta_fb_title.Attributes.Add("property", "og:title");
                    html_meta_fb_title.Content = ds_norma;

                    HtmlMeta html_meta_fb_description = new HtmlMeta();
                    html_meta_fb_description.Attributes.Add("property", "og:description");
                    html_meta_fb_description.Content = normaOv.ds_ementa;

                    HtmlMeta html_meta_fb_type = new HtmlMeta();
                    html_meta_fb_type.Attributes.Add("property", "og:type");
                    html_meta_fb_type.Content = "article";

                    HtmlMeta html_meta_fb_image = new HtmlMeta();
                    html_meta_fb_image.Attributes.Add("property", "og:image");
                    html_meta_fb_image.Content = Util.GetUri() + Util._urlPadrao + "/Imagens/favicon.png";

                    placeHolderHeader.Controls.Add(html_meta_fb_title);
                    placeHolderHeader.Controls.Add(html_meta_fb_description);
                    placeHolderHeader.Controls.Add(html_meta_fb_type);
                    placeHolderHeader.Controls.Add(html_meta_fb_image);

                    // Norma Detalhada
                    var sNorma = JSON.Serialize<NormaOV>(normaOv);
                    normaDetalhada = JSON.Deserializa<NormaDetalhada>(sNorma);
                    normaDetalhada.origensOv = new List<OrgaoOV>();
                    foreach (var origem in normaDetalhada.origens)
                    {
                        normaDetalhada.origensOv.Add(new OrgaoRN().Doc(origem.ch_orgao));
                    }
                    var tipoDeNormaOv = new TipoDeNormaRN().Doc(normaDetalhada.ch_tipo_norma);
                    var sTipoDeNormaOv = JSON.Serialize<TipoDeNormaOV>(tipoDeNormaOv);
                    normaDetalhada.tipoDeNorma = JSON.Deserializa<TipoDeNorma>(sTipoDeNormaOv);
                    normaDetalhada.ds_ementa = Regex.Replace(normaDetalhada.ds_ementa, "\\<[^\\>]*\\>", string.Empty);
                    normaDetalhada.ds_observacao = Regex.Replace(normaDetalhada.ds_observacao, "\\<[^\\>]*\\>", string.Empty);
                    json_norma = JSON.Serialize<NormaDetalhada>(normaDetalhada);

                }

            }
            catch (Exception Ex)
            {
                
            }

        }
    }
}
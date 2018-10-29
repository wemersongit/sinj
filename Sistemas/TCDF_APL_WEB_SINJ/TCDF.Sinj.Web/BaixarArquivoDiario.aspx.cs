using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using util.BRLight;
using System.Web.UI.HtmlControls;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Web
{
    public partial class BaixarArquivoDiario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            var _id_file = Request["id_file"];

            try
            {
                var oKeywords = Request.RequestContext.RouteData.Values["keywords"];
                if (oKeywords != null)
                {
                    var aKeywords = oKeywords.ToString().Split('/');
                    if (aKeywords.Length == 2)
                    {
                        _id_file = aKeywords[0];
                    }
                }
                if (!string.IsNullOrEmpty(_id_file))
                {
                    Util.rejeitarInject(_id_file);
                    var json_doc = new DiarioRN().GetDoc(_id_file);
                    if (json_doc.IndexOf("\"status\": 500") > -1)
                    {
                        throw new Exception("Erro ao obter arquivo.");
                    }
                    else if (json_doc.IndexOf("\"status\": 404") > -1)
                    {
                        throw new Exception("O arquivo não foi encontrado.");
                    }
                    else if (!string.IsNullOrEmpty(json_doc))
                    {
                        var diarioRn = new DiarioRN();
                        var doc = JSON.Deserializa<ArquivoFullOV>(json_doc);
                        var documento = diarioRn.Download(_id_file);
                        if (doc.id_doc != null && doc.id_doc != 0)
                        {
                            var diario = diarioRn.Doc(doc.id_doc);
                            var ds_diario = diario.nm_tipo_fonte + " Nº " + diario.nr_diario + " de " + diario.dt_assinatura;
                            Page.Title = ds_diario;
                            HtmlMeta html_meta_keywords = new HtmlMeta();
                            html_meta_keywords.Name = "keywords";
                            html_meta_keywords.Content = "sinj, distrito, federal, df," + diario.nm_tipo_fonte;
                            HtmlMeta html_meta_description = new HtmlMeta();
                            html_meta_description.Name = "description";
                            html_meta_description.Content = "Arquivo de " + ds_diario + " disponibilizado pelo SINJ.";
                            placeHolderHeader.Controls.Add(html_meta_keywords);
                            placeHolderHeader.Controls.Add(html_meta_description);
                        }
                        if (doc.mimetype.IndexOf("html")>-1)
                        {
                            div_texto.InnerHtml = Util.FileBytesInUTF8String(documento);
                        }
                        else
                        {
                            if (documento != null && documento.Length > 0)
                            {
                                Response.Clear();
                                Response.SetCookie(new HttpCookie("fileDownload", "true") { Path = "/" });
                                Response.ContentType = doc.mimetype;
                                Response.AppendHeader("Content-Length", documento.Length.ToString());
                                Response.AppendHeader("Content-Disposition", "inline; filename=\"" + doc.filename + "\"");
                                Response.BinaryWrite(documento);
                                Response.Flush();
                            }
                            else
                            {
                                throw new Exception("Arquivo não encontrado.");
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Arquivo não encontrado.");
                    }
                }
                else
                {
                    throw new Exception("Arquivo não encontrado.");
                }

            }
            catch (ParamDangerousException)
            {
                Response.Clear();
                Response.Write("<html><head></head><body><div id=\"div_erro\" style=\"color:#990000; width:500px; margin:auto; text-align:center;\">Erro ao obter arquivo.<br/><br/>Nossa equipe resolverá o problema, você pode tentar mais tarde ou entrar em contato conosco.</div></body></html>");
            }
            catch (Exception Ex)
            {
                Response.Clear();
                Response.Write("<html><head></head><body><div id=\"div_erro\" style=\"color:#990000; width:500px; margin:auto; text-align:center;\">" + util.BRLight.Excecao.LerInnerException(Ex, true) + "<br/><br/>Nossa equipe resolverá o problema, você pode tentar mais tarde ou entrar em contato conosco.</div></body></html>");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using util.BRLight;
using System.Web.UI.HtmlControls;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Web
{
    public partial class BaixarArquivoNorma : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var _id_file = Request["id_file"];
            var _id_norma = Request["id_norma"];
            NormaOV normaOv = null;
            var nm_file = "";

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
                if (!string.IsNullOrEmpty(_id_norma))
                {
                    Util.rejeitarInject(_id_norma);
                    normaOv = new NormaRN().Doc(_id_norma);
                }
                else if (!string.IsNullOrEmpty(_id_file))
                {
                    Util.rejeitarInject(_id_file);
                    var json_doc = new NormaRN().GetDoc(_id_file);
                    if (!string.IsNullOrEmpty(json_doc))
                    {
                        if (json_doc.IndexOf("\"status\": 500") > -1)
                        {
                            throw new Exception("Erro ao obter arquivo.");
                        }
                        else if (json_doc.IndexOf("\"status\": 404") > -1)
                        {
                            throw new Exception("O arquivo não foi encontrado.");
                        }
                        var doc = JSON.Deserializa<ArquivoFullOV>(json_doc);
                        if (doc.id_doc != null && doc.id_doc != 0)
                        {
                            normaOv = new NormaRN().Doc(doc.id_doc);
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
                if (normaOv != null)
                {
                    if (!string.IsNullOrEmpty(normaOv.ar_atualizado.id_file))
                    {
                        nm_file = normaOv.ar_atualizado.filename;
                    }
                    else
                    {
                        foreach (var fonte in normaOv.fontes)
                        {
                            if (!string.IsNullOrEmpty(fonte.ar_fonte.filename))
                            {
                                nm_file = fonte.ar_fonte.filename;
                                break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(nm_file))
                    {
                        //Redireciona para nova página de downloads da norma
                        Response.Redirect("./Norma/" + _id_norma + "/" + nm_file, true);
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
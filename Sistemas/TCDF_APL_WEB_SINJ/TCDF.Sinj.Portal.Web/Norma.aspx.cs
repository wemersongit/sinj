using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.OV;
using util.BRLight;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using TCDF.Sinj.Log;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;

namespace TCDF.Sinj.Portal.Web
{
    public partial class Norma : System.Web.UI.Page
    {
        protected string title = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
                var aKeywords = new string[0];
                var oKeywords = Request.RequestContext.RouteData.Values["keywords"];
                if (oKeywords != null)
                {
                    aKeywords = oKeywords.ToString().Split('/');
                }
                if (aKeywords.Length == 2)
                {
                    var _ch_norma = aKeywords[0];
                    var _title = aKeywords[1];
                    var normaOv = new NormaRN().Doc(_ch_norma);

                    var docRn = new Doc("sinj_norma");
                    var docOv = new File();

                    var id_file = "";
                    if (!string.IsNullOrEmpty(normaOv.ar_atualizado.id_file))
                    {
                        id_file = normaOv.ar_atualizado.id_file;
                    }
                    else
                    {
                        if (normaOv.fontes.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(normaOv.fontes[0].ar_fonte.id_file))
                            {
                                id_file = normaOv.fontes[0].ar_fonte.id_file;
                            }
                            foreach (var fonte in normaOv.fontes)
                            {
                                if (!string.IsNullOrEmpty(fonte.ar_fonte.id_file) && (fonte.nm_tipo_publicacao.Equals("republicação", StringComparison.InvariantCultureIgnoreCase) || fonte.nm_tipo_publicacao.Equals("rep", StringComparison.InvariantCultureIgnoreCase)))
                                {
                                    id_file = fonte.ar_fonte.id_file;
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(id_file))
                    {
                        docOv = docRn.doc(id_file);
                    }

                    if (!string.IsNullOrEmpty(docOv.id_file))
                    {
                        var file = docRn.download(docOv.id_file);
                        if (file != null && file.Length > 0)
                        {
                            title = normaOv.getDescricaoDaNorma();
                            if (docOv.mimetype.IndexOf("html") > -1)
                            {
                                Page.Title = title;
                                HtmlMeta html_meta_keywords = new HtmlMeta();
                                html_meta_keywords.Name = "keywords";
                                html_meta_keywords.Content = Page.Title + ",sinj,distrito,federal,df";
                                HtmlMeta html_meta_description = new HtmlMeta();
                                html_meta_description.Name = "description";
                                html_meta_description.Content = !string.IsNullOrEmpty(normaOv.ds_ementa) ? normaOv.ds_ementa : (Page.Title + " - texto vigente disponibilizado pelo SINJ-DF (Sistema Integrado de Normas Jurídicas do Distrito Federal).");

                                placeHolderHeader.Controls.Add(html_meta_keywords);
                                placeHolderHeader.Controls.Add(html_meta_description);

                                var msg = Encoding.UTF8.GetString(file);
                                if (msg.IndexOf("<h1 epigrafe") > -1 || msg.IndexOf("<p linkname") > -1)
                                {
                                    msg = msg.Replace("(_link_sistema_)", ResolveUrl("~"));
                                    msg = Regex.Replace(msg, "<html>.*<body>|</body></html>", String.Empty);
                                }
                                else
                                {
                                    Encoding wind1252 = Encoding.GetEncoding(1252);
                                    Encoding utf8 = Encoding.UTF8;
                                    byte[] wind1252Bytes = file;
                                    byte[] utfBytes = Encoding.Convert(wind1252, utf8, wind1252Bytes);
                                    msg = utf8.GetString(utfBytes);
                                }


                                div_texto.InnerHtml = msg;
                            }
                            else
                            {
                                var log_arquivo = new LogDownload
                                {
                                    arquivo = new ArquivoOV { filename = docOv.filename, filesize = docOv.filesize, id_file = docOv.id_file, mimetype = docOv.mimetype }
                                };
                                LogOperacao.gravar_operacao("NOR.DWN", log_arquivo, "", "");
                                Response.Clear();
                                Response.ContentType = docOv.mimetype;
                                Response.AppendHeader("Content-Length", file.Length.ToString());
                                Response.AppendHeader("Content-Disposition", "inline; filename=\"" + docOv.filename + "\"");
                                Response.BinaryWrite(file);
                                Response.Flush();
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
            }
            catch (Exception ex)
            {
                var mensagem = Excecao.LerTodasMensagensDaExcecao(ex, false);

                var erro = new ErroRequest
                {
                    Pagina = Request.Path,
                    RequestQueryString = Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                var nm_usuario = "visitante";
                var nm_login_usuario = "visitante";

                SessaoNotifiquemeOV sessao_push = Util.LerSessaoPush();
                if (sessao_push != null)
                {
                    nm_usuario = sessao_push.nm_usuario_push;
                    nm_login_usuario = sessao_push.email_usuario_push;
                }
                LogErro.gravar_erro("NOR.DWN", erro, nm_usuario, nm_login_usuario);

                Response.Clear();
                Response.Write("<html><head></head><body><div id=\"div_erro\" style=\"color:#990000; width:500px; margin:auto; text-align:center;\">" + mensagem + "<br/><br/>Tente mais tarde ou entre em contato com o administrador do sistema.</div></body><html>");
            }
        }
    }
}

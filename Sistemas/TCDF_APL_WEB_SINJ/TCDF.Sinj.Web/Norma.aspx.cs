using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using TCDF.Sinj.Log;
using util.BRLight;
using TCDF.Sinj.OV;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Text.RegularExpressions;

namespace TCDF.Sinj.Web
{
    public partial class Norma : System.Web.UI.Page
    {
        protected string title = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (Config.ValorChave("Aplicacao") == "CADASTRO")
                {
                    sessao_usuario = Util.ValidarAcesso(base.Page);
                    //Não faz nada se o usuário não tiver sessão no portal pois essa pesquisa pode ser feita por qualquer um.
                }
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
                    var normaOv = new NormaRN().Doc(aKeywords[0]);


                    var docRn = new Doc("sinj_norma");
                    var docOv = new File();

                    var id_file = normaOv.getIdFileArquivoVigente();

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
                            if(docOv.mimetype.IndexOf("html")>-1){
                                HtmlMeta html_meta_keywords = new HtmlMeta();
                                html_meta_keywords.Name = "keywords";
                                html_meta_keywords.Content = "sinj, distrito, federal, df," + Page.Title;
                                HtmlMeta html_meta_description = new HtmlMeta();
                                html_meta_description.Name = "description";
                                html_meta_description.Content = !string.IsNullOrEmpty(normaOv.ds_ementa) ? normaOv.ds_ementa : "Arquivo de " + Page.Title + " disponibilizado pelo SINJ-DF (Sistema Integrado de Normas Jurídicas do Distrito Federal).";
                                placeHolderHeader.Controls.Add(html_meta_keywords);
                                placeHolderHeader.Controls.Add(html_meta_description);
                                var msg = Util.FileBytesInUTF8String(file);
                                if (Regex.IsMatch(msg, "<h1.*epigrafe.*>"))
                                {
                                    msg = msg.Replace("(_link_sistema_)", ResolveUrl("~"));
                                    msg = Regex.Replace(msg, "<html>.*<body>|</body></html>", String.Empty);
                                }
                                div_texto.InnerHtml = msg;
                            }
                            else{
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
                if (sessao_usuario != null)
                {
                    LogErro.gravar_erro("NOR.DWN", erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }

                Response.Clear();
                Response.Write("<html><head></head><body><div id=\"div_erro\" style=\"color:#990000; width:500px; margin:auto; text-align:center;\">" + mensagem + "<br/><br/>Tente mais tarde ou entre em contato com o administrador do sistema.</div></body><html>");
            }
        }
    }
}

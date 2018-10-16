using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using neo.BRLightREST;
using TCDF.Sinj.RN;
using System.Web.UI.HtmlControls;
using System.Text;
using TCDF.Sinj.Log;
using util.BRLight;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Portal.Web
{
    public partial class Versao : System.Web.UI.Page
    {
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
                if (aKeywords.Length == 4 || aKeywords.Length == 5)
                {
                    var _ch_norma = aKeywords[0];
                    var _path = "";
                    var docRn = new Doc("sinj_norma");
                    //var docOv = docRn.doc(_id_file);
                    var docOv = new File();

                    if (!string.IsNullOrEmpty(_ch_norma))
                    {
                        var _id_file = "";
                        _path = aKeywords[2];
                        if (_path == "fontes" || _path == "atlz" || _path == "acao")
                        {
                            var normaOv = new NormaRN().Doc(_ch_norma);
                            if (_path == "fontes")
                            {
                                _id_file = normaOv.fontes[int.Parse(aKeywords[3])].ar_fonte.id_file;
                            }
                            else if (_path == "atlz")
                            {
                                _id_file = normaOv.ar_atualizado.id_file;
                            }
                            else if (_path == "acao")
                            {
                                _id_file = normaOv.ar_acao.id_file;
                            }
                        }

                        if (!string.IsNullOrEmpty(_id_file))
                        {
                            docOv = docRn.doc(_id_file);
                        }
                    }

                    if (!string.IsNullOrEmpty(docOv.id_file))
                    {
                        var file = docRn.download(docOv.id_file);
                        if (file != null && file.Length > 0)
                        {
                            if (docOv.mimetype.IndexOf("html") > -1)
                            {
                                Page.Title = docOv.filename.Substring(0, docOv.filename.IndexOf(".") - 1);
                                var msg = Encoding.UTF8.GetString(file);
                                msg = msg.Replace("(_link_sistema_)", ResolveUrl("~"));
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

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
using System.Web.UI.HtmlControls;
using System.Text;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web
{
    public partial class Versao : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (Config.ValorChave("Aplicacao") == "CADASTRO")
                {
                    sessao_usuario = Util.ValidarSessao();
                    //Não faz nada se o usuário não tiver sessão no portal pois essa pesquisa pode ser feita por qualquer um.
                }
                var aKeywords = new string[0];
                var oKeywords = Request.RequestContext.RouteData.Values["keywords"];
                if (oKeywords != null)
                {
                    aKeywords = oKeywords.ToString().Split('/');
                }
                if (aKeywords.Length == 4 || aKeywords.Length == 5)
                {
                    var _ch_norma = aKeywords[0];

                    var docRn = new Doc("sinj_norma");
                    var docOv = new File();

                    if (!string.IsNullOrEmpty(_ch_norma))
                    {
                        var _id_file = "";
                        var _path = aKeywords[2];
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
                                var msg = Util.FileBytesInUTF8String(file);
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
                                byte[] utfBytes = Util.FileBytesInUTF8(file);
                                Response.AppendHeader("Content-Length", utfBytes.Length.ToString());
                                Response.AppendHeader("Content-Disposition", "inline; filename=\"" + docOv.filename + "\"");
                                Response.BinaryWrite(utfBytes);
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

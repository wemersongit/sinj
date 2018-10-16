using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using neo.BRLightREST;
using TCDF.Sinj.Log;
using util.BRLight;
using TCDF.Sinj.RN;
using System.Text;

namespace TCDF.Sinj.Web
{
    public partial class Download : System.Web.UI.Page
    {
        protected string title = "";
        protected void Page_Load(object sender, EventArgs e)
        {

            var sessao_usuario = Util.ValidarAcesso(base.Page);

            var aKeywords = new string[0];
            var oKeywords = Request.RequestContext.RouteData.Values["keywords"];
            if (oKeywords != null)
            {
                aKeywords = oKeywords.ToString().Split('/');
            }
            if (aKeywords.Length == 3)
            {
                var _nm_base = aKeywords[0];
                var _id_file = aKeywords[1];
                try
                {
                    if (!string.IsNullOrEmpty(_id_file))
                    {
                        var docRn = new Doc(_nm_base);
                        var docOv = docRn.doc(_id_file);
                        if (docOv.id_file != null)
                        {
                            var file = docRn.download(_id_file);
                            if (file != null && file.Length > 0)
                            {
                                var log_arquivo = new LogDownload
                                {
                                    arquivo = new ArquivoOV { filename = docOv.filename, filesize = docOv.filesize, id_file = docOv.id_file, mimetype = docOv.mimetype }
                                };
                                LogOperacao.gravar_operacao("DWN", log_arquivo, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);

                                if (_nm_base == "sinj_norma" || _nm_base == "sinj_arquivo_versionado_norma")
                                {

                                    var msg = Encoding.UTF8.GetString(file);
                                    if (msg.IndexOf("<h1 epigrafe") > -1)
                                    {
                                        msg = msg.Replace("(_link_sistema_)", ResolveUrl("~"));
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
                                else if (_nm_base == "sinj_arquivo" && docOv.mimetype.IndexOf("html") > -1){
                                    var msg = Encoding.UTF8.GetString(file);
                                    div_texto.InnerHtml = msg;
                                }
                                else{
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
                        LogErro.gravar_erro("DWN", erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                    }

                    Response.Clear();
                    Response.Write("<html><head></head><body><div id=\"div_erro\" style=\"color:#990000; width:500px; margin:auto; text-align:center;\">" + mensagem + "<br/><br/>Tente mais tarde ou entre em contato com o administrador do sistema.</div></body><html>");
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using neo.BRLightREST;
using util.BRLight;
using TCDF.Sinj.Log;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Portal.Web
{
    public partial class Download : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

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
                        //if (_nm_base == "norma")
                        //{
                        //    _nm_base = "sinj_norma";
                        //    //nesse contexto o id_file é na verdade ch_norma
                        //    var normaOv = new NormaRN().Doc(_id_file);

                        //    if (!string.IsNullOrEmpty(normaOv.ar_atualizado.id_file))
                        //    {
                        //        _id_file = normaOv.ar_atualizado.id_file;
                        //    }
                        //    else
                        //    {
                        //        foreach (var fonte in normaOv.fontes)
                        //        {
                        //            if (!string.IsNullOrEmpty(fonte.ar_fonte.id_file))
                        //            {
                        //                _id_file = fonte.ar_fonte.id_file;
                        //            }
                        //        }
                        //    }
                        //}
                        var docRn = new Doc(_nm_base);
                        var docOv = docRn.doc(_id_file);
                        if (docOv.id_file != null)
                        {
                            var file = docRn.download(_id_file);
                            if (file != null && file.Length > 0)
                            {
                                //var log_arquivo = new LogDownload
                                //{
                                //    arquivo = new ArquivoOV { filename = docOv.filename, filesize = docOv.filesize, id_file = docOv.id_file, mimetype = docOv.mimetype }
                                //};
                                //LogOperacao.gravar_operacao("DWN", log_arquivo, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                                Response.Clear();
                                Response.ContentType = docOv.mimetype;
                                Response.AppendHeader("Content-Length", file.Length.ToString());
                                Response.AppendHeader("Content-Disposition", "inline; filename=\"" + docOv.filename + "\"");
                                Response.BinaryWrite(file);
                                Response.Flush();
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
                    LogErro.gravar_erro("DWN", erro, nm_usuario, nm_login_usuario);

                    Response.Clear();
                    Response.Write("<html><head></head><body><div id=\"div_erro\" style=\"color:#990000; width:500px; margin:auto; text-align:center;\">" + mensagem + "<br/><br/>Tente mais tarde ou entre em contato com o administrador do sistema.</div></body><html>");
                }
            }
        }
    }
}
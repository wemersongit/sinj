using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using TCDF.Sinj.Log;
using System.Text;

namespace TCDF.Sinj.Web
{
    public partial class Diario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (Config.ValorChave("Aplicacao") == "CADASTRO")
                {
                    Util.ValidarAcesso(base.Page);
                    //Não faz nada se o usuário não tiver sessão no portal pois essa pesquisa pode ser feita por qualquer um.
                }
                var aKeywords = new string[0];
                var oKeywords = Request.RequestContext.RouteData.Values["keywords"];
                if (oKeywords != null)
                {
                    aKeywords = oKeywords.ToString().Split('/');
                }

                File docOv = null;
                var docRn = new Doc("sinj_diario");

                if (aKeywords.Length == 5 || aKeywords.Length == 4)
                {
                    var _ch_diario = aKeywords[0];
                    var _id_file = aKeywords[1];
                    var _path = aKeywords[2];
                    
                    docOv = docRn.doc(_id_file);

                    if (string.IsNullOrEmpty(docOv.id_file))
                    {
                        _id_file = "";
                        if (!string.IsNullOrEmpty(_ch_diario))
                        {
                            if (_path == "arq")
                            {
                                var diarioOv = new DiarioRN().Doc(_ch_diario);
                                if (aKeywords.Length == 5)
                                {
                                    _id_file = diarioOv.arquivos[int.Parse(aKeywords[3])].arquivo_diario.id_file;
                                }
                                else
                                {
                                    _id_file = diarioOv.ar_diario.id_file;
                                }
                                
                            }

                            if (!string.IsNullOrEmpty(_id_file) && _id_file != aKeywords[1])
                            {
                                docOv = docRn.doc(_id_file);
                            }
                        }
                    }

                    

                }
                else if (aKeywords.Length == 2)
                {
                    var _id_file = aKeywords[0];
                    docOv = docRn.doc(_id_file);
                }

                if (docOv != null && !string.IsNullOrEmpty(docOv.id_file))
                {
                    var file = docRn.download(docOv.id_file);
                    if (file != null && file.Length > 0)
                    {
                        var log_arquivo = new LogDownload
                        {
                            arquivo = new ArquivoOV { filename = docOv.filename, filesize = docOv.filesize, id_file = docOv.id_file, mimetype = docOv.mimetype }
                        };
                        LogOperacao.gravar_operacao("DIO.DWN", log_arquivo, "", "");
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
                    LogErro.gravar_erro("DIO.DWN", erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }

                Response.Clear();
                Response.Write("<html><head></head><body><div id=\"div_erro\" style=\"color:#990000; width:500px; margin:auto; text-align:center;\">" + mensagem + "<br/><br/>Tente mais tarde ou entre em contato com o administrador do sistema.</div></body><html>");
            }
        }
    }
}

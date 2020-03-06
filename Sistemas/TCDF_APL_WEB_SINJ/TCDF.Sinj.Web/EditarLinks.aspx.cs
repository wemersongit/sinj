using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.OV;
using neo.BRLightREST;
using System.Text;
using TCDF.Sinj.Log;
using util.BRLight;

namespace TCDF.Sinj.Web
{
    public partial class EditarLinks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var _id_file = Request["id_file"];
            var _nm_base = Request["nm_base"];
            var _id_doc = Request["id_doc"];
            var sRetorno = "";

            SessaoUsuarioOV sessao_usuario = null;
            var action = AcoesDoUsuario.arq_pro;

            try
            {
                if (!string.IsNullOrEmpty(_id_file))
                {
                    sessao_usuario = Util.ValidarSessao();
                    var docRn = new Doc(_nm_base);
                    Util.rejeitarInject(_id_file);
                    var docOv = docRn.doc(_id_file);

                    if (docOv.id_file != null && docOv.mimetype == "text/html")
                    {
                        var file = docRn.download(_id_file);
                        if (file != null && file.Length > 0)
                        {
                            var log_arquivo = new LogDownload()
                            {
                                arquivo = new ArquivoOV { filename = docOv.filename, filesize = docOv.filesize, id_file = docOv.id_file, mimetype = docOv.mimetype }
                            };
                            LogOperacao.gravar_operacao(Util.GetEnumDescription(action) + ".HTML.VIS", log_arquivo, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                            div_conteudo_arquivo.InnerHtml = Util.FileBytesInUTF8String(file);
                            id_file.Value = _id_file;
                            Util.rejeitarInject(_id_doc);
                            id_doc.Value = _id_doc;
                            nm_arquivo.Value = docOv.filename.Replace(".html","");
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
                sRetorno = Excecao.LerTodasMensagensDaExcecao(ex, false);
                var erro = new ErroRequest
                {
                    Pagina = Request.Path,
                    RequestQueryString = Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                if (sessao_usuario != null)
                {
                    LogErro.gravar_erro(Util.GetEnumDescription(action) + ".HTML.VIS", erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }
                throw;
            }
        }
    }
}

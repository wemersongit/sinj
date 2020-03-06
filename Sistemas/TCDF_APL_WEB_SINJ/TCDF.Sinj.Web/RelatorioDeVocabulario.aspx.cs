using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using System.Text;
using util.BRLight;
using TCDF.Sinj.Log;

namespace TCDF.Sinj.Web
{
    public partial class RelatorioDeVocabulario : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var _tipo = Request["tipo"];
            var action = AcoesDoUsuario.voc_ger;
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                sessao_usuario = Util.ValidarSessao();
                Util.ValidarUsuario(sessao_usuario, action);

                var pesquisa = new Pesquisa();
                pesquisa.limit = null;
                if(_tipo != "*"){
                    pesquisa.literal = "ch_tipo_termo='"+_tipo+"'";
                }
                pesquisa.order_by=new Order_By{asc = new string[]{"nm_termo"}};

                var results = new VocabularioRN().Consultar(pesquisa);

                StringBuilder sb = new StringBuilder();

                //Format
                sb.Append("<style type=\"text/css\">\r\n");
                sb.Append(".tabHead\r\n");
                sb.Append("{\r\n");
                sb.Append("   background-color: #cccccc;\r\n");
                sb.Append("   border: solid 1px black;\r\n");
                sb.Append("}\r\n");
                sb.Append(".tabRow\r\n");
                sb.Append("{\r\n");
                sb.Append("   border: solid 1px black;\r\n");
                sb.Append("}\r\n");
                sb.Append("</style>\r\n\r\n");

                //Header
                sb.AppendFormat("<table>\r\n");
                sb.AppendFormat("<thead>\r\n");
                sb.AppendFormat("<tr>\r\n");
                sb.AppendFormat("\t<td class=\"tabHead\">Termo</td>\r\n");
                sb.AppendFormat("\t<td class=\"tabHead\">Tipo</td>\r\n");
                sb.AppendFormat("</tr>\r\n");
                sb.AppendFormat("</thead>\r\n");
                sb.AppendFormat("<tbody>\r\n");

                var stermos = JSON.Serialize<List<VocabularioOV>>(results.results);
                var termos_detalhados = JSON.Deserializa<List<VocabularioDetalhado>>(stermos);

                foreach (var termo in termos_detalhados)
                {
                    //Row   
                    sb.AppendFormat("<tr>\r\n");
                    sb.AppendFormat("\t<td class=\"tabRow\">" + termo.nm_termo + "</td>\r\n");
                    sb.AppendFormat("\t<td class=\"tabRow\">" + termo.nm_tipo_termo + "</td>\r\n");
                    sb.AppendFormat("</tr>\r\n");
                }

                //Footer
                sb.AppendFormat("</tbody>\r\n");
                sb.AppendFormat("</table>\r\n");
                var relatorio = new LogRelatorio
                {
                    RegistrosTotal = termos_detalhados.Count.ToString(),
                    PesquisaLight = pesquisa
                };
                LogOperacao.gravar_operacao(Util.GetEnumDescription(action), relatorio, "", "");

                Response.ContentType = "application/ms-excel";
                Response.AppendHeader("Content-Length", sb.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachement; filename=\"RelatorioDeVocabulario.xls\"");
                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
                Response.Write(sb.ToString());
            }
            catch(Exception ex)
            {
                var serro = "";
                if (ex is PermissionException || ex is SessionExpiredException)
                {
                    serro = ex.Message;
                }
                else
                {
                    serro = "Ocorreu um erro ao gerar o relatório.<br/>Tente mais tarde ou contate nossa equipe.";
                }

                var erro = new ErroRequest
                {
                    Pagina = Request.Path,
                    RequestQueryString = Request.QueryString,
                    MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                    StackTrace = ex.StackTrace
                };
                if (sessao_usuario != null)
                {
                    LogErro.gravar_erro(Util.GetEnumDescription(action), erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                }

                var html = "<html><head></head><body><div style='width:50%; margin:auto; text-align:center; color:#990000;'>" + serro + "</div></body></html>";
                Response.ContentType = "text/html";
                Response.Write(html);
            }
            Response.End();
        }
    }
}

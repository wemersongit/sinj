using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.OV;
using util.BRLight;
using TCDF.Sinj.RN;
using System.IO;
using System.Text;
using TCDF.Sinj.Log;
using TCDF.Sinj.ES;

namespace TCDF.Sinj.Portal.Web
{
    public partial class RelatorioDePesquisa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var _bbusca = Request["bbusca"];
            var _tp = Request["tp"];
            SessaoUsuarioOV sessao_usuario = null;
            try
            {
                if (Config.ValorChave("Aplicacao") == "CADASTRO")
                {
                    sessao_usuario = Util.ValidarSessao();
                }

                var link_app = Util.GetUriAndPath();

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
                var sResult = "";
                string filename = "";
                if (_bbusca == "sinj_norma")
                {
                    var action = AcoesDoUsuario.nor_pes;
                    try
                    {
                        //Header
                        sb.AppendFormat("<table>\r\n");
                        sb.AppendFormat("<thead>\r\n");
                        sb.AppendFormat("<tr>\r\n");
                        sb.AppendFormat("\t<td class=\"tabHead\">Norma</td>\r\n");
                        sb.AppendFormat("\t<td class=\"tabHead\">Data de Assinatura</td>\r\n");
                        sb.AppendFormat("\t<td class=\"tabHead\">Situação</td>\r\n");
                        sb.AppendFormat("\t<td class=\"tabHead\">Origem</td>\r\n");
                        sb.AppendFormat("\t<td class=\"tabHead\">Ementa</td>\r\n");
                        sb.AppendFormat("\t<td class=\"tabHead\">Link</td>\r\n");
                        sb.AppendFormat("</tr>\r\n");
                        sb.AppendFormat("</thead>\r\n");
                        sb.AppendFormat("<tbody>\r\n");
                        if (_tp == "lb")
                        {
                            var lb = new LB();
                            Util.ValidarUsuario(sessao_usuario, action);

                            var result_norma = lb.RelatorioDocs<NormaOV>(base.Context, sessao_usuario, "sinj_norma");
                            foreach (var norma in result_norma)
                            {
                                //Row   
                                sb.AppendFormat("<tr>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + norma.nm_tipo_norma + (norma.nr_norma != null ? " " + norma.nr_norma : "") + (!string.IsNullOrEmpty(norma.cr_norma) ? " " + norma.cr_norma : "") + "</td>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + norma.dt_assinatura + "</td>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + norma.nm_situacao + "</td>\r\n");
                                var sg_orgao = "";
                                for (var i = 0; i < norma.origens.Count(); i++)
                                {
                                    sg_orgao += (sg_orgao != "" ? "<br style=\"mso-data-placement:same-cell;\"/>" : "") + norma.origens[i].sg_orgao + " - " + norma.origens[i].nm_orgao;
                                }
                                sb.AppendFormat("\t<td class=\"tabRow\">" + sg_orgao + "</td>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + norma.ds_ementa + "</td>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + link_app + "/DetalhesDeNorma.aspx?id_norma=" + norma.ch_norma + "</td>\r\n");
                                sb.AppendFormat("</tr>\r\n");
                            }

                        }
                        else if (_tp == "es")
                        {
                            var es = new ESAd();
                            var result_norma = es.RelatorioDocs<NormaOV>(base.Context, sessao_usuario, _bbusca);
                            foreach (var hit in result_norma)
                            {
                                //Row   
                                sb.AppendFormat("<tr>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + hit.fields.partial[0].nm_tipo_norma + (hit.fields.partial[0].nr_norma != null ? " " + hit.fields.partial[0].nr_norma.ToString() : "") + (!string.IsNullOrEmpty(hit.fields.partial[0].cr_norma) ? " " + hit.fields.partial[0].cr_norma : "") + "</td>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + hit.fields.partial[0].dt_assinatura + "</td>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + hit.fields.partial[0].nm_situacao + "</td>\r\n");
                                var sg_orgao = "";
                                for (var i = 0; i < hit.fields.partial[0].origens.Count(); i++)
                                {
                                    sg_orgao += (sg_orgao != "" ? "<br style=\"mso-data-placement:same-cell;\"/>" : "") + hit.fields.partial[0].origens[i].sg_orgao + " - " + hit.fields.partial[0].origens[i].nm_orgao;
                                }
                                sb.AppendFormat("\t<td class=\"tabRow\">" + sg_orgao + "</td>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + hit.fields.partial[0].ds_ementa + "</td>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + link_app + "/DetalhesDeNorma.aspx?id_norma=" + hit.fields.partial[0].ch_norma + "</td>\r\n");
                                sb.AppendFormat("</tr>\r\n");
                            }
                        }

                        filename = "RelatorioNormas.xls";
                    }
                    catch (Exception ex)
                    {
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
                        throw;
                    }
                }
                else if (_bbusca == "sinj_diario")
                {
                    var action = AcoesDoUsuario.dio_pes;
                    try
                    {

                        //Header
                        sb.AppendFormat("<table>\r\n");
                        sb.AppendFormat("<thead>\r\n");
                        sb.AppendFormat("<tr>\r\n");
                        sb.AppendFormat("\t<td class=\"tabHead\">Número</td>\r\n");
                        sb.AppendFormat("\t<td class=\"tabHead\">Seção</td>\r\n");
                        sb.AppendFormat("\t<td class=\"tabHead\">Data de Publicação</td>\r\n");
                        sb.AppendFormat("</tr>\r\n");
                        sb.AppendFormat("</thead>\r\n");
                        sb.AppendFormat("<tbody>\r\n");

                        if (_tp == "lb")
                        {
                            var lb = new LB();
                            Util.ValidarUsuario(sessao_usuario, action);

                            var result_diario = lb.RelatorioDocs<DiarioOV>(base.Context, sessao_usuario, _bbusca);

                            foreach (var diario in result_diario)
                            {
                                //Row   
                                sb.AppendFormat("<tr>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + diario.nr_diario + (!string.IsNullOrEmpty(diario.cr_diario) ? " " + diario.cr_diario : "") + "</td>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + diario.secao_diario + "</td>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + diario.dt_assinatura + "</td>\r\n");
                                sb.AppendFormat("</tr>\r\n");
                            }

                        }
                        else if (_tp == "es")
                        {

                            var es = new ESAd();
                            var result_diario = es.RelatorioDocs<DiarioOV>(base.Context, sessao_usuario, _bbusca);
                            foreach (var hit in result_diario)
                            {
                                //Row   
                                sb.AppendFormat("<tr>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + hit.fields.partial[0].nr_diario + (!string.IsNullOrEmpty(hit.fields.partial[0].cr_diario) ? " " + hit.fields.partial[0].cr_diario : "") + "</td>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + hit.fields.partial[0].secao_diario + "</td>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + hit.fields.partial[0].dt_assinatura + "</td>\r\n");
                                sb.AppendFormat("</tr>\r\n");
                            }
                        }

                        filename = "RelatorioDiarios.xls";
                    }
                    catch (Exception ex)
                    {
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
                        throw;
                    }
                }
                else if (_bbusca == "cesta")
                {
                    var _base = Request["b"];
                    var es = new ESAd();
                    try
                    {
                        if (_base == "sinj_norma")
                        {
                            //Header
                            sb.AppendFormat("<table>\r\n");
                            sb.AppendFormat("<thead>\r\n");
                            sb.AppendFormat("<tr>\r\n");
                            sb.AppendFormat("\t<td class=\"tabHead\">Norma</td>\r\n");
                            sb.AppendFormat("\t<td class=\"tabHead\">Data de Assinatura</td>\r\n");
                            sb.AppendFormat("\t<td class=\"tabHead\">Situação</td>\r\n");
                            sb.AppendFormat("\t<td class=\"tabHead\">Origem</td>\r\n");
                            sb.AppendFormat("\t<td class=\"tabHead\">Ementa</td>\r\n");
                            sb.AppendFormat("\t<td class=\"tabHead\">Link</td>\r\n");
                            sb.AppendFormat("</tr>\r\n");
                            sb.AppendFormat("</thead>\r\n");
                            sb.AppendFormat("<tbody>\r\n");
                            var result_norma = es.RelatorioDocs<NormaOV>(base.Context, sessao_usuario, _bbusca);
                            foreach (var hit in result_norma)
                            {
                                //Row   
                                sb.AppendFormat("<tr>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + hit.fields.partial[0].nm_tipo_norma + (hit.fields.partial[0].nr_norma != null ? " " + hit.fields.partial[0].nr_norma.ToString() : "") + (!string.IsNullOrEmpty(hit.fields.partial[0].cr_norma) ? " " + hit.fields.partial[0].cr_norma : "") + "</td>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + hit.fields.partial[0].dt_assinatura + "</td>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + hit.fields.partial[0].nm_situacao + "</td>\r\n");
                                var sg_orgao = "";
                                for (var i = 0; i < hit.fields.partial[0].origens.Count(); i++)
                                {
                                    sg_orgao += (sg_orgao != "" ? "<br style=\"mso-data-placement:same-cell;\"/>" : "") + hit.fields.partial[0].origens[i].sg_orgao + " - " + hit.fields.partial[0].origens[i].nm_orgao;
                                }
                                sb.AppendFormat("\t<td class=\"tabRow\">" + sg_orgao + "</td>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + hit.fields.partial[0].ds_ementa + "</td>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + link_app + "/DetalhesDeNorma.aspx?id_norma=" + hit.fields.partial[0].ch_norma + "</td>\r\n");
                                sb.AppendFormat("</tr>\r\n");
                            }
                            filename = "RelatorioNormas.xls";
                        }
                        else if (_base == "sinj_diario")
                        {
                            //Header
                            sb.AppendFormat("<table>\r\n");
                            sb.AppendFormat("<thead>\r\n");
                            sb.AppendFormat("<tr>\r\n");
                            sb.AppendFormat("\t<td class=\"tabHead\">Número</td>\r\n");
                            sb.AppendFormat("\t<td class=\"tabHead\">Seção</td>\r\n");
                            sb.AppendFormat("\t<td class=\"tabHead\">Data de Publicação</td>\r\n");
                            sb.AppendFormat("</tr>\r\n");
                            sb.AppendFormat("</thead>\r\n");
                            sb.AppendFormat("<tbody>\r\n");
                            var result_diario = es.RelatorioDocs<DiarioOV>(base.Context, sessao_usuario, _bbusca);
                            foreach (var hit in result_diario)
                            {
                                //Row   
                                sb.AppendFormat("<tr>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + hit.fields.partial[0].nr_diario + (!string.IsNullOrEmpty(hit.fields.partial[0].cr_diario) ? " " + hit.fields.partial[0].cr_diario : "") + "</td>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + hit.fields.partial[0].secao_diario + "</td>\r\n");
                                sb.AppendFormat("\t<td class=\"tabRow\">" + hit.fields.partial[0].dt_assinatura + "</td>\r\n");
                                sb.AppendFormat("</tr>\r\n");
                            }
                            filename = "RelatorioDiarios.xls";
                        }
                    }
                    catch (Exception ex)
                    {
                        var erro = new ErroRequest
                        {
                            Pagina = Request.Path,
                            RequestQueryString = Request.QueryString,
                            MensagemDaExcecao = Excecao.LerTodasMensagensDaExcecao(ex, true),
                            StackTrace = ex.StackTrace
                        };
                        if (sessao_usuario != null)
                        {
                            LogErro.gravar_erro("CST.RLT", erro, sessao_usuario.nm_usuario, sessao_usuario.nm_login_usuario);
                        }
                        throw;
                    }
                }

                //Footer
                sb.AppendFormat("</tbody>\r\n");
                sb.AppendFormat("</table>\r\n");
                sResult = sb.ToString();

                Response.ContentType = "application/ms-excel";
                Response.AppendHeader("Content-Length", sResult.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachement; filename=\"" + filename + "\"");
                Response.ContentEncoding = Encoding.GetEncoding("iso-8859-1");
                Response.Write(sResult);
            }
            catch
            {
                string html = "<html><head></head><body><div style='width:50%; margin:auto; text-align:center; color:#990000;'>Ocorreu um erro ao gerar o relatório.<br/>Tente mais tarde ou contate nossa equipe.</div></body></html>";
                Response.ContentType = "text/html";
                Response.Write(html);
            }
            Response.End();
        }
    }
}

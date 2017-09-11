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

namespace TCDF.Sinj.Web
{
    public partial class RelatorioDePesquisa : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var _bbusca = Request["bbusca"];
            var _tp = Request["tp"];
            SessaoUsuarioOV sessao_usuario = null;
            var bCadastro = false;
            try
            {
                if (Config.ValorChave("Aplicacao") == "CADASTRO")
                {
                    sessao_usuario = Util.ValidarSessao();
                    bCadastro = true;
                }

                var link_app = Config.ValorChave("URLSinjPortal", true);

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
                sb.AppendFormat("<table>\r\n");
                var sResult = "";
                string filename = "";
                if (_bbusca == "sinj_norma")
                {
                    var action = AcoesDoUsuario.nor_pes;
                    try
                    {

                        sb.AppendFormat(GetHeadNorma(bCadastro));

                        sb.AppendFormat("<tbody>\r\n");

                        if (_tp == "lb")
                        {
                            var lb = new LB();
                            Util.ValidarUsuario(sessao_usuario, action);

                            var result_norma = lb.RelatorioDocs<NormaOV>(base.Context, sessao_usuario, "sinj_norma");
                            foreach (var norma in result_norma)
                            {
                                sb.AppendFormat(GetLinha(norma, bCadastro, link_app));
                            }

                        }
                        else if (_tp == "es")
                        {
                            var es = new ES();
                            var result_norma = es.RelatorioDocs<NormaOV>(base.Context, sessao_usuario, _bbusca);
                            foreach (var hit in result_norma)
                            {
                                sb.AppendFormat(GetLinha(hit.fields.partial[0], bCadastro, link_app));
                            }
                        }

                        //Footer
                        sb.AppendFormat("</tbody>\r\n");

                        filename = "RelatorioNormas"+DateTime.Now.ToString("ddMMMyyyy_hhmmss")+".xls";
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

                        sb.AppendFormat(GetHeadDiario(bCadastro));

                        sb.AppendFormat("<tbody>\r\n");
                        
                        if (_tp == "lb")
                        {
                            var lb = new LB();
                            Util.ValidarUsuario(sessao_usuario, action);

                            var result_diario = lb.RelatorioDocs<DiarioOV>(base.Context, sessao_usuario, _bbusca);

                            foreach (var diario in result_diario)
                            {
                                sb.AppendFormat(GetLinha(diario, bCadastro));
                            }

                        }
                        else if (_tp == "es")
                        {

                            var es = new ES();
                            var result_diario = es.RelatorioDocs<DiarioOV>(base.Context, sessao_usuario, _bbusca);
                            foreach (var hit in result_diario)
                            {
                                sb.AppendFormat(GetLinha(hit.fields.partial[0], bCadastro));
                            }
                        }

                        //Footer
                        sb.AppendFormat("</tbody>\r\n");

                        filename = "RelatorioDiarios" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".xls";
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
                    var es = new ES();
                    try
                    {
                        if (_base == "sinj_norma")
                        {

                            sb.AppendFormat(GetHeadNorma(bCadastro));

                            sb.AppendFormat("<tbody>\r\n");

                            var result_norma = es.RelatorioDocs<NormaOV>(base.Context, sessao_usuario, _bbusca);
                            foreach (var hit in result_norma)
                            {
                                sb.AppendFormat(GetLinha(hit.fields.partial[0], bCadastro, link_app));
                            }

                            //Footer
                            sb.AppendFormat("</tbody>\r\n");

                            filename = "RelatorioNormas" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".xls";
                        }
                        else if (_base == "sinj_diario")
                        {

                            sb.AppendFormat(GetHeadNorma(bCadastro));

                            sb.AppendFormat("<tbody>\r\n");

                            var result_diario = es.RelatorioDocs<DiarioOV>(base.Context, sessao_usuario, _bbusca);
                            foreach (var hit in result_diario)
                            {
                                sb.AppendFormat(GetLinha(hit.fields.partial[0], bCadastro));
                            }

                            //Footer
                            sb.AppendFormat("</tbody>\r\n");

                            filename = "RelatorioDiarios" + DateTime.Now.ToString("ddMMMyyyy_hhmmss") + ".xls";
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

                sb.AppendFormat("</table>\r\n");
                sResult = sb.ToString();

                Response.ContentType = "application/ms-excel";
                Response.AppendHeader("Content-Length", sResult.Length.ToString());
                Response.AddHeader("Content-Disposition", "attachement; filename=\""+filename+"\"");
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

        private string GetHeadNorma(bool bCadastro)
        {
            StringBuilder sb = new StringBuilder();
            //Header
            sb.AppendFormat("<thead>\r\n");
            sb.AppendFormat("<tr>\r\n");
            sb.AppendFormat("\t<td class=\"tabHead\">Norma</td>\r\n");
            sb.AppendFormat("\t<td class=\"tabHead\">Data de Assinatura</td>\r\n");
            sb.AppendFormat("\t<td class=\"tabHead\">Situação</td>\r\n");
            sb.AppendFormat("\t<td class=\"tabHead\">Origem</td>\r\n");
            sb.AppendFormat("\t<td class=\"tabHead\">Ementa</td>\r\n");
            sb.AppendFormat("\t<td class=\"tabHead\">Link</td>\r\n");
            if (bCadastro)
            {
                sb.AppendFormat("\t<td class=\"tabHead\">Vides</td>\r\n");
                sb.AppendFormat("\t<td class=\"tabHead\">Apelido</td>\r\n");
                sb.AppendFormat("\t<td class=\"tabHead\">Observação</td>\r\n");
                sb.AppendFormat("\t<td class=\"tabHead\">Autoria</td>\r\n");
                sb.AppendFormat("\t<td class=\"tabHead\">Lista de Nomes</td>\r\n");
                sb.AppendFormat("\t<td class=\"tabHead\">Fontes</td>\r\n");
                sb.AppendFormat("\t<td class=\"tabHead\">Indexação</td>\r\n");
                sb.AppendFormat("\t<td class=\"tabHead\">Data do Cadastro</td>\r\n");
                sb.AppendFormat("\t<td class=\"tabHead\">Cadastrador</td>\r\n");
            }
            sb.AppendFormat("</tr>\r\n");
            sb.AppendFormat("</thead>\r\n");
            return sb.ToString();
        }

        private string GetLinha(NormaOV norma, bool bCadastro, string link_app)
        {
            StringBuilder sb = new StringBuilder();

            var vides = "";
            var ds_dispositivo = "";
            var ds_norma = "";
            var sg_orgao = "";
            var nm_autorias = "";
            var nomes = "";
            var ds_fontes = "";
            var ds_indexacao = "";
            //Row   
            sb.AppendFormat("<tr>\r\n");
            sb.AppendFormat("\t<td class=\"tabRow\">" + norma.nm_tipo_norma + (norma.nr_norma != null ? " " + norma.nr_norma : "") + (!string.IsNullOrEmpty(norma.cr_norma) ? " " + norma.cr_norma : "") + "</td>\r\n");
            sb.AppendFormat("\t<td class=\"tabRow\">" + norma.dt_assinatura + "</td>\r\n");
            sb.AppendFormat("\t<td class=\"tabRow\">" + norma.nm_situacao + "</td>\r\n");
            foreach (var orgao in norma.origens)
            {
                sg_orgao += (sg_orgao != "" ? "<br style=\"mso-data-placement:same-cell;\"/>" : "") + orgao.sg_orgao + " - " + orgao.nm_orgao;
            }
            sb.AppendFormat("\t<td class=\"tabRow\">" + sg_orgao + "</td>\r\n");
            sb.AppendFormat("\t<td class=\"tabRow\">" + norma.ds_ementa.Replace("}","") + "</td>\r\n");
            sb.AppendFormat("\t<td class=\"tabRow\">" + link_app + "/DetalhesDeNorma.aspx?id_norma=" + norma.ch_norma + "</td>\r\n");
            if (bCadastro)
            {
                foreach (var vide in norma.vides)
                {
                    ds_dispositivo = "";
                    ds_norma = "";
                    if (vide.caput_norma_vide_outra != null && !string.IsNullOrEmpty(vide.caput_norma_vide_outra.ds_norma))
                    {
                        ds_norma = vide.caput_norma_vide_outra.ds_norma;
                    }
                    else
                    {
                        ds_norma = vide.nm_tipo_norma_vide + " " + vide.nr_norma_vide + " de " + vide.dt_assinatura_norma_vide;
                    }
                    if (vide.in_norma_afetada)
                    {
                        if (vide.caput_norma_vide != null)
                        {
                            if (!string.IsNullOrEmpty(vide.caput_norma_vide.ds_caput))
                            {
                                ds_dispositivo = vide.caput_norma_vide.ds_caput + " ";
                            }
                        }
                        if (string.IsNullOrEmpty(ds_dispositivo))
                        {
                            if (!string.IsNullOrEmpty(vide.artigo_norma_vide))
                            {
                                ds_dispositivo += "Art. " + vide.artigo_norma_vide;
                            }
                            if (!string.IsNullOrEmpty(vide.paragrafo_norma_vide))
                            {
                                ds_dispositivo += (!string.IsNullOrEmpty(ds_dispositivo) ? ", " : "") + "Par. " + vide.paragrafo_norma_vide;
                            }
                            if (!string.IsNullOrEmpty(vide.inciso_norma_vide))
                            {
                                ds_dispositivo += (!string.IsNullOrEmpty(ds_dispositivo) ? ", " : "") + "Inc. " + vide.inciso_norma_vide;
                            }
                            if (!string.IsNullOrEmpty(vide.alinea_norma_vide))
                            {
                                ds_dispositivo += (!string.IsNullOrEmpty(ds_dispositivo) ? ", " : "") + "Alí. " + vide.alinea_norma_vide;
                            }
                            if (!string.IsNullOrEmpty(vide.item_norma_vide))
                            {
                                ds_dispositivo += (!string.IsNullOrEmpty(ds_dispositivo) ? ", " : "") + "Item " + vide.item_norma_vide;
                            }
                            if (!string.IsNullOrEmpty(vide.anexo_norma_vide))
                            {
                                ds_dispositivo += (!string.IsNullOrEmpty(ds_dispositivo) ? ", " : "") + "Anexo " + vide.anexo_norma_vide;
                            }
                        }
                        vides += (vides != "" ? "<br style=\"mso-data-placement:same-cell;\"/>" : "") + ds_dispositivo + vide.ds_texto_relacao + " pelo(a) " + ds_norma;
                    }
                    else
                    {
                        if (vide.caput_norma_vide_outra != null)
                        {
                            if (!string.IsNullOrEmpty(vide.caput_norma_vide_outra.ds_caput))
                            {
                                ds_dispositivo = " " + vide.caput_norma_vide_outra.ds_caput + " do(a) ";
                            }
                        }
                        if (string.IsNullOrEmpty(ds_dispositivo))
                        {
                            if (!string.IsNullOrEmpty(vide.artigo_norma_vide_outra))
                            {
                                ds_dispositivo += "Art. " + vide.artigo_norma_vide_outra;
                            }
                            if (!string.IsNullOrEmpty(vide.paragrafo_norma_vide_outra))
                            {
                                ds_dispositivo += (!string.IsNullOrEmpty(ds_dispositivo) ? ", " : "") + "Par. " + vide.paragrafo_norma_vide_outra;
                            }
                            if (!string.IsNullOrEmpty(vide.inciso_norma_vide_outra))
                            {
                                ds_dispositivo += (!string.IsNullOrEmpty(ds_dispositivo) ? ", " : "") + "Inc. " + vide.inciso_norma_vide_outra;
                            }
                            if (!string.IsNullOrEmpty(vide.alinea_norma_vide_outra))
                            {
                                ds_dispositivo += (!string.IsNullOrEmpty(ds_dispositivo) ? ", " : "") + "Alí. " + vide.alinea_norma_vide_outra;
                            }
                            if (!string.IsNullOrEmpty(vide.item_norma_vide_outra))
                            {
                                ds_dispositivo += (!string.IsNullOrEmpty(ds_dispositivo) ? ", " : "") + "Item " + vide.item_norma_vide_outra;
                            }
                            if (!string.IsNullOrEmpty(vide.anexo_norma_vide_outra))
                            {
                                ds_dispositivo += (!string.IsNullOrEmpty(ds_dispositivo) ? ", " : "") + "Anexo " + vide.anexo_norma_vide_outra;
                            }
                        }
                        vides += (vides != "" ? "<br style=\"mso-data-placement:same-cell;\"/>" : "") + vide.ds_texto_relacao + (ds_dispositivo != "" ? ds_dispositivo : " o(a) ") + ds_norma;

                    }
                }
                sb.AppendFormat("\t<td class=\"tabRow\">" + vides + "</td>\r\n"); sb.AppendFormat("\t<td class=\"tabRow\">" + norma.nm_apelido + "</td>\r\n");
                sb.AppendFormat("\t<td class=\"tabRow\">" + norma.ds_observacao + "</td>\r\n");

                nm_autorias = "";
                foreach (var autoria in norma.autorias)
                {
                    nm_autorias += (nm_autorias != "" ? "<br style=\"mso-data-placement:same-cell;\"/>" : "") + autoria.nm_autoria;
                }
                sb.AppendFormat("\t<td class=\"tabRow\">" + nm_autorias + "</td>\r\n");

                nomes = "";
                foreach (var nm_pessoa in norma.nm_pessoa_fisica_e_juridica)
                {
                    nomes += (nomes != "" ? "<br style=\"mso-data-placement:same-cell;\"/>" : "") + nm_pessoa;
                }
                sb.AppendFormat("\t<td class=\"tabRow\">" + nomes + "</td>\r\n");

                ds_fontes = "";
                foreach (var fonte in norma.fontes)
                {
                    ds_fontes += (ds_fontes != "" ? "<br style=\"mso-data-placement:same-cell;\"/>" : "") + fonte.nm_tipo_publicacao + " " + fonte.ds_diario;
                }
                sb.AppendFormat("\t<td class=\"tabRow\">" + ds_fontes + "</td>\r\n");

                ds_indexacao = "";
                foreach (var indexacao in norma.indexacoes)
                {
                    ds_indexacao += ds_indexacao != "" ? "<br style=\"mso-data-placement:same-cell;\"/>" : "";
                    foreach (var vocabulario in indexacao.vocabulario)
                    {
                        ds_indexacao += (ds_indexacao != "" ? ", " : "") + vocabulario.nm_termo;
                    }
                }
                sb.AppendFormat("\t<td class=\"tabRow\">" + ds_indexacao + "</td>\r\n");
                sb.AppendFormat("\t<td class=\"tabRow\">" + norma.dt_cadastro + "</td>\r\n");
                sb.AppendFormat("\t<td class=\"tabRow\">" + norma.nm_login_usuario_cadastro + "</td>\r\n");
            }
            sb.AppendFormat("</tr>\r\n");

            return sb.ToString();
        }

        private string GetHeadDiario(bool bCadastro)
        {
            StringBuilder sb = new StringBuilder();
            //Header
            sb.AppendFormat("<thead>\r\n");
            sb.AppendFormat("<tr>\r\n");
            sb.AppendFormat("\t<td class=\"tabHead\">Número</td>\r\n");
            sb.AppendFormat("\t<td class=\"tabHead\">Seção</td>\r\n");
            sb.AppendFormat("\t<td class=\"tabHead\">Data de Publicação</td>\r\n");
            if (bCadastro)
            {
                sb.AppendFormat("\t<td class=\"tabHead\">Data do Cadastro</td>\r\n");
                sb.AppendFormat("\t<td class=\"tabHead\">Cadastrador</td>\r\n");
            }
            sb.AppendFormat("</tr>\r\n");
            sb.AppendFormat("</thead>\r\n");
            return sb.ToString();
        }

        private string GetLinha(DiarioOV diario, bool bCadastro)
        {
            StringBuilder sb = new StringBuilder();
            //Row   
            sb.AppendFormat("<tr>\r\n");
            sb.AppendFormat("\t<td class=\"tabRow\">" + diario.nr_diario + (!string.IsNullOrEmpty(diario.cr_diario) ? " " + diario.cr_diario : "") + "</td>\r\n");
            sb.AppendFormat("\t<td class=\"tabRow\">" + diario.secao_diario + "</td>\r\n");
            sb.AppendFormat("\t<td class=\"tabRow\">" + diario.dt_assinatura + "</td>\r\n");
            if (bCadastro)
            {
                sb.AppendFormat("\t<td class=\"tabRow\">" + diario.dt_cadastro + "</td>\r\n");
                sb.AppendFormat("\t<td class=\"tabRow\">" + diario.nm_login_usuario_cadastro + "</td>\r\n");
            }
            sb.AppendFormat("</tr>\r\n");

            return sb.ToString();
        }
    }
}
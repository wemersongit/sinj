using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using TCDF.Sinj.RN;
using neo.BRLightREST;
using util.BRLight;
using System.Web.UI.HtmlControls;
using TCDF.Sinj.OV;

namespace TCDF.Sinj.Portal.Web
{
	public partial class BaixarArquivoNorma : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
        {
            var _id_file = Request["id_file"];
            var _id_norma = Request["id_norma"];
            NormaOV normaOv = null;
            var id_file = "";

            try
            {
                var oKeywords = Request.RequestContext.RouteData.Values["keywords"];
                if (oKeywords != null)
                {
                    var aKeywords = oKeywords.ToString().Split('/');
                    if (aKeywords.Length == 2)
                    {
                        _id_file = aKeywords[0];
                    }
                }
                if (!string.IsNullOrEmpty(_id_norma))
                {
                    normaOv = new NormaRN().Doc(_id_norma);

                    if (!string.IsNullOrEmpty(normaOv.ar_atualizado.id_file))
                    {
                        id_file = normaOv.ar_atualizado.id_file;
                    }
                    else
                    {
                        foreach (var fonte in normaOv.fontes)
                        {
                            if (!string.IsNullOrEmpty(fonte.ar_fonte.id_file))
                            {
                                id_file = fonte.ar_fonte.id_file;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(id_file))
                    {
                        var json_doc = new NormaRN().GetDoc(id_file);
                        if (json_doc.IndexOf("\"status\": 500") > -1)
                        {
                            throw new Exception("Erro ao obter arquivo.");
                        }
                        else if (json_doc.IndexOf("\"status\": 404") > -1)
                        {
                            throw new Exception("O arquivo não foi encontrado.");
                        }
                        else if (!string.IsNullOrEmpty(json_doc))
                        {
                            var normaRn = new NormaRN();
                            var doc = JSON.Deserializa<ArquivoFullOV>(json_doc);
                            var documento = normaRn.Download(id_file);
                            if (doc.id_doc != null && doc.id_doc != 0)
                            {
                                var norma = normaRn.Doc(doc.id_doc);
                                var ds_norma = norma.nm_tipo_norma + (!string.IsNullOrEmpty(norma.nr_norma) ? " Nº " + norma.nr_norma : "") + " de " + norma.dt_assinatura;
                                Page.Title = ds_norma;
                                HtmlMeta html_meta_keywords = new HtmlMeta();
                                html_meta_keywords.Name = "keywords";
                                html_meta_keywords.Content = "sinj, distrito, federal, df," + norma.nm_tipo_norma;
                                HtmlMeta html_meta_description = new HtmlMeta();
                                html_meta_description.Name = "description";
                                html_meta_description.Content = "Arquivo de " + ds_norma + " disponibilizado pelo SINJ.";
                                placeHolderHeader.Controls.Add(html_meta_keywords);
                                placeHolderHeader.Controls.Add(html_meta_description);

                            }
                            if (doc.mimetype.IndexOf("html") > -1)
                            {
                                var msg = "";
                                var encoding_documento = "windows-1252"; //isso deverá ser preenchido dinamicamente

                                // Esse switch receberá todas possiveís opçoes de encoding do HTML
                                // Isso acontecerá para que a conversão de Encoding seja dinâmica
                                switch (encoding_documento)
                                {
                                    case "windows-1252":
                                        Encoding wind1252 = Encoding.GetEncoding(1252);
                                        Encoding utf8 = Encoding.UTF8;
                                        byte[] wind1252Bytes = documento;
                                        byte[] utfBytes = Encoding.Convert(wind1252, utf8, wind1252Bytes);
                                        msg = utf8.GetString(utfBytes);
                                        break;

                                    case "utf-8":
                                        msg = Encoding.UTF8.GetString(documento);
                                        break;
                                }

                                div_texto.InnerHtml = msg;
                            }
                            else
                            {
                                if (documento != null && documento.Length > 0)
                                {
                                    Response.Clear();
                                    Response.ContentType = doc.mimetype;
                                    Response.AppendHeader("Content-Length", documento.Length.ToString());
                                    Response.AppendHeader("Content-Disposition", "inline; filename=\"" + doc.filename + "\"");
                                    Response.BinaryWrite(documento);
                                    Response.Flush();
                                }
                                else
                                {
                                    throw new Exception("Arquivo não encontrado.");
                                }
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
                else if (!string.IsNullOrEmpty(_id_file))
                {
                    var json_doc = new NormaRN().GetDoc(_id_file);
                    if (json_doc.IndexOf("\"status\": 500") > -1)
                    {
                        throw new Exception("Erro ao obter arquivo.");
                    }
                    else if (json_doc.IndexOf("\"status\": 404") > -1)
                    {
                        throw new Exception("O arquivo não foi encontrado.");
                    }
                    else if (!string.IsNullOrEmpty(json_doc))
                    {
                        var normaRn = new NormaRN();
                        var doc = JSON.Deserializa<ArquivoFullOV>(json_doc);
                        var documento = normaRn.Download(_id_file);
                        if (doc.id_doc != null && doc.id_doc != 0)
                        {
                            var norma = normaRn.Doc(doc.id_doc);
                            var ds_norma = norma.nm_tipo_norma + (!string.IsNullOrEmpty(norma.nr_norma) ? " Nº " + norma.nr_norma : "") + " de " + norma.dt_assinatura;
                            Page.Title = ds_norma;
                            HtmlMeta html_meta_keywords = new HtmlMeta();
                            html_meta_keywords.Name = "keywords";
                            html_meta_keywords.Content = "sinj, distrito, federal, df," + norma.nm_tipo_norma;
                            HtmlMeta html_meta_description = new HtmlMeta();
                            html_meta_description.Name = "description";
                            html_meta_description.Content = "Arquivo de " + ds_norma + " disponibilizado pelo SINJ.";
                            placeHolderHeader.Controls.Add(html_meta_keywords);
                            placeHolderHeader.Controls.Add(html_meta_description);

                        }
                        if (doc.mimetype.IndexOf("html") > -1)
                        {
                            var msg = "";
                            var encoding_documento = "windows-1252"; //isso deverá ser preenchido dinamicamente

                            // Esse switch receberá todas possiveís opçoes de encoding do HTML
                            // Isso acontecerá para que a conversão de Encoding seja dinâmica
                            switch (encoding_documento)
                            {
                                case "windows-1252":
                                    Encoding wind1252 = Encoding.GetEncoding(1252);
                                    Encoding utf8 = Encoding.UTF8;
                                    byte[] wind1252Bytes = documento;
                                    byte[] utfBytes = Encoding.Convert(wind1252, utf8, wind1252Bytes);
                                    msg = utf8.GetString(utfBytes);
                                    break;

                                case "utf-8":
                                    msg = Encoding.UTF8.GetString(documento);
                                    break;
                            }

                            div_texto.InnerHtml = msg;
                            //							div_texto.InnerHtml = System.Text.Encoding.wind1252.GetString(documento);
                        }
                        else
                        {
                            if (documento != null && documento.Length > 0)
                            {
                                Response.Clear();
                                Response.ContentType = doc.mimetype;
                                Response.AppendHeader("Content-Length", documento.Length.ToString());
                                Response.AppendHeader("Content-Disposition", "inline; filename=\"" + doc.filename + "\"");
                                Response.BinaryWrite(documento);
                                Response.Flush();
                            }
                            else
                            {
                                throw new Exception("Arquivo não encontrado.");
                            }
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
            catch (Exception Ex)
            {
                Response.Clear();
                Response.Write("<html><head></head><body><div id=\"div_erro\" style=\"color:#990000; width:500px; margin:auto; text-align:center;\">" + util.BRLight.Excecao.LerInnerException(Ex, true) + "<br/><br/>Nossa equipe resolverá o problema, você pode tentar mais tarde ou entrar em contato conosco.</div></body><html>");
            }



            //var _id_file = Request["id_file"];
            //try
            //{
            //    if (!string.IsNullOrEmpty(_id_file))
            //    {
            //        var json_doc = new NormaRN().GetDoc(_id_file);
            //        if (json_doc.IndexOf("\"status\": 500") > -1)
            //        {
            //            throw new Exception("Erro ao obter arquivo.");
            //        }
            //        else if (json_doc.IndexOf("\"status\": 404") > -1)
            //        {
            //            throw new Exception("O arquivo não foi encontrado.");
            //        }
            //        else if (!string.IsNullOrEmpty(json_doc))
            //        {
            //            var normaRn = new NormaRN();
            //            var doc = JSON.Deserializa<ArquivoFullOV>(json_doc);
            //            var documento = normaRn.Download(_id_file);
            //            if (doc.id_doc != null && doc.id_doc != 0)
            //            {
            //                var norma = normaRn.Doc(doc.id_doc);
            //                var ds_norma = norma.nm_tipo_norma + (!string.IsNullOrEmpty(norma.nr_norma) ? " Nº " + norma.nr_norma : "") + " de " + norma.dt_assinatura;
            //                Page.Title = ds_norma;
            //                HtmlMeta html_meta_keywords = new HtmlMeta();
            //                html_meta_keywords.Name = "keywords";
            //                html_meta_keywords.Content = "sinj, distrito, federal, df," + norma.nm_tipo_norma;
            //                HtmlMeta html_meta_description = new HtmlMeta();
            //                html_meta_description.Name = "description";
            //                html_meta_description.Content = "Arquivo de " + ds_norma + " disponibilizado pelo SINJ.";
            //                placeHolderHeader.Controls.Add(html_meta_keywords);
            //                placeHolderHeader.Controls.Add(html_meta_description);
                            
            //            }
            //            if (doc.mimetype.IndexOf("html")>-1)
            //            {
            //                var msg = "";
            //                var encoding_documento = "windows-1252"; //isso deverá ser preenchido dinamicamente

            //                // Esse switch receberá todas possiveís opçoes de encoding do HTML
            //                // Isso acontecerá para que a conversão de Encoding seja dinâmica
            //                switch (encoding_documento)
            //                {
            //                case "windows-1252":
            //                    Encoding wind1252 = Encoding.GetEncoding(1252);  
            //                    Encoding utf8 = Encoding.UTF8;
            //                    byte[] wind1252Bytes = documento;
            //                    byte[] utfBytes = Encoding.Convert(wind1252, utf8, wind1252Bytes);
            //                    msg = utf8.GetString(utfBytes);
            //                    break;

            //                case "utf-8":
            //                    msg = Encoding.UTF8.GetString(documento);
            //                    break;
            //                }

            //                div_texto.InnerHtml = msg;
            //                //							div_texto.InnerHtml = System.Text.Encoding.wind1252.GetString(documento);
            //            }
            //            else
            //            {
            //                if (documento != null && documento.Length > 0)
            //                {
            //                    Response.Clear();
            //                    Response.ContentType = doc.mimetype;
            //                    Response.AppendHeader("Content-Length", documento.Length.ToString());
            //                    Response.AppendHeader("Content-Disposition", "inline; filename=\"" + doc.filename + "\"");
            //                    Response.BinaryWrite(documento);
            //                    Response.Flush();
            //                }
            //                else
            //                {
            //                    throw new Exception("Arquivo não encontrado.");
            //                }
            //            }
            //        }
            //        else
            //        {
            //            throw new Exception("Arquivo não encontrado.");
            //        }
            //    }
            //    else
            //    {
            //        throw new Exception("Arquivo não encontrado.");
            //    }

            //}
            //catch (Exception Ex)
            //{
            //    Response.Clear();
            //    Response.Write("<html><head></head><body><div id=\"div_erro\" style=\"color:#990000; width:500px; margin:auto; text-align:center;\">" + util.BRLight.Excecao.LerInnerException(Ex, true) + "<br/><br/>Nossa equipe resolverá o problema, você pode tentar mais tarde ou entrar em contato conosco.</div></body><html>");
            //}
		}
	}
}
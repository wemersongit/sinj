﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TCDF.Sinj;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;
using util.BRLight;

namespace SINJ_Atualiza_VacatioLegis.RN
{
    public class VacatioLegisNormaRN
    {
        private NormaRN _normaRn;
        private TipoDeRelacaoRN _tipoDeRelacaoRn;
        private UtilArquivoHtml _utilArquivoHtml;

        public VacatioLegisNormaRN(NormaRN normaRn, TipoDeRelacaoRN tipoDeRelacaoRn, UtilArquivoHtml utilArquivoHtml)
        {
            _normaRn = normaRn;
            _tipoDeRelacaoRn = tipoDeRelacaoRn;
            _utilArquivoHtml = utilArquivoHtml;
        }

        public void SalvarTextoAntigo(NormaOV norma, Vide vide)
        {
            if (vide.alteracao_texto_vide != null)
            {
                //Se possuir dispositivo na norma alterada
                if (!vide.alteracao_texto_vide.in_sem_arquivo && vide.alteracao_texto_vide.dispositivos_norma_vide != null)
                {
                    if (vide.in_norma_afetada)
                    {
                        if (vide.alteracao_texto_vide.dispositivos_norma_vide.Any() || UtilVides.EhInformacional(vide.ds_texto_relacao) || UtilVides.EhAlteracaoCompleta(norma.nm_situacao, vide.ds_texto_relacao))
                        {
                            _normaRn.SalvarTextoAntigoDaNorma(norma, vide, "vacatio_legis");
                        }
                    }
                    else
                    {
                        if(vide.alteracao_texto_vide.dispositivos_norma_vide.Any() || UtilVides.EhInformacional(vide.ds_texto_relacao))
                        {
                            _normaRn.SalvarTextoAntigoDaNorma(norma, vide, "vacatio_legis");
                        }
                    }
                }
            }
        }

        public void AlterarTextoDaNormaAlteradora(NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlterador)
        {
            DispositivoVide dispositivoAlterador;
            var doc = new HtmlDocument();
            var idFile = "";
            var texto = "";

            var linkNormaAlterada = CriarLinkDaNorma(normaAlterada, videAlterador);


            if (videAlterador.alteracao_texto_vide != null)
            {
                idFile = normaAlteradora.getIdFileArquivoVigente();
                texto = _utilArquivoHtml.GetHtmlFile(idFile, "sinj_norma", null);
                //converte o texto em HtmlDocument
                doc.LoadHtml(texto);
                //se o vide alterador possui dispositivo então o link vai ser atribuído ao texto do dispositivo
                if (videAlterador.alteracao_texto_vide.dispositivos_norma_vide.Any())
                {
                    dispositivoAlterador = videAlterador.alteracao_texto_vide.dispositivos_norma_vide.FirstOrDefault();

                    var node = doc.DocumentNode.Descendants("p").Where(p => p.GetAttributeValue("linkname", "").Equals(dispositivoAlterador.linkname)).FirstOrDefault();
                    if (!string.IsNullOrEmpty(node.InnerHtml))
                    {
                        node.InnerHtml = node.InnerHtml.Replace(dispositivoAlterador.texto, "<a href=\"" + linkNormaAlterada + ">" + dispositivoAlterador.texto + "</a>");
                    }
                    SaveFile(doc.DocumentNode.InnerHtml, normaAlteradora);
                }
                //ou se o o vide for do tipo LECO ele pode não ter dispositivo e, nesse caso, o link vai antes da epigrafe
                else if (UtilVides.EhLegislacaoCorrelata(videAlterador.ds_texto_relacao))
                {
                    var node = doc.DocumentNode.Descendants("h1").Where(h1 => !string.IsNullOrEmpty(h1.GetAttributeValue("epigrafe", ""))).FirstOrDefault();
                    doc.DocumentNode.InsertBefore(
                            HtmlNode.CreateNode("<p ch_norma_info=\"" + normaAlterada.ch_norma + "\"><a href=\"" + linkNormaAlterada + "\" >" + videAlterador.ds_texto_relacao + " - " + normaAlterada.getDescricaoDaNorma() + "</a></p>"),
                            node
                        );
                    SaveFile(doc.DocumentNode.InnerHtml, normaAlteradora);
                }
                
            }
                
        }

        public void AlterarTextoDaNormaAlterada(NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlterado)
        {
            if (videAlterado.alteracao_texto_vide != null)
            {
                var doc = new HtmlDocument();
                var idFile = normaAlteradora.getIdFileArquivoVigente();
                var texto = _utilArquivoHtml.GetHtmlFile(idFile, "sinj_norma", null);
                //converte o texto em HtmlDocument
                doc.LoadHtml(texto);

                if (UtilVides.EhAlteracaoCompleta(normaAlterada.nm_situacao, videAlterado.ds_texto_relacao))
                {
                    if (videAlterado.alteracao_texto_vide == null || videAlterado.alteracao_texto_vide.dispositivos_norma_vide == null || !videAlterado.alteracao_texto_vide.dispositivos_norma_vide.Any())
                    {
                        AlterarTextoCompletoDaNormaAlterada(doc, normaAlteradora, normaAlterada, videAlterado);
                    }
                    else
                    {
                        AlterarDispositivoNoTextoDaNormaAlterada(doc, normaAlteradora, normaAlterada, videAlterado);
                    }
                }
                else if (UtilVides.DesfazAlteracaoCompleta(videAlterado.ds_texto_relacao))
                {
                    if (videAlterado.alteracao_texto_vide == null || videAlterado.alteracao_texto_vide.dispositivos_norma_vide == null || !videAlterado.alteracao_texto_vide.dispositivos_norma_vide.Any())
                    {
                        DesfazerAlteracaoNoTextoCompletoDaNormaAlterada(doc, normaAlteradora, normaAlterada, videAlterado);
                    }
                    else
                    {
                        DesfazerAlteracaoNoDispositivoDoTextoDaNormaAlterada(doc, normaAlteradora, normaAlterada, videAlterado);
                    }
                }
                else if (UtilVides.EhInformacional(videAlterado.ds_texto_relacao))
                {
                    AcrescentarInformacaoNoTextoDaNormaAlterada(doc, normaAlteradora, normaAlterada, videAlterado);
                }
                else if (UtilVides.EhLegislacaoCorrelata(videAlterado.ds_texto_relacao))
                {
                    AcrescentarLecoNoTextoDaNormaAlterada(doc, normaAlteradora, normaAlterada, videAlterado);
                }
                else if (UtilVides.EhRenumeracao(videAlterado.ds_texto_relacao))
                {
                    RenumerarDispositivoNoTextoDaNormaAlterada(doc, normaAlteradora, normaAlterada, videAlterado);
                }
                else if (UtilVides.EhAcrescimo(videAlterado.ds_texto_relacao))
                {
                    AcrescentarDispositivoNoTextoDaNormaAlterada(doc, normaAlteradora, normaAlterada, videAlterado);
                }
                else
                {
                    AlterarDispositivoNoTextoDaNormaAlterada(doc, normaAlteradora, normaAlterada, videAlterado);
                }
                SaveFile(doc.DocumentNode.InnerHtml, normaAlterada);
            }
        }

        private void AlterarTextoCompletoDaNormaAlterada(HtmlDocument doc, NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlterado)
        {
            var linkNormaAlteradora = CriarLinkDaNorma(normaAlteradora, videAlterado);
            var newNode = HtmlNode.CreateNode("<p ch_norma_alteracao_completa=\"" + normaAlteradora.ch_norma + "\" style=\"text-align:center;\" ch_tipo_relacao=\"" + videAlterado.ch_tipo_relacao + "\" ><a href=\"" + linkNormaAlteradora + "\" >" + videAlterado.ds_texto_relacao + " pelo(a) " + normaAlteradora.getDescricaoDaNorma() + "</a></p>");
            var chNodesNormaAlteracaoCompleta = doc.DocumentNode.Descendants("p").Where(p => !string.IsNullOrEmpty(p.GetAttributeValue("ch_norma_alteracao_completa", "")));
            var pNodes = doc.DocumentNode.Descendants("p").Where(p => !string.IsNullOrEmpty(p.GetAttributeValue("linkname", ""))).ToList();
            HtmlNode insertAfterNode;
            if (chNodesNormaAlteracaoCompleta.Any())
            {
                insertAfterNode = chNodesNormaAlteracaoCompleta.Last();
            }
            else
            {
                insertAfterNode = doc.DocumentNode.Descendants("h1").Where(h1 => !string.IsNullOrEmpty(h1.GetAttributeValue("epigrafe", ""))).FirstOrDefault();
            }
            doc.DocumentNode.InsertAfter(
                newNode,
                insertAfterNode
            );

            if(!pNodes.FirstOrDefault().ParentNode.Name.Equals("s", StringComparison.InvariantCultureIgnoreCase))
            {
                var sNode = HtmlNode.CreateNode("<s></s>");
                doc.DocumentNode.InsertAfter(
                    sNode,
                    newNode
                );
                foreach (var pNode in pNodes)
                {
                    pNode.Remove();
                    sNode.AppendChild(pNode);
                }
            }
            SaveFile(doc.DocumentNode.InnerHtml, normaAlterada);

        }

        private void DesfazerAlteracaoNoTextoCompletoDaNormaAlterada(HtmlDocument doc, NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlterado)
        {

            var linkNormaAlteradora = CriarLinkDaNorma(normaAlteradora, videAlterado);
            var newNode = HtmlNode.CreateNode("<p ch_norma_alteracao_completa=\"" + normaAlteradora.ch_norma + "\" style=\"text-align:center;\" ch_tipo_relacao=\"" + videAlterado.ch_tipo_relacao + "\" ><a href=\"" + linkNormaAlteradora + "\" >" + videAlterado.ds_texto_relacao + " pelo(a) " + normaAlteradora.getDescricaoDaNorma() + "</a></p>");
            var chNodesNormaAlteracaoCompleta = doc.DocumentNode.Descendants("p").Where(p => !string.IsNullOrEmpty(p.GetAttributeValue("ch_norma_alteracao_completa", "")));
            var sNode = doc.DocumentNode.SelectSingleNode("s");
                //.Descendants("p").Where(p => !string.IsNullOrEmpty(p.GetAttributeValue("linkname", ""))).ToList();
            HtmlNode insertAfterNode;
            if (chNodesNormaAlteracaoCompleta.Any())
            {
                insertAfterNode = chNodesNormaAlteracaoCompleta.Last();
            }
            else
            {
                insertAfterNode = doc.DocumentNode.Descendants("h1").Where(h1 => !string.IsNullOrEmpty(h1.GetAttributeValue("epigrafe", ""))).FirstOrDefault();
            }
            doc.DocumentNode.InsertAfter(
                newNode,
                insertAfterNode
            );

            if (sNode != null)
            {
                sNode.Remove();
                foreach (var pNode in sNode.ChildNodes)
                {
                    doc.DocumentNode.InsertAfter(
                        pNode,
                        newNode
                    );
                }
            }
            SaveFile(doc.DocumentNode.InnerHtml, normaAlterada);
        }

        private void AcrescentarInformacaoNoTextoDaNormaAlterada(HtmlDocument doc, NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlterado)
        {
            var linkNormaAlteradora = CriarLinkDaNorma(normaAlteradora, videAlterado);
            var newNode = HtmlNode.CreateNode("<p ch_norma_info=\"" + normaAlteradora.ch_norma + "\" style=\"text-align:center;\"><a href=\"" + linkNormaAlteradora + "\" >" + videAlterado.ds_texto_relacao + " pelo(a) " + normaAlteradora.getDescricaoDaNorma() + "</a></p>");

            HtmlNode insertAfterNode = doc.DocumentNode.Descendants("h1").Where(h1 => !string.IsNullOrEmpty(h1.GetAttributeValue("epigrafe", ""))).FirstOrDefault();

            doc.DocumentNode.InsertAfter(
                newNode,
                insertAfterNode
            );

            SaveFile(doc.DocumentNode.InnerHtml, normaAlterada);
        }

        private void AcrescentarLecoNoTextoDaNormaAlterada(HtmlDocument doc, NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlterado)
        {
            var linkNormaAlteradora = CriarLinkDaNorma(normaAlteradora, videAlterado);
            var newNode = HtmlNode.CreateNode("<p ch_norma_info=\"" + normaAlteradora.ch_norma + "\" ><a href=\"" + linkNormaAlteradora + "\" >" + videAlterado.ds_texto_relacao + " - " + normaAlteradora.getDescricaoDaNorma() + "</a></p>");

            HtmlNode insertAfterNode = doc.DocumentNode.Descendants("h1").Where(h1 => !string.IsNullOrEmpty(h1.GetAttributeValue("epigrafe", ""))).FirstOrDefault();

            doc.DocumentNode.InsertBefore(
                newNode,
                insertAfterNode
            );

            SaveFile(doc.DocumentNode.InnerHtml, normaAlterada);
        }

        private void RenumerarDispositivoNoTextoDaNormaAlterada(HtmlDocument doc, NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlterado)
        {
            var tipoRelacao = _tipoDeRelacaoRn.Doc(videAlterado.ch_tipo_relacao);
            var linkNormaAlteradora = CriarLinkDaNorma(normaAlteradora, videAlterado);
            HtmlNode node;
            string linkname = "";
            foreach (var dispositivo in videAlterado.alteracao_texto_vide.dispositivos_norma_vide)
            {
                linkname = Regex.Replace(dispositivo.linkname, "_renum$", "");
                node = doc.DocumentNode.Descendants("p").Where(p => p.GetAttributeValue("linkname", "").Equals(linkname)).FirstOrDefault();
                if (node != null)
                {
                    node.InnerHtml = node.InnerHtml.Replace(dispositivo.texto_antigo, dispositivo.texto);
                    node.SetAttributeValue("name", dispositivo.linkname);
                    node.Descendants("a").Where(a => a.GetAttributeValue("name", "").Equals(linkname)).FirstOrDefault().SetAttributeValue("name", dispositivo.linkname);
                    node.AppendChild(HtmlNode.CreateNode("<a href=\"" + linkNormaAlteradora + "\">(" + (!string.IsNullOrEmpty(dispositivo.nm_linkname) ? dispositivo.nm_linkname + " " : "") + tipoRelacao.ds_texto_para_alterador + " pelo(a) " + normaAlteradora.getDescricaoDaNorma() + ")</a>"));
                }
            }

            SaveFile(doc.DocumentNode.InnerHtml, normaAlterada);
        }

        private void AcrescentarDispositivoNoTextoDaNormaAlterada(HtmlDocument doc, NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlterado)
        {
            var tipoRelacao = _tipoDeRelacaoRn.Doc(videAlterado.ch_tipo_relacao);
            var linkNormaAlteradora = CriarLinkDaNorma(normaAlteradora, videAlterado);
            string linkname = Regex.Replace(videAlterado.alteracao_texto_vide.dispositivos_norma_vide.FirstOrDefault().linkname, "_[a-z0-9]{4}_add$", "");
            HtmlNode node = doc.DocumentNode.Descendants("p").Where(p => p.GetAttributeValue("linkname", "").Equals(linkname)).FirstOrDefault();
            foreach (var dispositivo in videAlterado.alteracao_texto_vide.dispositivos_norma_vide)
            {
                node = doc.DocumentNode.InsertAfter(
                        HtmlNode.CreateNode("<p linkname=\"" + linkNormaAlteradora + "\">" + dispositivo.texto + " <a href=\"" + linkNormaAlteradora + "\">(" + (!string.IsNullOrEmpty(dispositivo.nm_linkname) ? dispositivo.nm_linkname + " " : "") + tipoRelacao.ds_texto_para_alterador + " pelo(a) " + normaAlteradora.getDescricaoDaNorma() + ")</a></p>"),
                        node
                    );
            }
            SaveFile(doc.DocumentNode.InnerHtml, normaAlterada);
        }

        private void AlterarDispositivoNoTextoDaNormaAlterada(HtmlDocument doc, NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlterado)
        {
            var tipoRelacao = _tipoDeRelacaoRn.Doc(videAlterado.ch_tipo_relacao);
            var linkNormaAlteradora = CriarLinkDaNorma(normaAlteradora, videAlterado);
            HtmlNode pNode, aNode;
            string linkname;
            int countReplaceds;
            foreach (var dispositivo in videAlterado.alteracao_texto_vide.dispositivos_norma_vide)
            {
                if (string.IsNullOrEmpty(dispositivo.texto))
                {
                    linkname = Regex.Replace(dispositivo.linkname, "(_replaced)*$", "");
                    pNode = doc.DocumentNode.Descendants("p").Where(p => p.GetAttributeValue("linkname", "").Equals(linkname)).FirstOrDefault();
                    aNode = pNode.Descendants("a").Where(a => a.GetAttributeValue("name", "").Equals(linkname)).FirstOrDefault();

                    pNode.SetAttributeValue("linkname", dispositivo.linkname);
                    pNode.SetAttributeValue("replaced_by", normaAlteradora.ch_norma);
                    pNode.Attributes.Remove("undone_by");
                    aNode.SetAttributeValue("name", dispositivo.linkname);
                    aNode.SetAttributeValue("id", dispositivo.linkname);

                    if (pNode.SelectSingleNode("s") == null)
                    {
                        RiscarTextoDoDispositivo(pNode);
                    }

                    pNode.AppendChild(HtmlNode.CreateNode("&nbsp;<a href=\"" + linkNormaAlteradora + "\">(" + (!string.IsNullOrEmpty(dispositivo.nm_linkname) ? dispositivo.nm_linkname + " " : "") + tipoRelacao.ds_texto_para_alterador + " pelo(a) " + normaAlteradora.getDescricaoDaNorma() + ")</a>"));
                }
                else
                {
                    linkname = dispositivo.linkname;
                    pNode = doc.DocumentNode.Descendants("p").Where(p => p.GetAttributeValue("linkname", "").Equals(linkname)).FirstOrDefault();
                    aNode = pNode.Descendants("a").Where(a => a.GetAttributeValue("name", "").Equals(linkname)).FirstOrDefault();
                    countReplaceds = doc.DocumentNode.Descendants("p").Where(p => Regex.Replace(p.GetAttributeValue("linkname", ""), "(_replaced)*$", "").Equals(linkname)).Count();
                    for (var i = 0; i < countReplaceds; i++)
                    {
                        linkname += "_replaced";
                    }
                    linkname += "_replaced";
                    pNode.SetAttributeValue("linkname", linkname);
                    pNode.SetAttributeValue("replaced_by", normaAlteradora.ch_norma);
                    aNode.SetAttributeValue("name", linkname);
                    aNode.SetAttributeValue("id", linkname);

                    RiscarTextoDoDispositivo(pNode);

                    doc.DocumentNode.InsertAfter(
                        HtmlNode.CreateNode("<p linkname=\"" + dispositivo.linkname + "\">" + dispositivo.texto + " <a href=\"" + dispositivo.linkname + "\">(" + (!string.IsNullOrEmpty(dispositivo.nm_linkname) ? dispositivo.nm_linkname + " " : "") + tipoRelacao.ds_texto_para_alterador + " pelo(a) " + normaAlteradora.getDescricaoDaNorma() + ")</a></p>"),
                        pNode
                    );
                }
            }

            SaveFile(doc.DocumentNode.InnerHtml, normaAlterada);
        }

        private void DesfazerAlteracaoNoDispositivoDoTextoDaNormaAlterada(HtmlDocument doc, NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlterado)
        {
            var tipoRelacao = _tipoDeRelacaoRn.Doc(videAlterado.ch_tipo_relacao);
            var linkNormaAlteradora = CriarLinkDaNorma(normaAlteradora, videAlterado);
            HtmlNode pNode, aNode;
            string linkname;
            foreach (var dispositivo in videAlterado.alteracao_texto_vide.dispositivos_norma_vide)
            {
                linkname = Regex.Replace(dispositivo.linkname, "(_undone)*$", "");
                pNode = doc.DocumentNode.Descendants("p").Where(p => p.GetAttributeValue("linkname", "").Equals(linkname)).FirstOrDefault();
                aNode = pNode.Descendants("a").Where(a => a.GetAttributeValue("name", "").Equals(linkname)).FirstOrDefault();

                pNode.SetAttributeValue("linkname", dispositivo.linkname);
                pNode.SetAttributeValue("undone_by", normaAlteradora.ch_norma);
                pNode.Attributes.Remove("replaced_by");
                aNode.SetAttributeValue("name", dispositivo.linkname);
                aNode.SetAttributeValue("id", dispositivo.linkname);

                DesfazerTextoRiscadoDoDispositivo(pNode);

                pNode.AppendChild(HtmlNode.CreateNode("&nbsp;<a href=\"" + linkNormaAlteradora + "\">(" + (!string.IsNullOrEmpty(dispositivo.nm_linkname) ? dispositivo.nm_linkname + " " : "") + tipoRelacao.ds_texto_para_alterador + " pelo(a) " + normaAlteradora.getDescricaoDaNorma() + ")</a>"));

            }

            SaveFile(doc.DocumentNode.InnerHtml, normaAlterada);
        }

        private void RiscarTextoDoDispositivo(HtmlNode pNode)
        {
            var pNodeAux = HtmlNode.CreateNode("<p></p>");
            foreach (var node in pNode.ChildNodes)
            {
                if (node.NodeType.Equals(HtmlNodeType.Text))
                {
                    pNodeAux.AppendChild(HtmlNode.CreateNode("<s>" + node.OuterHtml + "</s>"));
                }
                else
                {
                    pNodeAux.AppendChild(node);
                }
            }
            pNode.RemoveAllChildren();
            pNode.AppendChildren(pNodeAux.ChildNodes);
        }

        private void DesfazerTextoRiscadoDoDispositivo(HtmlNode pNode)
        {
            var pNodeAux = HtmlNode.CreateNode("<p></p>");
            foreach (var node in pNode.ChildNodes)
            {
                if (node.Name.Equals("s"))
                {
                    pNodeAux.AppendChild(HtmlNode.CreateNode(node.InnerHtml));
                }
                else
                {
                    pNodeAux.AppendChild(node);
                }
            }
            pNode.RemoveAllChildren();
            pNode.AppendChildren(pNodeAux.ChildNodes);
        }

        private void SaveFile(string texto, NormaOV norma)
        {
            var fileNorma = _utilArquivoHtml.AnexarHtml(texto, norma.getNameFileArquivoVigente(), "sinj_norma");
            if (fileNorma.IndexOf("id_file") > -1)
            {
                _normaRn.PathPut(norma._metadata.id_doc, "ar_atualizado", fileNorma, null);
            }
        }

        private string CriarLinkDaNorma(NormaOV norma, Vide vide)
        {
            var fileNameNormaAlteradora = norma.getNameFileArquivoVigente();
            //link da norma alterada que vai no texto da norma alteradora
            var link = "(_link_sistema_)DetalhesDeNorma.aspx?id_norma=" + norma.ch_norma;
            //se a norma alterada possui arquivo o link deve ser o do arquivo
            if (!string.IsNullOrEmpty(fileNameNormaAlteradora))
            {
                link += "/" + fileNameNormaAlteradora;
                //se o vide possuir dispositivo o link deve receber a ancora do primeiro
                if (vide.alteracao_texto_vide.dispositivos_norma_vide_outra.Any())
                {
                    link += "#" + vide.alteracao_texto_vide.dispositivos_norma_vide_outra.FirstOrDefault().linkname;
                }
            }
            return link;
        }
    }
}

using HtmlAgilityPack;
using System;
using System.Linq;
using TCDF.Sinj;
using TCDF.Sinj.OV;
using TCDF.Sinj.RN;

namespace SINJ_Atualiza_VacatioLegis.RN
{
    public class VacatioLegisNormaRN
    {
        private NormaRN _normaRn;
        private UtilArquivoHtml _utilArquivoHtml;

        public VacatioLegisNormaRN(NormaRN normaRn, UtilArquivoHtml utilArquivoHtml)
        {
            _normaRn = normaRn;
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
            var fileNameNormaAlterada = normaAlterada.getNameFileArquivoVigente();
            //link da norma alterada que vai no texto da norma alteradora
            var linkNormaAlterada = "(_link_sistema_)DetalhesDeNorma.aspx?id_norma=" + normaAlterada.ch_norma;
            //se a norma alterada possui arquivo o link deve ser o do arquivo
            if (!string.IsNullOrEmpty(fileNameNormaAlterada))
            {
                linkNormaAlterada += "/" + fileNameNormaAlterada;
                //se o vide possuir dispositivo o link deve receber a ancora do primeiro
                if (videAlterador.alteracao_texto_vide.dispositivos_norma_vide_outra.Any())
                {
                    linkNormaAlterada +=  "#" + videAlterador.alteracao_texto_vide.dispositivos_norma_vide_outra.FirstOrDefault().linkname;
                }
            }

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
                }
                //ou se o o vide for do tipo LECO ele pode não ter dispositivo e, nesse caso, o link vai antes da epigrafe
                else if (UtilVides.EhLegislacaoCorrelata(videAlterador.ds_texto_relacao))
                {
                    var node = doc.DocumentNode.Descendants("h1").Where(h1 => !string.IsNullOrEmpty(h1.GetAttributeValue("epigrafe", ""))).FirstOrDefault();
                    doc.DocumentNode.InsertBefore(
                            HtmlNode.CreateNode("<p ch_norma_info=\"" + normaAlterada.ch_norma + "\"><a href=\"" + linkNormaAlterada + "\" >" + videAlterador.ds_texto_relacao + " - " + normaAlterada.getDescricaoDaNorma() + "</a></p>"),
                            node
                        );

                    //var nodes = doc.DocumentNode.Descendants("p").Where(p => !string.IsNullOrEmpty(p.GetAttributeValue("ch_norma_info", "")));
                    //if (nodes.Any())
                    //{
                    //    doc.DocumentNode.InsertBefore(
                    //            newNode,
                    //            nodes.Last()
                    //        );
                    //}
                    //else
                    //{
                    //    var node = doc.DocumentNode.SelectSingleNode("//h1");
                    //    doc.DocumentNode.InsertAfter(
                    //            newNode,
                    //            node
                    //        );
                    //}
                    var teste = doc.DocumentNode.InnerHtml;
                    Console.WriteLine(teste);
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

                }
                else if (UtilVides.DesfazAlteracaoCompleta(videAlterado.ds_texto_relacao))
                {

                }
                else if (UtilVides.EhInformacional(videAlterado.ds_texto_relacao))
                {

                }
                else if (UtilVides.EhLegislacaoCorrelata(videAlterado.ds_texto_relacao))
                {

                }
                else if (UtilVides.EhRenumeracao(videAlterado.ds_texto_relacao))
                {

                }
                else if (UtilVides.EhAcrescimo(videAlterado.ds_texto_relacao))
                {

                }
                else
                {

                }
            }
        }

        private void AlterarTextoCompletoDaNormaAlterada(HtmlDocument doc, NormaOV normaAlteradora, NormaOV normaAlterada, Vide videAlterado)
        {
            var fileNameNormaAlteradora = normaAlteradora.getNameFileArquivoVigente();
            //link da norma alterada que vai no texto da norma alteradora
            var linkNormaAlteradora = "(_link_sistema_)DetalhesDeNorma.aspx?id_norma=" + normaAlteradora.ch_norma;
            //se a norma alterada possui arquivo o link deve ser o do arquivo
            if (!string.IsNullOrEmpty(fileNameNormaAlteradora))
            {
                linkNormaAlteradora += "/" + fileNameNormaAlteradora;
                //se o vide possuir dispositivo o link deve receber a ancora do primeiro
                if (videAlterado.alteracao_texto_vide.dispositivos_norma_vide_outra.Any())
                {
                    linkNormaAlteradora += "#" + videAlterado.alteracao_texto_vide.dispositivos_norma_vide_outra.FirstOrDefault().linkname;
                }
            }
            var newNode = HtmlNode.CreateNode("<p ch_norma_alteracao_completa=\"" + normaAlteradora.ch_norma + "\" style=\"text-align:center;\" ch_tipo_relacao=\"" + videAlterado.ch_tipo_relacao + "\" ><a href=\"" + linkNormaAlteradora + "\" >" + videAlterado.ds_texto_relacao + " - " + normaAlteradora.getDescricaoDaNorma() + "</a></p>");
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

        }
    }
}

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
            var linkNormaAlterada = "(_link_sistema_)Norma/" + videAlterador.ch_norma_vide;
            var fileNameNormaAlterada = normaAlterada.getNameFileArquivoVigente();
            if (!string.IsNullOrEmpty(fileNameNormaAlterada))
            {
                linkNormaAlterada += "/" + fileNameNormaAlterada;
                if (videAlterador.alteracao_texto_vide.dispositivos_norma_vide_outra.Any())
                {
                    linkNormaAlterada +=  "#" + videAlterador.alteracao_texto_vide.dispositivos_norma_vide_outra.FirstOrDefault().linkname;
                }
            }

            if (videAlterador.alteracao_texto_vide != null)
            {
                idFile = normaAlteradora.getIdFileArquivoVigente();
                texto = _utilArquivoHtml.GetHtmlFile(idFile, "sinj_norma", null);
                doc.LoadHtml(texto);
                if (videAlterador.alteracao_texto_vide.dispositivos_norma_vide.Any())
                {
                    dispositivoAlterador = videAlterador.alteracao_texto_vide.dispositivos_norma_vide.FirstOrDefault();

                    var node = doc.DocumentNode.Descendants("p").Where(p => p.GetAttributeValue("linkname", "").Equals(dispositivoAlterador.linkname)).FirstOrDefault();
                    if (!string.IsNullOrEmpty(node.InnerHtml))
                    {
                        node.InnerHtml = node.InnerHtml.Replace(dispositivoAlterador.texto, "<a href=\"" + linkNormaAlterada + ">" + dispositivoAlterador.texto + "</a>");
                    }
                }
                else if (UtilVides.EhLegislacaoCorrelata(videAlterador.ds_texto_relacao))
                {
                    var nodes = doc.DocumentNode.Descendants("p").Where(p => !string.IsNullOrEmpty(p.GetAttributeValue("ch_norma_info", "")));
                    var newNode = HtmlNode.CreateNode("<p ch_norma_info=\"" + normaAlterada.ch_norma + "\"><a href=\"" + linkNormaAlterada + "\" >" + videAlterador.ds_texto_relacao + " - " + normaAlterada.getDescricaoDaNorma() + "</a></p>");
                    if (nodes.Any())
                    {
                        doc.DocumentNode.InsertBefore(
                                newNode,
                                nodes.Last()
                            );
                    }
                    else
                    {
                        var node = doc.DocumentNode.SelectSingleNode("//h1");
                        doc.DocumentNode.InsertAfter(
                                newNode,
                                node
                            );
                    }
                    var teste = doc.DocumentNode.InnerHtml;
                    Console.WriteLine(teste);
                }
            }
                
        }
    }
}

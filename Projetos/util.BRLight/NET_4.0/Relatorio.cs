using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

namespace util.BRLight
{

    public class Relatorio
    {
        #region atributos do PDF
        public string tituloRelatorio { get; set; }
        public string cabecalho { get; set; }
        public enum tipo
        {
            Paisagem = 1,
            Retrato = 2
        }

        public tipo tipoRelatorio { get; set; }

        public static string sCaminhoFisicoPdfCompleto = "";
        public static string sImgCaminhoRelativo = "";
        public static string sCaminhoFisicoIMG = "";

        public static Font fontTotalRed()
        {
            return FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.BOLD, BaseColor.RED);
        }
        public Font fontBranca()
        {
            return FontFactory.GetFont(FontFactory.HELVETICA, 13, Font.BOLD, BaseColor.WHITE);
        }
        public static Font fontTituloPrimeiraPaginaBlue()
        {
            return FontFactory.GetFont(FontFactory.HELVETICA, 20, Font.BOLD, BaseColor.BLUE.Darker());
        }
        public static Font fontSubTituloPrimeiraPaginaBlue()
        {
            return FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.BLUE.Darker());
        }
        public static Font fontDataPrimeiraPaginaBlue()
        {
            return FontFactory.GetFont(FontFactory.HELVETICA, 9, Font.BOLD, BaseColor.BLUE.Darker());
        }
        public static Font fontTitulo()
        {
            return FontFactory.GetFont(FontFactory.HELVETICA, 14, Font.BOLD, BaseColor.BLACK);
        }
        public static Font fontSubTituloBlue()
        {
            return FontFactory.GetFont(FontFactory.HELVETICA, 12, Font.BOLD, BaseColor.BLACK);
        }
        public static Font fontInternaHeader()
        {
            return FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.BOLD, BaseColor.BLACK);
        }
        public static Font fontInterna()
        {
            return FontFactory.GetFont(FontFactory.HELVETICA, 10, Font.NORMAL, BaseColor.BLACK);
        }
        public static Font fontGlossario()
        {
            return FontFactory.GetFont(FontFactory.HELVETICA, 8, Font.NORMAL, BaseColor.BLACK);
        }
        #endregion 

        public Relatorio(){}
        public Relatorio(string _titulo, string _cabecalho)
        {
            
            tituloRelatorio = _titulo;
            cabecalho = _cabecalho;
        }
        /// <summary>
        /// Gera o relatório passando alguns parametros.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listaReg"></param>
        /// <param name="campos"></param>
        /// <param name="headerwidths"></param>
        /// <param name="urlImage"></param>
        /// <returns></returns>
        public byte[] GerarRelatorio<T>(List<T> listaReg, Dictionary<string, string> campos, float[] headerwidths, string urlImage)
        {
            try
            {
                // preenche ov com os dados do formulário \\

                if (listaReg.Count > 0) // verifica se existe resultado da pesquisa \\
                {
                    PdfPTable tabelas = new PdfPTable(1);
                    tabelas = GeraTabela(listaReg, campos, headerwidths);
                    var listaTabelas = new List<PdfPTable>();
                    listaTabelas.Add(tabelas);
                    tipoRelatorio = tipo.Paisagem;
                    return GeraRelatorioPDF(listaTabelas, urlImage,listaReg.Count);
                }
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("Não foi possível Gerar o Relatório", ex);
            }
            return null;
        }

        /// <summary>
        /// Gera as tabelas do relatório
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listaRegistro"></param>
        /// <param name="campos"></param>
        /// <param name="headerwidths"></param>
        /// <returns></returns>
        public PdfPTable GeraTabela<T>(List<T> listaRegistro, Dictionary<string, string> campos, float[] headerwidths)
        {
            var table = new PdfPTable(headerwidths.Length);

            table.WidthPercentage = 100;

            table.SetWidths(headerwidths);
            int count = 0;

            // Cria o título das coluna de acordo com o dicionario que foi passado
            foreach (var nomeCampo in campos.Values)
            {
                table.AddCell(newCell(nomeCampo, fontBranca(), -1, Element.ALIGN_CENTER, Element.ALIGN_MIDDLE, headerwidths[count], false, ""));
                count++;
            }
            // Cria o corpo da tabela com o resultado da pesquisa. Obs: A sequencia das coluna é de acordo com a sequencia do dicionario;
            foreach (var registro in listaRegistro)
            {
                foreach (var camposs in campos.Keys)
                {
                    foreach (var propertyInfo in registro.GetType().GetProperties())
                    {
                        var campo_chil = camposs.Split('.');
                        if (campo_chil[0] == propertyInfo.Name)
                        {
                            object valorObj;
                            if (!propertyInfo.PropertyType.Name.Contains("List"))
                            {
                                Paragraph paragrafo = new Paragraph("");
                                valorObj = propertyInfo.GetValue(registro, null);
                                if (valorObj != null)
                                {
                                    paragrafo = new Paragraph(valorObj.ToString());
                                }
                                else
                                {
                                    paragrafo = new Paragraph("  ");
                                }
                                var cell = new PdfPCell(paragrafo);
                                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                table.AddCell(cell);
                            }
                            else
                            {
                                //Essa parte cuida dos campos MultValorados.
                                try
                                {
                                    //Pega o Tipo do campo.
                                    Type tipoObj = propertyInfo.GetType();

                                    //Cria uma List do tipo do campo;
                                    IList listaObj = (IList)Activator.CreateInstance((typeof(List<>).MakeGenericType(tipoObj)));
                                    Paragraph paragrafo = new Paragraph();
                                    valorObj = propertyInfo.GetValue(registro, null);
                                    listaObj = valorObj as IList;
                                    int index = 0;

                                    #region Trata os campos Multivalorados
                                    foreach (var campo_reg in listaObj)
                                    {
                                        foreach (var campo_regs_prop in campo_reg.GetType().GetProperties())
                                        {
                                            foreach (var campo_children in campo_chil)
                                            {
                                                if (campo_children == campo_regs_prop.Name)
                                                {
                                                   var valor = campo_regs_prop.GetValue(campo_reg, null);
                                                   if (campo_chil.Length > 2)
                                                   {
                                                       //Pega a ultima repetição do multvalorado
                                                       if (campo_chil[2] == "last")
                                                       {
                                                           if (index == listaObj.Count - 1)
                                                           {
                                                               if (valor != null)
                                                               {
                                                                   paragrafo.Add(new Phrase(valor.ToString()));
                                                               }
                                                               else
                                                               {
                                                                   paragrafo.Add(new Phrase("  "));
                                                               }
                                                               paragrafo.Add(new Phrase(Environment.NewLine));
                                                           }
                                                       }
                                                       //Pega a primeira repetição do multvalorado
                                                       else if (campo_chil[2] == "first")
                                                       {
                                                           if(index == 0)
                                                           {
                                                               if (valor != null)
                                                               {
                                                                   paragrafo.Add(new Phrase(valor.ToString()));
                                                               }
                                                               else
                                                               {
                                                                   paragrafo.Add(new Phrase("  "));
                                                               }
                                                               paragrafo.Add(new Phrase(Environment.NewLine));
                                                           }
                                                       }
                                                   }
                                                   else
                                                   {
                                                       if (valor != null)
                                                       {
                                                           paragrafo.Add(new Phrase(valor.ToString()));
                                                       }
                                                       else
                                                       {
                                                           paragrafo.Add(new Phrase("  "));
                                                       }
                                                       paragrafo.Add(new Phrase(Environment.NewLine));
                                                   }
                                                    index++;
                                                }
                                            }

                                        }
                                    }
                                    #endregion

                                    if (listaObj.Count == 0)
                                    {
                                        paragrafo.Add(new Phrase("   "));
                                    }

                                    var cell = new PdfPCell(paragrafo);
                                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                                    table.AddCell(cell);
                                }catch(Exception ex)
                                {
                                    throw new FalhaOperacaoException("Não foi possível gerar a tabela do relatório", ex);
                                }
                            }
                            
                        }

                    }
                }
            }
            return table;
        }

        public PdfPCell newCell(string sTexto, Font fFont, int iColsspan, int iHorizontalAlignment, int iVerticalAlignment, float headerwidth, bool bAddGlossario, string sTema)
        {
            try
            {

                if (string.IsNullOrEmpty(sTexto))
                {
                    sTexto = "Campo vazio";
                }
                //var cell = new Cell { Border = 0 }; 
                var cell = new PdfPCell();
                cell.HorizontalAlignment = 1;
                if (iColsspan > 0) cell.Colspan = iColsspan;
                if (iVerticalAlignment != -1) cell.VerticalAlignment = iVerticalAlignment;
                cell.BackgroundColor = BaseColor.GRAY;

                Paragraph pTexto;
                if (bAddGlossario)
                {
                    pTexto = new Paragraph(new Chunk(sTexto, fFont).SetGenericTag(sTema + " -- " + sTexto));
                }
                else
                {
                    pTexto = new Paragraph(sTexto, fFont);
                }
                pTexto.Alignment = iHorizontalAlignment;
                cell.AddElement(pTexto);
                return cell;

            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("Não foi possível gerar a celula.", ex);
            }
        }

        public PdfPCell newCell(string sTexto, Font fFont, int iColsspan, int iRowspan, int iHorizontalAlignment, int iVerticalAlignment, float headerwidth, bool bAddGlossario, string sTema)
        {
            try
            {
                var cell = new PdfPCell();
                if (iRowspan > 0) cell.Rowspan = iRowspan;
                if (iColsspan > 0) cell.Colspan = iColsspan;

                if (iVerticalAlignment != -1) cell.VerticalAlignment = iVerticalAlignment;

                Paragraph pTexto;
                if (bAddGlossario) {
                    pTexto = new Paragraph(new Chunk(sTexto, fFont).SetGenericTag(sTema + " -- " + sTexto));
                } else {
                    pTexto = new Paragraph(sTexto, fFont);
                }
                pTexto.Alignment = iHorizontalAlignment;
                cell.AddElement(pTexto);
                return cell;
            
            }catch(Exception ex)
            {
                throw new FalhaOperacaoException("Não foi possível gerar a celula.", ex);
            }
        }

        /// <summary>
        /// Gera o arquivo binario com a extenção .pdf.
        /// </summary>
        /// <param name="tabela"></param>
        /// <param name="urlImage"></param>
        /// <param name="totalRegistros"></param>
        /// <returns></returns>
        public byte[] GeraRelatorioPDF(List<PdfPTable> tabela, string urlImage, int totalRegistros)
        {
            try
            {
            //Passo 1: Cria uma instancia do iTextSharp.text - objeto Documento \\
            Document document = (tipoRelatorio == tipo.Paisagem) ? new Document(PageSize.A4.Rotate()) : new Document(PageSize.A4);

            //Passo 1.1: Cria um memory stream para gravar o arquivo \\
            var mStream = new MemoryStream();

            //Passo 2: Cria um WRITER que vai "ouvir" o documento criado e gravar em uma STREAM escolhida \\
            PdfWriter writer = PdfWriter.GetInstance(document, mStream);

            var pageeventhandler = new EventoRelatorio();
            writer.PageEvent = pageeventhandler;

            // MONTA O CABECALHO \\
            cabecalho = cabecalho + Environment.NewLine + " " + tituloRelatorio;
            //cabecalho = cabecalho + Environment.NewLine + " Data de emissão: " + DateTime.Now.ToString("dd/MM/yyyy") + " às " + DateTime.Now.ToShortTimeString();

            //Image logo = Image.GetInstance(string.Concat(urlImage));
            //logo.Alignment = Element.ALIGN_CENTER;
            //logo.ScalePercent(55);
            //logo.Border = 0;
            Chunk linha = new Chunk(new LineSeparator(1f, 100f, BaseColor.GRAY, Element.ALIGN_CENTER, -1));
            document.Open();

            Paragraph header = new Paragraph(cabecalho, fontTitulo());
            Paragraph logo = new Paragraph("");

            header.Alignment = Element.ALIGN_RIGHT;


            PdfPTable headerTbl = new PdfPTable(6);
            headerTbl.WidthPercentage = 100;
            headerTbl.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell cell = new PdfPCell(header);
            PdfPCell cell2 = new PdfPCell(logo);
            cell.Colspan = 5;
            cell.Border = 0;
            cell2.Border = 0;
            headerTbl.AddCell(cell);
            headerTbl.AddCell(cell2);

            document.Add(headerTbl);
            document.Add(new Paragraph(String.Empty));
            document.Add(new Paragraph(String.Empty));
            document.Add(linha);
           

            foreach (var tab in tabela)
            {
                document.Add(tab);
                document.Add(new Paragraph(" "));
            }


            if (totalRegistros != -1)
            {
                document.Add(linha);
                document.Add(GerarTabelaTotalReg(totalRegistros, "Total de Registros = "));
            }

            document.Close();

            byte[] retorno = mStream.ToArray();

            mStream.Flush();
            mStream.Close();

           return retorno;
            }
            catch (Exception ex)
            {
                throw new FalhaOperacaoException("Não foi possível gerar o relatório.", ex);
            }
        }
        /// <summary>
        /// Gera uma tabela para mostrar a quantidade de registros.
        /// </summary>
        /// <param name="contador"></param>
        /// <param name="texto"></param>
        /// <returns></returns>
        public PdfPTable GerarTabelaTotalReg(int contador,string texto)
        {
             Paragraph vazio = new Paragraph("");
             Paragraph total = new Paragraph(texto + contador);
            total.Alignment = Element.ALIGN_RIGHT;

            PdfPTable tabela = new PdfPTable(6);
            tabela.WidthPercentage = 100;
            tabela.HorizontalAlignment = Element.ALIGN_CENTER;

            PdfPCell cell = new PdfPCell(vazio);
            PdfPCell cell2 = new PdfPCell(total);
            cell.Colspan = 4;
            cell2.Colspan = 2;
            cell.Border = 0;
            
            cell2.Border = 0;
            tabela.AddCell(cell);
            tabela.AddCell(cell2);

            return tabela;
        }

    }
}

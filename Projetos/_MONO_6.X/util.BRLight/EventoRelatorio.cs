using System;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace util.BRLight
{

    public class EventoRelatorio : PdfPageEventHelper
    {
        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // we will put the final number of pages in a template
        PdfTemplate template;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;

        #region Properties

        public string Title { get; set; }

        public string HeaderLeft { get; set; }

        public string HeaderRight { get; set; }

        public Font HeaderFont { get; set; }

        public Font FooterFont { get; set; }

        #endregion

        // we override the onOpenDocument method
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
                template = cb.CreateTemplate(50, 50);
            }
            catch
            {

            }
        }

        public override void OnStartPage(PdfWriter writer, Document document)
        {

        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            int pageN = writer.PageNumber;
            String text = pageN.ToString();
            float len = bf.GetWidthPoint(text, 10);

            Rectangle pageSize = document.PageSize;

            cb.SetRGBColorFill(100, 100, 100);

            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            cb.SetTextMatrix(pageSize.GetLeft(40), pageSize.GetBottom(20));
            cb.ShowTextAligned(PdfContentByte.ALIGN_RIGHT,
              text,
              pageSize.GetRight(40),
              pageSize.GetBottom(20), 0);
            cb.EndText();
            
            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT,
                "Criado em " + PrintTime.ToString(),
                pageSize.GetLeft(40),
                pageSize.GetBottom(20), 0);
            cb.EndText();
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);

            template.BeginText();
            template.SetFontAndSize(bf, 10);
            template.SetTextMatrix(0, 0);
            template.ShowText("" + (writer.PageNumber - 1));
            template.EndText();
        }

    }
}

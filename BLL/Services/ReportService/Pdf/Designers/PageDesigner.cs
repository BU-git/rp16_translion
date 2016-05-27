using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BLL.Services.ReportService.Pdf.Designers
{
    /// <summary>
    /// Pdf page title designer
    /// </summary>
    internal sealed class PageDesigner : PdfDesigner
    {
        public PageDesigner(Document document, PdfWriter writer) : base(document, writer)
        {
        }

        public override void Draw(string text)
        {
            var font = FontFactory.GetFont(BaseFont.TIMES_ROMAN, 24, Font.BOLD, BaseColor.BLACK);
            var paragraph = new Paragraph(text, font)
            {
                SpacingBefore = 20,
                Alignment = Element.ALIGN_CENTER
            };
            _document.Add(paragraph);
        }
    }
}

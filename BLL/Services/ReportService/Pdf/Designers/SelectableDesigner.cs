using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BLL.Services.ReportService.Pdf.Designers
{
    /// <summary>
    /// Pdf checked checkboxes and radiobuttons designer
    /// </summary>
    internal sealed class SelectableDesigner : PdfDesigner
    {
        public SelectableDesigner(Document document, PdfWriter writer) : base(document, writer)
        {
        }

        public override void Draw(string text)
        {
            var font = FontFactory.GetFont(BaseFont.TIMES_ROMAN, 14, Font.BOLDITALIC, BaseColor.BLACK);
            _document.Add(new Paragraph(text, font));
        }
    }
}

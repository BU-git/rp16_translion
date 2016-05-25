using iTextSharp.text.pdf;
using iTextSharp.text;

namespace BLL.Services.ReportService.Pdf.Designers
{
    /// <summary>
    /// Title of document designer
    /// </summary>
    internal sealed class TitleDesigner : PdfDesigner
    {
        public TitleDesigner(Document document, PdfWriter writer) : base(document, writer)
        {
        }

        public override void Draw(string text)
        {
            var font = FontFactory.GetFont(BaseFont.TIMES_ROMAN, 28, Font.ITALIC, BaseColor.BLACK);
            var table = new PdfPTable(1);
            var cell = new PdfPCell(new Phrase(text, font))
            {
                BorderColor = BaseColor.BLACK,
                Border = Rectangle.BOTTOM_BORDER,
                HorizontalAlignment = 1
            };
            table.AddCell(cell);
            _document.Add(table);
        }
    }
}

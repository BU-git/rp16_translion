using iTextSharp.text.pdf;
using iTextSharp.text;

namespace BLL.Services.ReportService.Pdf.Designers
{
    /// <summary>
    /// Question title designer
    /// </summary>
    internal sealed class QuestionTitleDesigner : PdfDesigner
    {
        public QuestionTitleDesigner(Document document, PdfWriter writer) : base(document, writer)
        {
        }

        public override void Draw(string text)
        {
            var font = FontFactory.GetFont(BaseFont.TIMES_ROMAN, 16, Font.BOLD, BaseColor.BLACK);
            var paragraph = new Paragraph(text, font)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingBefore = 15
            };
            _document.Add(paragraph);
        }
    }
}

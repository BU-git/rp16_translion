using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BLL.Services.ReportService.Pdf.Designers
{
    internal sealed class DatePickerDesigner : PdfDesigner
    {
        public DatePickerDesigner(Document document, PdfWriter writer) : base(document, writer)
        {
        }

        public override void Draw(string text)
        {
            var font = FontFactory.GetFont(BaseFont.TIMES_ROMAN, 14, Font.NORMAL, BaseColor.BLACK);
            _document.Add(new Paragraph(text, font)
            {
                Alignment = Element.ALIGN_CENTER
            });
        }
    }
}

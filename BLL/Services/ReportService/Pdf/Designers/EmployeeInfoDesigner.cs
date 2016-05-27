using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BLL.Services.ReportService.Pdf.Designers
{
    /// <summary>
    /// Employee info designer
    /// </summary>
    internal sealed class EmployeeInfoDesigner : PdfDesigner
    {
        public EmployeeInfoDesigner(Document document, PdfWriter writer) : base(document, writer)
        {
        }

        public override void Draw(string text)
        {
            var font = FontFactory.GetFont(BaseFont.TIMES_ROMAN, 14, Font.NORMAL, BaseColor.BLACK);
            var paragraph = new Paragraph(text, font)
            {
                Alignment = Element.ALIGN_CENTER
            };
            _document.Add(paragraph);
        }
    }
}

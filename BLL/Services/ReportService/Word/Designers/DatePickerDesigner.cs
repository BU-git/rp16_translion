using Novacode;
using System.Drawing;

namespace BLL.Services.ReportService.Word.Designers
{
    internal sealed class DatePickerDesigner : WordDesigner
    {
        public DatePickerDesigner(DocX document) : base(document)
        {
        }

        public override void Draw(string text)
        {
            Formatting format = new Formatting
            {
                FontColor = Color.Black,
                Size = 14,
                FontFamily = new FontFamily("Times New Roman")
            };
            var paragraph = _document.InsertParagraph(text, false, format);
            paragraph.Alignment = Alignment.center;
        }
    }
}

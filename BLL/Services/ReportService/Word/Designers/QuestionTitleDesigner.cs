using System.Drawing;
using Novacode;

namespace BLL.Services.ReportService.Word.Designers
{
    /// <summary>
    /// Question title designer
    /// </summary>
    internal sealed class QuestionTitleDesigner : WordDesigner
    {
        public QuestionTitleDesigner(DocX document) : base(document)
        {
        }

        public override void Draw(string text)
        {
            var format = new Formatting
            {
                FontColor = Color.Black,
                Size = 16,
                FontFamily = new FontFamily("Times New Roman"),
                Bold = true
            };
            var paragraph = _document.InsertParagraph(text, false, format);
            paragraph.Alignment = Alignment.center;
            paragraph.SpacingBefore(30.0);
        }
    }
}

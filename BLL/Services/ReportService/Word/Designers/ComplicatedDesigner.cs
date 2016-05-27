using System.Drawing;
using Novacode;

namespace BLL.Services.ReportService.Word.Designers
{
    /// <summary>
    /// Complicated types answer designer
    /// </summary>
    internal sealed class ComplicatedDesigner : WordDesigner
    {
        public ComplicatedDesigner(DocX document) : base(document)
        {
        }

        public override void Draw(string text)
        {
            var format = new Formatting
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

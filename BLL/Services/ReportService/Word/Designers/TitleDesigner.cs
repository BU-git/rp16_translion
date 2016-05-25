using System.Drawing;
using Novacode;

namespace BLL.Services.ReportService.Word.Designers
{
    /// <summary>
    /// Document title designer
    /// </summary>
    internal sealed class TitleDesigner : WordDesigner
    {
        public TitleDesigner(DocX document) : base(document)
        {
        }

        public override void Draw(string text)
        {
            var format = new Formatting
            {
                FontColor = Color.Black,
                Size = 28,
                FontFamily = new FontFamily("Times New Roman"),
                UnderlineStyle = UnderlineStyle.singleLine,
                UnderlineColor = Color.Black,
                Italic = true
            };
            var paragraph = _document.InsertParagraph(text, false, format);
            paragraph.Alignment = Alignment.center;
        }
    }
}

using System.Drawing;
using Novacode;

namespace BLL.Services.ReportService.Word.Designers
{
    /// <summary>
    /// Word page name designer
    /// </summary>
    internal sealed class PageDesigner : WordDesigner
    {
        public PageDesigner(DocX document) : base(document)
        {
        }

        public override void Draw(string text)
        {
            var format = new Formatting
            {
                FontColor = Color.Black,
                Size = 26,
                FontFamily = new FontFamily("Times New Roman"),
                Bold = true
            };
            var paragraph = _document.InsertParagraph(text, false, format);
            paragraph.Alignment = Alignment.center;
            paragraph.LineSpacingBefore = 50f;
        }
    }
}

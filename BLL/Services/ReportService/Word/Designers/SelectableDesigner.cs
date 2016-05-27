using System.Drawing;
using Novacode;

namespace BLL.Services.ReportService.Word.Designers
{
    /// <summary>
    /// Checked checkbox or radiobutton designer
    /// </summary>
    internal sealed class SelectableDesigner : WordDesigner
    {
        public SelectableDesigner(DocX document) : base(document)
        {
        }

        public override void Draw(string text)
        {
            var format = new Formatting
            {
                FontColor = Color.Black,
                Size = 14,
                FontFamily = new FontFamily("Times New Roman"),
                Bold = true,
                Italic = true
            };
            _document.InsertParagraph(text, false, format);
        }
    }
}

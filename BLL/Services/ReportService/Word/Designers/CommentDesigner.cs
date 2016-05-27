using Novacode;
using System.Drawing;

namespace BLL.Services.ReportService.Word.Designers
{
    /// <summary>
    /// Comment question designer
    /// </summary>
    internal sealed class CommentDesigner : WordDesigner
    {
        public CommentDesigner(DocX document) : base(document)
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
            _document.InsertParagraph(text, false, format);
        }
    }
}

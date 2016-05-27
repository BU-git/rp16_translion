using System.Drawing;
using Novacode;

namespace BLL.Services.ReportService.Word.Designers
{
    /// <summary>
    /// Employee info designer
    /// </summary>
    internal sealed class EmployeeInfoDesigner : WordDesigner
    {
        public EmployeeInfoDesigner(DocX document) : base(document)
        {
        }
        public override void Draw(string text)
        {
            var formatting = new Formatting
            {
                FontColor = Color.Black,
                Size = 14,
                FontFamily = new FontFamily("Times New Roman"),
            };
            var paragpaph = _document.InsertParagraph(text, false, formatting);
            paragpaph.Alignment = Alignment.center;
        }
    }
}

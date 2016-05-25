using BLL.Services.ReportService.Pdf.Designers;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace BLL.Services.ReportService.Factories
{
    /// <summary>
    /// Pdf designers factory
    /// </summary>
    public sealed class PdfDesignerFactory : DesignerFactory
    { 
        public PdfDesignerFactory(Document doc, PdfWriter writer) : base()
        {
            //populating designers
            _designers.Add("Title", new TitleDesigner(doc, writer));
            _designers.Add("EmployeeInfo", new EmployeeInfoDesigner(doc, writer));
            _designers.Add("Page", new PageDesigner(doc, writer));
            _designers.Add("Comment", new CommentDesigner(doc, writer));
            _designers.Add("Selectable", new SelectableDesigner(doc, writer));
            _designers.Add("Complicated", new ComplicatedDesigner(doc, writer));
            _designers.Add("QuestionTitle", new QuestionTitleDesigner(doc, writer));
        }
    }
}

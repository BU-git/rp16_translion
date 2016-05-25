using BLL.Services.ReportService.Word.Designers;
using Novacode;

namespace BLL.Services.ReportService.Factories
{
    /// <summary>
    /// Word designers factory
    /// </summary>
    public sealed class WordDesignerFactory : DesignerFactory
    {
        public WordDesignerFactory(DocX document)
        {
            //populate designers
            _designers.Add("Title", new TitleDesigner(document));
            _designers.Add("EmployeeInfo", new EmployeeInfoDesigner(document));
            _designers.Add("Page", new PageDesigner(document));
            _designers.Add("Comment", new CommentDesigner(document));
            _designers.Add("Selectable", new SelectableDesigner(document));
            _designers.Add("Complicated", new ComplicatedDesigner(document));
            _designers.Add("QuestionTitle", new QuestionTitleDesigner(document));
        }
    }
}

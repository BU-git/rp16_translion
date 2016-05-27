using System;
using BLL.Services.ReportService.Abstract;
using iTextSharp.text.pdf;

namespace BLL.Services.ReportService.Pdf.Designers
{
    /// <summary>
    /// Base class for all pdf designers
    /// </summary>
    internal abstract class PdfDesigner : Designer
    {
        protected readonly iTextSharp.text.Document _document;
        protected readonly PdfWriter _writer;

        protected PdfDesigner(iTextSharp.text.Document document, PdfWriter writer)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
            if (writer == null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            _document = document;
            _writer = writer;
        }
    }
}

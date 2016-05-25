using System.Collections.Generic;
using iTextSharp.text.pdf;
using System.IO;
using IDAL.Models;
using BLL.Services.ReportService.Factories;

namespace BLL.Services.ReportService.Pdf
{
    /// <summary>
    /// Generates pdf document as bytes array from pages list
    /// </summary>
    public sealed class PdfDocument : Abstract.Document
    {
        #region members
        #endregion

        public PdfDocument(List<Page> pages, Employee employee)
            : base(pages, employee, ".pdf")
        {
        }

        #region Get document
        public override byte[] GetDocument()
        {
            MemoryStream stream = new MemoryStream();
            var doc = new iTextSharp.text.Document();
            var writer = PdfWriter.GetInstance(doc, stream);

            doc.Open();
            ProcessDocument();
            WriteToDoc(new PdfDesignerFactory(doc, writer));
            doc.Close();

            var byteDoc = stream.ToArray();
            stream.Dispose();
            return byteDoc;
        }
        #endregion
    }
}

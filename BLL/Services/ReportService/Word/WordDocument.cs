using BLL.Services.ReportService.Abstract;
using System.Collections.Generic;
using IDAL.Models;
using Novacode;
using System.IO;
using BLL.Services.ReportService.Factories;

namespace BLL.Services.ReportService.Word
{
    /// <summary>
    /// Generates docx document as bytes array from pages list
    /// </summary>
    public sealed class WordDocument : Document
    {
        public WordDocument(List<Page> pages, Employee emplInfo) : base(pages, emplInfo, ".docx")
        {
        }

        public override byte[] GetDocument()
        {
            MemoryStream stream = new MemoryStream();
            var doc = DocX.Create(stream);

            ProcessDocument();
            WriteToDoc(new WordDesignerFactory(doc));
            doc.Save();

            var byteDoc = stream.ToArray();
            stream.Dispose();
            return byteDoc;
        }
    }
}

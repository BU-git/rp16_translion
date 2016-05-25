using System;
using BLL.Services.ReportService.Abstract;
using Novacode;

namespace BLL.Services.ReportService.Word.Designers
{
    /// <summary>
    /// Base class for all word designers
    /// </summary>
    internal abstract class WordDesigner : Designer
    {
        protected readonly DocX _document;

        protected WordDesigner(DocX document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }
            _document = document;
        }
    }
}

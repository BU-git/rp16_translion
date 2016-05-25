using BLL.Services.ReportService.Factories;
using System;

namespace BLL.Services.ReportService.Abstract
{
    /// <summary>
    /// Get's string representation of test entity
    /// </summary>
    public abstract class ElementTemplate
    {
        public ElementTemplate(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(nameof(name));
            }                
            _name = name;
        }

        /// <summary>
        /// Question's type answer is templates name
        /// </summary>
        protected string _name;
        
        /// <summary>
        /// Adds paragraph to document
        /// </summary>
        public abstract void AddParagraph(DesignerFactory desFactory);
    }
}

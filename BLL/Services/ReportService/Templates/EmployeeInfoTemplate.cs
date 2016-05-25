using System;
using BLL.Services.ReportService.Abstract;
using BLL.Services.ReportService.Factories;
using IDAL.Models;

namespace BLL.Services.ReportService.Templates
{
    /// <summary>
    /// Employee info template
    /// </summary>
    internal sealed class EmployeeInfoTemplate : ElementTemplate
    {
        private readonly Employee _emplInfo;
        private const string EMPL_PARAGRAPH = "Dit rapport is opgesteld en ingevuld naar " +
                "aanleiding van het ziekteverzuim van de heer/mevrouw: {0} werkzaam bij {1}";

        public EmployeeInfoTemplate(Employee emplInfo) : base("EmployeeInfo")
        {
            if (emplInfo == null)
                throw new ArgumentNullException(nameof(emplInfo));

            _emplInfo = emplInfo;
        }

        public override void AddParagraph(DesignerFactory desFactory)
        {
            desFactory.GetDesigner(_name)
                .Draw(String.Format(EMPL_PARAGRAPH,
                    $"{_emplInfo.FirstName} {_emplInfo.Prefix} {_emplInfo.LastName}",
                    _emplInfo.Employer.CompanyName)
                );
        }
    }
}

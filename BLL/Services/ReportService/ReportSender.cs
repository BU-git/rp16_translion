using BLL.Services.MailingService.Interfaces;
using BLL.Services.MailingService.MailMessageBuilders;
using BLL.Services.PersonageService;
using BLL.Services.ReportService.Abstract;
using BLL.Services.ReportService.Pdf;
using BLL.Services.ReportService.Word;
using IDAL;
using IDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services.ReportService
{
    /// <summary>
    /// Sends reports to revievers
    /// </summary>
    public sealed class ReportSender
    {
        #region Private
        private PersonManager<Admin> _adminManager;

        private IMailingService _mailService;

        private Employee _employee;

        private List<Page> _pages;
        #endregion

        public ReportSender(IMailingService mailService, PersonManager<Admin> admManager, Employee empl, List<Page> pages)
        {
            _mailService = mailService;
            _employee = empl;
            _pages = pages;
            _adminManager = admManager;
        }

        #region Send mail
        /// <summary>
        /// Send mails with reports to recievers
        /// </summary>
        /// <returns>Operation result</returns>
        public async Task<WorkResult> SendMailsToRecieversAsync()
        {
            if (_employee == null || _pages == null || _pages.Count == 0)
                return WorkResult.Failed("Params are not valid.");

            byte[] pdfReport, wordReport;
            Document pdf, docx;

            try
            {
                pdf = new PdfDocument(_pages, _employee);
                docx = new WordDocument(_pages, _employee);

                pdfReport = pdf.GetDocument();
                wordReport = docx.GetDocument();
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.ToString());
            }

            if (pdfReport.Length == 0 && wordReport.Length == 0)
                return WorkResult.Failed("Length of file is 0");


            return await SendMailsAsync(wordReport, docx.Name, pdfReport, pdf.Name);
        }

        /// <summary>
        /// Sends mails with reports to recievers
        /// </summary>
        /// <param name="word">Word doc</param>
        /// <param name="pdf">Pdf doc</param>
        /// <returns>Operation result</returns>
        private async Task<WorkResult> SendMailsAsync(byte[] word, string wordName, byte[] pdf, string pdfName)
        {
            var adminMail = new ReportCompltdAdminMessageBuilder(_employee);
            var employerMail = new ReportCompltdEmplMessageBuilder(_employee);
            var admins = await _adminManager.GetAll();

            var result = 
                await _mailService.SendMailAsync(adminMail.Body, adminMail.Subject, word, wordName,
                    admins.Select(a => a.User.Email).ToArray());

            if (!result.HasError)
                result = await _mailService.SendMailAsync(employerMail.Body, employerMail.Subject, pdf, pdfName, _employee.Employer.User.Email);

            if (result.HasError)
                return WorkResult.Failed(result.ErrorMessage);

            return WorkResult.Success();
        }
        #endregion
    }
}

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IDAL;
using IDAL.Models;

namespace BLL.Services.TestService.Interfaces
{
    /// <summary>
    /// Testing service. CRUD for pages, answers and questions
    /// </summary>
    public interface ITestService
    {
        #region Get all pages

        Task<List<Page>> GetAllPages();
        Task<List<Page>> GetAllPages(CancellationToken cancellationToken);

        #endregion

        #region Add page collection

        Task<WorkResult> AddPages(IEnumerable<Page> pages);
        Task<WorkResult> AddPages(CancellationToken token, IEnumerable<Page> pages);

        #endregion

        #region Get concrete page by id

        Task<Page> GetPageById(int pageId);
        Task<Page> GetPageById(CancellationToken cancellationToken, int pageId);

        #endregion

        #region Create page

        Task<WorkResult> CreatePage(Page page);
        Task<WorkResult> CreatePage(CancellationToken cancellationToken, Page page);

        #endregion

        #region Update page

        Task<WorkResult> UpdatePage(Page page);
        Task<WorkResult> UpdatePage(CancellationToken cancellationToken, Page page);

        #endregion

        #region Delete page

        Task<WorkResult> DeletePage(Page page);
        Task<WorkResult> DeletePage(CancellationToken cancellationToken, Page page);

        #endregion

        #region Delete all pages

        Task<WorkResult> DeleteAllPages();
        Task<WorkResult> DeleteAllPages(CancellationToken token);

        #endregion

        #region Add question to page

        Task<WorkResult> AddQuestion(Question question, Page page);
        Task<WorkResult> AddQuestion(CancellationToken cancellationToken, Question question, Page page);

        #endregion

        #region Remove question from page

        Task<WorkResult> RemoveQuestion(Question question, Page page);
        Task<WorkResult> RemoveQuestion(CancellationToken token, Question question, Page page);

        #endregion

        #region Update Question

        Task<WorkResult> UpdateQuestion(Question question);
        Task<WorkResult> UpdateQuestion(CancellationToken token, Question question);

        #endregion

        #region Get question

        Task<Question> GetQuestion(int questionId);
        Task<Question> GetQuestion(CancellationToken cancellationToken, int questionId);

        #endregion

        #region Add possible answer to question

        Task<WorkResult> AddAnswer(Answer answer, Question question);
        Task<WorkResult> AddAnswer(CancellationToken token, Answer answer, Question question);

        #endregion

        #region Remove answer from question

        Task<WorkResult> RemoveAnswer(Answer answer, Question question);
        Task<WorkResult> RemoveAnswer(CancellationToken token, Answer answer, Question question);

        #endregion

        #region Update answer

        Task<WorkResult> UpdateAnswer(Answer answer);
        Task<WorkResult> UpdateAnswer(CancellationToken cancellationToken, Answer answer);

        #endregion

        #region Get answer

        Task<Answer> GetAnswer(int answerId);
        Task<Answer> GetAnswer(CancellationToken cancellationToken, int answerId);

        #endregion

        #region Parse Answer Name

        void ParseAnswerName(string questionName, out int pageId, out int questionId);

        #endregion
    }
}
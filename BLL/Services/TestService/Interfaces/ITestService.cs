using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using IDAL.Models;

namespace BLL.Services.TestService.Interfaces
{
    /// <summary>
    /// Testing service. CRUD for pages, answers and questions
    /// </summary>
    public interface ITestService
    {
        #region Get all pages
        List<Page> GetAllPages();
        Task<List<Page>> GetAllPagesAsync();
        Task<List<Page>> GetAllPagesAsync(CancellationToken cancellationToken);
        #endregion

        #region Add page collection
        void AddPages(IEnumerable<Page> pages);
        Task<int> AddPagesAsync(IEnumerable<Page> pages);
        Task<int> AddPagesAsync(CancellationToken token, IEnumerable<Page> pages);
        #endregion

        #region Get concrete page by id
        Page GetPageById(int pageId);
        Task<Page> GetPageByIdAsync(int pageId);
        Task<Page> GetPageByIdAsync(CancellationToken cancellationToken, int pageId);
        #endregion

        #region Create page
        void CreatePage(Page page);
        Task<int> CreatePageAsync(Page page);
        Task<int> CreatePageAsync(CancellationToken cancellationToken, Page page);
        #endregion

        #region Update page
        void UpdatePage(Page page);
        Task<int> UpdatePageAsync(Page page);
        Task<int> UpdatePageAsync(CancellationToken cancellationToken, Page page);
        #endregion

        #region Delete page
        void DeletePage(Page page);
        Task<int> DeletePageAsync(Page page);
        Task<int> DeletePageAsync(CancellationToken cancellationToken, Page page);
        #endregion

        #region Delete all pages
        void DeleteAllPages();
        Task<int> DeleteAllPagesAsync();
        Task<int> DeleteAllPagesAsync(CancellationToken token);
        #endregion

        #region Add question to page
        void AddQuestion(Question question, Page page);
        Task<int> AddQuestionAsync(Question question, Page page);
        Task<int> AddQuestionAsync(CancellationToken token, Question question, Page page);
        #endregion

        #region Remove question from page
        void RemoveQuestion(Question question, Page page);
        Task<int> RemoveQuestionAsync(Question question, Page page);
        Task<int> RemoveQuestionAsync(CancellationToken token, Question question, Page page);
        #endregion

        #region Update Question
        void UpdateQuestion(Question question);
        Task<int> UpdateQuestionAsync(Question question);
        Task<int> UpdateQuestionAsync(CancellationToken token, Question question);
        #endregion

        #region Get question
        Question GetQuestion(int questionId);
        Task<Question> GetQuestionAsync(int questionId);
        Task<Question> GetQuestionAsync(CancellationToken token, int questionId);
        #endregion

        #region Add answer to question
        void AddAnswer(Answer answer, Question question);
        Task<int> AddAnswerAsync(Answer answer, Question question);
        Task<int> AddAnswerAsync(CancellationToken token, Answer answer, Question question);
        #endregion

        #region Remove answer from question
        void RemoveAnswer(Answer answer, Question question);
        Task<int> RemoveAnswerAsync(Answer answer, Question question);
        Task<int> RemoveAnswerAsync(CancellationToken token, Answer answer, Question question);
        #endregion

        #region Update answer
        void UpdateAnswer(Answer answer);
        Task<int> UpdateAnswerAsync(Answer answer);
        Task<int> UpdateAnswerAsync(CancellationToken token, Answer answer);
        #endregion

        #region Get answer
        Answer GetAnswer(int answerId);
        Task<Answer> GetAnswerAsync(int answerId);
        Task<Answer> GetAnswerAsync(CancellationToken token, int answerId);
        #endregion
    }
}

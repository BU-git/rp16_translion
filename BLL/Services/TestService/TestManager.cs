using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BLL.Services.TestService.Interfaces;
using IDAL.Interfaces;
using IDAL.Models;

namespace BLL.Services.TestService
{
    public class TestManager : ITestService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TestManager(IUnitOfWork uow)
        {
            _unitOfWork = uow;
        }

        #region Get all pages

        public List<Page> GetAllPages()
            => _unitOfWork.PageRepository.GetAll();

        public Task<List<Page>> GetAllPagesAsync()
            => _unitOfWork.PageRepository.GetAllAsync();

        public Task<List<Page>> GetAllPagesAsync(CancellationToken cancellationToken)
            => _unitOfWork.PageRepository.GetAllAsync(cancellationToken);
        #endregion

        #region Add pages
        public void AddPages(IEnumerable<Page> pages)
        {
            if (pages == null)
                throw new ArgumentNullException(nameof(pages));

            _unitOfWork.PageRepository.AddRange(pages);

            _unitOfWork.SaveChanges();
        }

        public Task<int> AddPagesAsync(IEnumerable<Page> pages)
        {
            if (pages == null)
                throw new ArgumentNullException(nameof(pages));

            _unitOfWork.PageRepository.AddRange(pages);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task<int> AddPagesAsync(CancellationToken token, IEnumerable<Page> pages)
        {
            if (pages == null)
                throw new ArgumentNullException(nameof(pages));

            _unitOfWork.PageRepository.AddRange(pages);

            return _unitOfWork.SaveChangesAsync(token);
        }
        #endregion

        #region Get page by id
        public Page GetPageById(int pageId)
        {
            if (pageId <= 0)
                throw new ArgumentException($"Id can't be {pageId}", nameof(pageId));

            return _unitOfWork.PageRepository.FindById(pageId);
        }

        public Task<Page> GetPageByIdAsync(int pageId)
        {
            if (pageId <= 0)
                throw new ArgumentException($"Id can't be {pageId}", nameof(pageId));

            return _unitOfWork.PageRepository.FindByIdAsync(pageId);
        }

        public Task<Page> GetPageByIdAsync(CancellationToken cancellationToken, int pageId)
        {
            if (pageId <= 0)
                throw new ArgumentException($"Id can't be {pageId}", nameof(pageId));

            return _unitOfWork.PageRepository.FindByIdAsync(cancellationToken, pageId);
        }
        #endregion

        #region Create page
        public void CreatePage(Page page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            _unitOfWork.PageRepository.Add(page);

            _unitOfWork.SaveChanges();
        }

        public Task<int> CreatePageAsync(Page page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            _unitOfWork.PageRepository.Add(page);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task<int> CreatePageAsync(CancellationToken cancellationToken, Page page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            _unitOfWork.PageRepository.Add(page);

            return _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        #endregion

        #region Update page
        public void UpdatePage(Page page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            _unitOfWork.PageRepository.Update(page);

            _unitOfWork.SaveChanges();
        }

        public Task<int> UpdatePageAsync(Page page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            _unitOfWork.PageRepository.Update(page);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task<int> UpdatePageAsync(CancellationToken cancellationToken, Page page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            _unitOfWork.PageRepository.Update(page);

            return _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        #endregion

        #region Delete page
        public void DeletePage(Page page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            _unitOfWork.PageRepository.Remove(page);

            _unitOfWork.SaveChanges();
        }

        public Task<int> DeletePageAsync(Page page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            _unitOfWork.PageRepository.Remove(page);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task<int> DeletePageAsync(CancellationToken cancellationToken, Page page)
        {
            if (page == null)
                throw new ArgumentNullException(nameof(page));

            _unitOfWork.PageRepository.Remove(page);

            return _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        #endregion

        #region Delete all pages
        public void DeleteAllPages()
        {
            var pages = _unitOfWork.PageRepository.GetAll();

            if (pages != null && pages.Count > 0)
            {
                foreach (var page in pages)
                    _unitOfWork.PageRepository.Remove(page);

                _unitOfWork.SaveChanges();
            }
        }

        public async Task<int> DeleteAllPagesAsync()
        {
            var pages = await _unitOfWork.PageRepository.GetAllAsync();

            if (pages != null && pages.Count > 0)
            {
                foreach (var page in pages)
                    _unitOfWork.PageRepository.Remove(page);

                return await _unitOfWork.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<int> DeleteAllPagesAsync(CancellationToken token)
        {
            var pages = await _unitOfWork.PageRepository.GetAllAsync(token);

            if (pages != null && pages.Count > 0)
            {
                foreach (var page in pages)
                    _unitOfWork.PageRepository.Remove(page);

                return await _unitOfWork.SaveChangesAsync(token);
            }

            return 0;
        }
        #endregion

        #region Add question
        public void AddQuestion(Question question, Page page)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            if (page == null)
                throw new ArgumentNullException(nameof(page));

            if (!page.Questions.Contains(question))
                page.Questions.Add(question);

            _unitOfWork.SaveChanges();
        }

        public Task<int> AddQuestionAsync(Question question, Page page)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            if (page == null)
                throw new ArgumentNullException(nameof(page));

            if (!page.Questions.Contains(question))
                page.Questions.Add(question);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task<int> AddQuestionAsync(CancellationToken token, Question question, Page page)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            if (page == null)
                throw new ArgumentNullException(nameof(page));

            if (!page.Questions.Contains(question))
                page.Questions.Add(question);

            return _unitOfWork.SaveChangesAsync(token);
        }
        #endregion

        #region Remove question
        public void RemoveQuestion(Question question, Page page)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            if (page == null)
                throw new ArgumentNullException(nameof(page));

            if (page.Questions.Contains(question))
                page.Questions.Remove(question);

            _unitOfWork.SaveChanges();
        }

        public Task<int> RemoveQuestionAsync(Question question, Page page)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            if (page == null)
                throw new ArgumentNullException(nameof(page));

            if (page.Questions.Contains(question))
                page.Questions.Remove(question);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task<int> RemoveQuestionAsync(CancellationToken token, Question question, Page page)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            if (page == null)
                throw new ArgumentNullException(nameof(page));

            if (page.Questions.Contains(question))
                page.Questions.Remove(question);

            return _unitOfWork.SaveChangesAsync(token);
        }
        #endregion

        #region Update question
        public void UpdateQuestion(Question question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            _unitOfWork.QuestionRepository.Update(question);

            _unitOfWork.SaveChanges();
        }

        public Task<int> UpdateQuestionAsync(Question question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            _unitOfWork.QuestionRepository.Update(question);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task<int> UpdateQuestionAsync(CancellationToken token, Question question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            _unitOfWork.QuestionRepository.Update(question);

            return _unitOfWork.SaveChangesAsync(token);
        }
        #endregion

        #region Get question
        public Question GetQuestion(int questionId)
        {
            if (questionId <= 0)
                throw new ArgumentException($"Question's id can't be {questionId}", nameof(questionId));

            return _unitOfWork.QuestionRepository.FindById(questionId);
        }

        public Task<Question> GetQuestionAsync(int questionId)
        {
            if (questionId <= 0)
                throw new ArgumentException($"Question's id can't be {questionId}", nameof(questionId));

            return _unitOfWork.QuestionRepository.FindByIdAsync(questionId);
        }

        public Task<Question> GetQuestionAsync(CancellationToken token, int questionId)
        {
            if (questionId <= 0)
                throw new ArgumentException($"Question's id can't be {questionId}", nameof(questionId));

            return _unitOfWork.QuestionRepository.FindByIdAsync(token, questionId);
        }

        #endregion

        #region Add answer
        public void AddAnswer(Answer answer, Question question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            if (answer == null)
                throw new ArgumentNullException(nameof(answer));

            if (!question.Answers.Contains(answer))
                question.Answers.Add(answer);

            _unitOfWork.SaveChanges();
        }

        public Task<int> AddAnswerAsync(Answer answer, Question question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            if (answer == null)
                throw new ArgumentNullException(nameof(answer));

            if (!question.Answers.Contains(answer))
                question.Answers.Add(answer);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task<int> AddAnswerAsync(CancellationToken token, Answer answer, Question question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            if (answer == null)
                throw new ArgumentNullException(nameof(answer));

            if (!question.Answers.Contains(answer))
                question.Answers.Add(answer);

            return _unitOfWork.SaveChangesAsync(token);
        }
        #endregion

        #region Remove answer
        public void RemoveAnswer(Answer answer, Question question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            if (answer == null)
                throw new ArgumentNullException(nameof(answer));

            if (question.Answers.Contains(answer))
                question.Answers.Remove(answer);

            _unitOfWork.SaveChanges();
        }

        public Task<int> RemoveAnswerAsync(Answer answer, Question question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            if (answer == null)
                throw new ArgumentNullException(nameof(answer));

            if (question.Answers.Contains(answer))
                question.Answers.Remove(answer);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task<int> RemoveAnswerAsync(CancellationToken token, Answer answer, Question question)
        {
            if (question == null)
                throw new ArgumentNullException(nameof(question));

            if (answer == null)
                throw new ArgumentNullException(nameof(answer));

            if (question.Answers.Contains(answer))
                question.Answers.Remove(answer);

            return _unitOfWork.SaveChangesAsync(token);
        }
        #endregion
        #region Update answer
        public void UpdateAnswer(Answer answer)
        {
            if (answer == null)
                throw new ArgumentNullException(nameof(answer));

            _unitOfWork.AnswerRepository.Update(answer);

            _unitOfWork.SaveChanges();
        }

        public Task<int> UpdateAnswerAsync(Answer answer)
        {
            if (answer == null)
                throw new ArgumentNullException(nameof(answer));

            _unitOfWork.AnswerRepository.Update(answer);

            return _unitOfWork.SaveChangesAsync();
        }

        public Task<int> UpdateAnswerAsync(CancellationToken token, Answer answer)
        {
            if (answer == null)
                throw new ArgumentNullException(nameof(answer));

            _unitOfWork.AnswerRepository.Update(answer);

            return _unitOfWork.SaveChangesAsync(token);
        }
        #endregion

        #region Get answer
        public Answer GetAnswer(int answerId)
        {
            if (answerId <= 0)
                throw new ArgumentException($"Answer's id can't be {answerId}", nameof(answerId));

            return _unitOfWork.AnswerRepository.FindById(answerId);
        }

        public Task<Answer> GetAnswerAsync(int answerId)
        {
            if (answerId <= 0)
                throw new ArgumentException($"Answer's id can't be {answerId}", nameof(answerId));

            return _unitOfWork.AnswerRepository.FindByIdAsync(answerId);
        }

        public Task<Answer> GetAnswerAsync(CancellationToken token, int answerId)
        {
            if (answerId <= 0)
                throw new ArgumentException($"Answer's id can't be {answerId}", nameof(answerId));

            return _unitOfWork.AnswerRepository.FindByIdAsync(token, answerId);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using BLL.Services.TestService.Interfaces;
using IDAL;
using IDAL.Interfaces;
using IDAL.Models;

namespace BLL.Services.TestService
{
    public class TestManager : ITestService
    {
        public TestManager(IUnitOfWork uow)
        {
            UnitOfWork = uow;
        }

        public IUnitOfWork UnitOfWork { get; }

        #region Get all pages

        public async Task<List<Page>> GetAllPages()
        {
            return await UnitOfWork.PageRepository.GetAll();
        }

        public Task<List<Page>> GetAllPages(CancellationToken cancellationToken)
        {
            return UnitOfWork.PageRepository.GetAll(cancellationToken);
        }

        #endregion

        #region Add pages

        public async Task<WorkResult> AddPages(IEnumerable<Page> pages)
        {
            if (pages == null)
            {
                return WorkResult.Failed("Pages cannot be null");
            }
            try
            {
                UnitOfWork.PageRepository.AddRange(pages);
                await UnitOfWork.SaveChanges();
                return WorkResult.Success();
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        public async Task<WorkResult> AddPages(CancellationToken cancellationToken, IEnumerable<Page> pages)
        {
            if (pages == null)
            {
                return WorkResult.Failed("Pages cannot be null");
            }
            try
            {
                UnitOfWork.PageRepository.AddRange(pages);
                await UnitOfWork.SaveChanges(cancellationToken);
                return WorkResult.Success();
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        #endregion

        #region Get page by id

        public async Task<Page> GetPageById(int pageId)
        {
            if (pageId <= 0)
            {
                return null;
            }
            return await UnitOfWork.PageRepository.FindById(pageId);
        }

        public async Task<Page> GetPageById(CancellationToken cancellationToken, int pageId)
        {
            if (pageId <= 0)
            {
                return null;
            }
            return await UnitOfWork.PageRepository.FindById(cancellationToken, pageId);
        }

        #endregion

        #region Create page

        public async Task<WorkResult> CreatePage(Page page)
        {
            if (page == null)
            {
                return WorkResult.Failed("Page cannot be null");
            }
            try
            {
                UnitOfWork.PageRepository.Add(page);
                int result = await UnitOfWork.SaveChanges();
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        public async Task<WorkResult> CreatePage(CancellationToken cancellationToken, Page page)
        {
            if (page == null)
            {
                return WorkResult.Failed("Page cannot be null");
            }
            try
            {
                UnitOfWork.PageRepository.Add(page);
                int result = await UnitOfWork.SaveChanges(cancellationToken);
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        #endregion

        #region Update page

        public async Task<WorkResult> UpdatePage(Page page)
        {
            if (page == null)
            {
                return WorkResult.Failed("Page cannot be null");
            }
            try
            {
                UnitOfWork.PageRepository.Update(page);
                int result = await UnitOfWork.SaveChanges();
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        public async Task<WorkResult> UpdatePage(CancellationToken cancellationToken, Page page)
        {
            if (page == null)
            {
                return WorkResult.Failed("Page cannot be null");
            }
            try
            {
                UnitOfWork.PageRepository.Update(page);
                int result = await UnitOfWork.SaveChanges(cancellationToken);
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        #endregion

        #region Delete page

        public async Task<WorkResult> DeletePage(Page page)
        {
            if (page == null)
            {
                return WorkResult.Failed("Page cannot be null");
            }
            try
            {
                UnitOfWork.PageRepository.Remove(page);
                int result = await UnitOfWork.SaveChanges();
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        public async Task<WorkResult> DeletePage(CancellationToken cancellationToken, Page page)
        {
            if (page == null)
            {
                return WorkResult.Failed("Page cannot be null");
            }
            try
            {
                UnitOfWork.PageRepository.Remove(page);
                int result = await UnitOfWork.SaveChanges(cancellationToken);
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        #endregion

        #region Delete all pages

        public async Task<WorkResult> DeleteAllPages()
        {
            List<Page> pages = await UnitOfWork.PageRepository.GetAll();
            if (pages == null || pages.Count == 0)
            {
                return WorkResult.Failed("Page cannot be null or empty");
            }
            try
            {
                foreach (var page in pages)
                {
                    UnitOfWork.PageRepository.Remove(page);
                }
                int result = await UnitOfWork.SaveChanges();
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        public async Task<WorkResult> DeleteAllPages(CancellationToken cancellationToken)
        {
            List<Page> pages = await UnitOfWork.PageRepository.GetAll(cancellationToken);
            if (pages == null || pages.Count == 0)
            {
                return WorkResult.Failed("Page cannot be null or empty");
            }
            try
            {
                foreach (var page in pages)
                {
                    UnitOfWork.PageRepository.Remove(page);
                }
                int result = await UnitOfWork.SaveChanges(cancellationToken);
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        #endregion

        #region Add question

        public async Task<WorkResult> AddQuestion(Question question, Page page)
        {
            if ((question == null) || (page == null))
            {
                return WorkResult.Failed("Page or question cannot be null");
            }
            try
            {
                if (!page.Questions.Contains(question))
                {
                    page.Questions.Add(question);
                    int result = await UnitOfWork.SaveChanges();
                    if (result > 0)
                    {
                        return WorkResult.Success();
                    }
                    return WorkResult.Failed("SaveChanges returned 0");
                }
                return WorkResult.Failed("Current page hold this question");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        public async Task<WorkResult> AddQuestion(CancellationToken cancellationToken, Question question, Page page)
        {
            if ((question == null) || (page == null))
            {
                return WorkResult.Failed("Page or question cannot be null");
            }
            try
            {
                if (!page.Questions.Contains(question))
                {
                    page.Questions.Add(question);
                    int result = await UnitOfWork.SaveChanges(cancellationToken);
                    if (result > 0)
                    {
                        return WorkResult.Success();
                    }
                    return WorkResult.Failed("SaveChanges returned 0");
                }
                return WorkResult.Failed("Current page hold this question");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        #endregion

        #region Remove question

        public async Task<WorkResult> RemoveQuestion(Question question, Page page)
        {
            if ((question == null) || (page == null))
            {
                return WorkResult.Failed("Page or question cannot be null");
            }
            try
            {
                if (page.Questions.Contains(question))
                {
                    page.Questions.Remove(question);
                    int result = await UnitOfWork.SaveChanges();
                    if (result > 0)
                    {
                        return WorkResult.Success();
                    }
                    return WorkResult.Failed("SaveChanges returned 0");
                }
                return WorkResult.Failed("Current page hold this question");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        public async Task<WorkResult> RemoveQuestion(CancellationToken cancellationToken, Question question, Page page)
        {
            if ((question == null) || (page == null))
            {
                return WorkResult.Failed("Page or question cannot be null");
            }
            try
            {
                if (page.Questions.Contains(question))
                {
                    page.Questions.Remove(question);
                    int result = await UnitOfWork.SaveChanges(cancellationToken);
                    if (result > 0)
                    {
                        return WorkResult.Success();
                    }
                    return WorkResult.Failed("SaveChanges returned 0");
                }
                return WorkResult.Failed("Current page hold this question");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        #endregion

        #region Update question

        public async Task<WorkResult> UpdateQuestion(Question question)
        {
            if (question == null)
            {
                return WorkResult.Failed("Question cannot be null");
            }
            try
            {
                UnitOfWork.QuestionRepository.Update(question);
                await UnitOfWork.SaveChanges();
                return WorkResult.Success();
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        public async Task<WorkResult> UpdateQuestion(CancellationToken cancellationToken, Question question)
        {
            if (question == null)
            {
                return WorkResult.Failed("Question cannot be null");
            }
            try
            {
                UnitOfWork.QuestionRepository.Update(question);
                await UnitOfWork.SaveChanges(cancellationToken);
                return WorkResult.Success();
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        #endregion

        #region Get question

        public Task<Question> GetQuestion(int questionId)
        {
            if (questionId <= 0)
            {
                return null;
            }
            return UnitOfWork.QuestionRepository.FindById(questionId);
        }

        public Task<Question> GetQuestion(CancellationToken cancellationToken, int questionId)
        {
            if (questionId <= 0)
            {
                return null;
            }
            return UnitOfWork.QuestionRepository.FindById(cancellationToken, questionId);
        }

        #endregion

        #region Add answer

        public async Task<WorkResult> AddAnswer(Answer answer, Question question)
        {
            if ((question == null) || (answer == null))
            {
                return WorkResult.Failed("Answer or question cannot be null");
            }
            try
            {
                if (!question.Answers.Contains(answer))
                {
                    question.Answers.Add(answer);
                    int result = await UnitOfWork.SaveChanges();
                    if (result > 0)
                    {
                        return WorkResult.Success();
                    }
                    return WorkResult.Failed("SaveChanges returned 0");
                }
                return WorkResult.Failed("Current question hold this answer");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        public async Task<WorkResult> AddAnswer(CancellationToken cancellationToken, Answer answer, Question question)
        {
            if ((question == null) || (answer == null))
            {
                return WorkResult.Failed("Answer or question cannot be null");
            }
            try
            {
                if (!question.Answers.Contains(answer))
                {
                    question.Answers.Add(answer);
                    int result = await UnitOfWork.SaveChanges(cancellationToken);
                    if (result > 0)
                    {
                        return WorkResult.Success();
                    }
                    return WorkResult.Failed("SaveChanges returned 0");
                }
                return WorkResult.Failed("Current question hold this answer");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        #endregion

        #region Remove answer

        public async Task<WorkResult> RemoveAnswer(Answer answer, Question question)
        {
            if ((question == null) || (answer == null))
            {
                return WorkResult.Failed("Answer or question cannot be null");
            }
            try
            {
                if (question.Answers.Contains(answer))
                {
                    question.Answers.Remove(answer);
                    int result = await UnitOfWork.SaveChanges();
                    if (result > 0)
                    {
                        return WorkResult.Success();
                    }
                    return WorkResult.Failed("SaveChanges returned 0");
                }
                return WorkResult.Failed("Current question not have this answer");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        public async Task<WorkResult> RemoveAnswer(CancellationToken cancellationToken, Answer answer, Question question)
        {
            if ((question == null) || (answer == null))
            {
                return WorkResult.Failed("Answer or question cannot be null");
            }
            try
            {
                if (question.Answers.Contains(answer))
                {
                    question.Answers.Remove(answer);
                    int result = await UnitOfWork.SaveChanges(cancellationToken);
                    if (result > 0)
                    {
                        return WorkResult.Success();
                    }
                    return WorkResult.Failed("SaveChanges returned 0");
                }
                return WorkResult.Failed("Current question not have this answer");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        #endregion

        #region Update answer

        public async Task<WorkResult> UpdateAnswer(Answer answer)
        {
            if (answer == null)
            {
                return WorkResult.Failed("Answer cannot be null");
            }
            try
            {
                UnitOfWork.AnswerRepository.Update(answer);
                int result = await UnitOfWork.SaveChanges();
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        public async Task<WorkResult> UpdateAnswer(CancellationToken cancellationToken, Answer answer)
        {
            if (answer == null)
            {
                return WorkResult.Failed("Answer cannot be null");
            }
            try
            {
                UnitOfWork.AnswerRepository.Update(answer);
                int result = await UnitOfWork.SaveChanges(cancellationToken);
                if (result > 0)
                {
                    return WorkResult.Success();
                }
                return WorkResult.Failed("SaveChanges returned 0");
            }
            catch (Exception ex)
            {
                return WorkResult.Failed(ex.Message);
            }
        }

        #endregion

        #region Get answer

        public Task<Answer> GetAnswer(int answerId)
        {
            if (answerId <= 0)
            {
                return null;
            }
            return UnitOfWork.AnswerRepository.FindById(answerId);
        }

        public Task<Answer> GetAnswer(CancellationToken cancellationToken, int answerId)
        {
            if (answerId <= 0)
            {
                return null;
            }
            return UnitOfWork.AnswerRepository.FindById(cancellationToken, answerId);
        }

        #endregion

        #region Parse answer name

        public void ParseAnswerName(string questionName, out int pageId, out int questionId, out int? questionType, out int? answerId)
        {
            string pattern = @"\d+";

            var pIdMatch = Regex.Matches(questionName, pattern)[0].Value;
            pageId = Convert.ToInt32(pIdMatch);
            var qIdMatch = Regex.Matches(questionName, pattern)[1].Value;
            questionId = Convert.ToInt32(qIdMatch);

            try
            {
                var qTypeMatch = Regex.Matches(questionName, pattern)[2].Value;
                questionType = Convert.ToInt32(qTypeMatch);
                var aIdMatch = Regex.Matches(questionName, pattern)[3].Value;
                answerId = Convert.ToInt32(aIdMatch);
            }
            catch (Exception)
            {
                questionType = null;
                answerId = null;
            }
        }
        #endregion
    }
}
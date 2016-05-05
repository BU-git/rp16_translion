using Xunit;
using Moq;
using IDAL.Models;
using System.Collections.Generic;
using IDAL.Interfaces;
using IDAL.Interfaces.IRepositories;
using BLL.Services.TestService;
using System.Threading;
using System;
using System.Linq;

namespace Tests.ServicesTests
{
    /// <summary>
    /// Test manager tests
    /// </summary>
    public class BLL_TestService_Tests
    {
        #region Lists
        private List<Page> _pages = new List<Page>
        {
            new Page { Id = 1, Name = "Simple name1", Order = 7, Questions = new List<Question>() },
            new Page { Id = 2, Name = "Simple name2", Order = 6, Questions = new List<Question>() },
            new Page { Id = 3, Name = "Simple name3", Order = 5, Questions = new List<Question>() },
            new Page { Id = 4, Name = "Simple name4", Order = 4, Questions = new List<Question>() },
            new Page { Id = 5, Name = "Simple name5", Order = 3, Questions = new List<Question>() },
            new Page { Id = 6, Name = "Simple name6", Order = 2, Questions = new List<Question>() },
            new Page { Id = 7, Name = "Simple name7", Order = 1, Questions = new List<Question>() }
        };

        private List<Page> _addPages = new List<Page>
        {
            new Page { Id = 8, Name = "Simple name6", Order = 24, Questions = new List<Question>() },
            new Page { Id = 9, Name = "Simple name7", Order = 35, Questions = new List<Question>() }
        };

        private List<Question> _addQuestions = new List<Question>
        {
            new Question { Id = 1, QuestionName = "New Name", TypeAnswer = "Unknown"},
            new Question { Id = 2, QuestionName = "New Name", TypeAnswer = "Unknown"}
        };

        private List<Answer> _addAnswers = new List<Answer>
        {
            new Answer { Id = 1, Name = "AAAAAA" },
            new Answer { Id = 2, Name = "BBBBBB" }
        };
        #endregion

        #region Get all pages tests
        [Fact]
        public void Get_all_pages_returns_pages()
        {
            //arrange
            Mock<IPageRepository> pageRepo = new Mock<IPageRepository>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

            pageRepo.Setup(rep => rep.GetAll()).ReturnsAsync(_pages);
            pageRepo.Setup(rep => rep.GetAll(It.IsAny<CancellationToken>())).ReturnsAsync(_pages);
            uow.Setup(u => u.PageRepository).Returns(pageRepo.Object);

            TestManager manager = new TestManager(uow.Object);

            //act
            var allPages = manager.GetAllPages().Result;
            var allPagesToken = manager.GetAllPages(default(CancellationToken)).Result;

            //assert
            Assert.Equal(_pages, allPages);
            Assert.Equal(_pages, allPagesToken);
        }
        #endregion

        #region Add pages test
        [Fact]
        public void AddPages_returns_failed_result_if_pages_are_null()
        {
            //arrange
            Mock<IPageRepository> pageRepo = new Mock<IPageRepository>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            pageRepo.Setup(rep => rep.AddRange(null)).Throws<ArgumentNullException>();
            pageRepo.Setup(rep => rep.AddRange(It.IsNotNull<IEnumerable<Page>>()));
            uow.Setup(u => u.PageRepository).Returns(pageRepo.Object);
            TestManager manager = new TestManager(uow.Object);
            //act
            var nullPagesAdd = manager.AddPages(null).Result;
            //assert
            Assert.False(nullPagesAdd.Succeeded);
        }

        [Fact]
        public void AddPages_returns_success_result_if_pages_are_empty()
        {
            //arrange
            Mock<IPageRepository> pageRepo = new Mock<IPageRepository>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            pageRepo.Setup(rep => rep.AddRange(null)).Throws<ArgumentNullException>();
            pageRepo.Setup(rep => rep.AddRange(It.IsNotNull<IEnumerable<Page>>()));
            uow.Setup(u => u.PageRepository).Returns(pageRepo.Object);
            TestManager manager = new TestManager(uow.Object);
            //act
            var emptyPagesAdd = manager.AddPages(new Page[] { }).Result;
            //assert
            Assert.True(emptyPagesAdd.Succeeded);
        }

        [Fact]
        public void AddPages_returns_success_result_if_pages_are_not_empty()
        {
            //arrange
            Mock<IPageRepository> pageRepo = new Mock<IPageRepository>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

            pageRepo.Setup(rep => rep.AddRange(It.IsNotNull<IEnumerable<Page>>()))
                .Callback<IEnumerable<Page>>(pages => _pages.AddRange(pages));
           
            uow.Setup(u => u.PageRepository).Returns(pageRepo.Object);

            TestManager manager = new TestManager(uow.Object);

            var oldCount = _pages.Count;
            //act

            var emptyPagesAdd = manager.AddPages(_addPages).Result;

            //assert
            Assert.True(emptyPagesAdd.Succeeded);
            Assert.Equal(_pages.Count, oldCount + _addPages.Count);
            Assert.True(_pages.Any(p => _addPages.Contains(p)));
            pageRepo.Verify(p => p.AddRange(It.IsNotNull<IEnumerable<Page>>()));
        }
        #endregion

        #region Get pages by id
        [Theory]
        [InlineData(-20)]
        [InlineData(-10)]
        [InlineData(500)]
        [InlineData(0)]
        public void GetPagesById_returns_null_if_id_is_not_valid_or_there_arent_page_with_id(int id)
        {
            //arrange
            Mock<IPageRepository> pageRepo = new Mock<IPageRepository>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

            pageRepo.Setup(rep => rep.FindById(It.Is<int>(e => e < 0))).ReturnsAsync(null);
            pageRepo.Setup(rep => rep.FindById(It.IsIn(_pages.Select(p => p.Id))))
                .ReturnsAsync(_pages.Find(p => p.Id == id));
            pageRepo.Setup(rep => rep.FindById(It.IsNotIn(_pages.Select(p => p.Id))))
                .ReturnsAsync(null);

            uow.Setup(u => u.PageRepository).Returns(pageRepo.Object);
            TestManager manager = new TestManager(uow.Object);
            //act
            var page = manager.GetPageById(id).Result;
            //assert
            Assert.Null(page);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(7)]
        [InlineData(5)]
        public void GetPagesById_returns_page_if_there_are_page_with_selected_id(int id)
        {
            //arrange
            Mock<IPageRepository> pageRepo = new Mock<IPageRepository>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

            pageRepo.Setup(rep => rep.FindById(It.IsIn(_pages.Select(p => p.Id))))
                .ReturnsAsync(_pages.Find(p => p.Id == id))
                .Verifiable();

            uow.Setup(u => u.PageRepository).Returns(pageRepo.Object);
            TestManager manager = new TestManager(uow.Object);
            //act
            var page = manager.GetPageById(id).Result;
            //assert
            Assert.NotNull(page);
        }
        #endregion

        #region Create page
        [Fact]
        public void CreatePage_returns_failed_result_if_page_is_null()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            TestManager manager = new TestManager(uow.Object);
            //act
            var pageAddResult = manager.CreatePage(null).Result;
            //assert
            Assert.False(pageAddResult.Succeeded);
        }

        
        [Fact]
        public void CreatePage_returns_failed_if_no_changes()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IPageRepository> repo = new Mock<IPageRepository>();

            uow.Setup(u => u.SaveChanges()).ReturnsAsync(0);
            repo.Setup(r => r.Add(It.IsNotNull<Page>()));
            uow.Setup(u => u.PageRepository).Returns(repo.Object);

            TestManager manager = new TestManager(uow.Object);
            //act
            var addIfFailed = manager.CreatePage(new Page()).Result;
            //assert
            Assert.False(addIfFailed.Succeeded);
            Assert.NotNull(addIfFailed.Errors);
            Assert.Equal("SaveChanges returned 0", addIfFailed.Errors.ElementAt(0));
        }

        [Fact]
        public void CreatePage_returns_succeeded_if_add_new_page()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IPageRepository> repo = new Mock<IPageRepository>();

            repo.Setup(r => r.Add(It.IsNotNull<Page>()))
                .Callback<Page>(p => _pages.Add(p))
                .Verifiable();
            uow.Setup(u => u.SaveChanges())
                .ReturnsAsync(1)
                .Verifiable();
            uow.Setup(u => u.PageRepository).Returns(repo.Object);

            TestManager manager = new TestManager(uow.Object);

            var pagesCount = _pages.Count();
            //act

            var addResult = manager.CreatePage(new Page()).Result;
            //assert
            Assert.True(addResult.Succeeded);
            Assert.Equal(pagesCount + 1, _pages.Count);
        }
        #endregion

        #region Update page
        [Fact]
        public void UpdatePage_returns_failed_result_if_page_is_null()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.UpdatePage(null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void UpdatePage_returns_failed_if_no_changes()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IPageRepository> repo = new Mock<IPageRepository>();

            uow.Setup(u => u.SaveChanges()).ReturnsAsync(0);
            repo.Setup(r => r.Update(It.IsNotNull<Page>()));
            uow.Setup(u => u.PageRepository).Returns(repo.Object);

            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.UpdatePage(new Page()).Result;
            //assert
            Assert.False(result.Succeeded);
            Assert.NotNull(result.Errors);
            Assert.Equal("SaveChanges returned 0", result.Errors.ElementAt(0));
        }

        [Fact]
        public void UpdatePage_returns_succeeded_if_update_page()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IPageRepository> repo = new Mock<IPageRepository>();

            repo.Setup(r => r.Update(It.Is<Page>(p => _pages.Any(page => p.Id == page.Id))))
                .Verifiable();

            uow.Setup(u => u.SaveChanges())
                .ReturnsAsync(1)
                .Verifiable();

            uow.Setup(u => u.PageRepository).Returns(repo.Object);

            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.UpdatePage(_pages[0]).Result;
            //assert
            Assert.True(result.Succeeded);
        }
        #endregion

        #region Delete all pages
        [Fact]
        public void DeleteAllPages_returns_failed_result_if_pages_are_null()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IPageRepository> repo = new Mock<IPageRepository>();

            repo.Setup(r => r.GetAll()).ReturnsAsync(null).Verifiable();
            uow.Setup(u => u.PageRepository).Returns(repo.Object);

            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.DeleteAllPages().Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteAllPages_returns_failed_result_if_pages_are_empty()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IPageRepository> repo = new Mock<IPageRepository>();

            repo.Setup(r => r.GetAll()).ReturnsAsync(new List<Page> { }).Verifiable();
            uow.Setup(u => u.PageRepository).Returns(repo.Object);

            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.DeleteAllPages().Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeleteAllPages_returns_failed_if_no_changes()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IPageRepository> repo = new Mock<IPageRepository>();

            repo.Setup(r => r.GetAll()).ReturnsAsync(_pages).Verifiable();
            uow.Setup(u => u.SaveChanges()).ReturnsAsync(0);
            uow.Setup(u => u.PageRepository).Returns(repo.Object);

            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.DeleteAllPages().Result;
            //assert
            Assert.False(result.Succeeded);
            Assert.NotNull(result.Errors);
            Assert.Equal("SaveChanges returned 0", result.Errors.ElementAt(0));
        }

        [Fact]
        public void DeleteAllPages_returns_succeeded_if_deleted_all_pages()
        {
            //arrange
            var pages = new List<Page> { new Page(), new Page(), new Page { } };
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IPageRepository> repo = new Mock<IPageRepository>();

            repo.Setup(r => r.GetAll())
                .ReturnsAsync(pages)
                .Verifiable();

            repo.Setup(r => r.Remove(It.IsAny<Page>()))
                .Callback<Page>(p => pages.Remove(p))
                .Verifiable();


            uow.Setup(u => u.PageRepository)
                .Returns(repo.Object);

            uow.Setup(u => u.SaveChanges())
                .ReturnsAsync(1) //doesn't matter => must be greater than 0
                .Verifiable();

            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.DeleteAllPages().Result;
            //assert
            repo.Verify();
            uow.Verify();
            Assert.True(result.Succeeded);
            Assert.Equal(0, pages.Count);
        }
        #endregion

        #region Delete page
        [Fact]
        public void DeletePage_returns_failed_result_if_page_is_null()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.DeletePage(null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void DeletePage_returns_failed_if_no_changes()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IPageRepository> repo = new Mock<IPageRepository>();

            repo.Setup(r => r.Remove(It.IsNotNull<Page>()))
                .Verifiable();

            uow.Setup(u => u.SaveChanges()).ReturnsAsync(0);
            uow.Setup(u => u.PageRepository).Returns(repo.Object);

            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.DeletePage(new Page()).Result;
            //assert
            Assert.False(result.Succeeded);
            Assert.NotNull(result.Errors);
            Assert.Equal("SaveChanges returned 0", result.Errors.ElementAt(0));
        }

        [Fact]
        public void DeletePage_returns_succeeded_if_update_page()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IPageRepository> repo = new Mock<IPageRepository>();

            repo.Setup(r => r.Remove(It.Is<Page>(p => _pages.Any(page => p.Id == page.Id))))
                .Callback<Page>(p => _pages.Remove(p))
                .Verifiable();

            uow.Setup(u => u.SaveChanges())
                .ReturnsAsync(1)
                .Verifiable();

            uow.Setup(u => u.PageRepository).Returns(repo.Object);

            TestManager manager = new TestManager(uow.Object);

            var currentCount = _pages.Count;
            var pageToDelete = _pages.Last();
            //act
            var result = manager.DeletePage(pageToDelete).Result;
            //assert
            Assert.True(result.Succeeded);
            Assert.Equal(currentCount - 1, _pages.Count);
            Assert.False(_pages.Contains(pageToDelete));
        }
        #endregion


        #region Add question
        [Fact]
        public void AddQuestion_failed_if_question_is_null()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.AddQuestion(null, new Page { }).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddQuestion_failed_if_page_is_null()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.AddQuestion(new Question() { }, null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddQuestion_failed_if_page_contains_question()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            TestManager manager = new TestManager(uow.Object);
            var testQuestion = new Question();
            var pageToTest = new Page { Questions = new List<Question> { testQuestion } };
            //act
            var result = manager.AddQuestion(testQuestion, pageToTest).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddQuestion_failed_if_no_changes_occured()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

            uow.Setup(u => u.SaveChanges()).ReturnsAsync(0);

            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.AddQuestion(new Question { }, _pages[0]).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddQuestion_succeded_if_page_and_question_valid()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IQuestionRepository> repo = new Mock<IQuestionRepository>();

            repo.Setup(r => r.Add(It.IsNotNull<Question>()))
                .Callback<Question>(q => _pages[0].Questions.Add(q))
                .Verifiable();
            uow.Setup(u => u.SaveChanges()).ReturnsAsync(1).Verifiable();

            TestManager manager = new TestManager(uow.Object);
            var questionsToModify = _pages[0].Questions;
            var expectedCount = questionsToModify.Count + 1;
            //act
            var result = manager.AddQuestion(new Question { }, _pages[0]).Result;
            //assert
            Assert.True(result.Succeeded);
            Assert.Equal(expectedCount, questionsToModify.Count);
        }
        #endregion

        #region Remove question
        [Fact]
        public void RemoveQuestion_failed_if_question_is_null()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.RemoveQuestion(null, new Page { }).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void RemoveQuestion_failed_if_page_is_null()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.RemoveQuestion(new Question() { }, null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddQuestion_failed_if_page_not_contains_question()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            TestManager manager = new TestManager(uow.Object);
            var testQuestion = new Question();
            var pageToTest = new Page { Questions = new List<Question> { } };
            //act
            var result = manager.RemoveQuestion(testQuestion, pageToTest).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void RemoveQuestion_failed_if_no_changes_occured()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

            uow.Setup(u => u.SaveChanges()).ReturnsAsync(0);

            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.RemoveQuestion(new Question { }, _pages[0]).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void RemoveQuestion_succeded_if_page_and_question_valid()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IQuestionRepository> repo = new Mock<IQuestionRepository>();

            repo.Setup(r => r.Add(It.IsNotNull<Question>()))
                .Callback<Question>(q => _pages[0].Questions.Remove(q))
                .Verifiable();
            uow.Setup(u => u.SaveChanges()).ReturnsAsync(1).Verifiable();

            TestManager manager = new TestManager(uow.Object);

            var questionToDelete = new Question();
            var questionsToModify = _pages[0].Questions;
            questionsToModify.Add(questionToDelete); //adding test question
            var expectedCount = questionsToModify.Count - 1;
            //act
            var result = manager.RemoveQuestion(questionToDelete, _pages[0]).Result; //removing test question
            //assert
            Assert.True(result.Succeeded);
            Assert.Equal(expectedCount, questionsToModify.Count);
        }
        #endregion

        #region Update question
        [Fact]
        public void UpdateQuestion_failed_if_question_is_null()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.UpdateQuestion(null).Result;
            //assert
            Assert.False(result.Succeeded);
        }
        #endregion

        #region Get question
        [Theory]
        [InlineData(-20)]
        [InlineData(-10)]
        [InlineData(500)]
        [InlineData(0)]
        public void GetQuestionById_returns_null_if_id_is_not_valid_or_there_arent_question_with_id(int id)
        {
            //arrange
            Mock<IQuestionRepository> repo = new Mock<IQuestionRepository>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

            //repo.Setup(rep => rep.FindById(It.Is<int>(e => e <= 0))).ReturnsAsync(null);
            repo.Setup(rep => rep.FindById(It.IsNotIn(_addQuestions.Select(q => q.Id).ToArray())))
                .ReturnsAsync(null);

            uow.Setup(u => u.QuestionRepository).Returns(repo.Object);
            TestManager manager = new TestManager(uow.Object);
            //act
            var page = manager.GetQuestion(id).Result;
            //assert
            Assert.Null(page);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetQuestionById_returns_question_if_there_are_question_with_selected_id(int id)
        {
            //arrange
            Mock<IQuestionRepository> repo = new Mock<IQuestionRepository>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

            repo.Setup(rep => rep.FindById(It.IsIn(_addQuestions.Select(q => q.Id))))
                .ReturnsAsync(_addQuestions.Find(q => q.Id == id))
                .Verifiable();

            uow.Setup(u => u.QuestionRepository).Returns(repo.Object);
            TestManager manager = new TestManager(uow.Object);
            //act
            var page = manager.GetQuestion(id).Result;
            //assert
            Assert.NotNull(page);
        }
        #endregion

        #region Add answer
        [Fact]
        public void AddAnswer_failed_if_answer_is_null()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.AddAnswer(null, new Question { }).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddAnswer_failed_if_question_is_null()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.AddAnswer(new Answer() { }, null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddAnswer_failed_if_question_contains_answer()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            TestManager manager = new TestManager(uow.Object);
            var testAnswer = new Answer();
            var questionToTest = new Question { Answers = new List<Answer> { testAnswer } };
            //act
            var result = manager.AddAnswer(testAnswer, questionToTest).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddAnswer_failed_if_no_changes_occured()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

            uow.Setup(u => u.SaveChanges()).ReturnsAsync(0);

            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.AddAnswer(new Answer { }, _addQuestions[0]).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void AddAnswer_succeded_if_answer_and_question_valid()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IAnswerRepository> repo = new Mock<IAnswerRepository>();

            repo.Setup(r => r.Add(It.IsNotNull<Answer>()))
                .Callback<Answer>(a => _addQuestions[0].Answers.Add(a))
                .Verifiable();

            uow.Setup(u => u.SaveChanges()).ReturnsAsync(1).Verifiable();

            TestManager manager = new TestManager(uow.Object);
            var answersToModify = _addQuestions[0].Answers;
            var expectedCount = answersToModify.Count + 1;
            //act
            var result = manager.AddAnswer(new Answer(), _addQuestions[0]).Result;
            //assert
            Assert.True(result.Succeeded);
            Assert.Equal(expectedCount, answersToModify.Count);
        }
        #endregion

        #region Remove answer
        [Fact]
        public void RemoveAnswer_failed_if_answer_is_null()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.RemoveAnswer(null, new Question { }).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void RemoveAnswer_failed_if_question_is_null()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.RemoveAnswer(new Answer() { }, null).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void RemoveAnswer_failed_if_question_not_contains_answer()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            TestManager manager = new TestManager(uow.Object);
            var testAnswer = new Answer();
            var questionToTest = new Question { Answers = new List<Answer> { } };
            //act
            var result = manager.RemoveAnswer(testAnswer, questionToTest).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void RemoveAnswer_failed_if_no_changes_occured()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

            uow.Setup(u => u.SaveChanges()).ReturnsAsync(0);


            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.AddAnswer(new Answer { }, _addQuestions[0]).Result;
            //assert
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void RemoveAnswer_succeded_if_answer_and_question_valid()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

            uow.Setup(u => u.SaveChanges()).ReturnsAsync(1).Verifiable();

            TestManager manager = new TestManager(uow.Object);

            var answerToRemove = new Answer { Id = 40 };
            _addQuestions[0].Answers.Add(answerToRemove);
            var expectedCount = _addQuestions[0].Answers.Count - 1;
            //act
            var result = manager.RemoveAnswer(answerToRemove, _addQuestions[0]).Result;
            //assert
            uow.Verify();
            Assert.Equal(expectedCount, _addQuestions[0].Answers.Count);
            Assert.True(result.Succeeded);

        }
        #endregion

        #region Update answer
        [Fact]
        public void UpdateUnswer_failed_if_answer_is_null()
        {
            //arrange
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            TestManager manager = new TestManager(uow.Object);
            //act
            var result = manager.UpdateAnswer(null).Result;
            //assert
            Assert.False(result.Succeeded);
        }
        #endregion

        #region Get answer
        [Theory]
        [InlineData(-20)]
        [InlineData(-9)]
        [InlineData(1204)]
        [InlineData(0)]
        public void GetAnswerById_returns_null_if_id_is_not_valid_or_there_arent_answer_with_id(int id)
        {
            //arrange
            Mock<IAnswerRepository> repo = new Mock<IAnswerRepository>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

            repo.Setup(rep => rep.FindById(It.Is<int>(e => e <= 0))).ReturnsAsync(null);
            //repo.Setup(rep => rep.FindById(It.IsNotIn(_addAnswers.Select(q => q.Id))))
             //   .ReturnsAsync(null);

            uow.Setup(u => u.AnswerRepository).Returns(repo.Object);
            TestManager manager = new TestManager(uow.Object);
            //act
            var page = manager.GetAnswer(id);
            //assert
            Assert.Null(page);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void GetAnswerById_returns_answer_if_there_are_answer_with_selected_id(int id)
        {
            //arrange
            Mock<IAnswerRepository> repo = new Mock<IAnswerRepository>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

            repo.Setup(rep => rep.FindById(It.IsAny<int>()))
                .ReturnsAsync(_addAnswers.Find(q => q.Id == id))
                .Verifiable();

            uow.Setup(u => u.AnswerRepository).Returns(repo.Object);
            TestManager manager = new TestManager(uow.Object);
            //act
            var page = manager.GetAnswer(id).Result;
            //assert
            Assert.NotNull(page);
        }
        #endregion
    }
}

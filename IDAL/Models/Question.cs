using System.Collections.Generic;

namespace IDAL.Models
{
    public class Question
    {
        public Question()
        {
            Answers = new List<Answer>();
        }

        public int Id { get; set; }
    
        #region Properties
        public string QuestionName { get; set; }

        public int PageId { get; set; }

        public string TypeAnswer { get; set; }

        public int AnswersCount { get; set; }
        #endregion

        #region Navigation properties
        public virtual Page Page { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Answer> UserAnswers { get; set; } 
        #endregion
    }
}

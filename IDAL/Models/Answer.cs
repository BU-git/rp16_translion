namespace IDAL.Models
{
    public class Answer
    {
        public int Id { get; set; }

        #region Properties
        public string Name { get; set; }

        public int QuestionId { get; set; }
        #endregion

        #region Navigation properties
        public virtual Question Question { get; set; }
        #endregion
    }
}

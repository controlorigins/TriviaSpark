namespace TriviaSpark.Web.Areas.Identity.Data
{
    public class MatchQuestionAnswer : BaseEntity
    {
        public MatchQuestionAnswer(Question theQuestion, QuestionAnswer theAnswer)
        {
            Question = theQuestion;
            Answer = theAnswer;
        }
        public MatchQuestionAnswer()
        {

        }
        public string QuestionId { get; set; }
        public int AnswerId { get; set; }
        public int MatchId { get; set; }
        public Match Match { get; set; }
        public Question Question { get; set; }
        public QuestionAnswer Answer { get; set; }

    }
}


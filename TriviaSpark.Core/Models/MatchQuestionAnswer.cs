namespace TriviaSpark.Core.Models
{
    public class MatchQuestionAnswerModel
    {
        public MatchQuestionAnswerModel(QuestionModel theQuestion, QuestionAnswerModel theAnswer)
        {
            Question = theQuestion;
            Answer = theAnswer;
        }
        public MatchQuestionAnswerModel()
        {

        }
        public string QuestionId { get; set; }
        public int AnswerId { get; set; }
        public int MatchId { get; set; }
        public MatchModel Match { get; set; }
        public QuestionModel Question { get; set; }
        public QuestionAnswerModel Answer { get; set; }

    }
}


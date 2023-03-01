namespace TriviaSpark.Core.Models
{
    public class MatchQuestionModel
    {
        public string QuestionId { get; set; }
        public int MatchId { get; set; }
        public QuestionModel Question { get; set; }
        public MatchModel Match { get; set; }

    }
}


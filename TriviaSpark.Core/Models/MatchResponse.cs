
namespace TriviaSpark.Core.Models
{
    public class MatchResponse
    {
        public List<Question> Questions { get; set; } = new List<Question>();
        public List<QuestionAnswer> Answers { get; set; } = new List<QuestionAnswer>();

    }
}

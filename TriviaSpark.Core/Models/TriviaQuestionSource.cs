
namespace TriviaSpark.Core.Models
{
    public class TriviaQuestionSource
    {
        public List<TriviaQuestion> Questions { get; set; } = new List<TriviaQuestion>();
        public List<TriviaQuestionAnswer> Answers { get; set; } = new List<TriviaQuestionAnswer>();

    }
}

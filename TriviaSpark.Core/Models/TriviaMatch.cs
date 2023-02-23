
namespace TriviaSpark.Web.Models.Trivia
{
    public class TriviaMatch
    {
        public List<TriviaQuestion> TriviaQuestions { get; set; } = new List<TriviaQuestion>();
        public List<TriviaAnswer> TriviaAnswers { get; set; } = new List<TriviaAnswer>();

    }
}

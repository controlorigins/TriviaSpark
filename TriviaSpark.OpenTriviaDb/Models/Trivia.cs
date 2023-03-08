using TriviaSpark.Core.Match;

namespace TriviaSpark.OpenTriviaDb.Models
{
    public class Trivia
    {
        public string category { get; set; }
        public string correct_answer { get; set; }
        public Difficulty difficulty { get; set; }
        public string[] incorrect_answers { get; set; }
        public string question { get; set; }
        public QuestionType type { get; set; }
    }
}

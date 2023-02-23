
namespace TriviaSpark.Web.Models.Trivia
{
    public class TriviaQuestion
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string CorrectAnswer { get; set; }
        public string Difficulty { get; set; }
        public string[] IncorrectAnswers { get; set; }
        public string Question { get; set; }
        public string Type { get; set; }
    }
}

namespace TriviaSpark.OpenTriviaDb.Models
{
    public class OpenTBbResponse
    {
        public int response_code { get; set; }
        public Trivia[] results { get; set; }
    }
}

namespace TriviaSpark.Core.OpenTriviaDb;

public class OpenTBbResponse
{
    public int response_code { get; set; }
    public OpenTriviaDb.Trivia[] results { get; set; }
}

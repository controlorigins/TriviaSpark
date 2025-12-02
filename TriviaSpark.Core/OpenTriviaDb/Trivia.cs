using System.Diagnostics.CodeAnalysis;

namespace TriviaSpark.Core.OpenTriviaDb;

public class Trivia
{
    [SetsRequiredMembers]
    public Trivia()
    {
        category = string.Empty;
        type = string.Empty;
        difficulty = string.Empty;
        question = string.Empty;
        correct_answer = string.Empty;
        incorrect_answers = Array.Empty<string>();
    }

    public required string category { get; set; }
    public required string type { get; set; }
    public required string difficulty { get; set; }
    public required string question { get; set; }
    public required string correct_answer { get; set; }
    public required string[] incorrect_answers { get; set; }
}

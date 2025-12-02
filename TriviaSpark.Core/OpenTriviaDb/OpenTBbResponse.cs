using System.Diagnostics.CodeAnalysis;

namespace TriviaSpark.Core.OpenTriviaDb;

public class OpenTBbResponse
{
    [SetsRequiredMembers]
    public OpenTBbResponse()
    {
        results = Array.Empty<Trivia>();
    }

    public int response_code { get; set; }
    public required OpenTriviaDb.Trivia[] results { get; set; }
}

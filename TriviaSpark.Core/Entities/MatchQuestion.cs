using System.Diagnostics.CodeAnalysis;

namespace TriviaSpark.Core.Entities;

public class MatchQuestion : BaseEntity
{
    [SetsRequiredMembers]
    public MatchQuestion()
    {
        QuestionId = string.Empty;
        Question = null!;
        Match = null!;
    }

    public required string QuestionId { get; set; }
    public int MatchId { get; set; }
    public required virtual Entities.Question Question { get; set; }
    public required virtual Entities.Match Match { get; set; }
}


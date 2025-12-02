using System.Diagnostics.CodeAnalysis;

namespace TriviaSpark.Core.Entities;

public class MatchQuestionAnswer : BaseEntity
{
    [SetsRequiredMembers]
    public MatchQuestionAnswer()
    {
        QuestionId = string.Empty;
        Match = null!;
        Question = null!;
        Answer = null!;
    }
    
    public required string QuestionId { get; set; }
    public int AnswerId { get; set; }
    public int MatchId { get; set; }
    public required virtual Match Match { get; set; }
    public required virtual Entities.Question Question { get; set; }
    public required virtual QuestionAnswer Answer { get; set; }
    public string? Comment { get; set; }
    public int ElapsedTime { get; set; }
}


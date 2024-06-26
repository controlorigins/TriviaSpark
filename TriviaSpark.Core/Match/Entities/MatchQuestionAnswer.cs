namespace TriviaSpark.Core.Match.Entities;

public class MatchQuestionAnswer : Entities.BaseEntity
{
    public MatchQuestionAnswer()
    {

    }
    public string QuestionId { get; set; }
    public int AnswerId { get; set; }
    public int MatchId { get; set; }
    public virtual Entities.Match Match { get; set; }
    public virtual Entities.Question Question { get; set; }
    public virtual Entities.QuestionAnswer Answer { get; set; }
    public string? Comment { get; set; }
    public int ElapsedTime { get; set; }
}


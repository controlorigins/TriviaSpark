namespace TriviaSpark.Core.Entities;

public class MatchQuestion : BaseEntity
{
    public string QuestionId { get; set; }
    public int MatchId { get; set; }
    public virtual Entities.Question Question { get; set; }
    public virtual Entities.Match Match { get; set; }

}


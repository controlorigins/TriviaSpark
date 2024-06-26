using System.ComponentModel.DataAnnotations;
using TriviaSpark.Core.Match.Models;

namespace TriviaSpark.Core.Match.Entities;

public class Match : BaseEntity
{
    [Key]
    public int MatchId { get; set; }
    public string UserId { get; set; }
    public string MatchName { get; set; }
    public MatchMode MatchMode { get; set; }
    public Difficulty Difficulty { get; set; }
    public QuestionType QuestionType { get; set; }

    public virtual ICollection<Entities.MatchQuestion> MatchQuestions { get; set; }
    public virtual ICollection<MatchQuestionAnswer> MatchQuestionAnswers { get; set; }

    // Foreign key to User table
    public virtual Entities.TriviaSparkWebUser User { get; set; }
}


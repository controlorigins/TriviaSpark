using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TriviaSpark.Core.Entities;

public class Match : BaseEntity
{
    [SetsRequiredMembers]
    public Match()
    {
        UserId = string.Empty;
        MatchName = string.Empty;
        MatchQuestions = new HashSet<MatchQuestion>();
        MatchQuestionAnswers = new HashSet<MatchQuestionAnswer>();
        User = null!;
    }

    [Key]
    public int MatchId { get; set; }
    public required string UserId { get; set; }
    public required string MatchName { get; set; }
    public Models.MatchMode MatchMode { get; set; }
    public Models.Difficulty Difficulty { get; set; }
    public Models.QuestionType QuestionType { get; set; }

    public required virtual ICollection<MatchQuestion> MatchQuestions { get; set; }
    public required virtual ICollection<Entities.MatchQuestionAnswer> MatchQuestionAnswers { get; set; }

    // Foreign key to User table
    public required virtual TriviaSparkWebUser User { get; set; }
}


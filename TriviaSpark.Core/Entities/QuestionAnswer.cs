using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TriviaSpark.Core.Entities;

public class QuestionAnswer : BaseEntity
{
    [SetsRequiredMembers]
    public QuestionAnswer()
    {
        QuestionId = string.Empty;
        Question = null!;
        AnswerText = string.Empty;
        ErrorMessage = "No Trivia QuestionId specified.";
        MatchQuestionAnswers = new HashSet<MatchQuestionAnswer>();
        IsValid = false;
        IsCorrect = false;
    }

    [Key]
    public int AnswerId { get; set; }
    public required string QuestionId { get; set; }
    public required Entities.Question Question { get; set; }
    public required string AnswerText { get; set; }
    public bool IsCorrect { get; set; }
    public bool IsValid { get; set; }
    public int Value { get; set; }
    public required string ErrorMessage { get; set; }
    public required virtual ICollection<Entities.MatchQuestionAnswer> MatchQuestionAnswers { get; set; }
}


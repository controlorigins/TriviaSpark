using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace TriviaSpark.Core.Models;

public class QuestionAnswerModel
{
    [SetsRequiredMembers]
    public QuestionAnswerModel()
    {
        QuestionId = string.Empty;
        AnswerText = string.Empty;
        ErrorMessage = "No Trivia QuestionId specified.";
        ElapsedTime = string.Empty;
        IsValid = false;
        IsCorrect = false;
    }

    [Key]
    public int AnswerId { get; set; }
    public required string QuestionId { get; set; }
    public required string AnswerText { get; set; }
    public bool IsCorrect { get; set; }
    public bool IsValid { get; set; }
    public int Value { get; set; }
    public required string ErrorMessage { get; set; }
    public required string ElapsedTime { get; set; }
}


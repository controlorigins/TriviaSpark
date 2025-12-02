using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace TriviaSpark.Core.Entities;

public class Question : BaseEntity
{
    [SetsRequiredMembers]
    public Question()
    {
        QuestionId = string.Empty;
        QuestionText = string.Empty;
        Category = string.Empty;
        Source = string.Empty;
        MatchQuestions = new HashSet<MatchQuestion>();
        MatchQuestionAnswers = new HashSet<MatchQuestionAnswer>();
        Answers = new HashSet<QuestionAnswer>();
    }

    [Key]
    public required string QuestionId { get; set; }
    public required string QuestionText { get; set; }
    public required string Category { get; set; }
    public Models.Difficulty Difficulty { get; set; }
    public Models.QuestionType Type { get; set; }
    public required string Source { get; set; }
    public required virtual ICollection<MatchQuestion> MatchQuestions { get; set; }
    public required virtual ICollection<MatchQuestionAnswer> MatchQuestionAnswers { get; set; }
    public required virtual ICollection<QuestionAnswer> Answers { get; set; }

    [NotMapped]
    public string? CorrectAnswer
    {
        get
        {
            Answers ??= [];

            return Answers.Where(w => w.IsCorrect == true).Select(s => s.AnswerText).FirstOrDefault();
        }
    }

    [NotMapped]
    public List<string> IncorrectAnswers
    {
        get
        {
            Answers ??= [];

            return Answers.Where(w => w.IsCorrect == false).Select(s => s.AnswerText).ToList();
        }
    }
}


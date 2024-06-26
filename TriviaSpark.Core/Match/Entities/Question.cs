using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TriviaSpark.Core.Match.Models;

namespace TriviaSpark.Core.Match.Entities;

public class Question : Entities.BaseEntity
{
    [Key]
    public string QuestionId { get; set; }
    public string QuestionText { get; set; }
    public string Category { get; set; }
    public Difficulty Difficulty { get; set; }
    public QuestionType Type { get; set; }
    public string Source { get; set; }
    public virtual ICollection<Entities.MatchQuestion> MatchQuestions { get; set; }
    public virtual ICollection<MatchQuestionAnswer> MatchQuestionAnswers { get; set; }
    public virtual ICollection<Entities.QuestionAnswer> Answers { get; set; }

    [NotMapped]
    public string CorrectAnswer
    {
        get
        {
            Answers ??= new List<Entities.QuestionAnswer>();

            return Answers.Where(w => w.IsCorrect == true).Select(s => s.AnswerText).FirstOrDefault();
        }
    }

    [NotMapped]
    public List<string> IncorrectAnswers
    {
        get
        {
            Answers ??= new List<Entities.QuestionAnswer>();

            return Answers.Where(w => w.IsCorrect == false).Select(s => s.AnswerText).ToList();
        }
    }
}


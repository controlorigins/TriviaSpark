﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TriviaSpark.Core.Models;

public class QuestionModel : IComparable<Models.QuestionModel>, IQuestionModel, IEquatable<Models.QuestionModel>
{

    public static bool operator !=(Models.QuestionModel a, Models.QuestionModel b)
    {
        return !(a == b);
    }

    public static bool operator ==(Models.QuestionModel a, Models.QuestionModel b)
    {
        if (ReferenceEquals(a, b)) return true;

        if (a is null || b is null) return false;

        return a.QuestionId == b.QuestionId;
    }

    public void AddAnswer(string answerText, bool isCorrect)
    {
        Answers ??= [];

        Answers.Add(new QuestionAnswerModel()
        {
            AnswerText = answerText,
            IsCorrect = isCorrect,
            QuestionId = QuestionId,
            ErrorMessage = string.Empty,
            IsValid = true,
            Value = isCorrect ? 1 : 0
        });
    }

    public int CompareTo(Models.QuestionModel? other)
    {
        if (other == null)
        {
            return 1;
        }
        return string.Compare(QuestionId, other.QuestionId, StringComparison.Ordinal);
    }
    public override bool Equals(object obj)
    {
        if (obj == null || !(obj is Models.QuestionModel))
        {
            return false;
        }

        return QuestionId == ((Models.QuestionModel)obj).QuestionId;
    }

    public override int GetHashCode()
    {
        return QuestionId.GetHashCode();
    }

    public bool Equals(Models.QuestionModel? other)
    {
        if (other is null) return false;

        return QuestionId == other.QuestionId;
    }

    public ICollection<QuestionAnswerModel> Answers { get; set; }
    public string Category { get; set; }

    [NotMapped]
    public string CorrectAnswer
    {
        get
        {
            Answers ??= [];

            return Answers.Where(w => w.IsCorrect == true).Select(s => s.AnswerText).FirstOrDefault();
        }
    }
    public Difficulty Difficulty { get; set; }

    [NotMapped]
    public List<string> IncorrectAnswers
    {
        get
        {
            Answers ??= [];

            return Answers.Where(w => w.IsCorrect == false).Select(s => s.AnswerText).ToList();
        }
    }
    [Key]
    public string QuestionId { get; set; }
    public string QuestionText { get; set; }

    public string Source { get; set; }
    public QuestionType Type { get; set; }
}

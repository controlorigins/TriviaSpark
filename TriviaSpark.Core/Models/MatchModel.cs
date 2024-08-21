﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TriviaSpark.Core.Models;

public class MatchModel : IComparable<Models.MatchModel>, IEquatable<Models.MatchModel>
{

    public static bool operator !=(Models.MatchModel a, Models.MatchModel b)
    {
        return !(a == b);
    }

    public static bool operator ==(Models.MatchModel a, Models.MatchModel b)
    {
        if (ReferenceEquals(a, b)) return true;

        if (a is null || b is null) return false;

        return a.MatchId == b.MatchId;
    }

    public int CompareTo(Models.MatchModel? other)
    {
        if (other is null) return 1;

        return MatchId.CompareTo(other.MatchId);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not Models.MatchModel)
        {
            return false;
        }
        return MatchId == ((Models.MatchModel)obj).MatchId;
    }

    public bool Equals(Models.MatchModel? other)
    {
        if (other is null) return false;

        return MatchId == other.MatchId;
    }

    public override int GetHashCode()
    {
        return MatchId.GetHashCode();
    }

    public QuestionAnswerModel? CurrentAnswer { get; set; }

    public Models.QuestionModel? CurrentQuestion { get; set; }

    [DisplayName("Difficulty")]
    public Difficulty Difficulty { get; set; } = Difficulty.Easy;
    public DateTime MatchDate { get; set; }
    [Key]
    public int MatchId { get; set; }
    [DisplayName("Mode of Game Play")]
    public MatchMode MatchMode { get; set; }
    public string? MatchName { get; set; }
    public List<Models.MatchQuestionAnswerModel> MatchQuestionAnswers { get; set; } = [];
    public Services.QuestionProvider MatchQuestions { get; set; } = new();

    [DisplayName("Number of Questions")]
    [Range(0, 50, ErrorMessage = "Please use values between 0 to 50")]
    public int NumberOfQuestions { get; set; }

    [DisplayName("Question Type")]
    public Models.QuestionType QuestionType { get; set; } = Models.QuestionType.Multiple;
    public Models.ScoreCardModel? ScoreCard { get; set; }
    public UserModel? User { get; set; }
    public string? UserId { get; set; }
}

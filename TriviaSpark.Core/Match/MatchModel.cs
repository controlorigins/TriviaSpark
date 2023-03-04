using System.ComponentModel.DataAnnotations;
using TriviaSpark.Core.Questions;

namespace TriviaSpark.Core.Match
{
    public class MatchModel : IComparable<MatchModel>, IEquatable<MatchModel>
    {
        [Key]
        public int MatchId { get; set; }
        public string? MatchName { get; set; }
        public DateTime MatchDate { get; set; }
        public QuestionProvider MatchQuestions { get; set; } = new();
        public ICollection<MatchQuestionAnswerModel> MatchQuestionAnswers { get; set; } = new List<MatchQuestionAnswerModel>();
        public string? UserId { get; set; }
        public UserModel? User { get; set; }

        public QuestionModel? CurrentQuestion { get; set; }
        public QuestionAnswerModel? CurrentAnswer { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not MatchModel)
            {
                return false;
            }
            return MatchId == ((MatchModel)obj).MatchId;
        }

        public override int GetHashCode()
        {
            return MatchId.GetHashCode();
        }

        public int CompareTo(MatchModel? other)
        {
            if (other is null) return 1;

            return MatchId.CompareTo(other.MatchId);
        }

        public bool Equals(MatchModel? other)
        {
            if (other is null) return false;

            return MatchId==other.MatchId;
        }

        public static bool operator ==(MatchModel a, MatchModel b)
        {
            if (ReferenceEquals(a, b)) return true;

            if ((a is null) || (b is null)) return false;

            return a.MatchId == b.MatchId;
        }

        public static bool operator !=(MatchModel a, MatchModel b)
        {
            return !(a == b);
        }
    }
}


using System.ComponentModel.DataAnnotations;
using TriviaSpark.Core.Questions;

namespace TriviaSpark.Core.Match
{
    public class MatchModel : IComparable<MatchModel>
    {
        [Key]
        public int MatchId { get; set; }
        public string MatchName { get; set; }
        public DateTime MatchDate { get; set; }
        public QuestionProvider MatchQuestions { get; set; } = new();
        public ICollection<MatchQuestionAnswerModel> MatchQuestionAnswers { get; set; } = new List<MatchQuestionAnswerModel>();
        public string UserId { get; set; }
        public UserModel User { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is MatchModel))
            {
                return false;
            }

            return MatchId == ((MatchModel)obj).MatchId;
        }

        public override int GetHashCode()
        {
            return MatchId.GetHashCode();
        }

        public int CompareTo(MatchModel other)
        {
            if (other == null) return 1;

            return MatchId.CompareTo(other.MatchId);
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


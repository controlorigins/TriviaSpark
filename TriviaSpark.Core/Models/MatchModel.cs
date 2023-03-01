using System.ComponentModel.DataAnnotations;

namespace TriviaSpark.Core.Models
{
    public class MatchModel
    {
        [Key]
        public int MatchId { get; set; }
        public string MatchName { get; set; }
        public DateTime MatchDate { get; set; }
        public ICollection<MatchQuestionModel> MatchQuestions { get; set; }
        public ICollection<MatchQuestionAnswerModel> MatchQuestionAnswers { get; set; }

        // Foreign key to User table
        public string UserId { get; set; }
        public UserModel User { get; set; }
    }
}


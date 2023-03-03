using System.ComponentModel.DataAnnotations;

namespace TriviaSpark.Web.Areas.Identity.Data
{
    public class Match : BaseEntity
    {
        [Key]
        public int MatchId { get; set; }
        public string UserId { get; set; }
        public string MatchName { get; set; }
        public virtual ICollection<MatchQuestion> MatchQuestions { get; set; }
        public virtual ICollection<MatchQuestionAnswer> MatchQuestionAnswers { get; set; }

        // Foreign key to User table
        public virtual TriviaSparkWebUser User { get; set; }
    }
}


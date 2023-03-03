﻿using System.ComponentModel.DataAnnotations;

namespace TriviaSpark.Web.Areas.Identity.Data
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public class Match : BaseEntity
    {
        [Key]
        public int MatchId { get; set; }
        public string MatchName { get; set; }
        public DateTime MatchDate { get; set; }
        public ICollection<MatchQuestion> MatchQuestions { get; set; }
        public ICollection<MatchQuestionAnswer> MatchQuestionAnswers { get; set; }

        // Foreign key to User table
        public string UserId { get; set; }
        public TriviaSparkWebUser User { get; set; }
    }
}

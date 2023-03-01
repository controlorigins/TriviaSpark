namespace TriviaSpark.Web.Areas.Identity.Data
{
    public class MatchQuestion
    {
        public string QuestionId { get; set; }
        public int MatchId { get; set; }
        public Question Question { get; set; }
        public Match Match { get; set; }

    }
}


using Microsoft.AspNetCore.Identity;

namespace TriviaSpark.Web.Areas.Identity.Data;

// Add profile data for application users by adding properties to the TriviaSparkWebUser class
public class TriviaSparkWebUser : IdentityUser
{
    // Navigation property to QuestionProvider table
    public virtual ICollection<Match> Matches { get; set; }
}


using Microsoft.AspNetCore.Identity;

namespace TriviaSpark.Core.Match.Entities;

// Add profile data for application users by adding properties to the TriviaSparkWebUser class
public class TriviaSparkWebUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public byte[]? ProfilePicture { get; set; }

    // Navigation property to QuestionProvider table
    public virtual ICollection<Match> Matches { get; set; }
}

public enum Roles
{
    SuperAdmin,
    Admin,
    Moderator,
    Basic
}
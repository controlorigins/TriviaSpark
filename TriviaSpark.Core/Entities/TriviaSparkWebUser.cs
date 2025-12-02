using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;

namespace TriviaSpark.Core.Entities;

// Add profile data for application users by adding properties to the TriviaSparkWebUser class
public class TriviaSparkWebUser : IdentityUser
{
    [SetsRequiredMembers]
    public TriviaSparkWebUser()
    {
        Matches = new HashSet<Match>();
    }

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public byte[]? ProfilePicture { get; set; }

    // Navigation property to QuestionProvider table
    public required virtual ICollection<Entities.Match> Matches { get; set; }
}

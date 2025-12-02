using System.Diagnostics.CodeAnalysis;

namespace TriviaSpark.Core.Models;

public class UserModel
{
    [SetsRequiredMembers]
    public UserModel()
    {
        UserId = string.Empty;
        UserRoles = [];
    }

    public required string UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<string> UserRoles { get; set; } = [];
}
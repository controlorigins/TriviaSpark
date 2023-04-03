﻿namespace TriviaSpark.Core.Match
{
    public class UserModel
    {
        public string UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<string> UserRoles { get; set; } = new List<string>();
    }
}
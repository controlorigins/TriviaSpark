using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TriviaSpark.Core.Models;
using TriviaSpark.Web.Areas.Identity.Data;

namespace TriviaSpark.Web.Data;

public class TriviaSparkWebContext : IdentityDbContext<TriviaSparkWebUser>
{
    public TriviaSparkWebContext(DbContextOptions<TriviaSparkWebContext> options)
        : base(options)
    {
    }
    public DbSet<Match> TriviaMatches { get; set; }
    public DbSet<Question> TriviaQuestions { get; set; }
    public DbSet<MatchAnswer> TriviaMatchAnswers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<Match>()
            .HasOne(t => t.User)
            .WithMany(u => u.Matches)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Match>()
            .HasMany(m => m.Questions)
            .WithMany(q => q.Matches);

    }
}

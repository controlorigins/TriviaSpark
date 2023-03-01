using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TriviaSpark.Web.Areas.Identity.Data;

public class TriviaSparkWebContext : IdentityDbContext<TriviaSparkWebUser>
{
    public TriviaSparkWebContext(DbContextOptions<TriviaSparkWebContext> options)
        : base(options)
    {
    }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
    public DbSet<MatchQuestionAnswer> MatchAnswers { get; set; }
    public DbSet<MatchQuestion> MatchQuestions { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        builder.Entity<Match>()
            .HasOne(o => o.User)
            .WithMany(m => m.Matches)
            .HasForeignKey(f => f.UserId);

        builder.Entity<MatchQuestion>()
             .HasKey(mq => new { mq.QuestionId, mq.MatchId });

        builder.Entity<MatchQuestion>()
            .HasOne(mq => mq.Question)
            .WithMany(q => q.MatchQuestions)
            .HasForeignKey(mq => mq.QuestionId);

        builder.Entity<MatchQuestion>()
            .HasOne(mq => mq.Match)
            .WithMany(m => m.MatchQuestions)
            .HasForeignKey(mq => mq.MatchId);

        builder.Entity<MatchQuestionAnswer>()
             .HasKey(mq => new { mq.MatchId, mq.QuestionId, mq.AnswerId });

        builder.Entity<MatchQuestionAnswer>()
            .HasOne(o => o.Match)
            .WithMany(m => m.MatchQuestionAnswers)
            .HasForeignKey(f => f.MatchId);

        builder.Entity<MatchQuestionAnswer>()
            .HasOne(o => o.Question)
            .WithMany(m => m.MatchQuestionAnswers)
            .HasForeignKey(f => f.QuestionId);

        builder.Entity<MatchQuestionAnswer>()
            .HasOne(o => o.Answer)
            .WithMany(m => m.MatchQuestionAnswers)
            .HasForeignKey(f => f.AnswerId);

    }
}

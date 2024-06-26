using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TriviaSpark.Core.Match.Entities;

public class TriviaSparkWebContext : IdentityDbContext<Entities.TriviaSparkWebUser>
{
    public TriviaSparkWebContext(DbContextOptions<Entities.TriviaSparkWebContext> options)
        : base(options)
    {
    }
    public DbSet<Entities.Match> Matches { get; set; }
    public DbSet<Entities.Question> Questions { get; set; }
    public DbSet<Entities.QuestionAnswer> QuestionAnswers { get; set; }
    public DbSet<Entities.MatchQuestionAnswer> MatchAnswers { get; set; }
    public DbSet<Entities.MatchQuestion> MatchQuestions { get; set; }


    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ProcessBaseEntityFields();
        var result = await base.SaveChangesAsync(true, cancellationToken);
        return result;
    }
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        ProcessBaseEntityFields();
        var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        return result;
    }

    private void ProcessBaseEntityFields()
    {
        try
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Entities.BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((Entities.BaseEntity)entityEntry.Entity).ModifiedDate = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((Entities.BaseEntity)entityEntry.Entity).CreatedDate = DateTime.Now;
                }
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"ProcessBaseEntityFields Exception:{ex.Message}");
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Entities.Match>()
            .HasOne(o => o.User)
            .WithMany(m => m.Matches)
            .HasForeignKey(f => f.UserId);

        builder.Entity<Entities.MatchQuestion>()
             .HasKey(mq => new { mq.QuestionId, mq.MatchId });

        builder.Entity<Entities.MatchQuestion>()
            .HasOne(mq => mq.Question)
            .WithMany(q => q.MatchQuestions)
            .HasForeignKey(mq => mq.QuestionId);

        builder.Entity<Entities.MatchQuestion>()
            .HasOne(mq => mq.Match)
            .WithMany(m => m.MatchQuestions)
            .HasForeignKey(mq => mq.MatchId);

        builder.Entity<Entities.MatchQuestionAnswer>()
             .HasKey(mq => new { mq.MatchId, mq.QuestionId, mq.AnswerId });

        builder.Entity<Entities.MatchQuestionAnswer>()
            .HasOne(o => o.Match)
            .WithMany(m => m.MatchQuestionAnswers)
            .HasForeignKey(f => f.MatchId);

        builder.Entity<Entities.MatchQuestionAnswer>()
            .HasOne(o => o.Question)
            .WithMany(m => m.MatchQuestionAnswers)
            .HasForeignKey(f => f.QuestionId);

        builder.Entity<Entities.MatchQuestionAnswer>()
            .HasOne(o => o.Answer)
            .WithMany(m => m.MatchQuestionAnswers)
            .HasForeignKey(f => f.AnswerId);

    }
}

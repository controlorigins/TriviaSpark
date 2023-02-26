using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TriviaSpark.Core.Models;
using TriviaSpark.Web.Data;

namespace TriviaSpark.Web.Areas.Identity.Data;

// Add profile data for application users by adding properties to the TriviaSparkWebUser class
public class TriviaSparkWebUser : IdentityUser
{
    // Navigation property to MatchResponse table
    public ICollection<Match> Matches { get; set; }
}

public class Questions : Question
{
    public ICollection<Match> Matches { get; set; }
}

public class MatchAnswer
{
    [Key]
    public int Id { get; set; }
    public string QuestionId { get; set; }
    public int TriviaMatchId { get; set; }
    public Match TriviaMatch { get; set; }
    public Question Question { get; set; }
    public QuestionAnswers Answers { get; set; }

}


public class Match
{
    [Key]
    public int Id { get; set; }
    public string MatchName { get; set; }
    public DateTime MatchDate { get; set; }
    public ICollection<Questions> Questions { get; set; }
    public ICollection<QuestionAnswer> Answers { get; set; }

    // Foreign key to User table
    public string UserId { get; set; }
    public TriviaSparkWebUser User { get; set; }

    public Questions AddQuestion(Question question)
    {
        var newQuestion = new Questions()
        {
            Id = question.Id,
            Category = question.Category,
            Difficulty = question.Difficulty,
            QuestionNm = question.QuestionNm,
            CorrectAnswer = question.CorrectAnswer,
            IncorrectAnswers = question.IncorrectAnswers
        };
        Questions.Add(newQuestion);
        return newQuestion;
    }




}

public static class MatchExtensions
{
    public static string GetMatchStatus(this Match match)
    {
        return $"{match.Answers.Select(s => s.Id).Distinct().Count()} of {match.Questions.Count} in {match.Answers.Count} tries.";
    }
    public static bool IsMatchFinished(this Match match)
    {
        if (match.Questions.Count < 1) return false;

        if (match.Answers.Count < 1) return false;

        return match.Answers.Select(s => s.Id).Distinct().Count() == match.Questions.Count;
    }
    public static async Task AddQuestions(this Match triviaMatch, List<Question> triviaQuestions, TriviaSparkWebContext db)
    {
        foreach (var question in triviaMatch.Questions)
        {
            var existingQuestion = db.TriviaQuestions.Find(question.Id);
            if (existingQuestion is null)
            {
                db.TriviaQuestions.Add(question);
            }
            await db.SaveChangesAsync();
        }
    }
    public static QuestionAnswer AddAnswer(this Match triviaMatch, QuestionAnswer triviaAnswer)
    {
        var theTrivia = triviaMatch.Questions.FirstOrDefault(w => w.Id == triviaAnswer?.Id);
        var theAnswer = new QuestionAnswer(theTrivia, triviaAnswer);
        triviaMatch.Answers.Add(theAnswer);
        return theAnswer;
    }
    public static Question? GetRandomTrivia(this Match match)
    {
        var result = match.Questions.Where(e => !match.Answers.Any(e2 => e2.Id == e.Id)).ToList();
        var random = new Random();
        if (result.Count > 0)
        {
            var index = random.Next(result.Count());
            return result.ElementAt(index);
        }
        return null;
    }
}


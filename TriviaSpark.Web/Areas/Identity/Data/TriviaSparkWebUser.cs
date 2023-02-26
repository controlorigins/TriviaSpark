using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TriviaSpark.Web.Data;

namespace TriviaSpark.Web.Areas.Identity.Data;

// Add profile data for application users by adding properties to the TriviaSparkWebUser class
public class TriviaSparkWebUser : IdentityUser
{
    // Navigation property to TriviaQuestionSource table
    public ICollection<Match> Matches { get; set; }
}
public class MatchResponse
{
    public List<Question> Questions { get; set; } = new List<Question>();
    public List<QuestionAnswer> Answers { get; set; } = new List<QuestionAnswer>();

}
public class QuestionAnswer
{
    public QuestionAnswer()
    {
        IsValid = false;
        ErrorMessage = "No Trivia QuestionId specified.";
        IsCorrect = false;
        AnswerText = string.Empty;
    }

    public QuestionAnswer(Question? theTrivia, QuestionAnswer answer)
    {
        ErrorMessage = string.Empty;
        if (theTrivia is null)
        {
            IsValid = false;
            ErrorMessage = "No Trivia specified.";
            return;
        }
        if (theTrivia?.QuestionId is null)
        {
            IsValid = false;
            ErrorMessage = "No Trivia QuestionId specified.";
        }
        else
        {
            IsValid = true;
            ErrorMessage = string.Empty;
            QuestionId = theTrivia.QuestionId;
            AnswerText = answer.AnswerText;
            IsCorrect = (theTrivia.CorrectAnswer == answer.AnswerText);
        }
    }
    [Key]
    public int AnswerId { get; set; }
    public string QuestionId { get; set; }
    public Question Question { get; set; }
    public ICollection<MatchQuestionAnswer> MatchQuestionAnswers { get; set; }

    public string AnswerText { get; set; }
    public bool IsCorrect { get; set; }
    public bool IsValid { get; set; }
    public int Value { get; set; }
    public string ErrorMessage { get; set; }
}


public class Match
{
    [Key]
    public int MatchId { get; set; }
    public string MatchName { get; set; }
    public DateTime MatchDate { get; set; }
    public ICollection<MatchQuestion> MatchQuestions { get; set; }
    public ICollection<MatchQuestionAnswer> MatchQuestionAnswers { get; set; }

    // Foreign key to User table
    public string UserId { get; set; }
    public TriviaSparkWebUser User { get; set; }

    public Question AddQuestion(Question question)
    {
        var newQuestion = new Question()
        {
            QuestionId = question.QuestionId,
            Category = question.Category,
            Difficulty = question.Difficulty,
            QuestionText = question.QuestionText,
            CorrectAnswer = question.CorrectAnswer,
            IncorrectAnswers = question.IncorrectAnswers
        };
        MatchQuestions.Add(Create(newQuestion));
        return newQuestion;
    }

    private MatchQuestion Create(Question newQuestion)
    {
        MatchQuestion question = new()
        {
            QuestionId = newQuestion.QuestionId,
            Question = newQuestion,
            MatchId = this.MatchId
        };
        return question;
    }
}
public class Question
{
    [Key]
    public string QuestionId { get; set; }
    public string QuestionText { get; set; }

    public string Category { get; set; }
    public string CorrectAnswer { get; set; }
    public string Difficulty { get; set; }
    public string Type { get; set; }
    public ICollection<MatchQuestion> MatchQuestions { get; set; }
    public ICollection<MatchQuestionAnswer> MatchQuestionAnswers { get; set; }
    public ICollection<QuestionAnswer> Answers { get; set; }

    [NotMapped]
    public List<string> IncorrectAnswers
    {
        get
        {
            if (Answers is null) Answers = new List<QuestionAnswer>();

            return Answers.Where(w => w.IsCorrect == false).Select(s => s.AnswerText).ToList();
        }
        set
        {
            if (Answers is null) Answers = new List<QuestionAnswer>();

            Answers.Clear();
            foreach (var answer in value)
            {
                Answers.Add(new QuestionAnswer()
                {
                    AnswerText = answer,
                    IsCorrect = false,
                    QuestionId = QuestionId,
                    Value = 0
                });
            }
        }
    }
}
public class MatchQuestion
{
    public string QuestionId { get; set; }
    public int MatchId { get; set; }
    public Question Question { get; set; }
    public Match Match { get; set; }

}
public class MatchQuestionAnswer
{
    public MatchQuestionAnswer(Question theQuestion, QuestionAnswer theAnswer)
    {
        Question = theQuestion;
        Answer = theAnswer;
    }
    public MatchQuestionAnswer()
    {

    }
    public string QuestionId { get; set; }
    public int AnswerId { get; set; }
    public int MatchId { get; set; }
    public Match Match { get; set; }
    public Question Question { get; set; }
    public QuestionAnswer Answer { get; set; }

}

public static class MatchExtensions
{
    public static string GetMatchStatus(this Match match)
    {
        return $"{match.MatchQuestionAnswers.Where(w => w.Answer.IsCorrect).Distinct().Count()} of {match.MatchQuestions.Count} in {match.MatchQuestionAnswers.Count} tries.";
    }
    public static bool IsMatchFinished(this Match match)
    {
        if (match.MatchQuestions.Count < 1) return false;

        if (match.MatchQuestionAnswers.Count < 1) return false;

        return match.MatchQuestionAnswers.Where(w => w.Answer.IsCorrect).Distinct().Count() == match.MatchQuestions.Count;
    }
    public static async Task AddQuestions(this Match triviaMatch, List<Question> newQuestions, TriviaSparkWebContext db)
    {
        foreach (var question in newQuestions)
        {
            // New Question for the Database
            var existingQuestion = db.Questions.Find(question.QuestionId);
            if (existingQuestion is null)
            {
                db.Questions.Add(question);
                await db.SaveChangesAsync();
            }

            // New Question for this Match
            var matchQuestion = triviaMatch.MatchQuestions.Where(w=> w.QuestionId == question.QuestionId).FirstOrDefault();
            if(matchQuestion is null) 
            {
                triviaMatch.MatchQuestions.Add(new MatchQuestion()
                {
                    QuestionId = question.QuestionId,
                    Question = question,
                    MatchId = triviaMatch.MatchId,
                    Match = triviaMatch
                }); ;
            }
        }
    }
    public static MatchQuestionAnswer AddAnswer(this Match match, QuestionAnswer answer)
    {
        MatchQuestionAnswer theAnswer = new()
        {
            MatchId = match.MatchId,
            Match = match,
            QuestionId = answer.QuestionId,
            Question = answer.Question,
            AnswerId = answer.AnswerId,
            Answer = answer,
        };
        match.MatchQuestionAnswers.Add(theAnswer);
        return theAnswer;
    }
    public static Question? GetRandomTrivia(this Match match)
    {
        var result = match.MatchQuestions.Where(e => !match.MatchQuestionAnswers.Any(a => a.QuestionId == e.QuestionId)).ToList();
        var random = new Random();
        if (result.Count > 0)
        {
            var index = random.Next(result.Count());
            return result.ElementAt(index).Question;
        }
        return null;
    }
}


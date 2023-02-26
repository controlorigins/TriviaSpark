using HttpClientDecorator.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TriviaSpark.Core.Models;
using TriviaSpark.OpenTriviaDb.Extensions;
using TriviaSpark.Web.Areas.Identity.Data;
using TriviaSpark.Web.Data;
using TriviaSpark.Web.Helpers;

namespace TriviaSpark.Web.Pages;

public class TriviaModel : PageModel
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<TriviaModel> _logger;
    private readonly IHttpGetCallService _service;
    private readonly TriviaSparkWebContext dbContext;
    private Match? sessionMatch;

    public TriviaModel(ILogger<TriviaModel> logger,
        IHttpGetCallService getCallService,
        IHttpContextAccessor httpContextAccessor,
        TriviaSparkWebContext triviaSparkWebContext)
    {
        _logger = logger;
        _service = getCallService;
        _httpContextAccessor = httpContextAccessor;
        dbContext = triviaSparkWebContext;
        sessionMatch ??= httpContextAccessor?.HttpContext?.Session?.GetObjectFromJson<Match>("TriviaQuestionSource") ?? CreateMatch();
    }

    private static Match CreateMatch()
    {
        return new Match()
        {
            UserId = "66eae7b7-c163-4913-8aaf-421a23f0d5d9",
            MatchQuestions = new List<MatchQuestion>(),
            MatchQuestionAnswers = new List<MatchQuestionAnswer>(),
            MatchDate = DateTime.Now,
            MatchId = 1,
            MatchName = "Trivia Match"
        };
    }

    private async Task<Match> GetMoreQuestions(CancellationToken ct)
    {
        sessionMatch = dbContext.Matches.Find(TriviaMatch.MatchId);
        if (sessionMatch is null)
        {
            dbContext.Matches.Add(TriviaMatch);
            await dbContext.SaveChangesAsync(ct);
        }
        sessionMatch = dbContext.Matches.Where(w => w.MatchId == TriviaMatch.MatchId)
            .Include(i => i.User)
            .Include(i => i.MatchQuestions).ThenInclude(i => i.Question)
            .Include(i => i.MatchQuestionAnswers).FirstOrDefault();

        MatchResponse response = new();
        TriviaQuestionSource source = new();
        await source.LoadTriviaQuestions(_service, 2, ct);
        foreach (var question in source.Questions)
        {
            var dbQuestion = Create(question);
            var existingQuestion = dbContext.Questions.Find(question.Id);
            if (existingQuestion is null)
            {
                dbContext.Questions.Add(dbQuestion);
                await dbContext.SaveChangesAsync(ct);
            }

            var matchQuestion = sessionMatch.MatchQuestions.Where(w => w.QuestionId == question.Id).FirstOrDefault();
            if (matchQuestion is null)
            {
                matchQuestion = new MatchQuestion()
                {
                    MatchId = sessionMatch.MatchId,
                    QuestionId = question.Id,
                    Question = dbQuestion,
                    Match = sessionMatch
                };
                sessionMatch.MatchQuestions.Add(matchQuestion);
                await dbContext.SaveChangesAsync(ct);
            }
        }
        return sessionMatch;
    }

    private Question Create(TriviaQuestion question)
    {
        List<QuestionAnswer> answers = new()
        {
            new QuestionAnswer()
            {
                AnswerText = question.CorrectAnswer,
                IsCorrect = true,
                IsValid = true,
                ErrorMessage = string.Empty,
            }
        };
        foreach (var answer in question.IncorrectAnswers) 
        {
            answers.Add(new QuestionAnswer()
            { 
                AnswerText = answer,
                IsCorrect = false,
                IsValid = true,
                ErrorMessage = string.Empty,
            });
        }
        return new Question()
        {
            QuestionId = question.Id,
            Category = question.Category,
            Type = question.Type,
            Difficulty = question.Difficulty,
            QuestionText = question.QuestionText,
            Answers = answers,
        };
    }

    private Match TriviaMatch
    {
        get
        {
            sessionMatch ??= _httpContextAccessor?.HttpContext?.Session?.GetObjectFromJson<Match>("TriviaQuestionSource") ?? CreateMatch();
            return sessionMatch;
        }
    }


    public async Task OnGet(CancellationToken ct = default)
    {
        sessionMatch = dbContext.Matches.Where(w => w.MatchId == TriviaMatch.MatchId)
            .Include(i => i.User)
            .Include(i => i.MatchQuestions).ThenInclude(i => i.Question).ThenInclude(i=>i.Answers)
            .Include(i => i.MatchQuestionAnswers).FirstOrDefault();

        if (TriviaMatch.MatchQuestions.Count == 0 || TriviaMatch.IsMatchFinished())
        {
            await GetMoreQuestions(ct);

            // _httpContextAccessor?.HttpContext?.Session.SetObjectAsJson("TriviaQuestionSource", TriviaMatch);
        }
        TheTrivia = TriviaMatch.GetRandomTrivia() ?? new Question() { };

    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            sessionMatch = dbContext.Matches.Where(w => w.MatchId == TriviaMatch.MatchId)
                .Include(i => i.User)
                .Include(i => i.MatchQuestions).ThenInclude(i => i.Question).ThenInclude(i => i.Answers)
                .Include(i => i.MatchQuestionAnswers).FirstOrDefault();

            TheTrivia = sessionMatch.MatchQuestions.FirstOrDefault(w => w.QuestionId == TheTrivia.QuestionId).Question;
            TheAnswer.QuestionId = TheTrivia.QuestionId;
            TheAnswer.Question = TheTrivia;
            TheAnswer = sessionMatch.AddAnswer(TheAnswer).Answer;
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Trivia Post");
            return Page();
        }
        return Page();
    }

    [BindProperty]
    public bool IsMatchFinished
    {
        get
        {
            return TriviaMatch.IsMatchFinished();
        }
    }

    [BindProperty]
    public QuestionAnswer TheAnswer { get; set; } = new QuestionAnswer();
    [BindProperty]
    public string theMatchStatus
    {
        get
        {
            return TriviaMatch.GetMatchStatus();
        }
    }

    [BindProperty]
    public Question TheTrivia { get; set; } = new Question();

}




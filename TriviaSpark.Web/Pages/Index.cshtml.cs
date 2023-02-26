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
    private Match sessionMatch;

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

    private async Task<MatchResponse> GetMoreQuestions(CancellationToken ct)
    {
        MatchResponse response = new();
        TriviaQuestionSource source = new();
        await source.LoadTriviaQuestions(_service, 2, ct);
        foreach (var question in source.Questions)
        {
            var existingQuestion = dbContext.Questions.Find(question.Id);
            if (existingQuestion is null)
            {
                var dbQuestion = Create(question);
                dbContext.Questions.Add(dbQuestion);
                response.Questions.Add(dbQuestion);

            }
            await dbContext.SaveChangesAsync(ct);

            var existingMatch = dbContext.Matches.Find(TriviaMatch.MatchId);
            if (existingMatch is null)
            {
                dbContext.Matches.Add(TriviaMatch);
                await dbContext.SaveChangesAsync(ct);
            }

            existingMatch = dbContext.Matches.Where(w => w.MatchId == TriviaMatch.MatchId)
                .Include(i => i.MatchQuestions).ThenInclude(i => i.Question)
                .Include(i=>i.MatchQuestionAnswers).FirstOrDefault();
            var matchQuestion = existingMatch.MatchQuestions.Where(w => w.QuestionId == question.Id).FirstOrDefault();
        }

        return response;
    }

    private Question Create(TriviaQuestion question)
    {
        return new Question()
        {
            QuestionId = question.Id,
            Category = question.Category,
            Type = question.Type,
            Difficulty = question.Difficulty,
            QuestionText = question.QuestionText,
            CorrectAnswer = question.CorrectAnswer,
            IncorrectAnswers = question.IncorrectAnswers,
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
        if (TriviaMatch.MatchQuestions.Count == 0 || TriviaMatch.IsMatchFinished())
        {
            MatchResponse response = await GetMoreQuestions(ct);
            foreach (var question in response.Questions)
            {
                var existingQuestion = TriviaMatch.MatchQuestions.Where(w => w.QuestionId == question.QuestionId).FirstOrDefault();
                if (existingQuestion is null)
                {
                    TriviaMatch.AddQuestion(question);
                }
            }
            var curMatch = dbContext.Matches.Find(TriviaMatch.MatchId);
            if (curMatch is null)
            {
                dbContext?.Matches.Add(TriviaMatch);
                await dbContext.SaveChangesAsync(ct);
            }
            _httpContextAccessor?.HttpContext?.Session.SetObjectAsJson("TriviaQuestionSource", TriviaMatch);
        }
        TheTrivia = TriviaMatch.GetRandomTrivia() ?? new Question() { };

    }

    public IActionResult OnPostAsync()
    {
        try
        {
            TheTrivia = TriviaMatch.MatchQuestions.FirstOrDefault(w => w.QuestionId == TheTrivia.QuestionId).Question;
            TheAnswer.QuestionId = TheTrivia.QuestionId;
            TheAnswer = TriviaMatch.AddAnswer(TheAnswer).Answer;
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson("TriviaQuestionSource", TriviaMatch);

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




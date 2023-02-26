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
        sessionMatch ??= httpContextAccessor?.HttpContext?.Session?.GetObjectFromJson<Match>("MatchResponse") ?? CreateMatch();
    }

    private static Match CreateMatch()
    {
        return new Match()
        {
            UserId = "66eae7b7-c163-4913-8aaf-421a23f0d5d9",
            Questions = new List<Questions>(),
            Answers = new List<QuestionAnswer>(),
            MatchDate = DateTime.Now,
            Id = 1,
            MatchName = "Trivia Match"
        };
    }

    private async Task<MatchResponse> GetMoreQuestions(CancellationToken ct)
    {
        MatchResponse response = new();
        await response.LoadTriviaQuestions(_service, 2, ct);
        foreach (var question in response.Questions)
        {
            var existingQuestion = dbContext.TriviaQuestions.Find(question.Id);
            if (existingQuestion is null)
            {
                dbContext.TriviaQuestions.Add(question);
            }
            await dbContext.SaveChangesAsync(ct);

            var existingMatch = dbContext.TriviaMatches.Find(TriviaMatch.Id);
            if (existingMatch is null)
            {
                dbContext.TriviaMatches.Add(TriviaMatch);
                await dbContext.SaveChangesAsync(ct);
            }

            existingMatch = dbContext.TriviaMatches.Where(w => w.Id == TriviaMatch.Id).Include(i => i.Questions).ThenInclude(i => i.Answers).FirstOrDefault();
            var matchQuestion = existingMatch.Questions.Where(w => w.Id == question.Id).FirstOrDefault();
        }

        return response;
    }

    private Match TriviaMatch
    {
        get
        {
            sessionMatch ??= _httpContextAccessor?.HttpContext?.Session?.GetObjectFromJson<Match>("MatchResponse") ?? CreateMatch();
            return sessionMatch;
        }
    }


    public async Task OnGet(CancellationToken ct = default)
    {
        if (TriviaMatch.Questions.Count == 0 || TriviaMatch.IsMatchFinished())
        {
            MatchResponse response = await GetMoreQuestions(ct);
            foreach (var question in response.Questions)
            {
                var existingQuestion = TriviaMatch.Questions.Where(w => w.Id == question.Id).FirstOrDefault();
                if (existingQuestion is null)
                {
                    TriviaMatch.AddQuestion(question);
                }
            }
            var curMatch = dbContext.TriviaMatches.Find(TriviaMatch.Id);
            if (curMatch is null)
            {
                dbContext?.TriviaMatches.Add(TriviaMatch);
                await dbContext.SaveChangesAsync(ct);
            }
            _httpContextAccessor?.HttpContext?.Session.SetObjectAsJson<Match>("MatchResponse", TriviaMatch);
        }
        TheTrivia = TriviaMatch.GetRandomTrivia() ?? new Question() { };

    }

    public IActionResult OnPostAsync()
    {
        try
        {
            TheTrivia = TriviaMatch.Questions.FirstOrDefault(w => w.Id == TheTrivia.Id);
            TheAnswer.Id = TheTrivia.Id;
            TheAnswer = TriviaMatch.AddAnswer(TheAnswer);


            _httpContextAccessor.HttpContext.Session.SetObjectAsJson<Match>("MatchResponse", TriviaMatch);

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




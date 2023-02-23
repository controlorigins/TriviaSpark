using HttpClientDecorator.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TriviaSpark.OpenTriviaDb.Extensions;
using TriviaSpark.Web.Helpers;
using TriviaSpark.Web.Models.Trivia;

namespace TriviaSpark.Web.Pages;

public class TriviaModel : PageModel
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<TriviaModel> _logger;
    private readonly IHttpGetCallService _service;
    private TriviaMatch sessionMatch;

    public TriviaModel(ILogger<TriviaModel> logger, IHttpGetCallService getCallService, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _service = getCallService;
        _httpContextAccessor = httpContextAccessor;
        sessionMatch ??= httpContextAccessor?.HttpContext?.Session?.GetObjectFromJson<TriviaMatch>("TriviaMatch");
        sessionMatch ??= new TriviaMatch();
    }

    private TriviaMatch TriviaMatch
    {
        get
        {
            sessionMatch ??= _httpContextAccessor.HttpContext.Session.GetObjectFromJson<TriviaMatch>("TriviaMatch");
            sessionMatch ??= new TriviaMatch();
            return sessionMatch;
        }
    }

    public async Task OnGet(CancellationToken ct = default)
    {
        if (TriviaMatch.TriviaQuestions.Count == 0 || TriviaMatch.IsMatchFinished())
        {
            await TriviaMatch.LoadTriviaQuestions(_service, 2, ct);
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson<TriviaMatch>("TriviaMatch", TriviaMatch);
        }
        TheTrivia = TriviaMatch.GetRandomTrivia() ?? new TriviaQuestion() { };

    }

    public IActionResult OnPostAsync()
    {
        try
        {
            TheTrivia = TriviaMatch.TriviaQuestions.FirstOrDefault(w => w.Id == TheTrivia.Id);
            TheAnswer.Id = TheTrivia.Id;
            TheAnswer = TriviaMatch.AddAnswer(TheAnswer);
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson<TriviaMatch>("TriviaMatch", TriviaMatch);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Trivia Post");
            return Page();

        }
        return Page();
    }

    [BindProperty]
    public TriviaQuestion TheTrivia { get; set; } = new TriviaQuestion();

    [BindProperty]
    public TriviaAnswer TheAnswer { get; set; } = new TriviaAnswer();

    [BindProperty]
    public bool IsMatchFinished
    {
        get
        {
            return TriviaMatch.IsMatchFinished();
        }
    }
    [BindProperty]
    public string theMatchStatus
    {
        get
        {
            return TriviaMatch.GetMatchStatus();
        }
    }

}




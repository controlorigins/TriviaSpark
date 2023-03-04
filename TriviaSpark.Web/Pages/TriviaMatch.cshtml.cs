using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TriviaSpark.Core.Match;
using TriviaSpark.Core.Questions;
using TriviaSpark.Web.Areas.Identity.Services;

namespace TriviaSpark.Web.Pages
{
    [Authorize]
    public class TriviaMatchModel : PageModel
    {
        private readonly ILogger<TriviaMatchModel> _logger;
        private readonly ITriviaMatchService _matchService;
        public TriviaMatchModel(ITriviaMatchService matchService, ILogger<TriviaMatchModel> logger)
        {
            this._matchService = matchService;
            _logger = logger;
        }

        public async Task OnGet(CancellationToken ct)
        {
            if (triviaMatch is null)
            {
                SetMatch(await _matchService.GetUserMatch(User, MatchId, ct));
            }
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            try
            {
                if (currentQuestion?.QuestionId is not null)
                {
                    SetMatch(await _matchService.AddAnswerAsync(MatchId, currentAnswer, ct));
                }
                else
                {
                    if (AddQuestions)
                    {
                        SetMatch(await _matchService.GetMoreQuestions(MatchId, NumberOfQuestionsToAdd: 2, ct: ct));
                    }
                    else
                    {
                        SetMatch(await _matchService.GetUserMatch(User, MatchId, ct));
                    }
                }
            }
            catch (Exception ex)
            {
                theMatchStatus = $"Error in Trivia Post: {ex.Message}";
                _logger.LogError(ex, "Error in Trivia Post");
            }
            return RedirectToPage();
        }

        private void SetMatch(MatchModel? match)
        {
            if (match is null)
            {
                IsMatchFinished = true;
                theMatchStatus = "Match Error";
                return;
            }
            triviaMatch = match;
            MatchId = match.MatchId;
            currentQuestion = match.CurrentQuestion;
            currentAnswer = match.CurrentAnswer;
            IsMatchFinished = match?.IsMatchFinished() ?? false;
            theMatchStatus = match?.GetMatchStatus() ?? "Trivia Match is not started";
        }

        [BindProperty]
        public bool IsMatchFinished { get; set; }
        [BindProperty]
        public string theMatchStatus { get; set; }
        [BindProperty]
        public QuestionAnswerModel currentAnswer { get; set; } = new QuestionAnswerModel();
        [BindProperty]
        public int MatchId { get; set; }
        [BindProperty]
        public QuestionModel? currentQuestion { get; set; }
        [BindProperty]
        public MatchModel triviaMatch { get; set; }
        [BindProperty]
        public bool AddQuestions { get; set; }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TriviaSpark.Core.Models;
using TriviaSpark.Web.Areas.Identity.Services;

namespace TriviaSpark.Web.Pages
{
    [Authorize]
    public class TriviaMatchModel : PageModel
    {
        private readonly ILogger<TriviaMatchModel> _logger;
        private readonly ITriviaMatchService _matchService;
        private MatchModel? triviaMatch;
        public TriviaMatchModel(ITriviaMatchService matchService, ILogger<TriviaMatchModel> logger)
        {
            this._matchService = matchService;
            _logger = logger;
        }

        public async Task OnGet(CancellationToken ct)
        {
            triviaMatch = await _matchService.GetUserMatch(User);
            if (triviaMatch.MatchQuestions.Count == 0 || triviaMatch.IsMatchFinished())
            {
                triviaMatch = await _matchService.GetMoreQuestions(triviaMatch, ct);
            }
            currentQuestion = triviaMatch.GetNextQuestion() ?? new QuestionModel() { };
        }
        public async Task<IActionResult> OnPostAsync()
        {
            triviaMatch = await _matchService.GetUserMatch(User);
            try
            {
                currentQuestion = triviaMatch.MatchQuestions.FirstOrDefault(w => w.QuestionId == currentQuestion.QuestionId).Question;
                currentAnswer.QuestionId = currentQuestion.QuestionId;
                currentAnswer.Question = currentQuestion;
                currentAnswer = await _matchService.AddAnswerAsync(triviaMatch, currentAnswer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Trivia Post");
                return Page();
            }
            if (currentAnswer.IsCorrect)
            {
                return RedirectToPage("TriviaMatch");
            }
            else
            {
                return Page();
            }
        }

        [BindProperty]
        public bool IsMatchFinished
        {
            get
            {
                return triviaMatch?.IsMatchFinished() ?? false;
            }
        }

        [BindProperty]
        public QuestionAnswerModel currentAnswer { get; set; } = new QuestionAnswerModel();
        [BindProperty]
        public string theMatchStatus
        {
            get
            {
                return triviaMatch?.GetMatchStatus() ?? "Trivia Match is not started";
            }
        }

        [BindProperty]
        public QuestionModel currentQuestion { get; set; } = new QuestionModel();

    }
}

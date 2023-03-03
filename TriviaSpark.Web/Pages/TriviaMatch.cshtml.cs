using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TriviaSpark.Core.Extensions;
using TriviaSpark.Core.Models;
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
            triviaMatch = await _matchService.GetUserMatch(User, MatchId);
            MatchId = triviaMatch.MatchId;

            if (triviaMatch.MatchQuestions.Count == 0 || triviaMatch.IsMatchFinished())
            {
                triviaMatch = await _matchService.GetMoreQuestions(triviaMatch, ct);
            }
            currentQuestion = triviaMatch.GetNextQuestion() ?? new QuestionModel() { };
            currentAnswer.QuestionId = currentQuestion.QuestionId;
        }
        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            try
            {
                currentAnswer = await _matchService.AddAnswerAsync(MatchId, currentAnswer);


                triviaMatch = await _matchService.GetUserMatch(User, MatchId);

                if (currentAnswer.IsCorrect)
                {
                    if (triviaMatch.MatchQuestions.Count == 0 || triviaMatch.IsMatchFinished())
                    {
                        // triviaMatch = await _matchService.GetMoreQuestions(triviaMatch, ct);
                    }
                    currentQuestion = triviaMatch.GetNextQuestion() ?? new QuestionModel() { };
                    currentAnswer = new QuestionAnswerModel()
                    {
                        IsCorrect = false,
                        Question = currentQuestion,
                        QuestionId = currentQuestion?.QuestionId ?? string.Empty
                    };
                }
                else
                {
                    currentQuestion = triviaMatch.MatchQuestions.Get(currentAnswer.QuestionId);
                    currentAnswer.QuestionId = currentQuestion.QuestionId;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Trivia Post");
            }
            return Page();
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
        public int MatchId { get; set; }

        [BindProperty]
        public QuestionModel? currentQuestion { get; set; }

        [BindProperty]
        public MatchModel triviaMatch { get; set; }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TriviaSpark.Core.Match.Models;
using TriviaSpark.Core.Questions;
using TriviaSpark.Web.Areas.Identity.Services;

namespace TriviaSpark.Web.Pages
{
    [Authorize]
    [Route("/TriviaMatch/{id}")]
    public class TriviaMatchModel(Core.Match.Services.IMatchService matchService, ILogger<TriviaMatchModel> logger) : PageModel
    {
        private readonly ILogger<TriviaMatchModel> _logger = logger;
        private readonly Core.Match.Services.IMatchService _matchService = matchService;

        public async Task OnGet(int? id = 0, CancellationToken ct = default)
        {
            if (triviaMatch is null)
            {
                if (id is not null) MatchId = id.Value;

                SetMatch(await _matchService.GetUserMatchAsync(User, MatchId, ct));
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
                    if (AddQuestions > 0)
                    {
                        SetMatch(await _matchService.GetMoreQuestionsAsync(MatchId, NumberOfQuestionsToAdd: AddQuestions, Difficulty.Easy, ct: ct));
                    }
                    else
                    {
                        SetMatch(await _matchService.GetUserMatchAsync(User, MatchId, ct));
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
            currentAnswer = match?.CurrentAnswer;
            IsMatchFinished = _matchService.IsMatchFinished(match);
            theMatchStatus = _matchService.GetMatchStatus(match) ?? "Trivia Match is not started";
        }

        [BindProperty]
        public bool IsMatchFinished { get; set; }
        [BindProperty]
        public string theMatchStatus { get; set; }
        [BindProperty]
        public QuestionAnswerModel? currentAnswer { get; set; } = new QuestionAnswerModel();
        [BindProperty]
        public int MatchId { get; set; }
        [BindProperty]
        public QuestionModel? currentQuestion { get; set; }
        [BindProperty]
        public MatchModel triviaMatch { get; set; }

        [BindProperty]
        [DisplayName("Add Questions")]
        [Range(0, 30, ErrorMessage = "Please use values between 0 to 30")]
        public int AddQuestions { get; set; }

    }
}

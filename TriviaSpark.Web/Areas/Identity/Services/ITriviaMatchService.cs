using System.Security.Claims;
using TriviaSpark.Core.Match;
using TriviaSpark.Core.Questions;

namespace TriviaSpark.Web.Areas.Identity.Services
{
    public interface ITriviaMatchService
    {
        Task<MatchModel?> GetUserMatch(ClaimsPrincipal user, int? matchID,CancellationToken ct = default);
        Task<MatchModel?> GetMoreQuestions(int MatchId, int NumberOfQuestionsToAdd = 1, CancellationToken ct = default);
        Task<MatchModel?> AddAnswerAsync(int MatchId, QuestionAnswerModel currentAnswer, CancellationToken ct = default);
    }
}

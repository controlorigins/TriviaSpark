using System.Security.Claims;
using TriviaSpark.Core.Models;

namespace TriviaSpark.Web.Areas.Identity.Services
{
    public interface ITriviaMatchService
    {
        Task<MatchModel?> GetUserMatch(ClaimsPrincipal user, int? matchID);
        Task<MatchModel> GetMoreQuestions(MatchModel TheMatch, CancellationToken ct);
        Task<QuestionAnswerModel> AddAnswerAsync(int MatchId, QuestionAnswerModel currentAnswer, CancellationToken ct = default);
    }
}

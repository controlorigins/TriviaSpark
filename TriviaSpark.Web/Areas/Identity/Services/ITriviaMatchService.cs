using System.Security.Claims;
using TriviaSpark.Core.Models;

namespace TriviaSpark.Web.Areas.Identity.Services
{
    public interface ITriviaMatchService
    {
        Task<MatchModel?> GetUserMatch(ClaimsPrincipal user);
        Task<MatchModel> GetMoreQuestions(MatchModel TheMatch, CancellationToken ct);
        Task<QuestionAnswerModel> AddAnswerAsync(MatchModel triviaMatch, QuestionAnswerModel currentAnswer, CancellationToken ct = default);
    }
}

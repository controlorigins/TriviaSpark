using TriviaSpark.Web.Areas.Identity.Data;

namespace TriviaSpark.Web.Areas.Identity.Services
{
    public interface ITriviaMatchService
    {
        Task<Match> CreateMatchAsync(string userId, string matchName);
        Task<Match> GetMoreQuestions(CancellationToken ct);
        Task<Question> GetQuestionAsync(CancellationToken ct = default);
        Task<MatchQuestionAnswer?> SaveQuestionAnswerAsync(QuestionAnswer TheAnswer);
    }
}

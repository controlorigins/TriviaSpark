using TriviaSpark.Core.Match;
using TriviaSpark.Core.Questions;
using TriviaSpark.Web.Areas.Identity.Services;

namespace TriviaSpark.Core.Interfaces
{
    public interface IQuestionSourceAdapter
    {
        public Task<List<QuestionModel>> GetQuestions(int questionCount = 1, CancellationToken ct = default);
    }
}

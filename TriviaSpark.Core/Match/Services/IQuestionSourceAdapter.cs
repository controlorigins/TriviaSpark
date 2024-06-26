using TriviaSpark.Core.Match.Models;
using TriviaSpark.Core.Questions;

namespace TriviaSpark.Core.Match.Services
{
    public interface IQuestionSourceAdapter
    {
        public Task<List<QuestionModel>> GetQuestions(int questionCount = 1, Difficulty difficulty = Difficulty.Easy, CancellationToken ct = default);
    }
}

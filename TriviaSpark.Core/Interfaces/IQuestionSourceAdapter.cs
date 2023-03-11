using TriviaSpark.Core.Questions;

namespace TriviaSpark.Core.Interfaces
{
    public interface IQuestionSourceAdapter
    {
        public Task<List<QuestionModel>> GetQuestions(int questionCount = 1, CancellationToken ct = default);
    }
}

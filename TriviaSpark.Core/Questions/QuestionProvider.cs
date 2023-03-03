using TriviaSpark.Core.Match;
using TriviaSpark.Core.Utility;

namespace TriviaSpark.Core.Questions
{
    /// <summary>
    /// Represents a provider of items.
    /// </summary>
    public class QuestionProvider : ListProvider<QuestionModel>
    {
        public QuestionModel? Get(string QuestionId)
        {
            return Items.Where(w => w.QuestionId == QuestionId).FirstOrDefault();
        }

        public List<QuestionModel> GetCorrectQuestions(IEnumerable<MatchQuestionAnswerModel> matchQuestionAnswers)
        {
            var correctAnswerIds = Items.SelectMany(q => q.Answers)
                                             .Where(a => a.IsCorrect)
                                             .Select(a => a.AnswerId)
                                             .ToList();
            var answerIds = matchQuestionAnswers.Select(s => s.AnswerId).ToList();
            return Items.Where(q => q.Answers.Any(a => answerIds.Contains(a.AnswerId) && a.IsCorrect) == true)
                            .ToList();
        }

        public List<QuestionModel> GetIncorrectQuestions(IEnumerable<MatchQuestionAnswerModel> matchQuestionAnswers)
        {
            var correctAnswerIds = Items.SelectMany(q => q.Answers)
                                             .Where(a => a.IsCorrect)
                                             .Select(a => a.AnswerId)
                                             .ToList();
            var answerIds = matchQuestionAnswers.Select(s => s.AnswerId).ToList();
            return Items.Where(q => q.Answers.Any(a => answerIds.Contains(a.AnswerId) && a.IsCorrect) == false ||
                                         q.Answers.Any(a => correctAnswerIds.Contains(a.AnswerId) && a.IsCorrect == false))
                            .ToList();
        }
    }
}

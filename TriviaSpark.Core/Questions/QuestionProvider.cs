using TriviaSpark.Core.Match;
using TriviaSpark.Core.Utility;

namespace TriviaSpark.Core.Questions
{

    /// <summary>
    /// Represents a provider of items.
    /// </summary>
    public class QuestionProvider : ListProvider<QuestionModel>
    {
        private List<string> GetQuestionsWithoutCorrectResponse(List<QuestionAnswerSet> correct, List<QuestionAnswerSet> answered)
        {
            // Create a hash set of correct answer IDs for each question
            var correctAnswersByQuestion = correct.GroupBy(c => c.QuestionId)
                                                  .ToDictionary(g => g.Key, g => new HashSet<int>(g.Select(a => a.AnswerId)));

            // Filter the question IDs that have been attempted but not correctly answered
            var questionsWithoutCorrectResponse = answered.GroupBy(a => a.QuestionId)
                                                           .Where(g => correctAnswersByQuestion.ContainsKey(g.Key))
                                                           .Where(g => !g.Any(a => correctAnswersByQuestion[g.Key].Contains(a.AnswerId)))
                                                           .Select(g => g.Key)
                                                           .ToList();

            return questionsWithoutCorrectResponse;
        }

        internal ScoreCardModel? GetScoreCard(ICollection<MatchQuestionAnswerModel> matchQuestionAnswers)
        {
            return CalculateScore(matchQuestionAnswers.ToArray());
        }

        public ScoreCardModel CalculateScore(IEnumerable<MatchQuestionAnswerModel> matchQuestionAnswers)
        {
            List<QuestionAnswerSet> incorrectAnswers = new();
            List<QuestionAnswerSet> unasweredQuestions = new();
            // Create a dictionary of correct answer IDs for each question
            Dictionary<string, HashSet<int>> correctAnswersByQuestion = Items
                .SelectMany(q => q.Answers)
                .Where(a => a.IsCorrect)
                .Select(a => new QuestionAnswerSet()
                {
                    QuestionId = a.QuestionId,
                    AnswerId = a.AnswerId
                }).GroupBy(c => c.QuestionId)
                .ToDictionary(g => g.Key, g => new HashSet<int>(g.Select(a => a.AnswerId)));

            // Create a dictionary of attempted answer IDs for each question
            Dictionary<string, HashSet<int>> attemptedAnswersByQuestion = matchQuestionAnswers
                .Select(a => new QuestionAnswerSet()
                {
                    QuestionId = a.QuestionId,
                    AnswerId = a.AnswerId
                }).GroupBy(a => a.QuestionId)
                .ToDictionary(g => g.Key, g => new HashSet<int>(g.Select(a => a.AnswerId)));

            // Calculate the number of questions and number of correct answers
            int numQuestions = Items.Count();
            double runningScore = 0.00;
            var correctQuestions = GetCorrectQuestions(matchQuestionAnswers);
            var numTries = matchQuestionAnswers.Count();


            int numCorrect = 0;
            int numInCorrectAnswers = 0;
            foreach (var questionId in correctAnswersByQuestion.Keys)
            {
                int correctAnserId = correctAnswersByQuestion[questionId].FirstOrDefault();
                if (!attemptedAnswersByQuestion.ContainsKey(questionId))
                {
                    // Question not attempted
                    unasweredQuestions.Add(new QuestionAnswerSet { QuestionId = questionId, AnswerId = 0 });
                }
                else if (attemptedAnswersByQuestion[questionId].TryGetValue(correctAnserId, out int correctAttemptId))
                {
                    var questionAnswers = attemptedAnswersByQuestion[questionId].Count;
                    runningScore += (1.00 - ((questionAnswers - 1) * .2));
                    // Question attempted and correct 
                    numCorrect++;
                    numInCorrectAnswers += (questionAnswers - 1);
                }
                else
                {
                    numInCorrectAnswers += attemptedAnswersByQuestion[questionId].Count;
                }
            }

            double percentageScore = Math.Round((double)numCorrect / numQuestions, 2);
            // Create the score card object
            var scoreCard = new ScoreCardModel
            {
                PercentCorrect = percentageScore,
                AdjustedScore = (runningScore / numQuestions),
                NumQuestions = numQuestions,
                NumCorrect = numCorrect,
                NumIncorrect = numInCorrectAnswers,
            };
            return scoreCard;
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
            var correctAnswers = Items
                .SelectMany(q => q.Answers)
                .Where(a => a.IsCorrect)
                .Select(a => new QuestionAnswerSet()
                {
                    QuestionId = a.QuestionId,
                    AnswerId = a.AnswerId
                }).ToList();

            List<QuestionAnswerSet> questionIds = matchQuestionAnswers
                .Select(a => new QuestionAnswerSet()
                {
                    QuestionId = a.QuestionId,
                    AnswerId = a.AnswerId
                }).ToList();

            var badAnswers = GetQuestionsWithoutCorrectResponse(correctAnswers, questionIds);
            List<QuestionModel> result = Items.Where(w => badAnswers.Contains(w.QuestionId)).ToList();
            return result;
        }

        public List<QuestionModel> GetUnansweredQuestions(IEnumerable<MatchQuestionAnswerModel> matchQuestionAnswers)
        {
            var answeredQuestionIds = matchQuestionAnswers.Select(s => s.QuestionId).Distinct().ToList();

            return Items.Where(q => q.Answers.All(a => !answeredQuestionIds.Contains(a.QuestionId)))
                            .ToList();
        }

        internal class QuestionAnswerSet
        {
            public int AnswerId { get; set; }
            public string QuestionId { get; set; } = string.Empty;
        }
    }
}

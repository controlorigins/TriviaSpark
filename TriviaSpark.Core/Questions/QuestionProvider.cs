using System.Collections.Generic;
using TriviaSpark.Core.Match;
using TriviaSpark.Core.Utility;
using static System.Net.Mime.MediaTypeNames;

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
        public List<QuestionModel> GetUnansweredQuestions(IEnumerable<MatchQuestionAnswerModel> matchQuestionAnswers)
        {
            var answeredQuestionIds = matchQuestionAnswers.Select(s => s.QuestionId).Distinct().ToList();

            return Items.Where(q => q.Answers.All(a => !answeredQuestionIds.Contains(a.QuestionId)))
                            .ToList();
        }

        public class QuestionAnswer
        {
            public string QuestionId { get; set; }
            public int AnswerId { get; set; }
        }
        public ScoreCard GetScoreCard(IEnumerable<MatchQuestionAnswerModel> matchQuestionAnswers)
        {
            var correctAnswers = Items
                .SelectMany(q => q.Answers)
                .Where(a => a.IsCorrect)
                .Select(a => new QuestionAnswer()
                {
                    QuestionId = a.QuestionId,
                    AnswerId = a.AnswerId
                }).ToList();

            List<QuestionAnswer> questionIds = matchQuestionAnswers
                .Select(a => new QuestionAnswer()
                {
                    QuestionId = a.QuestionId,
                    AnswerId = a.AnswerId
                }).ToList();


            var badAnswers = GetQuestionsWithoutCorrectResponse(correctAnswers, questionIds);
            var scoreCard = CalculateScore(correctAnswers, questionIds);

            List<QuestionModel> result = Items.Where(w => badAnswers.Contains(w.QuestionId)).ToList();

            return scoreCard;
        }
        public List<QuestionModel> GetIncorrectQuestions(IEnumerable<MatchQuestionAnswerModel> matchQuestionAnswers)
        {
            var correctAnswers = Items
                .SelectMany(q => q.Answers)
                .Where(a => a.IsCorrect)
                .Select(a => new QuestionAnswer()
                {
                    QuestionId = a.QuestionId,
                    AnswerId = a.AnswerId
                }).ToList();

            List<QuestionAnswer> questionIds = matchQuestionAnswers
                .Select(a => new QuestionAnswer()
                {
                    QuestionId = a.QuestionId,
                    AnswerId = a.AnswerId
                }).ToList();


            var badAnswers = GetQuestionsWithoutCorrectResponse(correctAnswers, questionIds);
            var scoreCard = CalculateScore(correctAnswers, questionIds);

            List<QuestionModel> result = Items.Where(w => badAnswers.Contains(w.QuestionId)).ToList();

            return result;
        }
        public List<string> GetQuestionsWithoutCorrectResponse(List<QuestionAnswer> correct, List<QuestionAnswer> answered)
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

        public class ScoreCard
        {
            public int NumQuestions { get; set; }
            public int NumCorrect { get; set; }
            public List<QuestionAnswer> IncorrectAnswers { get; set; }
            public double PercentCorrect { get; internal set; }
            public int NumIncorrect { get; internal set; }
            public double AdjustedScore { get; internal set; }
        }

        public ScoreCard CalculateScore(List<QuestionAnswer> correctAnswers, List<QuestionAnswer> attemptedAnswers)
        {
            // Create a dictionary of correct answer IDs for each question
            var correctAnswersByQuestion = correctAnswers.GroupBy(c => c.QuestionId)
                                                          .ToDictionary(g => g.Key, g => new HashSet<int>(g.Select(a => a.AnswerId)));

            // Create a dictionary of attempted answer IDs for each question
            var attemptedAnswersByQuestion = attemptedAnswers.GroupBy(a => a.QuestionId)
                                                              .ToDictionary(g => g.Key, g => new HashSet<int>(g.Select(a => a.AnswerId)));

            // Calculate the number of questions and number of correct answers
            int numQuestions = correctAnswersByQuestion.Count;
            int numCorrect = 0;
            double runingScore = 0.00;

            List<QuestionAnswer> incorrectAnswers = new List<QuestionAnswer>();
            List<QuestionAnswer> unasweredQuestions = new List<QuestionAnswer>();

            foreach (var questionId in correctAnswersByQuestion.Keys)
            {
                int correctAnserId = correctAnswersByQuestion[questionId].FirstOrDefault();

                if (!attemptedAnswersByQuestion.ContainsKey(questionId))
                {
                    // Question not attempted
                    unasweredQuestions.Add(new QuestionAnswer { QuestionId = questionId, AnswerId = 0 });
                }

                if (attemptedAnswersByQuestion[questionId].TryGetValue(correctAnserId, out int correctAttemptId))
                {
                    int badCount = attemptedAnswersByQuestion[questionId]
                        .Except(correctAnswersByQuestion[questionId])
                        .Select(answerId => new QuestionAnswer { QuestionId = questionId, AnswerId = answerId }).Count();

                    runingScore += (1.00 - (badCount * .2));


                    // Question attempted and correct 
                    numCorrect++;
                }
                // Question attempted but incorrect
                incorrectAnswers
                    .AddRange(attemptedAnswersByQuestion[questionId]
                    .Except(correctAnswersByQuestion[questionId])
                    .Select(answerId => new QuestionAnswer { QuestionId = questionId, AnswerId = answerId }));
            }

            // Calculate the percentage score
            double percentageScore = Math.Round((double)numCorrect / numQuestions * 100, 2);

            // Create the score card object
            var scoreCard = new ScoreCard
            {
                PercentCorrect = percentageScore,
                AdjustedScore = runingScore,
                NumQuestions = numQuestions,
                NumCorrect = numCorrect,
                NumIncorrect = incorrectAnswers.Count,
                IncorrectAnswers = incorrectAnswers
            };
            return scoreCard;
        }
    }
}

using TriviaSpark.Core.Models;

namespace TriviaSpark.Core.Extensions
{
    public static class MatchModel_Extensions
    {
        public static string GetMatchStatus(this MatchModel match)
        {
            var correctQuestions = match.MatchQuestions.GetCorrectQuestions(match.MatchQuestionAnswers);
            return $"{correctQuestions.Count()} of {match.MatchQuestions.Count} in {match.MatchQuestionAnswers.Count} tries.";
        }
        public static bool IsMatchFinished(this MatchModel match)
        {
            if (match.MatchQuestions.Count < 1) return false;

            if (match.MatchQuestionAnswers.Count < 1) return false;

            return match.MatchQuestions.GetIncorrectQuestions(match.MatchQuestionAnswers).Count == 0;
        }
        public static MatchQuestionAnswerModel? AddAnswer(this MatchModel match, QuestionAnswerModel answer)
        {
            var question = match.MatchQuestions.Get(answer.QuestionId);
            if (question is null)
            {
                return null;
            }
            else
            {
                var matchAnswer = question.Answers.Where(w => w.AnswerText == answer.AnswerText).FirstOrDefault();

                if (matchAnswer is null) return null;

                MatchQuestionAnswerModel theAnswer = new()
                {
                    MatchId = match.MatchId,
                    QuestionId = question.QuestionId,
                    AnswerId = matchAnswer.AnswerId,
                };
                match.MatchQuestionAnswers.Add(theAnswer);
                return theAnswer;
            }
        }
        public static QuestionModel? GetNextQuestion(this MatchModel match)
        {
            var result = match.MatchQuestions.GetIncorrectQuestions(match.MatchQuestionAnswers);
            var random = new Random();
            if (result.Count() > 0)
            {
                var index = random.Next(result.Count());
                return result.ElementAt(index);
            }
            return null;
        }
    }
}


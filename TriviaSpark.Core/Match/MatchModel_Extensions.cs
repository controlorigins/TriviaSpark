using TriviaSpark.Core.Questions;

namespace TriviaSpark.Core.Match
{
    public static class MatchModel_Extensions
    {
        public static string GetMatchStatus(this MatchModel match)
        {
            var correctQuestions = match.MatchQuestions.GetCorrectQuestions(match.MatchQuestionAnswers);
            return $"{correctQuestions.Count} of {match.MatchQuestions.Count} in {match.MatchQuestionAnswers.Count} tries.";
        }
        public static bool IsMatchFinished(this MatchModel match)
        {
            if (match.MatchQuestions.Count < 1) return false;

            if (match.MatchQuestionAnswers.Count < 1) return false;

            var result = match.MatchQuestions.GetIncorrectQuestions(match.MatchQuestionAnswers);

            if (result.Count == 0) result = match.MatchQuestions.GetUnansweredQuestions(match.MatchQuestionAnswers);

            return result.Count == 0;
        }
        public static QuestionModel? GetNextQuestion(this MatchModel match)
        {
            var result = match.MatchQuestions.GetIncorrectQuestions(match.MatchQuestionAnswers);

            if(result.Count == 0) result = match.MatchQuestions.GetUnansweredQuestions(match.MatchQuestionAnswers);

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


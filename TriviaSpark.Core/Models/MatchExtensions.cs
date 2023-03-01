
namespace TriviaSpark.Core.Models
{
    public static class MatchExtensions
    {
        public static string GetMatchStatus(this MatchModel match)
        {
            return $"{match.MatchQuestionAnswers.Where(w => w.Answer.IsCorrect).Distinct().Count()} of {match.MatchQuestions.Count} in {match.MatchQuestionAnswers.Count} tries.";
        }
        public static bool IsMatchFinished(this MatchModel match)
        {
            if (match.MatchQuestions.Count < 1) return false;

            if (match.MatchQuestionAnswers.Count < 1) return false;

            return match.MatchQuestionAnswers.Where(w => w.Answer.IsCorrect).Distinct().Count() == match.MatchQuestions.Count;
        }
        public static MatchQuestionAnswerModel? AddAnswer(this MatchModel match, QuestionAnswerModel answer)
        {
            var question = match.MatchQuestions.Where(w => w.QuestionId == answer.QuestionId).FirstOrDefault();
            if (question is null)
            {
                return null;
            }
            else
            {
                var matchAnswer = question.Question.Answers.Where(w => w.AnswerText == answer.AnswerText).FirstOrDefault();

                if (matchAnswer is null) return null;

                MatchQuestionAnswerModel theAnswer = new()
                {
                    MatchId = match.MatchId,
                    Match = match,
                    QuestionId = answer.QuestionId,
                    Question = question.Question,
                    AnswerId = matchAnswer.AnswerId,
                    Answer = matchAnswer,
                };
                match.MatchQuestionAnswers.Add(theAnswer);
                return theAnswer;
            }


        }
        public static QuestionModel? GetNextQuestion(this MatchModel match)
        {
            var answeredQuestions = match.MatchQuestionAnswers.Where(w => w.Answer.IsCorrect).Select(s => s.QuestionId).Distinct().ToList();
            var result = match.MatchQuestions.Where(w => !answeredQuestions.Contains(w.QuestionId)).ToList();
            var random = new Random();
            if (result.Count > 0)
            {
                var index = random.Next(result.Count());
                return result.ElementAt(index).Question;
            }
            return null;
        }
    }
}


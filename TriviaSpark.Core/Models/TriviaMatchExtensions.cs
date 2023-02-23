
namespace TriviaSpark.Web.Models.Trivia
{
    public static class TriviaMatchExtensions
    {
        public static string GetMatchStatus(this TriviaMatch match)
        {
            return $"{match.TriviaAnswers.Select(s => s.Id).Distinct().Count()} of {match.TriviaQuestions.Count} in {match.TriviaAnswers.Count} tries.";
        }
        public static bool IsMatchFinished(this TriviaMatch match)
        {
            return match.TriviaAnswers.Select(s => s.Id).Distinct().Count() == match.TriviaQuestions.Count;
        }
        public static void AddQuestions(this TriviaMatch triviaMatch, List<TriviaQuestion> triviaQuestions)
        {
            triviaMatch.TriviaQuestions.AddRange(triviaQuestions);
        }
        public static TriviaAnswer AddAnswer(this TriviaMatch triviaMatch, TriviaAnswer triviaAnswer)
        {
            var theTrivia = triviaMatch.TriviaQuestions.FirstOrDefault(w => w.Id == triviaAnswer?.Id);
            var theAnswer = new TriviaAnswer(theTrivia, triviaAnswer);
            triviaMatch.TriviaAnswers.Add(theAnswer);
            return theAnswer;
        }
        public static TriviaQuestion? GetRandomTrivia(this TriviaMatch triviaMatch)
        {
            var result = triviaMatch.TriviaQuestions.Where(e => !triviaMatch.TriviaAnswers.Any(e2 => e2.Id == e.Id)).ToList();
            var random = new Random();
            if (result.Count > 0)
            {
                var index = random.Next(result.Count());
                return result.ElementAt(index);
            }
            return null;
        }
    }
}


namespace TriviaSpark.Core.Models
{
    public static class TriviaMatchExtensions
    {
        public static string GetMatchStatus(this MatchResponse match)
        {
            return $"{match.Answers.Select(s => s.Id).Distinct().Count()} of {match.Questions.Count} in {match.Answers.Count} tries.";
        }
        public static bool IsMatchFinished(this MatchResponse match)
        {
            return match.Answers.Select(s => s.Id).Distinct().Count() == match.Questions.Count;
        }
        public static void AddQuestions(this MatchResponse triviaMatch, List<Question> triviaQuestions)
        {
            triviaMatch.Questions.AddRange(triviaQuestions);
        }
        public static QuestionAnswer AddAnswer(this MatchResponse triviaMatch, QuestionAnswer triviaAnswer)
        {
            var theTrivia = triviaMatch.Questions.FirstOrDefault(w => w.Id == triviaAnswer?.Id);
            var theAnswer = new QuestionAnswer(theTrivia, triviaAnswer);
            triviaMatch.Answers.Add(theAnswer);
            return theAnswer;
        }
        public static Question? GetRandomTrivia(this MatchResponse triviaMatch)
        {
            var result = triviaMatch.Questions.Where(e => !triviaMatch.Answers.Any(e2 => e2.Id == e.Id)).ToList();
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

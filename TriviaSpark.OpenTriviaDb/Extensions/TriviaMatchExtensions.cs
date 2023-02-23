using HttpClientDecorator.Interfaces;
using HttpClientDecorator.Models;
using TriviaSpark.Web.Models.Trivia;

namespace TriviaSpark.OpenTriviaDb.Extensions
{
    public static class TriviaMatchExtensions
    {
        internal class OpenTBbResponse
        {
            public int response_code { get; set; }
            public Trivia[] results { get; set; } 
        }

        internal enum Difficulty
        {
            None,
            Easy,
            Medium,
            Hard
        }
        internal class Trivia
        {
            public string category { get; set; }
            public string correct_answer { get; set; }
            public string difficulty { get; set; }
            public string[] incorrect_answers { get; set; }
            public string question { get; set; }
            public string type { get; set; }
        }
        public static async Task LoadTriviaQuestions(this TriviaMatch triviaMatch, IHttpGetCallService _service, int questionCount = 1, CancellationToken ct = default)
        {
            var results = new HttpGetCallResults<OpenTBbResponse>
            {
                RequestPath = $"https://opentdb.com/api.php?amount={questionCount}&difficulty=easy&type=multiple"
            };
            results = await _service.GetAsync(results, ct);

            if (results?.ResponseResults?.results is null)
            {
                // Log Error
            }
            foreach (var trivia in results.ResponseResults.results)
            {
                triviaMatch.TriviaQuestions.Add(Create(trivia));
            }
        }
        public static string GetQuestionId()
        {
            Thread.Sleep(1);//make everything unique while looping
            long ticks = (long)(DateTime.UtcNow
            .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, 0))).TotalMilliseconds;//EPOCH
            char[] baseChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"
            .ToCharArray();

            int i = 32;
            char[] buffer = new char[i];
            int targetBase = baseChars.Length;

            do
            {
                buffer[--i] = baseChars[ticks % targetBase];
                ticks = ticks / targetBase;
            }
            while (ticks > 0);

            char[] result = new char[32 - i];
            Array.Copy(buffer, i, result, 0, 32 - i);

            return new string(result);
        }
        private static TriviaQuestion Create(Trivia trivia)
        {
            return new TriviaQuestion
            {
                Id = GetQuestionId(),
                Category = trivia.category,
                CorrectAnswer = trivia.correct_answer,
                Difficulty = trivia.difficulty,
                IncorrectAnswers = trivia.incorrect_answers,
                Question = trivia.question,
                Type = trivia.type
            };
        }
    }
}

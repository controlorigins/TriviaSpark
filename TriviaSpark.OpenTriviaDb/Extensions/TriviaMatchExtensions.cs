using HttpClientDecorator.Interfaces;
using HttpClientDecorator.Models;
using TriviaSpark.Core.Extensions;
using TriviaSpark.Core.Models;

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
        public static async Task LoadTriviaQuestions(this TriviaQuestionSource triviaMatch, IHttpGetCallService _service, int questionCount = 1, CancellationToken ct = default)
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
                triviaMatch.Questions.Add(Create(trivia));
            }
        }
        private static QuestionModel Create(Trivia trivia)
        {
            QuestionModel questionModel = new QuestionModel
            {
                QuestionId = trivia.question.GetDeterministicHashCode().ToString(),
                Category = trivia.category,
                Difficulty = trivia.difficulty,
                QuestionText = trivia.question,
                Type = trivia.type
            };
            questionModel.AddAnswer(trivia.correct_answer, true);
            foreach (var answer in trivia.incorrect_answers)
            {
                questionModel.AddAnswer(answer, false);
            }
            return questionModel;
        }
    }
}

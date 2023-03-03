using HttpClientDecorator.Interfaces;
using HttpClientDecorator.Models;
using TriviaSpark.Core.Extensions;
using TriviaSpark.Core.Questions;

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
        public static async Task LoadTriviaQuestions(this QuestionProvider questionProvider, IHttpGetCallService _service, int questionCount = 1, CancellationToken ct = default)
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
            questionProvider.Add(Create(results?.ResponseResults?.results));
        }
        /// <summary>
        /// Creates a sequence of QuestionModel objects from the specified sequence of Trivia objects.
        /// </summary>
        /// <param name="trivia">The sequence of Trivia objects to create questions from.</param>
        /// <returns>A sequence of QuestionModel objects.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the trivia parameter is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the trivia parameter contains null or empty elements.</exception>
        private static IEnumerable<QuestionModel> Create(IEnumerable<Trivia>? trivia)
        {
            if (trivia == null)
            {
                throw new ArgumentNullException(nameof(trivia), "The trivia parameter cannot be null.");
            }

            if (trivia.Any(t => t == null))
            {
                throw new ArgumentException("The trivia parameter cannot contain null elements.", nameof(trivia));
            }
            return trivia.Select(t => Create(t));
        }

        private static QuestionModel Create(Trivia trivia)
        {
            QuestionModel questionModel = new QuestionModel
            {
                QuestionId = trivia.question.GetDeterministicHashCode().ToString(),
                Category = trivia.category,
                Difficulty = trivia.difficulty,
                QuestionText = trivia.question,
                Type = trivia.type,
                Source = "OpenTriviaDb"
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

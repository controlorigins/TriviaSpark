using HttpClientDecorator.Interfaces;
using HttpClientDecorator.Models;
using System;
using TriviaSpark.Core.Extensions;
using TriviaSpark.Core.Interfaces;
using TriviaSpark.Core.Questions;
using TriviaSpark.OpenTriviaDb.Models;

namespace TriviaSpark.OpenTriviaDb.Services
{
    public class OpenTriviaDbQuestionSource : IQuestionSourceAdapter
    {
        private readonly IHttpGetCallService _httpClientService;

        public OpenTriviaDbQuestionSource(IHttpGetCallService httpGetCallService)
        {
            _httpClientService = httpGetCallService;
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
                Difficulty = trivia.difficulty.ParseEnum<Core.Match.Difficulty>(),
                QuestionText = trivia.question,
                Type = trivia.type.ParseEnum<Core.Match.QuestionType>(),
                Source = "OpenTriviaDb"
            };
            questionModel.AddAnswer(trivia.correct_answer, true);
            foreach (var answer in trivia.incorrect_answers)
            {
                questionModel.AddAnswer(answer, false);
            }
            return questionModel;
        }

        public async Task<List<QuestionModel>> GetQuestions(int questionCount = 1, CancellationToken ct = default)
        {
            var questionList = new List<QuestionModel>();
            var results = new HttpGetCallResults<OpenTBbResponse>
            {
                RequestPath = $"https://opentdb.com/api.php?amount={questionCount}&difficulty=easy&type=multiple"
            };
            results = await _httpClientService.GetAsync(results, ct);

            if (results?.ResponseResults?.results is null)
            {
                // Log Error
            }
            questionList.AddRange(Create(results?.ResponseResults?.results));
            return questionList;
        }
    }
}

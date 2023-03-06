using System.Security.Claims;
using TriviaSpark.Core.Match;
using TriviaSpark.Core.Questions;

namespace TriviaSpark.Web.Areas.Identity.Services
{
    public abstract class BaseMatchService : IMatchService
    {
        private MatchModel _match;
        public BaseMatchService(MatchModel match)
        {
            _match = match ?? throw new ArgumentNullException(nameof(match));
        }
        protected virtual MatchModel Match => _match ?? throw new InvalidOperationException("Match not set yet");

        protected static MatchModel CreateMatch()
        {
            return new MatchModel()
            {
                UserId = "66eae7b7-c163-4913-8aaf-421a23f0d5d9",
                MatchQuestions = new QuestionProvider(),
                MatchQuestionAnswers = new List<MatchQuestionAnswerModel>(),
                MatchName = "Trivia Match"
            };
        }
        public virtual Task<MatchModel?> AddAnswerAsync(int MatchId, QuestionAnswerModel currentAnswer, CancellationToken ct = default)
        {
            return Task.FromResult<MatchModel?>(CreateMatch());
        }
        public virtual string GetMatchStatus(MatchModel match)
        {
            var correctQuestions = match.MatchQuestions.GetCorrectQuestions(match.MatchQuestionAnswers);
            return $"{correctQuestions.Count} of {match.MatchQuestions.Count} in {match.MatchQuestionAnswers.Count} tries.";
        }
        public virtual Task<MatchModel?> GetMoreQuestions(int MatchId, int NumberOfQuestionsToAdd = 1, CancellationToken ct = default)
        {
            return Task.FromResult<MatchModel?>(CreateMatch());
        }
        public virtual QuestionModel? GetNextQuestion(MatchModel match)
        {
            return null;
        }
        public virtual Task<MatchModel?> GetUserMatch(ClaimsPrincipal user, int? matchID, CancellationToken ct = default)
        {
            return Task.FromResult<MatchModel?>(CreateMatch());
        }
        public virtual bool IsMatchFinished(MatchModel match)
        {
            if (match.MatchQuestions.Count == 0) return true;

            if (match.MatchQuestionAnswers.Count < 1) return false;

            var result = match.MatchQuestions.GetIncorrectQuestions(match.MatchQuestionAnswers);

            if (result.Count == 0) result = match.MatchQuestions.GetUnansweredQuestions(match.MatchQuestionAnswers);

            return result.Count == 0;
        }
    }
}

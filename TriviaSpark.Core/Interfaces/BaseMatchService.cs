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
            match.ScoreCard = match.MatchQuestions.CalculateScore(match.MatchQuestionAnswers);

            var correctQuestions = match.MatchQuestions.GetCorrectQuestions(match.MatchQuestionAnswers);
            return $"{correctQuestions.Count} correct out of {match.MatchQuestions.Count} total in {match.MatchQuestionAnswers.Count()} attempts.";
        }
        public virtual Task<MatchModel?> GetMoreQuestionsAsync(int MatchId, int NumberOfQuestionsToAdd = 1, Difficulty difficulty = Difficulty.Easy, CancellationToken ct = default)
        {
            return Task.FromResult<MatchModel?>(CreateMatch());
        }
        public virtual QuestionModel? GetNextQuestion(MatchModel match)
        {
            return null;
        }
        public virtual Task<List<MatchModel>> GetUserMatchesAsync(ClaimsPrincipal user, int? matchID, CancellationToken ct = default)
        {
            return Task.FromResult(new List<MatchModel>());
        }
        public virtual Task<MatchModel?> GetUserMatchAsync(ClaimsPrincipal user, int? matchID, CancellationToken ct = default)
        {
            return Task.FromResult<MatchModel?>(CreateMatch());
        }
        public virtual Task<MatchModel?> GetMatchAsync(int? matchID, CancellationToken ct = default)
        {
            return Task.FromResult<MatchModel?>(CreateMatch());
        }
        public virtual bool IsMatchFinished(MatchModel match)
        {
            if (match.MatchQuestions.Count == 0) return true;

            if (match.MatchQuestionAnswers.Count < 1) return false;

            switch (match.MatchMode)
            {
                case MatchMode.OneAndDone:
                    return match.MatchQuestions.GetAttemptedQuestions(match.MatchQuestionAnswers).Count == match.MatchQuestions.Count;
                    break;
                default:
                    var result = match.MatchQuestions.GetIncorrectQuestions(match.MatchQuestionAnswers);
                    if (result.Count == 0) result = match.MatchQuestions.GetIncorrectQuestions(match.MatchQuestionAnswers);
                    return result.Count == 0;
            }
        }

        public virtual Task<MatchModel> CreateMatchAsync(MatchModel newMatch, ClaimsPrincipal user, CancellationToken ct = default)
        {
            return Task.FromResult<MatchModel>(CreateMatch());
        }

        public virtual Task<List<UserModel>> GetUsersAsync(CancellationToken ct)
        {
            return Task.FromResult<List<UserModel>>(new List<UserModel>());
        }

        public virtual Task<MatchModel> UpdateMatchAsync(MatchModel match, CancellationToken ct)
        {
            return Task.FromResult<MatchModel>(match);
        }

        public virtual Task<List<MatchModel>> GetMatchesAsync(CancellationToken ct)
        {
            return Task.FromResult(new List<MatchModel>());
        }
        public virtual Task<int> DeleteUserMatchAsync(ClaimsPrincipal user, int? id, CancellationToken ct)
        {
            return Task.FromResult(0);
        }
    }
}

using HttpClientDecorator.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TriviaSpark.Core.Interfaces;
using TriviaSpark.Core.Match;
using TriviaSpark.Core.Questions;
using TriviaSpark.Web.Areas.Identity.Data;

namespace TriviaSpark.Web.Areas.Identity.Services
{
    public class TriviaMatchService : ITriviaMatchService
    {
        private readonly TriviaSparkWebContext _db;
        private readonly ILogger<TriviaMatchService> _logger;
        private readonly IQuestionSourceAdapter _service;
        private readonly UserManager<TriviaSparkWebUser> _userManager;

        public TriviaMatchService(ILogger<TriviaMatchService> logger,
        IQuestionSourceAdapter questionSource,
        TriviaSparkWebContext triviaSparkWebContext,
        UserManager<TriviaSparkWebUser> userManager)
        {
            _userManager = userManager;
            _logger = logger;
            _service = questionSource;
            _db = triviaSparkWebContext;
        }

        private MatchModel Create(Match dbMatch, QuestionAnswer? dbAnswer = null)
        {
            if (dbMatch.MatchId == 0)
            {
                throw new ArgumentNullException(nameof(dbMatch));
            }

            MatchModel match = new();
            try
            {
                match.MatchId = dbMatch.MatchId;
                match.MatchName = dbMatch.MatchName;
                match.MatchDate = dbMatch.CreatedDate;
                match.UserId = dbMatch.UserId;
                match.User = new UserModel()
                {
                    UserId = dbMatch.User.Id,
                    UserName = dbMatch.User.UserName,
                    Email = dbMatch.User.Email,
                    PhoneNumber = dbMatch.User.PhoneNumber
                };

                match.MatchQuestions.Add(dbMatch.MatchQuestions
                    .Select(s => Create(s.Question))
                    .ToList() ?? new List<QuestionModel>());

                match.MatchQuestionAnswers = dbMatch.MatchQuestionAnswers
                    .Select(s => new MatchQuestionAnswerModel()
                    {
                        MatchId = s.MatchId,
                        QuestionId = s.QuestionId,
                        AnswerId = s.AnswerId,
                    }).ToList();


                if (match.MatchQuestions.Count == 0 || match.IsMatchFinished())
                {
                    // triviaMatch = await _matchService.GetMoreQuestions(triviaMatch, ct);
                }
                else
                {
                    if (dbAnswer?.IsCorrect ?? false)
                    {
                        match.CurrentQuestion = match.GetNextQuestion() ?? new QuestionModel() { };
                    }
                    else
                    {
                        match.CurrentQuestion = match.GetNextQuestion() ?? new QuestionModel() { };
                    }
                    match.CurrentAnswer = new QuestionAnswerModel()
                    {
                        QuestionId = match.CurrentQuestion.QuestionId
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateMatchModelFromMatch:Exception", ex);
            }

            return match;
        }
        private QuestionModel Create(Question question)
        {
            var result = new QuestionModel();
            try
            {
                result.QuestionId = question.QuestionId;
                result.Category = question.Category;
                result.Type = question.Type;
                result.Difficulty = question.Difficulty;
                result.QuestionText = question.QuestionText;
                if (question.QuestionText != null)
                {
                    result.Answers = question.Answers.Select(s => new QuestionAnswerModel()
                    {
                        AnswerId = s.AnswerId,
                        QuestionId = s.QuestionId,
                        AnswerText = s.AnswerText,
                        IsCorrect = s.IsCorrect,
                        IsValid = s.IsValid,
                        ErrorMessage = s.ErrorMessage
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateQuestionModelFromQuestion:Exception", ex);
            }
            return result;
        }

        private Question Create(QuestionModel question)
        {
            Question results = new();
            try
            {
                List<QuestionAnswer> answers = new()
                {
                    new QuestionAnswer()
                    {
                        AnswerText = question.CorrectAnswer,
                        IsCorrect = true,
                        IsValid = true,
                        ErrorMessage = string.Empty,
                    }
                };
                foreach (var answer in question.IncorrectAnswers)
                {
                    answers.Add(new QuestionAnswer()
                    {
                        AnswerText = answer,
                        IsCorrect = false,
                        IsValid = true,
                        ErrorMessage = string.Empty,
                    });
                }
                return new Question()
                {
                    QuestionId = question.QuestionId,
                    Category = question.Category,
                    Type = question.Type,
                    Difficulty = question.Difficulty,
                    QuestionText = question.QuestionText,
                    Answers = answers,
                    Source = question.Source,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError("CreateQuestionFromTriviaQuestion:Exception", ex);

            }
            return results;
        }
        private static Match CreateMatch()
        {
            return new Match()
            {
                UserId = "66eae7b7-c163-4913-8aaf-421a23f0d5d9",
                MatchQuestions = new List<MatchQuestion>(),
                MatchQuestionAnswers = new List<MatchQuestionAnswer>(),
                MatchName = "Trivia Match"
            };
        }

        private async Task<Match> GetMatchAsync(int MatchId, CancellationToken ct = default)
        {
            var match = await _db.Matches
                .Where(w => w.MatchId == MatchId)
                .Include(i => i.User)
                .Include(i => i.MatchQuestions).ThenInclude(i => i.Question).ThenInclude(i => i.Answers)
                .Include(i => i.MatchQuestionAnswers)
                .AsNoTracking()
                .FirstOrDefaultAsync(ct);
            return match ?? CreateMatch();
        }

        public async Task<MatchModel?> AddAnswerAsync(int matchId, QuestionAnswerModel currentAnswer, CancellationToken ct = default)
        {

            try
            {
                QuestionAnswer? dbAnswer = await SetMatchAnswer(matchId, currentAnswer, ct);

                Match match = await GetMatchAsync(matchId, ct);
                return Create(match, dbAnswer);
            }
            catch (Exception ex)
            {
                _logger.LogError("AddAnswerAsync:Exception", ex);
            }
            return null;
        }

        private async Task<QuestionAnswer?> SetMatchAnswer(int matchId, QuestionAnswerModel currentAnswer, CancellationToken ct)
        {
            try
            {
                var dbQuestion = await _db.MatchQuestions
                    .Where(w => w.MatchId == matchId && w.QuestionId == currentAnswer.QuestionId)
                    .Include(i => i.Question).ThenInclude(i => i.Answers)
                    .Select(s => s.Question)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(ct) ?? throw new Exception("Question Not Found");

                var dbAnswer = dbQuestion.Answers
                    .Where(w => w.AnswerText == currentAnswer.AnswerText)
                    .FirstOrDefault();

                if (dbAnswer is null) throw new Exception("Answer Not Found");

                var dbMatchAnswer = await _db.MatchAnswers
                    .Where(w => w.MatchId == matchId)
                    .Where(w => w.QuestionId == dbQuestion.QuestionId)
                    .Where(w => w.AnswerId == dbAnswer.AnswerId)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(ct);

                if (dbMatchAnswer is null)
                {
                    _db.MatchAnswers.Add(new MatchQuestionAnswer()
                    {
                        AnswerId = dbAnswer.AnswerId,
                        MatchId = matchId,
                        QuestionId = dbQuestion.QuestionId
                    });
                    await _db.SaveChangesAsync(ct);
                    _db.ChangeTracker.Clear();
                }
                return dbAnswer;
            }
            catch (Exception ex)
            {
                _logger.LogCritical("SetMatchAnswer Exception", ex);
            }
            return null;
        }

        public async Task<MatchModel?> GetMoreQuestions(int MatchId, int NumberOfQuestionsToAdd = 1, CancellationToken ct = default)
        {
            try
            {
                QuestionProvider source = new();
                var newQuestions = await _service.GetQuestions(NumberOfQuestionsToAdd, ct);
                foreach (var question in newQuestions)
                {
                    var existingQuestion = await _db.Questions.FindAsync(question.QuestionId);
                    if (existingQuestion is null)
                    {
                        Question dbQuestion = Create(question);
                        _db.Questions.Add(dbQuestion);
                    }

                    var matchQuestion = _db.MatchQuestions.Where(w => w.MatchId == MatchId && w.QuestionId == question.QuestionId)
                        .AsNoTracking()
                        .FirstOrDefault();
                    if (matchQuestion is null)
                    {
                        matchQuestion = new MatchQuestion()
                        {
                            MatchId = MatchId,
                            QuestionId = question.QuestionId,
                        };
                        _db.MatchQuestions.Add(matchQuestion);
                    }
                }
                await _db.SaveChangesAsync(ct);
                _db.ChangeTracker.Clear();

                return Create(await GetMatchAsync(MatchId, ct));

            }
            catch (Exception ex)
            {
                _logger.LogCritical("GetMoreQuestions:Exception", ex);
            }
            return null;
        }

        public async Task<MatchModel> GetUserMatch(ClaimsPrincipal user, int? MatchId = null, CancellationToken ct = default)
        {
            var currentUserId = await _db.Users.Where(w => w.UserName == user.Identity.Name).Select(s => s.Id).AsNoTracking().FirstOrDefaultAsync();

            Match? match;

            if ((MatchId is null || MatchId == 0) && currentUserId is not null)
            {
                match = await _db.Matches
                    .Where(w => w.UserId == currentUserId)
                    .Include(i => i.User)
                    .Include(i => i.MatchQuestions).ThenInclude(i => i.Question).ThenInclude(i => i.Answers)
                    .Include(i => i.MatchQuestionAnswers)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(ct);
            }
            else
            {
                match = await _db.Matches
                    .Where(w => w.MatchId == MatchId)
                    .Include(i => i.User)
                    .Include(i => i.MatchQuestions).ThenInclude(i => i.Question).ThenInclude(i => i.Answers)
                    .Include(i => i.MatchQuestionAnswers)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(ct);
            }

            if (match is not null) return Create(match);

            match = new Match()
            {
                MatchQuestions = new List<MatchQuestion>(),
                MatchQuestionAnswers = new List<MatchQuestionAnswer>(),
                MatchName = "match",
                UserId = currentUserId
            };
            _db.Matches.Add(match);
            await _db.SaveChangesAsync(ct);
            MatchId = match.MatchId;

            match = await _db.Matches
                .Where(w => w.MatchId == MatchId)
                .Include(i => i.User)
                .Include(i => i.MatchQuestions).ThenInclude(i => i.Question).ThenInclude(i => i.Answers)
                .Include(i => i.MatchQuestionAnswers)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            _db.ChangeTracker.Clear();
            return Create(match);
        }
    }
}

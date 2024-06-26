using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace TriviaSpark.Core.Services;

public class TriviaMatchService : Services.BaseMatchService, Services.IMatchService
{
    private readonly Entities.TriviaSparkWebContext _db;
    private readonly ILogger<Services.TriviaMatchService> _logger;
    private readonly Services.IQuestionSourceAdapter _service;

    public TriviaMatchService(ILogger<Services.TriviaMatchService> logger,
    Services.IQuestionSourceAdapter questionSource,
    Entities.TriviaSparkWebContext triviaSparkWebContext) : base(new Models.MatchModel())
    {
        _logger = logger;
        _service = questionSource;
        _db = triviaSparkWebContext;
    }

    private Entities.Question Create(Models.QuestionModel question)
    {
        Entities.Question results = new();
        try
        {
            List<Entities.QuestionAnswer> answers = new()
            {
                new Entities.QuestionAnswer()
                {
                    AnswerText = question.CorrectAnswer,
                    IsCorrect = true,
                    IsValid = true,
                    ErrorMessage = string.Empty,
                }
            };
            foreach (var answer in question.IncorrectAnswers)
            {
                answers.Add(new Entities.QuestionAnswer()
                {
                    AnswerText = answer,
                    IsCorrect = false,
                    IsValid = true,
                    ErrorMessage = string.Empty,
                });
            }
            return new Entities.Question()
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
    private Models.MatchModel? Create(Entities.Match? dbMatch, Entities.QuestionAnswer? dbAnswer = null)
    {
        if (dbMatch is null)
        {
            return default;
        }

        if (dbMatch.MatchId == 0)
        {
            return default;
        }

        Models.MatchModel match = new();
        try
        {
            match.MatchId = dbMatch.MatchId;
            match.MatchName = dbMatch.MatchName;
            match.MatchDate = dbMatch.CreatedDate;
            match.UserId = dbMatch.UserId;
            match.MatchMode = dbMatch.MatchMode;
            match.Difficulty = dbMatch.Difficulty;
            match.QuestionType = dbMatch.QuestionType;
            match.User = new Models.UserModel()
            {
                UserId = dbMatch.User.Id,
                UserName = dbMatch.User.UserName,
                Email = dbMatch.User.Email,
                PhoneNumber = dbMatch.User.PhoneNumber
            };

            match.MatchQuestions.Add(dbMatch.MatchQuestions
                .Where(s => s is not null)
                .Select(s => Create(s))
                .ToList() ?? new List<Models.QuestionModel>());

            match.MatchQuestionAnswers = dbMatch.MatchQuestionAnswers
                .Select(s => new Models.MatchQuestionAnswerModel()
                {
                    MatchId = s.MatchId,
                    QuestionId = s.QuestionId,
                    AnswerId = s.AnswerId,
                }).ToList();


            if (match.MatchQuestions.Count == 0 || IsMatchFinished(match))
            {

            }
            else
            {
                if (dbAnswer?.IsCorrect ?? false)
                {
                    match.CurrentQuestion = GetNextQuestion(match) ?? new Models.QuestionModel() { };
                }
                else
                {
                    match.CurrentQuestion = GetNextQuestion(match) ?? new Models.QuestionModel() { };
                }
                match.CurrentAnswer = new Models.QuestionAnswerModel()
                {
                    QuestionId = match.CurrentQuestion.QuestionId
                };
            }
            match.NumberOfQuestions = match.MatchQuestions.Count;

            match.ScoreCard = match.MatchQuestions.CalculateScore(match.MatchQuestionAnswers);

        }
        catch (Exception ex)
        {
            _logger.LogError("CreateMatchModelFromMatch:Exception", ex);
        }
        return match;
    }

    private static Models.QuestionModel Create(Entities.MatchQuestion matchQuestion)
    {
        if (matchQuestion is null)
        {
            return new Models.QuestionModel();
        }
        if (matchQuestion.Question is null)
        {
            return new Models.QuestionModel()
            {
                QuestionId = matchQuestion.QuestionId,

            };
        }

        return new Models.QuestionModel()
        {
            QuestionId = matchQuestion.QuestionId,
            Category = matchQuestion.Question.Category,
            Type = matchQuestion.Question.Type,
            Difficulty = matchQuestion.Question.Difficulty,
            QuestionText = matchQuestion.Question.QuestionText,
            Answers = matchQuestion.Question.Answers.Select(s => new Models.QuestionAnswerModel()
            {
                AnswerId = s.AnswerId,
                QuestionId = s.QuestionId,
                AnswerText = s.AnswerText,
                IsCorrect = s.IsCorrect,
                IsValid = s.IsValid,
                ErrorMessage = s.ErrorMessage
            }).ToList(),
            Source = matchQuestion.Question.Source,
        };
    }

    private static new Entities.Match CreateMatch()
    {
        return new Entities.Match()
        {
            UserId = "66eae7b7-c163-4913-8aaf-421a23f0d5d9",
            MatchQuestions = new List<Entities.MatchQuestion>(),
            MatchQuestionAnswers = new List<Entities.MatchQuestionAnswer>(),
            MatchName = "Trivia Match",
            Difficulty = Models.Difficulty.Easy,
            QuestionType = Models.QuestionType.Multiple,
            MatchMode = Models.MatchMode.OneAndDone
        };
    }

    private async Task<Entities.Match> GetMatchAsync(int MatchId, CancellationToken ct = default)
    {
        var match = await _db.Matches
            .Where(w => w.MatchId == MatchId)
            .Include(i => i.User)
            .Include(i => i.MatchQuestions).ThenInclude(i => i.Question).ThenInclude(i => i.Answers)
            .Include(i => i.MatchQuestionAnswers)
            .AsSingleQuery()
            .AsNoTracking()
            .FirstOrDefaultAsync(ct);
        return match ?? CreateMatch();
    }

    public override async Task<List<Models.MatchModel>> GetMatchesAsync(CancellationToken ct)
    {
        var match = await _db.Matches
            .Include(i => i.User)
            .Include(i => i.MatchQuestionAnswers)
            .Include(i => i.MatchQuestions).ThenInclude(i => i.Question).ThenInclude(i => i.Answers)
            .AsSingleQuery()
            .AsNoTracking()
            .ToListAsync(ct);
        return match.Select(s => Create(s)).ToList() ?? [];
    }
    public override async Task<int> DeleteUserMatchAsync(ClaimsPrincipal user, int? id, CancellationToken ct)
    {
        var match = await _db.Matches.FindAsync([id], cancellationToken: ct);

        if (match is null)
        {
            return 0;
        }
        var currentUserName = user?.Identity?.Name ?? string.Empty;
        var currentUserId = await _db.Users
            .Where(w => w.UserName == currentUserName)
            .Select(s => s.Id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken: ct);

        if (match.UserId == currentUserId)
        {
            _db.Matches.Remove(match);
            await _db.SaveChangesAsync(ct);
            return 1;
        }

        return 0;
    }


    private async Task<Entities.QuestionAnswer?> SetMatchAnswer(int matchId, Models.QuestionAnswerModel currentAnswer, CancellationToken ct)
    {
        try
        {
            var dbQuestion = await _db.MatchQuestions
                .Where(w => w.MatchId == matchId && w.QuestionId == currentAnswer.QuestionId)
                .Include(i => i.Question).ThenInclude(i => i.Answers)
                .Select(s => s.Question)
                .AsSingleQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(ct) ?? throw new Exception("Question Not Found");

            var dbAnswer = dbQuestion.Answers
                .Where(w => w.AnswerText == currentAnswer.AnswerText)
                .FirstOrDefault() ?? throw new Exception("Answer Not Found");

            var dbMatchAnswer = await _db.MatchAnswers
                .Where(w => w.MatchId == matchId)
                .Where(w => w.QuestionId == dbQuestion.QuestionId)
                .Where(w => w.AnswerId == dbAnswer.AnswerId)
                .AsSingleQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(ct);

            if (dbMatchAnswer is null)
            {
                if (double.TryParse(currentAnswer.ElapsedTime, out double timeTook))
                {
                    timeTook *= 1000;
                }
                _db.MatchAnswers.Add(new Entities.MatchQuestionAnswer()
                {
                    AnswerId = dbAnswer.AnswerId,
                    MatchId = matchId,
                    QuestionId = dbQuestion.QuestionId,
                    ElapsedTime = (int)timeTook
                });
                await _db.SaveChangesAsync(ct);
            }
            return dbAnswer;
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "SetMatchAnswer Exception");
        }
        finally
        {

        }
        return null;
    }

    public override async Task<Models.MatchModel?> AddAnswerAsync(int matchId, Models.QuestionAnswerModel? currentAnswer, CancellationToken ct = default)
    {
        if (currentAnswer is null) return null;

        try
        {
            Entities.QuestionAnswer? dbAnswer = await SetMatchAnswer(matchId, currentAnswer, ct);
            Entities.Match match = await GetMatchAsync(matchId, ct);
            return Create(match, dbAnswer);
        }
        catch (Exception ex)
        {
            _logger.LogError("AddAnswerAsync:Exception", ex);
        }
        finally
        {
            _db.ChangeTracker.Clear();
            SqliteConnection.ClearAllPools();
        }
        return null;
    }

    public override async Task<List<Models.UserModel>> GetUsersAsync(CancellationToken ct)
    {
        try
        {
            return await _db.Users.Select(s => new Models.UserModel()
            {
                UserId = s.Id,
                UserName = s.UserName,
                Email = s.Email,
                PhoneNumber = s.PhoneNumber
            }).ToListAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError("AddAnswerAsync:Exception", ex);
        }
        finally
        {
            _db.ChangeTracker.Clear();
            SqliteConnection.ClearAllPools();
        }
        return new List<Models.UserModel>();
    }

    public override async Task<Models.MatchModel?> GetMoreQuestionsAsync(int MatchId, int NumberOfQuestionsToAdd = 1, Models.Difficulty difficulty = Models.Difficulty.Easy, CancellationToken ct = default)
    {
        try
        {
            Services.QuestionProvider source = new();
            var newQuestions = await _service.GetQuestions(NumberOfQuestionsToAdd, difficulty, ct);
            foreach (var question in newQuestions)
            {
                var existingQuestion = await _db.Questions.FindAsync([question.QuestionId], cancellationToken: ct);
                if (existingQuestion is null)
                {
                    Entities.Question dbQuestion = Create(question);
                    _db.Questions.Add(dbQuestion);
                }

                var matchQuestion = _db.MatchQuestions.Where(w => w.MatchId == MatchId && w.QuestionId == question.QuestionId)
                    .AsNoTracking()
                    .FirstOrDefault();
                if (matchQuestion is null)
                {
                    matchQuestion = new Entities.MatchQuestion()
                    {
                        MatchId = MatchId,
                        QuestionId = question.QuestionId,
                    };
                    _db.MatchQuestions.Add(matchQuestion);
                }
            }
            await _db.SaveChangesAsync(ct);
            return Create(await GetMatchAsync(MatchId, ct));
        }
        catch (Exception ex)
        {
            _logger.LogCritical("GetMoreQuestionsAsync:Exception", ex);
        }
        finally
        {
            _db.ChangeTracker.Clear();
            SqliteConnection.ClearAllPools();
        }
        return null;
    }
    public override async Task<Models.MatchModel> UpdateMatchAsync(Models.MatchModel match, CancellationToken ct)
    {
        try
        {
            var dbMatch = await _db.Matches.FindAsync([match.MatchId], cancellationToken: ct)
                ?? throw new Exception("Match Not Found");

            dbMatch.MatchName = match?.MatchName ?? dbMatch.MatchName;
            dbMatch.MatchMode = match?.MatchMode ?? dbMatch.MatchMode;
            dbMatch.UserId = match?.UserId ?? dbMatch.UserId;
            dbMatch.Difficulty = match?.Difficulty ?? dbMatch.Difficulty;
            dbMatch.QuestionType = match?.QuestionType ?? dbMatch.QuestionType;

            await _db.SaveChangesAsync(ct);
            return Create(await GetMatchAsync(match.MatchId, ct));

        }
        catch (Exception ex)
        {
            _logger.LogCritical("UpdateMatchAsync:Exception", ex);
        }
        finally
        {
            _db.ChangeTracker.Clear();
            SqliteConnection.ClearAllPools();
        }
        return match;


    }


    public Models.QuestionModel? GetNextQuestion(Models.MatchModel match)
    {
        try
        {

            var result = match.MatchQuestions.GetIncorrectQuestions(match.MatchQuestionAnswers);
            var unansweredQuestion = match.MatchQuestions.GetUnansweredQuestions(match.MatchQuestionAnswers);
            var random = new Random();

            switch (match.MatchMode)
            {
                case Models.MatchMode.OneAndDone:
                    if (unansweredQuestion.Count > 0)
                    {
                        var index = random.Next(unansweredQuestion.Count);
                        return unansweredQuestion.ElementAt(index);
                    }
                    break;

                case Models.MatchMode.Sequential:
                case Models.MatchMode.Adaptive:
                default:
                    if (unansweredQuestion.Count > 0)
                    {
                        var index = random.Next(unansweredQuestion.Count);
                        return unansweredQuestion.ElementAt(index);
                    }
                    else if (result.Count > 0)
                    {
                        var index = random.Next(result.Count);
                        return result.ElementAt(index);
                    }
                    break;
            }


        }
        catch (Exception ex)
        {
            _logger.LogCritical("GetNextQuestion:Exception", ex);
        }
        finally
        {
            _db.ChangeTracker.Clear();
            SqliteConnection.ClearAllPools();
        }

        return null;
    }
    public override async Task<List<Models.MatchModel>> GetUserMatchesAsync(ClaimsPrincipal user, int? MatchId = null, CancellationToken ct = default)
    {
        List<Models.MatchModel> userMatchList = new();
        try
        {
            var currentUserName = user?.Identity?.Name ?? string.Empty;
            var currentUserId = await _db.Users
                .Where(w => w.UserName == currentUserName)
                .Select(s => s.Id)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken: ct);


            // .ThenInclude(i => i.Question).ThenInclude(i => i.Answers)

            var userMatches = await _db.Matches
                .Where(w => w.UserId == currentUserId)
                .Include(i => i.User)
                .Include(i => i.MatchQuestions)
                .Include(i => i.MatchQuestionAnswers)
                .AsSingleQuery()
                .AsNoTracking()
                .ToListAsync(ct);

            foreach (Entities.Match match in userMatches)
            {
                if (match is null) continue;

                userMatchList.Add(Create(match) ?? new());
            }

        }
        catch (Exception ex)
        {
            _logger.LogCritical("GetUserMatchesAsync:Exception", ex);
        }
        finally
        {
            _db.ChangeTracker.Clear();
            SqliteConnection.ClearAllPools();
        }
        return userMatchList;

    }

    public override async Task<Models.MatchModel> CreateMatchAsync(Models.MatchModel newMatch, ClaimsPrincipal user, CancellationToken ct = default)
    {
        var currentUserName = user?.Identity?.Name ?? string.Empty;
        var currentUserId = await _db.Users
            .Where(w => w.UserName == currentUserName)
            .Select(s => s.Id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken: ct);
        Entities.Match match = Create(newMatch, currentUserId);
        _db.Matches.Add(match);
        await _db.SaveChangesAsync(ct);

        newMatch = await GetMoreQuestionsAsync(match.MatchId, newMatch.NumberOfQuestions, newMatch.Difficulty, ct);

        return newMatch;
    }

    private Entities.Match Create(Models.MatchModel newMatch, string? currentUserId)
    {
        return new Entities.Match()
        {
            MatchQuestions = new List<Entities.MatchQuestion>(),
            MatchQuestionAnswers = new List<Entities.MatchQuestionAnswer>(),
            MatchName = "UserMatch",
            UserId = currentUserId ?? string.Empty,
            MatchMode = newMatch.MatchMode,
            Difficulty = newMatch.Difficulty,
            QuestionType = newMatch.QuestionType,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
        };
    }

    public override async Task<Models.MatchModel?> GetUserMatchAsync(ClaimsPrincipal user, int? MatchId = null, CancellationToken ct = default)
    {
        try
        {
            var currentUserName = user?.Identity?.Name ?? string.Empty;
            var currentUserId = await _db.Users
                .Where(w => w.UserName == currentUserName)
                .Select(s => s.Id)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken: ct);

            Entities.Match? match;
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
            Models.MatchModel newMatch = new();
            match = Create(newMatch, currentUserId);
            _db.Matches.Add(match);
            await _db.SaveChangesAsync(ct);
            MatchId = match.MatchId;

            match = await _db.Matches
                .Where(w => w.MatchId == MatchId)
                .Include(i => i.User)
                .Include(i => i.MatchQuestions).ThenInclude(i => i.Question).ThenInclude(i => i.Answers)
                .Include(i => i.MatchQuestionAnswers)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken: ct);

            return Create(match);
        }
        catch (Exception ex)
        {
            _logger.LogCritical("GetUserMatchAsync:Exception", ex);
        }
        finally
        {
            _db.ChangeTracker.Clear();
            SqliteConnection.ClearAllPools();
        }
        return null;
    }

    public override async Task<Models.MatchModel?> GetMatchAsync(int? MatchId = null, CancellationToken ct = default)
    {
        try
        {
            Entities.Match? match;
            if (MatchId is null || MatchId == 0) throw new ArgumentNullException(nameof(MatchId));

            match = await _db.Matches
                .Where(w => w.MatchId == MatchId)
                .Include(i => i.User)
                .Include(i => i.MatchQuestions).ThenInclude(i => i.Question).ThenInclude(i => i.Answers)
                .Include(i => i.MatchQuestionAnswers)
                .AsNoTracking()
                .FirstOrDefaultAsync(ct);

            if (match is not null) return Create(match);

            throw new Exception("Match Not Found");
        }
        catch (Exception ex)
        {
            _logger.LogCritical("GetUserMatchAsync:Exception", ex);
        }
        finally
        {
            _db.ChangeTracker.Clear();
            SqliteConnection.ClearAllPools();
        }
        return null;
    }

}

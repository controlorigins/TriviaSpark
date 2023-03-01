using HttpClientDecorator.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TriviaSpark.Core.Models;
using TriviaSpark.OpenTriviaDb.Extensions;
using TriviaSpark.Web.Areas.Identity.Data;

namespace TriviaSpark.Web.Areas.Identity.Services
{
    public class TriviaMatchService : ITriviaMatchService
    {
        private readonly TriviaSparkWebContext _db;
        private readonly ILogger<TriviaMatchService> _logger;
        private readonly IHttpGetCallService _service;
        private readonly UserManager<TriviaSparkWebUser> _userManager;

        public TriviaMatchService(ILogger<TriviaMatchService> logger,
        IHttpGetCallService getCallService,
        TriviaSparkWebContext triviaSparkWebContext,
        UserManager<TriviaSparkWebUser> userManager)
        {
            _userManager = userManager;
            _logger = logger;
            _service = getCallService;
            _db = triviaSparkWebContext;
        }

        private MatchModel Create(Match match)
        {
            if (match.MatchId == 0)
            {
                throw new ArgumentNullException(nameof(match.MatchId));
            }

            MatchModel results = new();
            try
            {
                results.MatchId = match.MatchId;
                results.MatchName = match.MatchName;
                results.MatchDate = match.MatchDate;
                results.UserId = match.UserId;
                results.User = new UserModel()
                {
                    UserId = match.User.Id,
                    UserName = match.User.UserName,
                    Email = match.User.Email,
                    PhoneNumber = match.User.PhoneNumber
                };

                results.MatchQuestions = match.MatchQuestions
                    .Select(s => new MatchQuestionModel()
                    {
                        MatchId = s.MatchId,
                        QuestionId = s.QuestionId,
                        Question = new QuestionModel()
                        {
                            QuestionId = s.Question.QuestionId,
                            Category = s.Question.Category,
                            Type = s.Question.Type,
                            Difficulty = s.Question.Difficulty,
                            QuestionText = s.Question.QuestionText,
                            Answers = s.Question?.Answers.Select(s => new QuestionAnswerModel()
                            {
                                AnswerId = s.AnswerId,
                                QuestionId = s.QuestionId,
                                AnswerText = s.AnswerText,
                                IsCorrect = s.IsCorrect,
                                IsValid = s.IsValid,
                                ErrorMessage = s.ErrorMessage
                            }).ToList() ?? new List<QuestionAnswerModel>()
                        }
                    }).ToList();

                results.MatchQuestionAnswers = match.MatchQuestionAnswers
                    .Select(s => new MatchQuestionAnswerModel()
                    {
                        MatchId = s.MatchId,
                        QuestionId = s.QuestionId,
                        AnswerId = s.AnswerId,
                        Answer = new QuestionAnswerModel()
                        {
                            AnswerId = s.Answer.AnswerId,
                            QuestionId = s.Answer.QuestionId,
                            AnswerText = s.Answer.AnswerText,
                            IsCorrect = s.Answer.IsCorrect,
                            IsValid = s.Answer.IsValid,
                            ErrorMessage = s.Answer.ErrorMessage
                        }
                    }).ToList();

            }
            catch (Exception ex)
            {
                _logger.LogError("CreateMatchModelFromMatch:Exception", ex);
            }

            return results;
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
            catch (Exception ex)
            {
                _logger.LogError("CreateQuestionModelFromQuestion:Exception", ex);
            }
            return result;
        }
        private QuestionAnswerModel Create(QuestionAnswer? answer)
        {
            if (answer is null) return new QuestionAnswerModel();

            return new QuestionAnswerModel()
            {
                AnswerId = answer.AnswerId,
                AnswerText = answer.AnswerText,
                ErrorMessage = answer.ErrorMessage,
                IsCorrect = answer.IsCorrect,
                IsValid = answer.IsValid,
                QuestionId = answer.QuestionId,
                Question = Create(answer.Question),
                Value = answer.Value
            };
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
                MatchDate = DateTime.Now,
                MatchName = "Trivia Match"
            };
        }

        private async Task<Match> GetMatchAsync(int MatchId, CancellationToken ct = default)
        {
            var match = await _db.Matches
                .Where(w => w.MatchId == MatchId)
                .Include(i => i.User)
                .Include(i => i.MatchQuestions).ThenInclude(i => i.Question).ThenInclude(i => i.Answers)
                .Include(i => i.MatchQuestionAnswers).ThenInclude(i => i.Answer).ThenInclude(i => i.Question).ThenInclude(i => i.Answers)
                .FirstOrDefaultAsync(ct);
            return match ?? CreateMatch();
        }


        public async Task<QuestionAnswerModel> AddAnswerAsync(MatchModel triviaMatch, QuestionAnswerModel currentAnswer, CancellationToken ct = default)
        {
            try
            {
                var dbMatch = await _db.Matches.FindAsync(triviaMatch.MatchId, ct);

                if (dbMatch is null) return currentAnswer;

                var dbQuestion = dbMatch.MatchQuestions.Where(w => w.QuestionId == currentAnswer.QuestionId).FirstOrDefault();
                if (dbQuestion is null) return currentAnswer;

                var dbAnswer = dbQuestion.Question.Answers.Where(w => w.AnswerText == currentAnswer.AnswerText).FirstOrDefault();
                if (dbAnswer is null) return currentAnswer;

                dbMatch.MatchQuestionAnswers.Add(new MatchQuestionAnswer()
                {
                    AnswerId = dbAnswer.AnswerId,
                    MatchId = dbMatch.MatchId,
                    QuestionId = dbQuestion.QuestionId
                });
                await _db.SaveChangesAsync(ct);

                return Create(dbAnswer);
            }
            catch (Exception ex)
            {
                _logger.LogError("AddAnswerAsync:Exception", ex);
            }
            return currentAnswer;
        }
        public async Task<MatchModel> GetMoreQuestions(MatchModel TriviaMatch, CancellationToken ct)
        {
            Match dbMatch = await GetMatchAsync(TriviaMatch.MatchId, ct);


            if (dbMatch is null) return TriviaMatch;

            TriviaQuestionSource source = new();
            await source.LoadTriviaQuestions(_service, 1, ct);
            foreach (var question in source.Questions)
            {
                var existingQuestion = _db.Questions.Find(question.QuestionId);
                if (existingQuestion is null)
                {
                    Question dbQuestion = Create(question);
                    _db.Questions.Add(dbQuestion);
                    await _db.SaveChangesAsync(ct);
                }

                var matchQuestion = dbMatch.MatchQuestions.Where(w => w.QuestionId == question.QuestionId).FirstOrDefault();
                if (matchQuestion is null)
                {
                    matchQuestion = new MatchQuestion()
                    {
                        MatchId = dbMatch.MatchId,
                        QuestionId = question.QuestionId,
                    };
                    dbMatch.MatchQuestions.Add(matchQuestion);
                    await _db.SaveChangesAsync(ct);
                }
            }
            dbMatch = await GetMatchAsync(TriviaMatch.MatchId, ct);
            return Create(dbMatch);
        }

        public async Task<MatchModel> GetUserMatch(ClaimsPrincipal user)
        {
            var currentUser = await _userManager.GetUserAsync(user);
            var currentUserId = currentUser?.Id;

            var match = await _db.Matches
                .Where(w => w.UserId == currentUserId)
                .Include(i => i.User)
                .Include(i => i.MatchQuestions).ThenInclude(i => i.Question).ThenInclude(i => i.Answers)
                .Include(i => i.MatchQuestionAnswers)
                .FirstOrDefaultAsync();

            if (match is not null) return Create(match);

            match = new Match()
            {
                MatchQuestions = new List<MatchQuestion>(),
                MatchQuestionAnswers = new List<MatchQuestionAnswer>(),
                MatchDate = DateTime.Now,
                MatchName = "TriviaMatch",
                UserId = currentUserId
            };
            _db.Matches.Add(match);
            await _db.SaveChangesAsync();



            return Create(match);
        }

    }
}

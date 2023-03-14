﻿using System.Security.Claims;
using TriviaSpark.Core.Match;
using TriviaSpark.Core.Questions;

namespace TriviaSpark.Web.Areas.Identity.Services
{
    public interface IMatchService
    {
        Task<MatchModel?> GetUserMatchAsync(ClaimsPrincipal user, int? matchID, CancellationToken ct = default);
        Task<MatchModel?> GetMoreQuestionsAsync(int MatchId, int NumberOfQuestionsToAdd = 1, Difficulty difficulty = Difficulty.Easy, CancellationToken ct = default);
        Task<MatchModel?> AddAnswerAsync(int MatchId, QuestionAnswerModel currentAnswer, CancellationToken ct = default);
        QuestionModel? GetNextQuestion(MatchModel match);
        bool IsMatchFinished(MatchModel match);
        string GetMatchStatus(MatchModel match);
        Task<List<MatchModel>> GetUserMatchesAsync(ClaimsPrincipal user, int? MatchId = null, CancellationToken ct = default);
        Task<MatchModel> CreateMatchAsync(MatchModel newMatch, ClaimsPrincipal user, CancellationToken ct = default);
    }
}

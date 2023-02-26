using TriviaSpark.Web.Data;

namespace TriviaSpark.Web.Areas.Identity.Data
{
    public static class MatchExtensions
    {
        public static string GetMatchStatus(this Match match)
        {
            return $"{match.MatchQuestionAnswers.Where(w => w.Answer.IsCorrect).Distinct().Count()} of {match.MatchQuestions.Count} in {match.MatchQuestionAnswers.Count} tries.";
        }
        public static bool IsMatchFinished(this Match match)
        {
            if (match.MatchQuestions.Count < 1) return false;

            if (match.MatchQuestionAnswers.Count < 1) return false;

            return match.MatchQuestionAnswers.Where(w => w.Answer.IsCorrect).Distinct().Count() == match.MatchQuestions.Count;
        }
        public static async Task AddQuestions(this Match triviaMatch, List<Question> newQuestions, TriviaSparkWebContext db)
        {
            foreach (var question in newQuestions)
            {
                // New Question for the Database
                var existingQuestion = db.Questions.Find(question.QuestionId);
                if (existingQuestion is null)
                {
                    db.Questions.Add(question);
                    await db.SaveChangesAsync();
                }

                // New Question for this Match
                var matchQuestion = triviaMatch.MatchQuestions.Where(w => w.QuestionId == question.QuestionId).FirstOrDefault();
                if (matchQuestion is null)
                {
                    triviaMatch.MatchQuestions.Add(new MatchQuestion()
                    {
                        QuestionId = question.QuestionId,
                        Question = question,
                        MatchId = triviaMatch.MatchId,
                        Match = triviaMatch
                    }); ;
                }
            }
        }
        public static MatchQuestionAnswer? AddAnswer(this Match match, QuestionAnswer answer)
        {
            var question = match.MatchQuestions.Where(w => w.QuestionId == answer.QuestionId).FirstOrDefault();
            if (question is null)
            {
                return null;
            }
            else
            {
                var matchAnswer = question.Question.Answers.Where(w => w.AnswerText == answer.AnswerText).FirstOrDefault();

                if (matchAnswer is null) return null;

                MatchQuestionAnswer theAnswer = new()
                {
                    MatchId = match.MatchId,
                    Match = match,
                    QuestionId = answer.QuestionId,
                    Question = question.Question,
                    AnswerId = matchAnswer.AnswerId,
                    Answer = matchAnswer,
                };
                match.MatchQuestionAnswers.Add(theAnswer);
                return theAnswer;
            }


        }
        public static Question? GetRandomTrivia(this Match match)
        {
            var result = match.MatchQuestions.Where(e => !match.MatchQuestionAnswers.Any(a => a.QuestionId == e.QuestionId)).ToList();
            var random = new Random();
            if (result.Count > 0)
            {
                var index = random.Next(result.Count());
                return result.ElementAt(index).Question;
            }
            return null;
        }
    }
}


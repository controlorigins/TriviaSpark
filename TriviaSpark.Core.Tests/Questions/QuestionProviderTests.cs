namespace TriviaSpark.Core.Tests.Questions;

[TestClass]
public class QuestionProviderTests
{
    private Core.Models.MatchModel myMatch = new();

    [TestInitialize]
    public void TestInit()
    {
        myMatch.MatchQuestions.Add(
            new Core.Models.QuestionModel
            {
                QuestionId = "Q1",
                QuestionText = "What is the capital of France?",
                Answers = new List<Core.Models.QuestionAnswerModel>
                {
                    new Core.Models.QuestionAnswerModel {QuestionId="Q1", AnswerId = 1, AnswerText = "Paris", IsCorrect = true },
                    new Core.Models.QuestionAnswerModel {QuestionId="Q1", AnswerId = 2, AnswerText = "London", IsCorrect = false },
                    new Core.Models.QuestionAnswerModel {QuestionId="Q1", AnswerId = 3, AnswerText = "Rome", IsCorrect = false },
                    new Core.Models.QuestionAnswerModel {QuestionId="Q1", AnswerId = 4, AnswerText = "Berlin", IsCorrect = false }
                }
            });
        myMatch.MatchQuestions.Add(
            new Core.Models.QuestionModel
            {
                QuestionId = "Q2",
                QuestionText = "What is the capital of the United Kingdom?",
                Answers = new List<Core.Models.QuestionAnswerModel>
                {
                    new Core.Models.QuestionAnswerModel {QuestionId="Q2", AnswerId = 5, AnswerText = "London", IsCorrect = true },
                    new Core.Models.QuestionAnswerModel {QuestionId="Q2", AnswerId = 6, AnswerText = "Paris", IsCorrect = false },
                    new Core.Models.QuestionAnswerModel {QuestionId="Q2", AnswerId = 7, AnswerText = "Rome", IsCorrect = false },
                    new Core.Models.QuestionAnswerModel {QuestionId="Q2", AnswerId = 8, AnswerText = "Berlin", IsCorrect = false }
                }
            });
        myMatch.MatchQuestions.Add(
            new Core.Models.QuestionModel
            {
                QuestionId = "Q3",
                QuestionText = "What is the capital of Italy?",
                Answers = new List<Core.Models.QuestionAnswerModel>
                {
                    new Core.Models.QuestionAnswerModel {QuestionId="Q3", AnswerId = 9, AnswerText = "Rome", IsCorrect = true },
                    new Core.Models.QuestionAnswerModel {QuestionId="Q3", AnswerId = 10, AnswerText = "London", IsCorrect = false},
                    new Core.Models.QuestionAnswerModel {QuestionId="Q3", AnswerId = 11, AnswerText = "Paris", IsCorrect = false },
                    new Core.Models.QuestionAnswerModel {QuestionId="Q3", AnswerId = 12, AnswerText = "Berlin", IsCorrect = false }
                }
            });
        myMatch.MatchQuestions.Add(
            new Core.Models.QuestionModel
            {
                QuestionId = "Q4",
                QuestionText = "What is the capital of Germany?",
                Answers = new List<Core.Models.QuestionAnswerModel>
                {
                    new Core.Models.QuestionAnswerModel {QuestionId="Q4", AnswerId = 13, AnswerText = "Berlin", IsCorrect = true },
                    new Core.Models.QuestionAnswerModel {QuestionId="Q4", AnswerId = 14, AnswerText = "Rome", IsCorrect = false },
                    new Core.Models.QuestionAnswerModel {QuestionId="Q4", AnswerId = 15, AnswerText = "London", IsCorrect = false},
                    new Core.Models.QuestionAnswerModel {QuestionId="Q4", AnswerId = 16, AnswerText = "Paris", IsCorrect = false }
                }
            });

    }


    [TestMethod]
    public void CalculateScore_StateUnderTest_ExpectedBehavior()
    {
        // Arrange
        var NumQuestions = myMatch.MatchQuestions.Count;
        var NumCorrect = myMatch.MatchQuestions.GetCorrectQuestions(myMatch.MatchQuestionAnswers).Count;
        var NumAnswerd = myMatch.MatchQuestions.GetAttemptedQuestions(myMatch.MatchQuestionAnswers).Count;
        var NotAnswered = myMatch.MatchQuestions.GetUnansweredQuestions(myMatch.MatchQuestionAnswers).Count;

        // Act
        var firstQuestion = myMatch.MatchQuestions.Items.FirstOrDefault();
        var firstAnswer = firstQuestion.Answers.Where(w => w.IsCorrect == true)
            .Select(s => Create(s, myMatch)).FirstOrDefault();

        var badAnswer = firstQuestion.Answers.Where(w => w.IsCorrect == false)
            .Select(s => Create(s, myMatch)).FirstOrDefault();

        var badAnswer2 = myMatch.MatchQuestions.Items.ToArray()[1].Answers.Where(w => w.IsCorrect == false)
            .Select(s => Create(s, myMatch)).FirstOrDefault();

        var badAnswer3 = myMatch.MatchQuestions.Items.ToArray()[2].Answers.Where(w => w.IsCorrect == false)
            .Select(s => Create(s, myMatch)).FirstOrDefault();

        var answers = new List<Core.Models.MatchQuestionAnswerModel>
        {
            firstAnswer,badAnswer,badAnswer2,badAnswer3,
        };

        myMatch.MatchQuestionAnswers = answers;

        var CorrectAfter = myMatch.MatchQuestions.GetCorrectQuestions(myMatch.MatchQuestionAnswers).Count;
        var AttemptedAfter = myMatch.MatchQuestions.GetAttemptedQuestions(myMatch.MatchQuestionAnswers).Count;
        var NotAnsweredAfter = myMatch.MatchQuestions.GetUnansweredQuestions(myMatch.MatchQuestionAnswers).Count;
        var NoCorrectAnswer = myMatch.MatchQuestions.GetIncorrectQuestions(myMatch.MatchQuestionAnswers).Count;
        myMatch.ScoreCard = myMatch.MatchQuestions.CalculateScore(myMatch.MatchQuestionAnswers);

        // Assert
        Assert.AreEqual(NumQuestions, 4);

        Assert.AreEqual(NumCorrect, 0);
        Assert.AreEqual(NumAnswerd, 0);
        Assert.AreEqual(NotAnswered, 4);

        Assert.AreEqual(AttemptedAfter, myMatch.ScoreCard.QuestionsAttempted);
        Assert.AreEqual(CorrectAfter, myMatch.ScoreCard.CorrectAnswers);
        Assert.AreEqual(NotAnsweredAfter, 1);
        Assert.AreEqual(NoCorrectAnswer, 3);
    }

    private static Core.Models.MatchQuestionAnswerModel Create(Core.Models.QuestionAnswerModel s, Core.Models.MatchModel matchModel)
    {
        return new Core.Models.MatchQuestionAnswerModel()
        {
            QuestionId = s.QuestionId,
            AnswerId = s.AnswerId,
            MatchId = matchModel.MatchId
        };
    }
}

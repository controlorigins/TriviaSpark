namespace TriviaSpark.Core.Tests.Match;

[TestClass]
public class MatchQuestionAnswerModelTests
{
    [TestMethod]
    public void MatchQuestionAnswerModel_ShouldHaveQuestionIdProperty()
    {
        // Arrange
        var model = new Core.Models.MatchQuestionAnswerModel
        {
            // Act
            QuestionId = "Q123"
        };

        // Assert
        Assert.AreEqual("Q123", model.QuestionId);
    }

    [TestMethod]
    public void MatchQuestionAnswerModel_ShouldHaveAnswerIdProperty()
    {
        // Arrange
        var model = new Core.Models.MatchQuestionAnswerModel
        {
            // Act
            AnswerId = 1
        };

        // Assert
        Assert.AreEqual(1, model.AnswerId);
    }

    [TestMethod]
    public void MatchQuestionAnswerModel_ShouldHaveMatchIdProperty()
    {
        // Arrange
        var model = new Core.Models.MatchQuestionAnswerModel
        {
            // Act
            MatchId = 2
        };

        // Assert
        Assert.AreEqual(2, model.MatchId);
    }
}

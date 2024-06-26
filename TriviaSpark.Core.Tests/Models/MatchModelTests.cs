namespace TriviaSpark.Core.Tests.Models;


[TestClass]
public class MatchModelTests
{
    [TestMethod]
    public void MatchModel_MatchId_IsSet()
    {
        // Arrange
        var matchModel = new Core.Models.MatchModel
        {
            // Act
            MatchId = 1
        };

        // Assert
        Assert.AreEqual(1, matchModel.MatchId);
    }

    [TestMethod]
    public void MatchModel_MatchName_IsSet()
    {
        // Arrange
        var matchModel = new Core.Models.MatchModel
        {
            // Act
            MatchName = "Test Match"
        };

        // Assert
        Assert.AreEqual("Test Match", matchModel.MatchName);
    }

    [TestMethod]
    public void MatchModel_MatchDate_IsSet()
    {
        // Arrange
        var matchModel = new Core.Models.MatchModel
        {
            // Act
            MatchDate = new DateTime(2022, 3, 3)
        };

        // Assert
        Assert.AreEqual(new DateTime(2022, 3, 3), matchModel.MatchDate);
    }

    [TestMethod]
    public void MatchModel_MatchQuestions_IsSet()
    {
        // Arrange
        var matchModel = new Core.Models.MatchModel
        {
            // Act
            MatchQuestions = new Services.QuestionProvider()
        };

        // Assert
        Assert.IsNotNull(matchModel.MatchQuestions);
    }

    [TestMethod]
    public void MatchModel_MatchQuestionAnswers_IsSet()
    {
        // Arrange
        var matchModel = new Core.Models.MatchModel
        {
            // Act
            MatchQuestionAnswers = []
        };

        // Assert
        Assert.IsNotNull(matchModel.MatchQuestionAnswers);
    }

    [TestMethod]
    public void MatchModel_UserId_IsSet()
    {
        // Arrange
        var matchModel = new Core.Models.MatchModel
        {
            // Act
            UserId = "123"
        };

        // Assert
        Assert.AreEqual("123", matchModel.UserId);
    }

    [TestMethod]
    public void MatchModel_User_IsSet()
    {
        // Arrange
        var matchModel = new Core.Models.MatchModel();
        var user = new Core.Models.UserModel { UserId = "123", UserName = "Test User" };

        // Act
        matchModel.User = user;

        // Assert
        Assert.IsNotNull(matchModel.User);
        Assert.AreEqual("123", matchModel.User.UserId);
        Assert.AreEqual("Test User", matchModel.User.UserName);
    }

    [TestMethod]
    public void MatchModel_MatchQuestions_AreEqual()
    {
        // Arrange
        var matchModel1 = new Core.Models.MatchModel
        {
            MatchId = 1,
            MatchName = "Test Match",
            MatchDate = new DateTime(2022, 3, 3),
            MatchQuestions = new Services.QuestionProvider()
        };

        var matchModel2 = new Core.Models.MatchModel
        {
            MatchId = 1,
            MatchName = "Test Match",
            MatchDate = new DateTime(2022, 3, 3),
            MatchQuestions = new Services.QuestionProvider()
        };

        // Act
        var areEqual = matchModel1 == matchModel2;

        // Assert
        Assert.IsTrue(areEqual);
    }

    [TestMethod]
    public void Equals_ReturnsTrue_WhenMatchIdsAreEqual()
    {
        // Arrange
        var match1 = new Core.Models.MatchModel { MatchId = 1 };
        var match2 = new Core.Models.MatchModel { MatchId = 1 };

        // Act
        var result = match1.Equals(match2);

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Equals_ReturnsFalse_WhenMatchIdsAreDifferent()
    {
        // Arrange
        var match1 = new Core.Models.MatchModel { MatchId = 1 };
        var match2 = new Core.Models.MatchModel { MatchId = 2 };

        // Act
        var result = match1.Equals(match2);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_ReturnsFalse_WhenObjectIsNull()
    {
        // Arrange
        var match = new Core.Models.MatchModel { MatchId = 1 };

        // Act
        var result = match.Equals(null);

        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void Equals_ReturnsFalse_WhenObjectIsNotMatchModel()
    {
        // Arrange
        var match = new Core.Models.MatchModel { MatchId = 1 };
        var obj = new object();

        // Act
        var result = match.Equals(obj);

        // Assert
        Assert.IsFalse(result);
    }
}



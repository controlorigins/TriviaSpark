using TriviaSpark.Core.Match;
using TriviaSpark.Core.Questions;

namespace TriviaSpark.Core.Tests.Models
{

    [TestClass]
    public class MatchModelTests
    {
        [TestMethod]
        public void MatchModel_MatchId_IsSet()
        {
            // Arrange
            var matchModel = new MatchModel();

            // Act
            matchModel.MatchId = 1;

            // Assert
            Assert.AreEqual(1, matchModel.MatchId);
        }

        [TestMethod]
        public void MatchModel_MatchName_IsSet()
        {
            // Arrange
            var matchModel = new MatchModel();

            // Act
            matchModel.MatchName = "Test Match";

            // Assert
            Assert.AreEqual("Test Match", matchModel.MatchName);
        }

        [TestMethod]
        public void MatchModel_MatchDate_IsSet()
        {
            // Arrange
            var matchModel = new MatchModel();

            // Act
            matchModel.MatchDate = new DateTime(2022, 3, 3);

            // Assert
            Assert.AreEqual(new DateTime(2022, 3, 3), matchModel.MatchDate);
        }

        [TestMethod]
        public void MatchModel_MatchQuestions_IsSet()
        {
            // Arrange
            var matchModel = new MatchModel();

            // Act
            matchModel.MatchQuestions = new QuestionProvider();

            // Assert
            Assert.IsNotNull(matchModel.MatchQuestions);
        }

        [TestMethod]
        public void MatchModel_MatchQuestionAnswers_IsSet()
        {
            // Arrange
            var matchModel = new MatchModel();

            // Act
            matchModel.MatchQuestionAnswers = new List<MatchQuestionAnswerModel>();

            // Assert
            Assert.IsNotNull(matchModel.MatchQuestionAnswers);
        }

        [TestMethod]
        public void MatchModel_UserId_IsSet()
        {
            // Arrange
            var matchModel = new MatchModel();

            // Act
            matchModel.UserId = "123";

            // Assert
            Assert.AreEqual("123", matchModel.UserId);
        }

        [TestMethod]
        public void MatchModel_User_IsSet()
        {
            // Arrange
            var matchModel = new MatchModel();
            var user = new UserModel { UserId = "123", UserName = "Test User" };

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
            var matchModel1 = new MatchModel
            {
                MatchId = 1,
                MatchName = "Test Match",
                MatchDate = new DateTime(2022, 3, 3),
                MatchQuestions = new QuestionProvider()
            };

            var matchModel2 = new MatchModel
            {
                MatchId = 1,
                MatchName = "Test Match",
                MatchDate = new DateTime(2022, 3, 3),
                MatchQuestions = new QuestionProvider()
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
            var match1 = new MatchModel { MatchId = 1 };
            var match2 = new MatchModel { MatchId = 1 };

            // Act
            var result = match1.Equals(match2);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Equals_ReturnsFalse_WhenMatchIdsAreDifferent()
        {
            // Arrange
            var match1 = new MatchModel { MatchId = 1 };
            var match2 = new MatchModel { MatchId = 2 };

            // Act
            var result = match1.Equals(match2);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Equals_ReturnsFalse_WhenObjectIsNull()
        {
            // Arrange
            var match = new MatchModel { MatchId = 1 };

            // Act
            var result = match.Equals(null);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Equals_ReturnsFalse_WhenObjectIsNotMatchModel()
        {
            // Arrange
            var match = new MatchModel { MatchId = 1 };
            var obj = new object();

            // Act
            var result = match.Equals(obj);

            // Assert
            Assert.IsFalse(result);
        }
    }
}



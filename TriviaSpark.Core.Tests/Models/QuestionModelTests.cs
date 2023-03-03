using TriviaSpark.Core.Questions;

namespace TriviaSpark.Core.Tests.Models
{

    [TestClass]
    public class QuestionModelTests
    {
        private QuestionModel? _question;

        [TestInitialize]
        public void Setup()
        {
            _question = new QuestionModel()
            {
                QuestionId = "Q001",
                QuestionText = "What is the capital of France?",
                Category = "Geography",
                Difficulty = "Easy",
                Type = "Multiple Choice",
                Answers = new List<QuestionAnswerModel>()
            {
                new QuestionAnswerModel()
                {
                    AnswerText = "Paris",
                    IsCorrect = true,
                    QuestionId = "Q001",
                    ErrorMessage = string.Empty,
                    IsValid = true,
                    Value = 1
                },
                new QuestionAnswerModel()
                {
                    AnswerText = "London",
                    IsCorrect = false,
                    QuestionId = "Q001",
                    ErrorMessage = string.Empty,
                    IsValid = true,
                    Value = 0
                },
                new QuestionAnswerModel()
                {
                    AnswerText = "Madrid",
                    IsCorrect = false,
                    QuestionId = "Q001",
                    ErrorMessage = string.Empty,
                    IsValid = true,
                    Value = 0
                },
                new QuestionAnswerModel()
                {
                    AnswerText = "Berlin",
                    IsCorrect = false,
                    QuestionId = "Q001",
                    ErrorMessage = string.Empty,
                    IsValid = true,
                    Value = 0
                }
            }
            };
        }

        [TestMethod]
        public void CorrectAnswer_ReturnsCorrectAnswer()
        {
            // Act
            var correctAnswer = _question?.CorrectAnswer;

            // Assert
            Assert.AreEqual("Paris", correctAnswer);
        }

        [TestMethod]
        public void IncorrectAnswers_ReturnsIncorrectAnswers()
        {
            // Act
            var incorrectAnswers = _question?.IncorrectAnswers;

            // Assert
            Assert.AreEqual(3, incorrectAnswers?.Count ?? 0);
            Assert.IsTrue(incorrectAnswers?.Contains("London") ?? false);
            Assert.IsTrue(incorrectAnswers?.Contains("Madrid") ?? false);
            Assert.IsTrue(incorrectAnswers?.Contains("Berlin") ?? false);
        }

        [TestMethod]
        public void AddAnswer_AddsAnswerToAnswersCollection()
        {
            // Act
            _question?.AddAnswer("New York", false);

            // Assert
            Assert.AreEqual(5, _question?.Answers.Count ?? 0);
            Assert.IsTrue(_question?.Answers.Any(a => a.AnswerText == "New York" && a.IsCorrect == false) ?? false);
        }

        [TestMethod]
        public void CompareTo_ReturnsZero_WhenOtherQuestionModelIsEqual()
        {
            // Arrange
            var otherQuestion = new QuestionModel() { QuestionId = "Q001" };

            // Act
            var result = _question?.CompareTo(otherQuestion) ?? 0;

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void CompareTo_ReturnsNegativeNumber_WhenOtherQuestionModelIsGreaterThanCurrent()
        {
            // Arrange
            var otherQuestion = new QuestionModel() { QuestionId = "Q002" };

            // Act
            var result = _question?.CompareTo(otherQuestion) ?? 0;

            // Assert
            Assert.IsTrue(result < 0);
        }

        [TestMethod]
        public void CompareTo_ReturnsPositiveNumber_WhenOtherQuestionModelIsLessThanCurrent()
        {
            // Arrange
            var otherQuestion = new QuestionModel() { QuestionId = "Q000" };

            // Act
            var result = _question?.CompareTo(otherQuestion) ?? 0;

            // Assert
            Assert.IsTrue(result > 0);
        }
    }



}

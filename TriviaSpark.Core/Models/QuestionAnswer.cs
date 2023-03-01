using System.ComponentModel.DataAnnotations;

namespace TriviaSpark.Core.Models
{
    public class QuestionAnswerModel
    {
        public QuestionAnswerModel()
        {
            IsValid = false;
            ErrorMessage = "No Trivia QuestionId specified.";
            IsCorrect = false;
            AnswerText = string.Empty;
        }

        public QuestionAnswerModel(QuestionModel? question, QuestionAnswerModel answer)
        {
            ErrorMessage = string.Empty;
            if (question is null)
            {
                IsValid = false;
                ErrorMessage = "No Question specified.";
                return;
            }
            if (question?.QuestionId is null)
            {
                IsValid = false;
                ErrorMessage = "No QuestionId specified.";
            }
            else
            {
                IsValid = true;
                ErrorMessage = string.Empty;
                QuestionId = question.QuestionId;
                AnswerText = answer.AnswerText;
                IsCorrect = question.CorrectAnswer == answer.AnswerText;
            }
        }
        [Key]
        public int AnswerId { get; set; }
        public string QuestionId { get; set; }
        public QuestionModel Question { get; set; }
        public ICollection<MatchQuestionAnswerModel> MatchQuestionAnswers { get; set; }

        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsValid { get; set; }
        public int Value { get; set; }
        public string ErrorMessage { get; set; }
    }
}


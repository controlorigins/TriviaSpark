using System.ComponentModel.DataAnnotations;

namespace TriviaSpark.Web.Areas.Identity.Data
{
    public class QuestionAnswer : BaseEntity
    {
        public QuestionAnswer()
        {
            IsValid = false;
            ErrorMessage = "No Trivia QuestionId specified.";
            IsCorrect = false;
            AnswerText = string.Empty;
        }

        public QuestionAnswer(Question? question, QuestionAnswer answer)
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
        public Question Question { get; set; }
        public ICollection<MatchQuestionAnswer> MatchQuestionAnswers { get; set; }

        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsValid { get; set; }
        public int Value { get; set; }
        public string ErrorMessage { get; set; }
    }
}


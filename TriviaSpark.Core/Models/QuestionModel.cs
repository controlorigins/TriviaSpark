using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TriviaSpark.Core.Models
{
    public class QuestionModel
    {
        [Key]
        public string QuestionId { get; set; }
        public string QuestionText { get; set; }

        public string Category { get; set; }
        public string Difficulty { get; set; }
        public string Type { get; set; }
        public ICollection<MatchQuestionModel> MatchQuestions { get; set; }
        public ICollection<MatchQuestionAnswerModel> MatchQuestionAnswers { get; set; }
        public ICollection<QuestionAnswerModel> Answers { get; set; }

        [NotMapped]
        public string CorrectAnswer
        {
            get
            {
                Answers ??= new List<QuestionAnswerModel>();

                return Answers.Where(w => w.IsCorrect == true).Select(s => s.AnswerText).FirstOrDefault();
            }
        }

        [NotMapped]
        public List<string> IncorrectAnswers
        {
            get
            {
                Answers ??= new List<QuestionAnswerModel>();

                return Answers.Where(w => w.IsCorrect == false).Select(s => s.AnswerText).ToList();
            }
        }
        public void AddAnswer(string answerText, bool isCorrect)
        {
            Answers ??= new List<QuestionAnswerModel>();

            Answers.Add(new QuestionAnswerModel()
            {
                AnswerText = answerText,
                IsCorrect = isCorrect,
                QuestionId = this.QuestionId,
                ErrorMessage = string.Empty,
                IsValid = true,
                Value = isCorrect ? 1 : 0
            });
        }
    }
}


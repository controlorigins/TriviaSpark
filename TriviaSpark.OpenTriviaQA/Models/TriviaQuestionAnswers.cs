
using System.ComponentModel.DataAnnotations;

namespace OpenTriviaQA.Models
{
    public class TriviaQuestionAnswers
    {
        [Key]
        public int Id { get; set; }
        public string QuestionId { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public int Value { get; set; }
    }
}

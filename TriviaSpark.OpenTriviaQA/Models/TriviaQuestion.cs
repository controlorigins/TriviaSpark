using System.ComponentModel.DataAnnotations.Schema;

namespace OpenTriviaQA.Models
{
    public class TriviaQuestion
    {
        public string Id { get; set; }
        public string Category { get; set; }
        public string CorrectAnswer { get; set; }
        public string Difficulty { get; set; }
        public List<TriviaQuestionAnswers> Answers { get; set; }
        public string QuestionText { get; set; }
        public string Type { get; set; }

        [NotMapped]
        public List<string> IncorrectAnswers
        {
            get
            {
                if (Answers is null) Answers = new List<TriviaQuestionAnswers>();

                return Answers.Where(w => w.IsCorrect == false).Select(s => s.AnswerText).ToList();
            }
            set
            {
                if (Answers is null) Answers = new List<TriviaQuestionAnswers>();

                Answers.Clear();
                foreach (var answer in value)
                {
                    Answers.Add(new TriviaQuestionAnswers()
                    {
                        AnswerText = answer,
                        IsCorrect = false,
                        QuestionId = Id,
                        Value = 0
                    });
                }
            }
        }
    }
}


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace TriviaSpark.Core.Models
{
    public class Question
    {
        public ICollection<Match> Matches;

        public string Id { get; set; }
        public string Category { get; set; }
        public string CorrectAnswer { get; set; }
        public string Difficulty { get; set; }
        public List<QuestionAnswers> Answers { get; set; }
        public string QuestionNm { get; set; }
        public string Type { get; set; }

        [NotMapped]
        public List<string> IncorrectAnswers
        { 
            get
            {
                if (Answers is null) Answers = new List<QuestionAnswers>();

                return Answers.Where(w => w.IsCorrect == false).Select(s => s.Answer).ToList();
            }
            set
            {
                if(Answers is null) Answers = new List<QuestionAnswers>();

                Answers.Clear();
                foreach(var answer in value)
                {
                    Answers.Add(new QuestionAnswers()
                    {
                        Answer = answer,
                        IsCorrect = false,
                        QuestionId = Id,
                        Value = 0
                    });
                }
            }
        }
    }

    public class QuestionAnswers
    { 
        [Key]
        public int Id { get; set; } 
        public string QuestionId { get; set; }
        public string Answer { get; set; }
        public bool IsCorrect { get; set; }
        public int Value { get; set; }
    }

}

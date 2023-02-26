﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TriviaSpark.Web.Areas.Identity.Data
{
    public class Question
    {
        [Key]
        public string QuestionId { get; set; }
        public string QuestionText { get; set; }

        public string Category { get; set; }
        public string Difficulty { get; set; }
        public string Type { get; set; }
        public ICollection<MatchQuestion> MatchQuestions { get; set; }
        public ICollection<MatchQuestionAnswer> MatchQuestionAnswers { get; set; }
        public ICollection<QuestionAnswer> Answers { get; set; }

        [NotMapped]
        public string CorrectAnswer
        {
            get
            {
                Answers ??= new List<QuestionAnswer>();

                return Answers.Where(w => w.IsCorrect == true).Select(s => s.AnswerText).FirstOrDefault();
            }
        }

        [NotMapped]
        public List<string> IncorrectAnswers
        {
            get
            {
                Answers ??= new List<QuestionAnswer>();

                return Answers.Where(w => w.IsCorrect == false).Select(s => s.AnswerText).ToList();
            }
        }
    }
}


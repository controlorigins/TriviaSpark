﻿
using System.ComponentModel;

namespace TriviaSpark.Web.Models.Trivia
{
    public class TriviaAnswer
    {
        public TriviaAnswer()
        {
            Id = string.Empty;
            IsValid = false;
            ErrorMessage = "No Trivia Id specified.";
            Correct = false;
            Answer = string.Empty;
        }

        public TriviaAnswer(TriviaQuestion? theTrivia, TriviaAnswer answer)
        {
            ErrorMessage = string.Empty;
            if (theTrivia is null)
            {
                IsValid = false;
                ErrorMessage = "No Trivia specified.";
                return;
            }
            if (theTrivia?.Id is null)
            {
                IsValid = false;
                ErrorMessage = "No Trivia Id specified.";
            }
            else
            {
                IsValid = true;
                ErrorMessage = string.Empty;
                Id = theTrivia.Id;
                Answer = answer.Answer;
                Correct = (theTrivia.CorrectAnswer == answer.Answer);
            }
        }
        public string Id { get; set; }
        public string Answer { get; set; }
        public bool Correct { get; set; }
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }
}

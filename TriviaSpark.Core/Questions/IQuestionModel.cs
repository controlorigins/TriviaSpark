namespace TriviaSpark.Core.Questions
{
    public interface IQuestionModel
    {
        void AddAnswer(string answerText, bool isCorrect);
        bool Equals(object obj);
        int GetHashCode();

        ICollection<QuestionAnswerModel> Answers { get; set; }
        string Category { get; set; }
        string CorrectAnswer { get; }
        string Difficulty { get; set; }
        List<string> IncorrectAnswers { get; }
        string QuestionId { get; set; }
        string QuestionText { get; set; }
        string Source { get; set; }
        string Type { get; set; }
    }
}


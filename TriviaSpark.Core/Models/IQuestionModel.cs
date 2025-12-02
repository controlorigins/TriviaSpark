namespace TriviaSpark.Core.Models;

public interface IQuestionModel
{
    void AddAnswer(string answerText, bool isCorrect);
    bool Equals(object obj);
    int GetHashCode();

    ICollection<Models.QuestionAnswerModel> Answers { get; set; }
    string Category { get; set; }
    string? CorrectAnswer { get; }
    Models.Difficulty Difficulty { get; set; }
    List<string> IncorrectAnswers { get; }
    string QuestionId { get; set; }
    string QuestionText { get; set; }
    string Source { get; set; }
    Models.QuestionType Type { get; set; }
}


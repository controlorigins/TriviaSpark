using JeopardyData;
using System.Text.Json;

string jsonFilePath = "JEOPARDY_QUESTIONS.json";
string json = File.ReadAllText(jsonFilePath);
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};
var questions = JsonSerializer.Deserialize<JeopardyQuestions>(json, options);
questions.DisplayQuestionsFromRandomCategory();

using OpenTriviaQA;
using TriviaSpark.Web.Models.Trivia;

Console.WriteLine("Start");
var files = TriviaQuestion_Extensions.GetFilePathsInFolder(@"C:\GitHub\ControlOrigins\TriviaSpark\TriviaSpark.OpenTriviaQA\categories\");
var questions = new List<TriviaQuestion>();
foreach (var file in files)
{
    Console.WriteLine($"Processing:{Path.GetFileName(file)}");
    questions.AddRange(TriviaQuestion_Extensions.ExtractQuestions(file));
}
TriviaQuestion_Extensions.WriteQuestionsToJsonFile(questions, @"C:\GitHub\ControlOrigins\TriviaSpark\TriviaSpark.OpenTriviaQA\questions.json");
Console.WriteLine("Done");

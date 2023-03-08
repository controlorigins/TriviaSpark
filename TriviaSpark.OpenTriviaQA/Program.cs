using OpenTriviaQA;
using OpenTriviaQA.Models;

Console.WriteLine("Start");

// Get the path of the categories folder
string categoriesPath = Path.Combine(AppContext.BaseDirectory, "categories");
// Check if the categories folder exists
if (!Directory.Exists(categoriesPath))
{
    Console.WriteLine("Categories folder not found.");
    return;
}

// Get a list of all files in the categories folder
string[] files = Directory.GetFiles(categoriesPath);
var questions = new List<TriviaQuestion>();
foreach (var file in files)
{
    Console.WriteLine($"Processing:{Path.GetFileName(file)}");
    questions.AddRange(TriviaQuestion_Extensions.ExtractQuestions(file));
}
TriviaQuestion_Extensions.WriteQuestionsToJsonFile(questions, $"{AppContext.BaseDirectory}questions.json");
Console.WriteLine("Done");

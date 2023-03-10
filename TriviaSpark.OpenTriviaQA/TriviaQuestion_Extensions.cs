using OpenTriviaQA.Models;
using System.Text.Json;
using TriviaSpark.Core.Extensions;

namespace OpenTriviaQA
{
    public class TriviaQuestion_Extensions
    {
        public static List<TriviaQuestion> ExtractQuestions(string filePath)
        {
            var lines = File.ReadAllLines(filePath);

            var questions = new List<TriviaQuestion>();
            var badanswers = new List<string>();
            var currentQuestion = new TriviaQuestion();
            var readingAnswers = false;

            foreach (var line in lines)
            {
                if (line.StartsWith("#Q"))
                {
                    if (readingAnswers)
                    {
                        currentQuestion.IncorrectAnswers = badanswers.Where(w => w != currentQuestion.CorrectAnswer).ToList();
                        questions.Add(currentQuestion);
                        currentQuestion = new TriviaQuestion();
                        badanswers = new List<string>();
                        readingAnswers = false;
                    }
                    currentQuestion.Category = Path.GetFileName(filePath);
                    currentQuestion.Difficulty = "Medium";
                    currentQuestion.Type = "Multiple Choice";
                    currentQuestion.QuestionText = line.Substring(3).Trim();
                    currentQuestion.Id = currentQuestion.QuestionText.GetDeterministicHashCode().ToString();
                }
                else if (line.StartsWith("^"))
                {
                    currentQuestion.CorrectAnswer = line.Substring(1).Trim();
                    readingAnswers = true;
                }
                else if (line.StartsWith("A"))
                {
                    badanswers.Add(line.Substring(1).Trim());
                }
                else if (line.StartsWith("B"))
                {
                    badanswers.Add(line.Substring(1).Trim());
                }
                else if (line.StartsWith("C"))
                {
                    badanswers.Add(line.Substring(1).Trim());
                }
                else if (line.StartsWith("D"))
                {
                    badanswers.Add(line.Substring(1).Trim());
                }
                else if (line.StartsWith("E"))
                {
                    badanswers.Add(line.Substring(1).Trim());
                }
            }

            questions.Add(currentQuestion);

            return questions;
        }

        public static void WriteQuestionsToJsonFile(List<TriviaQuestion> questions, string filePath)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(questions, options);

            File.WriteAllText(filePath, json);
        }
        public static List<string> GetFilePathsInFolder(string folderPath)
        {
            var filePaths = new List<string>();

            if (!Directory.Exists(folderPath))
            {
                Console.WriteLine($"Folder '{folderPath}' does not exist.");
                return filePaths;
            }

            var files = Directory.GetFiles(folderPath);

            foreach (var filePath in files)
            {
                try
                {
                    var fileInfo = new FileInfo(filePath);
                    filePaths.Add(fileInfo.FullName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to process file '{filePath}': {ex.Message}");
                }
            }
            return filePaths;
        }
    }
}

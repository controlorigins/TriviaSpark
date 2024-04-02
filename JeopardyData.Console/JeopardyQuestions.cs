namespace JeopardyData;

public class JeopardyQuestions : List<JeopardyQuestion>
{
    public void DisplayQuestionsFromRandomCategory()
    {
        if (this.Count == 0)
        {
            Console.WriteLine("No questions available.");
            return;
        }

        // Group questions by category, air date, and round, then select a random group
        var random = new Random();
        var groupedQuestions = this
            .GroupBy(q => new { q.Category, q.AirDate, q.Round })
            .ToList();

        if (groupedQuestions.Count == 0)
        {
            Console.WriteLine("No groups of questions available.");
            return;
        }

        var randomGroup = groupedQuestions[random.Next(groupedQuestions.Count)];
        var selectedQuestions = randomGroup.Take(5).ToList(); // Take up to 5 questions from the selected group

        // Displaying the details of the questions from the selected group
        if (selectedQuestions.Any())
        {
            Console.WriteLine($"Selected Category: {selectedQuestions.First().Category}");
            Console.WriteLine($"Air Date: {selectedQuestions.First().AirDate}");
            Console.WriteLine($"Round: {selectedQuestions.First().Round}");

            foreach (var question in selectedQuestions)
            {
                Console.WriteLine($"----------");
                Console.WriteLine($"Question: {question.Question}");
                Console.WriteLine($"Value: {question.Value}");
                Console.WriteLine($"Answer: {question.Answer}");
            }
        }
        else
        {
            Console.WriteLine("No questions found in the selected group.");
        }
    }
}

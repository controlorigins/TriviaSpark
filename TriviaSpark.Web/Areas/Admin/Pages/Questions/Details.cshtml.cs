using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TriviaSpark.Core.Entities;

namespace TriviaSpark.Web.Areas.Admin.Pages.Questions;
public class DetailsModel(TriviaSparkWebContext context) : AdminPageModel(context)
{
    public Question Question { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (id == null || _context.Questions == null)
        {
            return NotFound();
        }

        var question = await _context.Questions.Include(a => a.Answers).FirstOrDefaultAsync(m => m.QuestionId == id);
        if (question == null)
        {
            return NotFound();
        }
        else
        {
            Question = question;
        }
        return Page();
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TriviaSpark.Web.Areas.Admin.Pages.Questions;
public class DeleteModel : AdminPageModel
{
    public DeleteModel(Core.Match.Entities.TriviaSparkWebContext context) : base(context)
    {
    }

    [BindProperty]
    public Core.Match.Entities.Question Question { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string id)
    {
        if (id == null || _context.Questions == null)
        {
            return NotFound();
        }

        var question = await _context.Questions.FirstOrDefaultAsync(m => m.QuestionId == id);

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

    public async Task<IActionResult> OnPostAsync(string id)
    {
        if (id == null || _context.Questions == null)
        {
            return NotFound();
        }
        var question = await _context.Questions.FindAsync(id);

        if (question != null)
        {
            Question = question;
            _context.Questions.Remove(Question);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}

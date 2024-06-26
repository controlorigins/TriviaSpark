using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TriviaSpark.Web.Areas.Admin.Pages.Questions;

public class EditModel : AdminPageModel
{
    public EditModel(Core.Match.Entities.TriviaSparkWebContext context) : base(context)
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
        Question = question;
        return Page();
    }

    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see https://aka.ms/RazorPagesCRUD.
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Attach(Question).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!QuestionExists(Question.QuestionId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return RedirectToPage("./Index");
    }

    private bool QuestionExists(string id)
    {
        return (_context.Questions?.Any(e => e.QuestionId == id)).GetValueOrDefault();
    }
}

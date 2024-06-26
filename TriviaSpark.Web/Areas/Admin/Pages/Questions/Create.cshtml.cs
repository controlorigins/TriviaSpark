using Microsoft.AspNetCore.Mvc;

namespace TriviaSpark.Web.Areas.Admin.Pages.Questions;

public class CreateModel : AdminPageModel
{
    public CreateModel(Core.Match.Entities.TriviaSparkWebContext context) : base(context)
    {
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Core.Match.Entities.Question Question { get; set; } = default!;


    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || _context.Questions == null || Question == null)
        {
            return Page();
        }
        _context.Questions.Add(Question);
        await _context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}

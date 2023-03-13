using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TriviaSpark.Web.Areas.Identity.Data;

namespace TriviaSpark.Web.Areas.Admin.Pages.Matches;

public class DeleteModel : PageModel
{
    private readonly TriviaSparkWebContext _context;

    public DeleteModel(TriviaSparkWebContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Match Match { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null || _context.Matches == null)
        {
            return NotFound();
        }

        var match = await _context.Matches.FirstOrDefaultAsync(m => m.MatchId == id);

        if (match == null)
        {
            return NotFound();
        }
        else
        {
            Match = match;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id)
    {
        if (id == null || _context.Matches == null)
        {
            return NotFound();
        }
        var match = await _context.Matches.FindAsync(id);

        if (match != null)
        {
            Match = match;
            _context.MatchAnswers.RemoveRange(_context.MatchAnswers.Where(ma => ma.MatchId == Match.MatchId));
            _context.MatchQuestions.RemoveRange(_context.MatchQuestions.Where(mq => mq.MatchId == Match.MatchId));
            _context.Matches.Remove(Match);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("./Index");
    }
}

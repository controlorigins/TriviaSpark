using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TriviaSpark.Core.Match;
using TriviaSpark.Web.Areas.Identity.Services;

namespace TriviaSpark.Web.Areas.Admin.Pages.Matches;

public class DeleteModel : PageModel
{
    private readonly IMatchService _matchService;

    public DeleteModel(IMatchService matchService)
    {
        _matchService = matchService;
    }

    public async Task<IActionResult> OnGetAsync(int? id, CancellationToken ct)
    {
        if (id == null)
        {
            return NotFound();
        }
        var match = await _matchService.GetUserMatchAsync(User, id, ct);
        if (match is null)
        {
            return NotFound();
        }
        Match = match;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int? id, CancellationToken ct)
    {
        if (id == null)
        {
            return NotFound();
        }
        return RedirectToPage("./Index");
    }

    [BindProperty]
    public MatchModel Match { get; set; } = default!;
}

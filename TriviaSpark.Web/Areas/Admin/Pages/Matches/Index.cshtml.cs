using Microsoft.AspNetCore.Mvc.RazorPages;
using TriviaSpark.Core.Models;
using TriviaSpark.Core.Services;

namespace TriviaSpark.Web.Areas.Admin.Pages.Matches;

public class IndexModel(IMatchService _matchService) : PageModel
{
    public async Task OnGetAsync(CancellationToken ct)
    {
        Match = await _matchService.GetMatchesAsync(ct);
    }
    public IList<MatchModel> Match { get; set; } = default!;
}

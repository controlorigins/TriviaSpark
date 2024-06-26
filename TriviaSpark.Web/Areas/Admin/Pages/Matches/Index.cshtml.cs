using Microsoft.AspNetCore.Mvc.RazorPages;
using TriviaSpark.Core.Match.Models;

namespace TriviaSpark.Web.Areas.Admin.Pages.Matches
{
    public class IndexModel(Core.Match.Services.IMatchService matchService) : PageModel
    {
        private readonly Core.Match.Services.IMatchService _matchService = matchService;

        public async Task OnGetAsync(CancellationToken ct)
        {
            Match = await _matchService.GetMatchesAsync(ct);
        }

        public IList<MatchModel> Match { get; set; } = default!;
    }
}

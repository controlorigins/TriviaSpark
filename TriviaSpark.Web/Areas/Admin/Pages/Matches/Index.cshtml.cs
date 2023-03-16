using Microsoft.AspNetCore.Mvc.RazorPages;
using TriviaSpark.Core.Match;
using TriviaSpark.Web.Areas.Identity.Services;

namespace TriviaSpark.Web.Areas.Admin.Pages.Matches
{
    public class IndexModel : PageModel
    {
        private readonly IMatchService _matchService;

        public IndexModel(IMatchService matchService)
        {
            _matchService = matchService;
        }

        public async Task OnGetAsync(CancellationToken ct)
        {
            Match = await _matchService.GetMatchesAsync(ct);
        }

        public IList<MatchModel> Match { get; set; } = default!;
    }
}

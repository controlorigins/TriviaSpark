using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TriviaSpark.Web.Areas.Admin.Pages.Matches
{
    public class DetailsModel : PageModel
    {
        private readonly Core.Services.IMatchService _matchService;

        public DetailsModel(Core.Services.IMatchService matchService)
        {
            _matchService = matchService;
        }

        public Core.Models.MatchModel Match { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            var mat = await _matchService.GetMatchAsync(id.Value);
            if (mat is null)
            {
                return NotFound();
            }
            else
            {
                Match = mat;
            }
            return Page();
        }
    }
}

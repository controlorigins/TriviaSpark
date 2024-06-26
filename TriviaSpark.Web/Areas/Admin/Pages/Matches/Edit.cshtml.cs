using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TriviaSpark.Core.Match.Models;

namespace TriviaSpark.Web.Areas.Admin.Pages.Matches
{
    public class EditModel : PageModel
    {
        private readonly Core.Match.Services.IMatchService _matchService;

        public EditModel(Core.Match.Services.IMatchService matchService)
        {
            _matchService = matchService;
        }

        private static SelectListItem Create(UserModel user)
        {
            return new SelectListItem()
            {
                Text = user.UserName,
                Value = user.UserId
            };
        }

        private IEnumerable<SelectListItem> Create(List<UserModel> users)
        {
            return users.Select(Create);
        }

        public async Task<IActionResult> OnGetAsync(int? id, CancellationToken ct = default)
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
            Users = Create(await _matchService.GetUsersAsync(ct));
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var result = await _matchService.UpdateMatchAsync(Match, ct);

            return RedirectToPage("./Index");
        }

        [BindProperty]
        public MatchModel Match { get; set; } = default!;
        [BindProperty]
        public IEnumerable<SelectListItem> Users { get; set; }

    }
}

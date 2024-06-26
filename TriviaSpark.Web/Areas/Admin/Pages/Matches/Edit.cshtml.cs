using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TriviaSpark.Web.Areas.Admin.Pages.Matches;

public class EditModel(Core.Services.IMatchService matchService) : PageModel
{
    private static SelectListItem Create(Core.Models.UserModel user)
    {
        return new SelectListItem()
        {
            Text = user.UserName,
            Value = user.UserId
        };
    }

    private IEnumerable<SelectListItem> Create(List<Core.Models.UserModel> users)
    {
        return users.Select(Create);
    }

    public async Task<IActionResult> OnGetAsync(int? id, CancellationToken ct = default)
    {
        if (id == null)
        {
            return NotFound();
        }
        var match = await matchService.GetUserMatchAsync(User, id, ct);
        if (match is null)
        {
            return NotFound();
        }
        Match = match;
        Users = Create(await matchService.GetUsersAsync(ct));
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var result = await matchService.UpdateMatchAsync(Match, ct);

        return RedirectToPage("./Index");
    }

    [BindProperty]
    public Core.Models.MatchModel Match { get; set; } = default!;
    [BindProperty]
    public IEnumerable<SelectListItem> Users { get; set; }

}

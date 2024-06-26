using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TriviaSpark.Core.Models;
using TriviaSpark.Core.Services;

namespace TriviaSpark.Web.Pages;

[Authorize]
public class UserMatchesModel(
    ILogger<UserMatchesModel> logger,
    IMatchService MatchService) : PageModel
{
    [BindProperty]
    public List<MatchModel> Matches { get; set; } = new List<MatchModel>();

    [BindProperty]
    public MatchModel NewMatch { get; set; } = new MatchModel();

    public async Task OnGet()
    {
        Matches = await MatchService.GetUserMatchesAsync(User, null);
    }

    public async Task<IActionResult> OnPost(CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            NewMatch = await MatchService.CreateMatchAsync(NewMatch, User, ct);
            return RedirectToPage("./UserMatches");
        }
        else
        {
            return Page();
        }
    }
}


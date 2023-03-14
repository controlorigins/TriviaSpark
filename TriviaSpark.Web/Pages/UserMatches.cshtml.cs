using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TriviaSpark.Core.Match;
using TriviaSpark.Web.Areas.Identity.Services;

namespace TriviaSpark.Web.Pages;

[Authorize]
public class UserMatchesModel : PageModel
{
    private readonly ILogger<UserMatchesModel> _logger;
    private readonly IMatchService _matchService;

    public UserMatchesModel(
        ILogger<UserMatchesModel> logger,
        IMatchService MatchService)
    {
        _logger = logger;
        _matchService = MatchService;
        Matches = new List<MatchModel>();
        NewMatch = new MatchModel();
    }

    [BindProperty]
    public List<MatchModel> Matches { get; set; }

    [BindProperty]
    public MatchModel NewMatch { get; set; }

    public async Task OnGet()
    {
        Matches = await _matchService.GetUserMatchesAsync(User, null);
    }

    public async Task<IActionResult> OnPost(CancellationToken ct)
    {
        if (ModelState.IsValid)
        {
            NewMatch = await _matchService.CreateMatchAsync(NewMatch, User, ct);
            return RedirectToPage("./UserMatches");
        }
        else
        {
            return Page();
        }
    }
}


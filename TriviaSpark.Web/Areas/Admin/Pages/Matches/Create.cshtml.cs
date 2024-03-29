﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TriviaSpark.Core.Match;
using TriviaSpark.Web.Areas.Identity.Services;

namespace TriviaSpark.Web.Areas.Admin.Pages.Matches
{
    public class CreateModel : PageModel
    {
        private readonly IMatchService _matchService;

        public CreateModel(IMatchService matchService)
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

        public async Task<IActionResult> OnGet(CancellationToken ct)
        {
            Users = Create(await _matchService.GetUsersAsync(ct));
            return Page();
        }


        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            if (!ModelState.IsValid || Match == null)
            {
                return Page();
            }
            var result = await _matchService.CreateMatchAsync(Match, User, ct);

            return RedirectToPage("./Index");
        }

        [BindProperty]
        public MatchModel Match { get; set; } = default!;

        [BindProperty]
        public IEnumerable<SelectListItem> Users { get; set; }
    }
}

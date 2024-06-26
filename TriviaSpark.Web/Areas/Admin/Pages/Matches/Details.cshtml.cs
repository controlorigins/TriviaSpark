﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TriviaSpark.Core.Match.Models;
using TriviaSpark.Web.Areas.Identity.Services;

namespace TriviaSpark.Web.Areas.Admin.Pages.Matches
{
    public class DetailsModel : PageModel
    {
        private readonly Core.Match.Services.IMatchService _matchService;

        public DetailsModel(Core.Match.Services.IMatchService matchService)
        {
            _matchService = matchService;
        }

        public MatchModel Match { get; set; } = default!;

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

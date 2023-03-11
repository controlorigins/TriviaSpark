using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TriviaSpark.Web.Areas.Identity.Data;

namespace TriviaSpark.Web.Pages.MatchAdmin
{
    public class DeleteModel : PageModel
    {
        private readonly TriviaSpark.Web.Areas.Identity.Data.TriviaSparkWebContext _context;

        public DeleteModel(TriviaSpark.Web.Areas.Identity.Data.TriviaSparkWebContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Match Match { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Matches == null)
            {
                return NotFound();
            }

            var match = await _context.Matches.FirstOrDefaultAsync(m => m.MatchId == id);

            if (match == null)
            {
                return NotFound();
            }
            else
            {
                Match = match;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Matches == null)
            {
                return NotFound();
            }
            var match = await _context.Matches.FindAsync(id);

            if (match != null)
            {
                Match = match;
                _context.Matches.Remove(Match);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}

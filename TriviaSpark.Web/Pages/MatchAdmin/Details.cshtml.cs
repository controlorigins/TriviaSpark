using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TriviaSpark.Web.Areas.Identity.Data;

namespace TriviaSpark.Web.Pages.MatchAdmin
{
    public class DetailsModel : PageModel
    {
        private readonly TriviaSparkWebContext _context;

        public DetailsModel(TriviaSparkWebContext context)
        {
            _context = context;
        }

      public Match Match { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Matches == null)
            {
                return NotFound();
            }

            var match = await _context.Matches.Include(i=>i.User).FirstOrDefaultAsync(m => m.MatchId == id);
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
    }
}

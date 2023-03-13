using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TriviaSpark.Web.Areas.Identity.Data;

namespace TriviaSpark.Web.Areas.Admin.Pages.Matches
{
    public class IndexModel : PageModel
    {
        private readonly TriviaSpark.Web.Areas.Identity.Data.TriviaSparkWebContext _context;

        public IndexModel(TriviaSpark.Web.Areas.Identity.Data.TriviaSparkWebContext context)
        {
            _context = context;
        }

        public IList<Match> Match { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Matches != null)
            {
                Match = await _context.Matches
                .Include(m => m.User).ToListAsync();
            }
        }
    }
}

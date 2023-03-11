using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TriviaSpark.Web.Areas.Identity.Data;

namespace TriviaSpark.Web.Pages.MatchAdmin
{
    public class EditModel : PageModel
    {
        private readonly TriviaSparkWebContext _context;

        public EditModel(TriviaSparkWebContext context)
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
            Match = match;
            Users = Create(await _context.Users.ToListAsync());
            return Page();
        }

        private static SelectListItem Create(TriviaSparkWebUser user)
        {
            return new SelectListItem()
            {
                Text = user.UserName,
                Value = user.Id
            };
        }

        private IEnumerable<SelectListItem> Create(List<TriviaSparkWebUser> users)
        {
            return users.Select(Create);
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Match).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatchExists(Match.MatchId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }
        [BindProperty]
        public IEnumerable<SelectListItem> Users { get; set; }

        private bool MatchExists(int id)
        {
            return (_context.Matches?.Any(e => e.MatchId == id)).GetValueOrDefault();
        }
    }
}

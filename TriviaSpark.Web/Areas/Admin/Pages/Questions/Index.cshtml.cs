using Microsoft.EntityFrameworkCore;
using TriviaSpark.Core.Entities;

namespace TriviaSpark.Web.Areas.Admin.Pages.Questions;

public class IndexModel(TriviaSparkWebContext context) : AdminPageModel(context)
{
    public IList<Question> Question { get; set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Questions != null)
        {
            Question = await _context.Questions.ToListAsync();
        }
    }
}

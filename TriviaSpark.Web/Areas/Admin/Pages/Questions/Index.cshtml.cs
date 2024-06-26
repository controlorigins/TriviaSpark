using Microsoft.EntityFrameworkCore;

namespace TriviaSpark.Web.Areas.Admin.Pages.Questions;

public class IndexModel : AdminPageModel
{
    public IndexModel(Core.Match.Entities.TriviaSparkWebContext context) : base(context)
    {
    }

    public IList<Core.Match.Entities.Question> Question { get; set; } = default!;

    public async Task OnGetAsync()
    {
        if (_context.Questions != null)
        {
            Question = await _context.Questions.ToListAsync();
        }
    }
}

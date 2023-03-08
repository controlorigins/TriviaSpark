using Microsoft.EntityFrameworkCore;
using TriviaSpark.Web.Areas.Identity.Data;

namespace TriviaSpark.Web.Pages.QuestionAdmin
{
    public class IndexModel : AdminPageModel
    {
        public IndexModel(TriviaSparkWebContext context) : base(context)
        {
        }

        public IList<Question> Question { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Questions != null)
            {
                Question = await _context.Questions.ToListAsync();
            }
        }
    }
}

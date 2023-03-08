using Microsoft.AspNetCore.Mvc.RazorPages;
using TriviaSpark.Web.Areas.Identity.Data;

namespace TriviaSpark.Web.Pages.QuestionAdmin
{
    public class AdminPageModel : PageModel
    {
        protected readonly TriviaSparkWebContext _context;

        public AdminPageModel(TriviaSparkWebContext context)
        {
            _context = context;
        }
    }
}

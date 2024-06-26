using Microsoft.AspNetCore.Mvc.RazorPages;
using TriviaSpark.Core.Match.Entities;

namespace TriviaSpark.Web.Areas.Admin.Pages.Questions;

public class AdminPageModel : PageModel
{
    protected readonly TriviaSparkWebContext _context;

    public AdminPageModel(TriviaSparkWebContext context)
    {
        _context = context;
    }
}

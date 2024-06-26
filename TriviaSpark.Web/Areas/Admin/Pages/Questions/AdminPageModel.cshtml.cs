using Microsoft.AspNetCore.Mvc.RazorPages;
using TriviaSpark.Core.Entities;

namespace TriviaSpark.Web.Areas.Admin.Pages.Questions;

public class AdminPageModel(TriviaSparkWebContext context) : PageModel
{
    public TriviaSparkWebContext _context { get; } = context;
}

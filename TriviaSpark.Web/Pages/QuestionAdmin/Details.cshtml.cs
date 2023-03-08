using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TriviaSpark.Web.Areas.Identity.Data;

namespace TriviaSpark.Web.Pages.QuestionAdmin
{
    public class DetailsModel : AdminPageModel
    {
        public DetailsModel(TriviaSparkWebContext context): base(context)
        {
        }

      public Question Question { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Questions == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.Include(a=>a.Answers).FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }
            else 
            {
                Question = question;
            }
            return Page();
        }
    }
}

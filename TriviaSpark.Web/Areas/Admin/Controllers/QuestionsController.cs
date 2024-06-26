using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TriviaSpark.Core.Entities;

namespace TriviaSpark.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class QuestionsController(TriviaSparkWebContext context) : Controller
{

    // GET: Admin/Questions
    public async Task<IActionResult> Index()
    {
        return context.Questions != null ?
                    View(await context.Questions.ToListAsync()) :
                    Problem("Entity set 'TriviaSparkWebContext.Questions'  is null.");
    }

    // GET: Admin/Questions/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null || context.Questions == null)
        {
            return NotFound();
        }

        var question = await context.Questions
            .FirstOrDefaultAsync(m => m.QuestionId == id);
        if (question == null)
        {
            return NotFound();
        }

        return View(question);
    }

    // GET: Admin/Questions/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Admin/Questions/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("QuestionId,QuestionText,Category,Difficulty,Type,Source,CreatedDate,ModifiedDate")] Core.Entities.Question question)
    {
        if (ModelState.IsValid)
        {
            context.Add(question);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(question);
    }

    // GET: Admin/Questions/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null || context.Questions == null)
        {
            return NotFound();
        }

        var question = await context.Questions.FindAsync(id);
        if (question == null)
        {
            return NotFound();
        }
        return View(question);
    }

    // POST: Admin/Questions/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("QuestionId,QuestionText,Category,Difficulty,Type,Source,CreatedDate,ModifiedDate")] Core.Entities.Question question)
    {
        if (id != question.QuestionId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(question);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(question.QuestionId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(question);
    }

    // GET: Admin/Questions/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null || context.Questions == null)
        {
            return NotFound();
        }

        var question = await context.Questions
            .FirstOrDefaultAsync(m => m.QuestionId == id);
        if (question == null)
        {
            return NotFound();
        }

        return View(question);
    }

    // POST: Admin/Questions/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        if (context.Questions == null)
        {
            return Problem("Entity set 'TriviaSparkWebContext.Questions'  is null.");
        }
        var question = await context.Questions.FindAsync(id);
        if (question != null)
        {
            context.Questions.Remove(question);
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool QuestionExists(string id)
    {
        return (context.Questions?.Any(e => e.QuestionId == id)).GetValueOrDefault();
    }
}

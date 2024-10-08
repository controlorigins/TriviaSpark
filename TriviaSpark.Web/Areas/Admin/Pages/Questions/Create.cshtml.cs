﻿using Microsoft.AspNetCore.Mvc;
using TriviaSpark.Core.Entities;

namespace TriviaSpark.Web.Areas.Admin.Pages.Questions;

public class CreateModel(TriviaSparkWebContext context) : AdminPageModel(context)
{
    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Question Question { get; set; } = default!;


    // To protect from over posting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || _context.Questions == null || Question == null)
        {
            return Page();
        }
        _context.Questions.Add(Question);
        await _context.SaveChangesAsync();
        return RedirectToPage("./Index");
    }
}

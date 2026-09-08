using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ModestFeedback.Data;
using ModestFeedback.Models;
using System.ComponentModel.DataAnnotations;
public class IndexModel : PageModel
{
    private readonly Context ctx;
    public IndexModel(Context ctx)
    {
        this.ctx = ctx;
    }
    [BindProperty]
    [StringLength(100)]
    public string? Name { get; set; }

    [BindProperty]
    [Range(1, 5, ErrorMessage = "Choose a rating between 1 and 5.")]
    public int Rating { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Please write a comment.")]
    [StringLength(2000, ErrorMessage = "Keep your comment under 2,001 characters.")]
    public string? Comment { get; set; }
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var submission = new Submission
        {
            Name = Name,
            Rating = Rating,
            Comment = Comment!
        };

        ctx.Submissions.Add(submission);
        await ctx.SaveChangesAsync();

        return RedirectToPage("/Thanks");
    }
}
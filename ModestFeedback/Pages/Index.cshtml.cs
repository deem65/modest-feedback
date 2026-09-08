using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModestFeedback.Data;
using ModestFeedback.Models;
using ModestFeedback.Services;
using System.ComponentModel.DataAnnotations;
public class IndexModel : PageModel
{
    private readonly Context ctx;
    private readonly EmailService emailService;
    private readonly ClassificationService classificationService;

    public IndexModel(Context ctx, EmailService emailService, ClassificationService classificationService)
    {
        this.ctx = ctx;
        this.emailService = emailService;
        this.classificationService = classificationService;
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

        await ctx.SaveChangesAsync(); //safe
        await ClassifySubmissionAsync(submission);
        await ctx.SaveChangesAsync(); //depend
        try
        {
            await emailService.SendAsync(
                "davidsoloca06@gmail.com",
                $"New feedback #{submission.Id}",
                $"Name: {submission.Name ?? "Anonymous"}\n" +
                $"Rating: {submission.Rating}/5\n" +
                $"Comment: {submission.Comment}\n\n" +
                $"Decision: {submission.Decision?.ToString() ?? "Not classified"}\n" +
                $"Reason: {submission.Reason ?? "Classification unavailable; review manually."}"
                );
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Email failed for submission #{submission.Id}: {ex.Message}");

            return RedirectToPage("/Thanks");
        }
        submission.IsEmailSent = true;
        return RedirectToPage("/Thanks");
    }
    private async Task ClassifySubmissionAsync(Submission submission)
    {
        try
        {
            ResultType result = await classificationService.ClassifyAsync(submission.Comment);

            submission.Decision = result.Decision;
            submission.Reason = result.Reason;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Classification failed for submission #{submission.Id}: {ex.Message}");
        }
    }
}
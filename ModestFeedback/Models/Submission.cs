namespace ModestFeedback.Models;

public class Submission
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime SubmittedAtUtc { get; set; } = DateTime.UtcNow;
    public bool IsEmailSent { get; set; }
    public Decision? Decision { get; set; }
    public string? Reason { get; set; }
}
using Microsoft.EntityFrameworkCore;
using ModestFeedback.Models;


namespace ModestFeedback.Data;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    public DbSet<Submission> Submissions { get; set; }
}
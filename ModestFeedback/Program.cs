using Microsoft.EntityFrameworkCore;
using ModestFeedback.Data;
using ModestFeedback.Services;
using Org.BouncyCastle.Security;

string srcString = "DataSource";
string src = "feedback.db";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<Context>(options => options.UseSqlite($"{srcString}={src}"));
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<ClassificationService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();
app.Run();


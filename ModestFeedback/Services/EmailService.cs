using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace ModestFeedback.Services;

public class EmailService
{
    private readonly string address;
    private readonly string appPassword;

    public EmailService(IConfiguration configuration)
    {
        address = 
            configuration["Email:Address"]
            ?? 
            throw new InvalidOperationException("Email address is missing.");

        appPassword = 
            configuration["Email:AppPassword"]
            ?? 
            throw new InvalidOperationException("Email app password is missing.");
    }

    public async Task SendAsync(string recipient, string subject, string body)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress("Modest Feedback", address));
        message.To.Add(MailboxAddress.Parse(recipient));
        message.Subject = subject;
        message.Body = new TextPart("plain")
        {
            Text = body
        };

        using var client = new SmtpClient();

        await client.ConnectAsync(
            "smtp.gmail.com",
            587,
            SecureSocketOptions.StartTls);

        await client.AuthenticateAsync(address, appPassword);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
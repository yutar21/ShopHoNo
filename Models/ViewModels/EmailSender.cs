using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger<EmailSender> _logger;

    public EmailSender(ILogger<EmailSender> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        try
        {
            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587, // Cổng cho Gmail
                Credentials = new NetworkCredential("phiphong668@gmail.com", "oxuj kghg ckga magt"),
                EnableSsl = true, // Bật SSL
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("phiphong668@gmail.com"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true, // Nếu nội dung email là HTML
            };

            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);

            _logger.LogInformation($"Email sent to {email}.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error sending email: {ex.Message}");
        }
    }
}

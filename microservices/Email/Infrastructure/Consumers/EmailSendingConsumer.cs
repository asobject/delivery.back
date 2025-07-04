
using BuildingBlocks.Exceptions;
using BuildingBlocks.Messaging.Events;
using Domain.Models;
using MailKit.Net.Smtp;
using MassTransit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Consumers;

public class EmailSendingConsumer(
    IOptions<SmtpSettings> smtpSettings,
    ILogger<EmailSendingConsumer> logger)
    : IConsumer<EmailSendingEvent>
{
    public async Task Consume(ConsumeContext<EmailSendingEvent> context)
    {
        var message = context.Message;
        logger.LogInformation("Processing email for {To}", message.To);

        try
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(smtpSettings.Value.Username));
            email.To.Add(MailboxAddress.Parse(message.To));
            email.Subject = message.Subject;
            email.Body = new TextPart("plain") { Text = message.Body };

            using var client = new SmtpClient();

            await client.ConnectAsync(
                smtpSettings.Value.Server,
                smtpSettings.Value.Port,
                MailKit.Security.SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                smtpSettings.Value.Username,
                smtpSettings.Value.Password);

            await client.SendAsync(email);
            await client.DisconnectAsync(true);

            logger.LogInformation("Email sent to {To}", message.To);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending email to {To}", message.To);
            throw new EmailSendingException(message.To, ex);
        }
    }
}
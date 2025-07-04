

using Domain.Models;

namespace Domain.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage message);
}
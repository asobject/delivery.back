
using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IUserService
{
    Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user);
    Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
}

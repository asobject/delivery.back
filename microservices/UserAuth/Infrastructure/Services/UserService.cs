

using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services;

public class UserService(UserManager<ApplicationUser> userManager) : IUserService
{
    public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user) =>
          await userManager.GenerateEmailConfirmationTokenAsync(user);

    public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user) =>
        await userManager.GeneratePasswordResetTokenAsync(user);
}

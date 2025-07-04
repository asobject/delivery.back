

namespace Domain.Models;

public record ChangeUserPasswordRequest(string CurrentPassword, string NewPassword);



namespace Domain.Interfaces;

public interface IAiResponseGenerator
{
    Task<string> GenerateResponseAsync(string prompt);
}
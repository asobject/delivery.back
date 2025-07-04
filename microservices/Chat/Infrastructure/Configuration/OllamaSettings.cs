

namespace Infrastructure.Configuration;

public class OllamaSettings
{
    public const string SectionName = "Ollama";

    public string BaseUrl { get; set; } = null!;
    public string Model { get; set; } = null!;
    public int TimeoutSeconds { get; set; } = 30;
}

namespace Domain.Models;

public class ChatMessage
{
    public string Content { get; set; } = null!;
    public bool IsUserMessage { get; set; }
    public DateTime Timestamp { get; set; }
}
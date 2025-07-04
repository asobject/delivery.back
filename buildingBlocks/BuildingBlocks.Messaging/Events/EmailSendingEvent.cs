
namespace BuildingBlocks.Messaging.Events;

public record EmailSendingEvent(
    string To,
    string Subject,
    string Body);
﻿

namespace BuildingBlocks.Messaging.Events;

public record OrderCreatedEvent(
    int OrderId,
    string ReceiverEmail);
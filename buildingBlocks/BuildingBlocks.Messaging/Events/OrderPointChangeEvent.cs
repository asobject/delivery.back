

namespace BuildingBlocks.Messaging.Events;

public record  OrderPointChangeEvent(Guid Tracker,int PointId);

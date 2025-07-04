

using BuildingBlocks.Enums;
using Domain.DTOs;
using Domain.Entities;

namespace Application.Mappings;

public static class OrderMapper
{

    public static OrderDTO MapOrder(Order order, Dictionary<int, string> addressLookup)
    {
        return new OrderDTO(
            Tracker: order.Tracker,
            CurrentPointAddress: order.CurrentPointId.HasValue
            ? addressLookup.GetValueOrDefault(order.CurrentPointId.Value, "Адрес недоступен")
            : null,
            SenderAddress: GetAddress(order.SenderDeliveryPoint, addressLookup),
            ReceiverAddress: GetAddress(order.ReceiverDeliveryPoint, addressLookup),
            Status: order.OrderStatus
        );
    }

    private static string GetAddress(DeliveryPoint point, Dictionary<int, string> addressLookup)
    {
        return point.Method switch
        {
            DeliveryMethod.CourierCall =>
                point.CustomPoint?.Address ?? "Адрес не указан",

            DeliveryMethod.PickupPoint when point.CompanyPointId.HasValue =>
                addressLookup.TryGetValue(point.CompanyPointId.Value, out var addr)
                    ? addr
                    : "Адрес пункта выдачи недоступен",

            _ => "Неизвестный метод доставки"
        };
    }
}
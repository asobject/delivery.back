
namespace Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderResponse
{
    public Guid Tracker { get; set; }
    public string Message { get; set; } = null!;
}

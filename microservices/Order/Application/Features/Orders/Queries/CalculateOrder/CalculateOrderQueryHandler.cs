using Application.Interfaces;
using MediatR;

namespace Application.Features.Orders.Queries.CalculateOrder;

public class CalculateOrderQueryHandler(ICalculationService calculationService) : IRequestHandler<CalculateOrderQuery, CalculateOrderResponse>
{
    public async Task<CalculateOrderResponse> Handle(CalculateOrderQuery request, CancellationToken cancellationToken)
    {

        var price = await calculationService.CalculatePriceAsync(request);
        return new CalculateOrderResponse
        {
            Price = price
        };
    }
}

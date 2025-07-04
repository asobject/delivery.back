
using Application.Features.Orders.Queries.CalculateOrder;

namespace Application.Interfaces;
public interface ICalculationService
{
    Task<double> CalculatePriceAsync(CalculateOrderQuery order);
}
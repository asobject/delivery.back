
using Application.Features.OrderChanges.Queries.PointChange;
using Application.Features.OrderChanges.Queries.StatusChange;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace OrderHistory.API.Controllers;

[Route("api/orders/{tracker}/history")]
[ApiController]
public class OrderHistory(IMediator mediator) : ControllerBase
{
   
    [HttpGet]
    public async Task<IActionResult> OrderChanges(Guid tracker)
    {
        var pointChangeQuery = new OrderPointChangeQuery(tracker);
        var statusChangeQuery = new OrderStatusChangeQuery(tracker);

        var pointChanges = await mediator.Send(pointChangeQuery);
        var statusChanges = await mediator.Send(statusChangeQuery);
        return Ok(new { pointChanges.OrderPointChanges, statusChanges.OrderStatusChanges });
    }
}

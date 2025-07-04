
using Application.Features.Orders.Commands.CreateOrder;
using Application.Features.Orders.Commands.PointChange;
using Application.Features.Orders.Commands.StatusChange;
using Application.Features.Orders.Queries.CalculateOrder;
using Application.Features.Orders.Queries.GetOrderByTracker;
using Application.Features.Orders.Queries.GetOrdersPaged;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Order.API.Controllers;

[Route("api/orders")]
[ApiController]
public class OrdersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, [FromHeader(Name = "X-User-Sub")] string sub, [FromHeader(Name = "X-User-Email")] string email)
    {
        _ = await mediator.Send(new CreateOrderCommand(request, sub, email));
        return Created();
    }
    [HttpPost("calculate")]
    public async Task<IActionResult> CalculateOrderPrice([FromBody] CalculateOrderQuery query)
    {
        var price = await mediator.Send(query);
        return Ok(price);
    }
    [HttpPatch("{tracker}/status")]
    public async Task<IActionResult> OrderStatusChange(Guid tracker, [FromBody] OrderStatusChangeRequest request)
    {
        var response = await mediator.Send(new OrderStatusChangeCommand(tracker, request.Status));
        return Ok(response);
    }
    [HttpPatch("{tracker}/point")]
    public async Task<IActionResult> OrderPointChange(Guid tracker, [FromBody] OrderPointChangeRequest request)
    {
        var response = await mediator.Send(new OrderPointChangeCommand(tracker, request.PointId));
        return Ok(response);
    }
    [HttpGet("{tracker?}")]
    public async Task<IActionResult> GetOrders([FromHeader(Name = "X-User-Sub")] string? sub,Guid? tracker, [FromQuery] int pageNumber = 1,
          [FromQuery] int pageSize = 10)
    {
        if (tracker.HasValue)
        {
            var query = new GetOrderByTrackerQuery(tracker.Value);
            var response = await mediator.Send(query);
            return Ok(response);
        }

        if (string.IsNullOrEmpty(sub))
        {
            return BadRequest("User sub is required for orders pagination");
        }

        var pagedQuery = new GetOrdersPagedQuery(sub, pageNumber, pageSize);
        var pagedResponse = await mediator.Send(pagedQuery);
        return Ok(pagedResponse);
    }
}

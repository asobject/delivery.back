
using Application.Features.Points.Commands.CreatePoint;
using Application.Features.Points.Commands.DeletePoints;
using Application.Features.Points.Commands.UpdatePoint;
using Application.Features.Points.Queries.GetCheckPointExistInRadius;
using Application.Features.Points.Queries.GetClusters;
using Application.Features.Points.Queries.GetPagedPoints;
using Application.Features.Points.Queries.GetPointById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompanyPoints.API.Controllers;

[Route("api/points")]
[ApiController]
public class PointsController(IMediator mediator) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> CreatePoint([FromBody] CreatePointCommand command)
    {
        _ = await mediator.Send(command);
        return Created();
    }
    [HttpGet]
    public async Task<IActionResult> GetPoints(
           [FromQuery] int? id,
           [FromQuery] double? lat,
           [FromQuery] double? lon,
           [FromQuery] int pageNumber = 1,
           [FromQuery] int pageSize = 10)
    {
        if (id.HasValue)
        {
            var byId = new GetPointByIdQuery
            {
                Id = id.Value
            };
            var point = await mediator.Send(byId);
            return Ok(point);
        }
        if (lat.HasValue && lon.HasValue)
        {
            var existQuery = new GetCheckPointExistInRadiusQuery(
                new BuildingBlocks.Models.GeoPoint(lat.Value, lon.Value));
            var exist = await mediator.Send(existQuery);
            return Ok(exist);
        }
        var paged = new GetPagedPointsQuery
        {
            PageSize = pageSize,
            PageNumber = pageNumber
        };
        var points = await mediator.Send(paged);
        return Ok(points);
    }
    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchPoint(int id, [FromBody] UpdatePointCommand command)
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeletePoints([FromBody] DeletePointsCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
    [HttpGet("clusters")]
    public async Task<IActionResult> GetClusters()
    {
        var result = await mediator.Send(new GetClustersQuery());
        return Ok(result);
    }
}


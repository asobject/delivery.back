using BuildingBlocks.Models;
using MediatR;

namespace Application.Features.Points.Queries.GetCheckPointExistInRadius;

public record GetCheckPointExistInRadiusQuery(GeoPoint Coordinates) : IRequest<GetCheckPointExistInRadiusResponse>;

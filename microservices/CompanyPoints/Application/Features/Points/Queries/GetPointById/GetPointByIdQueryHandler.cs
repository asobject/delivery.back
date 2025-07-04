
using Application.Mappings;
using BuildingBlocks.Exceptions;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Points.Queries.GetPointById;

public class GetPointByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetPointByIdQuery, GetPointByIdResponse>
{
    public async Task<GetPointByIdResponse> Handle(GetPointByIdQuery request, CancellationToken cancellationToken)
    {
        if (request.Id <= 0)
            throw new ConflictException("id <= 0");
        var point = await unitOfWork.CompanyPoints.GetByIdAsync(request.Id) ?? throw new NotFoundException("point not found");
        return new GetPointByIdResponse
        {
            Data = point.ToDTO()
        };
    }
}

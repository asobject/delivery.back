

using BuildingBlocks.Exceptions;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Features.Points.Commands.UpdatePoint;

public class UpdatePointCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdatePointCommand, UpdatePointResponse>
{
    public async Task<UpdatePointResponse> Handle(UpdatePointCommand request, CancellationToken cancellationToken)
    {
        var point = await unitOfWork.CompanyPoints.GetByIdAsync(request.Id) ?? throw new NotFoundException($"point with id={request.Id} not found");


        var properties = request.GetType().GetProperties();
        bool allFieldsNull = properties.All(p => p.GetValue(request) == null);

        if (allFieldsNull)
        {
            throw new ConflictException("Все поля запроса равны null, обновление невозможно.");
        }

        foreach (var property in properties)
        {
            var newValue = property.GetValue(request);
            if (newValue != null)
            {
                var entityProperty = point.GetType().GetProperty(property.Name);
                if (entityProperty != null && entityProperty.CanWrite)
                {
                    entityProperty.SetValue(point, newValue);
                }
            }
        }
        var existingPointName = await unitOfWork.CompanyPoints.GetExistingPointName(point, point.Id);
        if (existingPointName != null)
        {
            throw new AlreadyExistsException($"Point {existingPointName} already exists");
        }

        await unitOfWork.CompanyPoints.UpdateAsync(point);
        await unitOfWork.CompleteAsync();
        return new UpdatePointResponse
        {
            Message = "point updated sucessfull"
        };
    }
}

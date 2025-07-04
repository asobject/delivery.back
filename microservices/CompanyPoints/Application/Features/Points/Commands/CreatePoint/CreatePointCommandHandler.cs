

using BuildingBlocks.Exceptions;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;
namespace Application.Features.Points.Commands.CreatePoint;

public class CreatePointCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreatePointCommand, CreatePointResponse>
{
    public async Task<CreatePointResponse> Handle(CreatePointCommand request, CancellationToken cancellationToken)
    {

        using var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            var countries = await unitOfWork.Countries.FindAsync(c => c.Name == request.Country);
            Country country;
            if (!countries.Any())
            {
                country = new Country { Name = request.Country };
                await unitOfWork.Countries.AddAsync(country);
                await unitOfWork.CompleteAsync();
            }
            else
            {
                country = countries.Single();
            }
            var provinces = await unitOfWork.Provinces.FindAsync(p => p.Name == request.Province && p.CountryId == country.Id);
            Province province;
            if (!provinces.Any())
            {
                province = new Province
                {
                    Name = request.Province,
                    CountryId = country.Id
                };
                await unitOfWork.Provinces.AddAsync(province);
                await unitOfWork.CompleteAsync();
            }
            else
            {
                province = provinces.Single();
            }
            var localities = await unitOfWork.Localities.FindAsync(l => l.Name == request.Locality && l.ProvinceId == province.Id);
            Locality locality;
            if (!localities.Any())
            {
                locality = new Locality
                {
                    Name = request.Locality,
                    ProvinceId = province.Id
                };
                await unitOfWork.Localities.AddAsync(locality);
                await unitOfWork.CompleteAsync();
            }
            else
            {
                locality = localities.Single();
            }
            var point = new CompanyPoint
            {
                Name = request.Name,
                PointType = request.PointType,
                Address = request.Address,
                Coordinates = request.Coordinates,
                PointStatus = request.PointStatus,
                LocalityId = locality.Id
            };

            var existingPointName = await unitOfWork.CompanyPoints.GetExistingPointName(point);
            if (existingPointName is not null)
            {
                throw new AlreadyExistsException($"Point {existingPointName} already exists");
            }

            await unitOfWork.CompanyPoints.AddAsync(point);
            await unitOfWork.CompleteAsync();
            await transaction.CommitAsync();

            return new CreatePointResponse
            {
                Message = "Point created successfully"
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

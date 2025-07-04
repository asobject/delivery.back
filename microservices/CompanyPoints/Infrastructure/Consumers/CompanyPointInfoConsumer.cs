

using BuildingBlocks.Messaging.Requests;
using BuildingBlocks.Messaging.Responses;
using Domain.Interfaces.Repositories;
using MassTransit;

namespace Infrastructure.Consumers;

public class CompanyPointInfoConsumer(ICompanyPointRepository repository) : IConsumer<GetCompanyPointInfosRequest>
{
    public async Task Consume(ConsumeContext<GetCompanyPointInfosRequest> context)
    {
        var request = context.Message;

        var points = await repository.GetByIdsAsync(request.CompanyPointIds);

        var pointsDict = points.ToDictionary(p => p.Id);

        var response = new GetCompanyPointInfosResponse
        {
            RequestId = request.RequestId,
            Points = [.. request.CompanyPointIds
                .Select(id => pointsDict.TryGetValue(id, out var point)
                    ? new CompanyPointInfoResponse
                    {
                        CompanyPointId = id,
                        Address = point.Address,
                        Coordinates = point.Coordinates
                    }
                    : new CompanyPointInfoResponse
                    {
                        CompanyPointId = id,
                        Address = null,
                        Coordinates = null
                    })]
        };

        await context.RespondAsync<GetCompanyPointInfosResponse>(response);
    }
}

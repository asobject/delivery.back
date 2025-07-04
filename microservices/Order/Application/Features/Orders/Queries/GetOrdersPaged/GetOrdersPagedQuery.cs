using MediatR;

namespace Application.Features.Orders.Queries.GetOrdersPaged;

public record GetOrdersPagedQuery(string UserId, int PageNumber, int PageSize) : IRequest<GetOrdersPagedResponse>;
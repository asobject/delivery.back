

using MediatR;

namespace Application.Features.Order.Commands;

public record SendOrderConfirmationCommand(string Email,string Tracker) : IRequest;


using Application.Interfaces;
using BuildingBlocks.Enums;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Messaging.Requests;
using BuildingBlocks.Messaging.Responses;
using BuildingBlocks.Models;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MassTransit;
using MediatR;
namespace Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler(IUnitOfWork unitOfWork, IPublishEndpoint publishEndpoint, IRequestClient<GetCompanyPointInfosRequest> client, ICalculationService calculationService) : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var (calculatedPrice, _, _) = await ValidatePrice(request);
        if (request.Price != calculatedPrice)
            throw new ConflictException("incorrect price");
        using var transaction = await unitOfWork.BeginTransactionAsync();
        try
        {
            var senderPoint = await GetOrCreateDeliveryPointAsync(
                request.SenderDeliveryMethod,
                request.SenderCompanyPointId,
                request.SenderAddress,
                request.SenderCoordinates
            );

            var receiverPoint = await GetOrCreateDeliveryPointAsync(
                request.ReceiverDeliveryMethod,
                request.ReceiverCompanyPointId,
                request.ReceiverAddress,
                request.ReceiverCoordinates
            );

            var order = new Order
            {
                Price = request.Price,
                PaymentMethod = request.PaymentType,
                SenderId = request.SenderId,
                PackageSize = request.PackageSize,
                SenderDeliveryPointId = senderPoint.Id,
                ReceiverDeliveryPointId = receiverPoint.Id,
                ReceiverEmail = request.ReceiverEmail
            };

            await unitOfWork.Orders.AddAsync(order);
            await unitOfWork.CompleteAsync();
            await transaction.CommitAsync();

            await publishEndpoint.Publish(new OrderCreatedEvent(
               OrderId: order.Id,
               ReceiverEmail: request.ReceiverEmail
           ), cancellationToken);

            await publishEndpoint.Publish(new EmailSendingEvent(
                To: order.ReceiverEmail,
                Subject: "Отправление создано",
               Body: $"Трек-номер: {order.Tracker}"
          ), cancellationToken);

            await publishEndpoint.Publish(new EmailSendingEvent(
               To: request.SenderEmail,
               Subject: "Отправление создано",
               Body: $"Трек-номер: {order.Tracker}"
         ), cancellationToken);

            return new CreateOrderResponse
            {
                Tracker = order.Tracker,
                Message = "Order created successfully"
            };
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
    private async Task<(double CalculatedPrice, CompanyPointInfoResponse? SenderCompanyPointInfo, CompanyPointInfoResponse? ReceiverCompanyPointInfo)> ValidatePrice(CreateOrderCommand command)
    {
        CompanyPointInfoResponse? senderCompanyPointInfo = null;
        CompanyPointInfoResponse? receiverCompanyPointInfo = null;
        if (command.SenderDeliveryMethod == DeliveryMethod.PickupPoint)
        {
            var response = await client.GetResponse<GetCompanyPointInfosResponse>(
                   new { RequestId = NewId.NextGuid(), CompanyPointIds = new[] { command.SenderCompanyPointId } });
            senderCompanyPointInfo = response.Message.Points.First();
            if (senderCompanyPointInfo.Coordinates is null)
                throw new NotFoundException($"companyPoint with id={senderCompanyPointInfo.CompanyPointId} not found");
        }
        if (command.ReceiverDeliveryMethod == DeliveryMethod.PickupPoint)
        {
            var response = await client.GetResponse<GetCompanyPointInfosResponse>(
                   new { RequestId = NewId.NextGuid(), CompanyPointIds = new[] { command.ReceiverCompanyPointId } });
            receiverCompanyPointInfo = response.Message.Points.First();
            if (receiverCompanyPointInfo.Coordinates is null)
                throw new NotFoundException($"companyPoint with id={receiverCompanyPointInfo.CompanyPointId} not found");
        }
        var calculatedPrice = await calculationService.CalculatePriceAsync(
            new Queries.CalculateOrder.CalculateOrderQuery
            {
                PackageSize = command.PackageSize,
                PaymentMethod = command.PaymentType,
                SenderDeliveryMethod = command.SenderDeliveryMethod,
                ReceiverDeliveryMethod = command.ReceiverDeliveryMethod,
                SenderPointCoordinates = command.SenderDeliveryMethod == DeliveryMethod.PickupPoint ? senderCompanyPointInfo!.Coordinates! : command.SenderCoordinates!,
                ReceiverPointCoordinates = command.ReceiverDeliveryMethod == DeliveryMethod.PickupPoint ? receiverCompanyPointInfo!.Coordinates! : command.ReceiverCoordinates!
            });
        return (calculatedPrice, senderCompanyPointInfo, receiverCompanyPointInfo);
    }
    private async Task<DeliveryPoint> GetOrCreateDeliveryPointAsync(
     DeliveryMethod method,
     int? companyPointId,
     string? address,
     GeoPoint? coordinates)
    {
        DeliveryPoint? existingPoint = null;

        if (method == DeliveryMethod.PickupPoint)
        {
            // Для пункта выдачи ищем по companyPointId
            existingPoint = (await unitOfWork.DeliveryPoints.FindAsync(p =>
                p.Method == method &&
                p.CompanyPointId == companyPointId))
                .FirstOrDefault();
            if (existingPoint != null)
                return existingPoint;
        }
        else if (method == DeliveryMethod.CourierCall)
        {
            if (string.IsNullOrEmpty(address))
                throw new ConflictException("address required for courier delivery");
            if (coordinates is null)
                throw new ConflictException("coordinates required for courier delivery");

            var customPoint = await GetOrCreateCustomPointAsync(address, coordinates);

            // Ищем точку доставки с привязкой к кастомному пункту
            existingPoint = (await unitOfWork.DeliveryPoints.FindAsync(p =>
                p.Method == method &&
                p.CustomPointId == customPoint.Id))
                .FirstOrDefault();
            if (existingPoint != null)
                return existingPoint;
        }
        var newPoint = new DeliveryPoint
        {
            Method = method,
            CompanyPointId = method == DeliveryMethod.PickupPoint ? companyPointId : null,
            CustomPointId = method == DeliveryMethod.CourierCall
                ? (await GetOrCreateCustomPointAsync(address, coordinates)).Id
                : null
        };

        await unitOfWork.DeliveryPoints.AddAsync(newPoint);
        await unitOfWork.CompleteAsync();
        return newPoint;
    }

    private async Task<CustomPoint> GetOrCreateCustomPointAsync(string? address, GeoPoint? coordinates)
    {
        var existing = (await unitOfWork.CustomPoints.FindAsync(c =>
            c.Address == address))
            .FirstOrDefault();

        if (existing is not null)
            return existing;

        var newPoint = new CustomPoint { Address = address, Coordinates = coordinates };
        await unitOfWork.CustomPoints.AddAsync(newPoint);
        await unitOfWork.CompleteAsync();
        return newPoint;
    }
}

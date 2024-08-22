using System.Threading.Tasks;
using System.Text;
using System.Linq;
using System.Text.Json;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Azure.Messaging.ServiceBus;
using System;
using Microsoft.eShopWeb.ApplicationCore.Services;
using Microsoft.Extensions.Logging;

namespace Microsoft.eShopWeb.Infrastructure.Services;
public class SbOrderItemReserver : IOrderItemsReserver
{
    const string ServiceBusConnectionString = "Endpoint=sb://order-sb-namespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=NI75+z7CvlyNCe5jre0F1cRuOOynMq90Z+ASbI47t4Q=";
    const string QueueName = "Orders";
    private IAppLogger<SbOrderItemReserver> logger;
    public SbOrderItemReserver(IAppLogger<SbOrderItemReserver> logger)
    {
        this.logger = logger;
    }

    public async Task Reserve(Order order)
    {
        var dto = GetOrderDto(order);
        var json = JsonSerializer.Serialize(dto);
        await using var client = new ServiceBusClient(ServiceBusConnectionString);
        await using ServiceBusSender sender = client.CreateSender(QueueName);
        try
        {
            var message = new ServiceBusMessage(json);
            logger.LogInformation($"Sending message to ServiceBus: {json}");
            await sender.SendMessageAsync(message);
        }
        catch (Exception exception)
        {
            logger.LogWarning($"Exception: {exception.Message}");
        }
        finally
        {
            await sender.DisposeAsync();
            await client.DisposeAsync();
        }
    }

    private OrderDto GetOrderDto(Order order)
    {
        return new OrderDto
        {
            id = order.Id.ToString(),
            ShipToAddress = order.ShipToAddress,
            OrderItems = order.OrderItems
                .Select(item => new OrderItemDto()
                {
                    CatalogItemId = item.ItemOrdered.CatalogItemId,
                    ProductName = item.ItemOrdered.ProductName,
                    UnitPrice = item.UnitPrice,
                    Units = item.Units
                }),
            FinalPrice = order.Total()
        };
    }
}

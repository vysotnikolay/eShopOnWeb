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


namespace Microsoft.eShopWeb.Infrastructure.Services;
public class SbOrderItemReserver : IOrderItemsReserver
{
    // TODO: ChangeConnection sring
    const string ServiceBusConnectionString = "Endpoint=sb://e-shop-on-web-sbus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=jmwe628C6XmVOzodxeGTPDOmjaXroLtTP+ASbKrYz+A=";
    const string QueueName = "Orders";
    private IAppLogger<SbOrderItemReserver> logger;
    public SbOrderItemReserver(IAppLogger<SbOrderItemReserver> logger)
    {
        this.logger = logger;
    }

    public async Task Reserve(Order order)
    {
        var dto = OrderMapper.MapOrderToOrderDto(order);
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
}

using System.Threading.Tasks;
using System.Text;
using System.Linq;
using System.Text.Json;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using Microsoft.eShopWeb.Infrastructure.Services;

namespace Microsoft.eShopWeb.Infrastructure;

public class DeliveryOrderProcessor : IDeliveryOrderProcessor
{
    // TODO: add function URL
    private string _url="https://e-shop-on-web-functions.azurewebsites.net/api/SaveOrderToCosmosDb?code=kSnBqqhq5cNvtyOdbIHjGu1cCTKgsIWBSV-YAZ7RykDsAzFuXOsRBw%3D%3D";
    private IAppLogger<SbOrderItemReserver> _logger;

    public DeliveryOrderProcessor(IAppLogger<SbOrderItemReserver> logger)
    {
        _logger = logger;
    }

    public async Task Deliver(Order order)
    {
        var dto = OrderMapper.MapOrderToOrderDto(order);
        var json = JsonSerializer.Serialize(dto);
        HttpResponseMessage? result = null;
        using(var client = new HttpClient())
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            result = await client.PostAsync(_url, content);
            _logger.LogInformation($"Delivery order creation status: {result.StatusCode}");
        }
    }
}

public class OrderDto
{
    public OrderDto() {}

    public string id { get; set; }
    public Address ShipToAddress { get; set; }
    public IEnumerable<OrderItemDto> OrderItems { get; set; }
    public decimal FinalPrice { get; set; }

}

public class OrderItemDto
{
	public int CatalogItemId { get; set; }
    public string ProductName { get; set; }
	public decimal UnitPrice { get; set; }
    public int Units { get; set; }
}

public class OrderMapper
{
    public static OrderDto MapOrderToOrderDto(Order order)
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

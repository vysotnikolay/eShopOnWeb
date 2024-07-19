using System.Threading.Tasks;
using System.Text;
using System.Linq;
using System.Text.Json;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

namespace Microsoft.eShopWeb.Infrastructure;

public class OrderItemsReserver : IOrderItemsReserver
{
    private string _url="https://e-shop-on-web-functions.azurewebsites.net/api/SaveOrderToCosmosDb?code=kSnBqqhq5cNvtyOdbIHjGu1cCTKgsIWBSV-YAZ7RykDsAzFuXOsRBw%3D%3D";

    public async Task Reserve(Order order)
    {
        var dto = GetOrderDto(order);
        var json = JsonSerializer.Serialize(dto);
        // string filenameQueryParameter = $"&filename={order.Id}";
        // HttpResponseMessage? result = null;
        // using(var client = new HttpClient())
        // {
        //     var content = new StringContent(json, Encoding.UTF8, "application/json");
        //     result = await client.PostAsync(_url + filenameQueryParameter, content);
        // }

        HttpResponseMessage? result = null;
        using(var client = new HttpClient())
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            result = await client.PostAsync(_url, content);
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

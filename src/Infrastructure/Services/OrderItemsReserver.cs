using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;

namespace Microsoft.eShopWeb.Infrastructure;

public class OrderItemsReserver : IOrderItemsReserver
{
    private string _url="https://e-shop-on-web-functions.azurewebsites.net/api/UploadToBlob?code=GO1_S9b7KGMowz46T8cKDHYZWu_id0s0Q3_wuHOtR2KQAzFuv17WAg%3D%3D";

    public async Task Reserve(Order order)
    {
        var json = JsonSerializer.Serialize(order);
        string filenameQueryParameter = $"&filename={order.Id}";
        HttpResponseMessage? result = null;
        using(var client = new HttpClient())
        {
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            result = await client.PostAsync(_url + filenameQueryParameter, content);
        }

    }
}

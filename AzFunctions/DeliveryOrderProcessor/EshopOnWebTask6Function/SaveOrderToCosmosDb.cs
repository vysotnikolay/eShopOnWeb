using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Extensions.CosmosDB;

using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using System.Net;

namespace EshopOnWebTask6Function
{
    public class SaveOrderToCosmosDb
    {
        private readonly ILogger<SaveOrderToCosmosDb> _logger;

        public SaveOrderToCosmosDb(ILogger<SaveOrderToCosmosDb> logger)
        {
            _logger = logger;
        }

        [Function("SaveOrderToCosmosDb")]
        public async Task<MultiResponse> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
            FunctionContext ctx
            )
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            Order order = JsonConvert.DeserializeObject<Order>(requestBody);
            var okResponse = new OkObjectResult(order);
            
            return new MultiResponse()
            {
                Order = order,
                ActionResult = okResponse
            };
        }
    }

    public class MultiResponse
    {
        [CosmosDBOutput("OrdersDb", "OrdersContainer", Connection = "CosmosDbConnectionSetting", CreateIfNotExists = false)]
        public Order Order { get; set; }
        
        public IActionResult ActionResult { get; set; }
    }

    public class Order
    {
        public string id { get; set; }
        public Address ShipToAddress { get; set; }
        public IReadOnlyCollection<OrderItemDto> OrderItems { get; set; }
        public decimal FinalPrice { get; set; }
    }

    public class OrderItemDto
    {
        public int CatalogItemId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Units { get; set; }
    }

    public class Address // ValueObject
    {
        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public string ZipCode { get; set; }

        private Address() { }
    }
}

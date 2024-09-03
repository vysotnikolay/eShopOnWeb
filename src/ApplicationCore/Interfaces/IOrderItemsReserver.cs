using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using System.Net.Http;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces;

public interface IOrderItemsReserver
{
    Task Reserve(Order order);
}

public interface IDeliveryOrderProcessor
{
    Task Deliver(Order order);
}

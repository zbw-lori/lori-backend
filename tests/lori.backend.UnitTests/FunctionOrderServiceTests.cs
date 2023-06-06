using Xunit;
using lori.backend.Infrastructure.Services;
using lori.backend.Core.Models;

namespace lori.backend.UnitTests;
public class CreateOrderPrio_IsSorted
{
  [Fact]
  public void IsSorted_Input3Orders_ReturnTrue()
  {
    var orderService = new OrderService();
    var orders = GetOrders();

    var result = orderService.SortOrders(orders);

    Assert.Equal(expectedOrders, result);
  }
  public static List<Order> GetOrders()
  {
    // Create a list of Orders (sample data)
    var orders = new List<Order>
    {
        new Order { Priority = Priority.High, EarliestDelivery = new DateTime(2023, 6, 6, 6, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 12, 0, 0) },
        new Order { Priority = Priority.Medium, EarliestDelivery = new DateTime(2023, 6, 6, 8, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 13, 0, 0) },
        new Order { Priority = Priority.High, EarliestDelivery = new DateTime(2023, 6, 6, 10, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 12, 0, 0) },
        new Order { Priority = Priority.Low, EarliestDelivery = new DateTime(2023, 6, 6, 9, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 16, 0, 0) },
        new Order { Priority = Priority.Medium, EarliestDelivery = new DateTime(2023, 6, 6, 12, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 15, 0, 0) }
    };

    return orders;
  }

  private List<Order> expectedOrders = new List<Order>
  {
    new Order { Priority = Priority.High, EarliestDelivery = new DateTime(2023, 6, 6, 6, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 12, 0, 0) },
    new Order { Priority = Priority.High, EarliestDelivery = new DateTime(2023, 6, 6, 10, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 12, 0, 0) },
    new Order { Priority = Priority.Medium, EarliestDelivery = new DateTime(2023, 6, 6, 8, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 13, 0, 0) },
    new Order { Priority = Priority.Medium, EarliestDelivery = new DateTime(2023, 6, 6, 12, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 15, 0, 0) },
    new Order { Priority = Priority.Low, EarliestDelivery = new DateTime(2023, 6, 6, 9, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 16, 0, 0) },
  };
}

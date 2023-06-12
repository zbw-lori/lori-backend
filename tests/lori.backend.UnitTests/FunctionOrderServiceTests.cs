using Xunit;
using lori.backend.Infrastructure.Services;
using lori.backend.Core.Models;

namespace lori.backend.UnitTests;

public class CreateOrderPrio_IsSorted
{
  [Fact]
  public void IsSorted_CompareOrder0_ReturnTrue()
  {
    var orderService = new OrderService();
    var orders = GetOrders();

    var result = orderService.SortOrders(orders);

    Assert.Equal(_expectedOrders[1].Id, result[1].Id);
  }

  [Fact]
  public void IsSorted_CompareOrder1_ReturnTrue()
  {
    var orderService = new OrderService();
    var orders = GetOrders();

    var result = orderService.SortOrders(orders);

    Assert.Equal(_expectedOrders[1].Id, result[1].Id);
  }

  [Fact]
  public void IsSorted_CompareOrder2_ReturnTrue()
  {
    var orderService = new OrderService();
    var orders = GetOrders();

    var result = orderService.SortOrders(orders);

    Assert.Equal(_expectedOrders[2].Id, result[2].Id);
  }

  [Fact]
  public void IsSorted_CompareOrder3_ReturnTrue()
  {
    var orderService = new OrderService();
    var orders = GetOrders();

    var result = orderService.SortOrders(orders);

    Assert.Equal(_expectedOrders[3].Id, result[3].Id);
  }

  [Fact]
  public void IsSorted_CompareOrder4_ReturnTrue()
  {
    var orderService = new OrderService();
    var orders = GetOrders();

    var result = orderService.SortOrders(orders);

    Assert.Equal(_expectedOrders[4].Id, result[4].Id);
  }

  [Fact]
  public void IsSorted_CompareOrder4With3_ReturnFalse()
  {
    var orderService = new OrderService();
    var orders = GetOrders();

    var result = orderService.SortOrders(orders);

    Assert.NotEqual(_expectedOrders[4].Id, result[3].Id);
  }

  public static List<Order> GetOrders()
  {
    // Create a list of Orders (sample data)
    var orders = new List<Order>
  {
      new Order { Id = 1, Status = "Pending", Priority = Priority.High, EarliestDelivery = new DateTime(2023, 6, 6, 6, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 12, 0, 0), ReceiptType = ReceiptType.Auto, CustomerId = 1 },
      new Order { Id = 2, Status = "Pending", Priority = Priority.Medium, EarliestDelivery = new DateTime(2023, 6, 6, 8, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 13, 0, 0), ReceiptType = ReceiptType.Manual, CustomerId = 2 },
      new Order { Id = 3, Status = "Pending", Priority = Priority.High, EarliestDelivery = new DateTime(2023, 6, 6, 10, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 12, 0, 0), ReceiptType = ReceiptType.Auto, CustomerId = 3 },
      new Order { Id = 4, Status = "Pending", Priority = Priority.Low, EarliestDelivery = new DateTime(2023, 6, 6, 9, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 16, 0, 0), ReceiptType = ReceiptType.Manual, CustomerId = 2 },
      new Order { Id = 5, Status = "Pending", Priority = Priority.Medium, EarliestDelivery = new DateTime(2023, 6, 6, 12, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 15, 0, 0), ReceiptType = ReceiptType.Auto, CustomerId = 4 },
  };

    return orders;
  }

  private readonly List<Order> _expectedOrders = new()
  {
  new Order { Id = 1, Status = "Pending", Priority = Priority.High, EarliestDelivery = new DateTime(2023, 6, 6, 6, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 12, 0, 0), ReceiptType = ReceiptType.Auto, CustomerId = 1 },
  new Order { Id = 3, Status = "Pending", Priority = Priority.High, EarliestDelivery = new DateTime(2023, 6, 6, 10, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 12, 0, 0), ReceiptType = ReceiptType.Auto, CustomerId = 3 },
  new Order { Id = 2, Status = "Pending", Priority = Priority.Medium, EarliestDelivery = new DateTime(2023, 6, 6, 8, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 13, 0, 0), ReceiptType = ReceiptType.Manual, CustomerId = 2 },
  new Order { Id = 5, Status = "Pending", Priority = Priority.Medium, EarliestDelivery = new DateTime(2023, 6, 6, 12, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 15, 0, 0), ReceiptType = ReceiptType.Auto, CustomerId = 4 },
  new Order { Id = 4, Status = "Pending", Priority = Priority.Low, EarliestDelivery = new DateTime(2023, 6, 6, 9, 0, 0), LatestDelivery = new DateTime(2023, 6, 6, 16, 0, 0), ReceiptType = ReceiptType.Manual, CustomerId = 2 }
};
}

namespace lori.backend.Infrastructure.Services;

using lori.backend.Core.Models;
public class OrderService
{
  public List<Order> CreateOrderPrio(List<Order> orders)
  {
    // Get all orders sorted by Prio
    var sortedOrders = SortOrders(orders);
    
    // Set delivery time of each order as early as possible while ensuring uniqueness and not earlier than the earliest delivery time or later than the latest delivery time
    DateTime currentDeliveryTime = orders[0].EarliestDelivery;
    foreach (var order in sortedOrders)
    {
      if (!order.SetDelivery.HasValue)
      {
        DateTime deliveryTime = currentDeliveryTime;
        if (deliveryTime < order.EarliestDelivery)
        {
          deliveryTime = order.EarliestDelivery;
        }
        else if (deliveryTime > order.LatestDelivery)
        {
          deliveryTime = order.LatestDelivery;
        }

        order.SetDelivery = deliveryTime;
        currentDeliveryTime = GetNextUniqueDeliveryTime(deliveryTime, orders);
      }

      // Console.WriteLine($"Order: {order.Name}, Priority: {order.Priority}, Earliest Delivery Time: {order.EarliestDelivery}, Latest Delivery Time: {order.LatestDelivery}, Set Delivery Time: {order.SetDelivery}");
    }

    return sortedOrders;
  }

  public static DateTime GetNextUniqueDeliveryTime(DateTime currentDeliveryTime, List<Order> orders)
  {
    DateTime nextDeliveryTime = currentDeliveryTime;

    while (orders.Any(order => order.SetDelivery.HasValue && order.SetDelivery.Value == nextDeliveryTime))
    {
      nextDeliveryTime = nextDeliveryTime.AddMinutes(15);
    }

    return nextDeliveryTime;
  }

  public List<Order> SortOrders(List<Order> orders)
  {
 
    // Sort orders by priority
    orders.Sort((order1, order2) => order2.Priority.CompareTo(order1.Priority));

    return orders;
  }
}

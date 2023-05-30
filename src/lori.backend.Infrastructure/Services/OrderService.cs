namespace lori.backend.Core.Services;

using lori.backend.Core.Models;
using lori.backend.Infrastructure.Data;
public class OrderService
{
  private readonly LoriDbContext _context;

  public OrderService(LoriDbContext context)
  {
    _context = context;
  }

  public List<Order> GetSortedOrders()
  {
    var sortedOrders = _context.Orders
        .OrderBy(o => o.Priority)
        .ThenBy(o => o.EarliestDelivery)
        .ToList();

    return sortedOrders;
  }
}

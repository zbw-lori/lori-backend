using Microsoft.EntityFrameworkCore;

namespace lori.backend.Core.Models;

[PrimaryKey(nameof(OrderId), nameof(ItemId))]
public class OrderItem
{
  public int OrderId { get; set; }
  public int ItemId { get; set; }
  public int Quantity { get; set; }
}

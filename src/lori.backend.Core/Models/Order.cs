namespace lori.backend.Core.Models;
public class Order
{
  public int Id { get; set; }
  public string Status { get; set; } = null!;
  public Priority Priority { get; set; }
  public DateTime EarliestDelivery { get; set; }
  public DateTime LatestDelivery { get; set; }
  public ReceiptType ReceiptType { get; set; }
  public int CustomerId { get; set; }
  public Customer Customer { get; set; } = null!;
}

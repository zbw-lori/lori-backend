namespace lori.backend.Core.Models;
public class Order
{
  public int Id { get; set; }
  public string Status { get; set; } = null!;
  public int Priority { get; set; }
  public DateTime Created { get; set; }
  public string ReceiptType { get; set;} = null!;
  public int CustomerId { get; set; }
  public Customer Customer { get; set; } = null!;
}

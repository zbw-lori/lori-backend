namespace lori.backend.Web.ApiModels;

public class OrderDTO
{
  public int Id { get; set; }
  public string Status { get; set; } = null!;

  public int Priority { get; set; }

  public DateTime Created { get; set; }

  public string ReceiptType { get; set; } = null!;
  public int CustomerId { get; set; }
}

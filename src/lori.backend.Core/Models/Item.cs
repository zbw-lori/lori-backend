namespace lori.backend.Core.Models;
public class Item
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public decimal Price { get; set; }
  public int StockQuantity { get; set; }
  public int StoreId { get; set; }
  public Store Store { get; set; } = null!;
}

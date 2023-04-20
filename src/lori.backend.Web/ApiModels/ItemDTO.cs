namespace lori.backend.Web.ApiModels;

public class ItemDTO
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public decimal Price { get; set; }
  public int StoreId { get; set; }
}

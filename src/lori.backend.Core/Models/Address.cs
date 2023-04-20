namespace lori.backend.Core.Models;

public class Address
{
  public int Id { get; set; }
  public string Street { get; set; } = null!;
  public int StreetNumber { get; set; }
  public string City { get; set; } = null!;
  public int CityCode { get; set; }
}


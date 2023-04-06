using System;
namespace lori.backend.Infrastructure.Models;

public class Address
{
  public int Id { get; set; }
  public string Street { get; set; } = null!;
  public int StreetNumber { get; set; }
  public string City { get; set; } = null!;
  public int CityCode { get; set; }
}


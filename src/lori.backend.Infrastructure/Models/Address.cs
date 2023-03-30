using System;
namespace lori.backend.Infrastructure.Models
{
  public class Address
  {
    public int Id { get; set; }
    public string Street { get; set; }
    public int StreetNumber { get; set; }
    public string City { get; set; }
    public int CityCode { get; set; }
  }
}


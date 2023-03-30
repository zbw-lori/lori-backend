using System;
namespace lori.backend.Infrastructure.Models
{
  public class Customer
  {
    public int Id { get; set; }
    public string Forename { get; set; }
    public string Surename { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public int AddressId { get; set; }
  }
}


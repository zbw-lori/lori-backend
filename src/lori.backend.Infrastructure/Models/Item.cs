using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lori.backend.Infrastructure.Models;
public class Item
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public decimal Price { get; set; }
  public int StoreId { get; set; }
  public Store Store { get; set; } = null!;
}

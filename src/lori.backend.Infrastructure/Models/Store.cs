using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lori.backend.Infrastructure.Models;
public class Store
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public int Quantity { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lori.backend.Infrastructure.Models;
public class Order
{
  public int Id { get; set; }
  public string Status { get; set; } = null!;
  public int Priority { get; set; }
  public DateTime dateTime { get; set; }
  public string ReceiptType { get; set;} = null!;
  public int CustomerId { get; set; }
  public Customer Customer { get; set; } = null!;
}

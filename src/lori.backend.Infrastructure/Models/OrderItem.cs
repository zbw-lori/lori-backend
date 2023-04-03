using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lori.backend.Infrastructure.Models;
public class OrderItem
{
  public int Id { get; set; }
  public int OrderId { get; set; }
  public int ItemId { get; set; }
  public int Quantity { get; set; }
}

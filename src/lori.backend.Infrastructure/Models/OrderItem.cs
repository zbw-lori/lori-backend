using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace lori.backend.Infrastructure.Models;

[PrimaryKey(nameof(OrderId), nameof(ItemId))]
public class OrderItem
{
  public int OrderId { get; set; }
  public int ItemId { get; set; }
  public int Quantity { get; set; }
}

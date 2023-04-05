using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace lori.backend.Infrastructure.Models;
public class Route
{
  public int Id { get; set; }
  [Column(TypeName = "json")]
  public string PriorityList { get; set; } = null!;

  public int RobotId { get; set; }
  public Robot Robot { get; set; } = null!;

  public int OrderId { get; set; }
  public Order Order { get; set; } = null!;
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lori.backend.Infrastructure.Models;
public class Login
{
  [Key]
  public string Username { get; set; } = null!;
  [Key]
  public Byte[] Password { get; set; } = null!;
}

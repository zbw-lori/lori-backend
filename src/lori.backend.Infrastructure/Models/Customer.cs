﻿using System;
namespace lori.backend.Infrastructure.Models;

public class Customer
{
  public int Id { get; set; }
  public string Forename { get; set; } = null!;
  public string Surename { get; set; } = null!;
  public string Email { get; set; } = null!;
  public string Phone { get; set; } = null!;
  public int AddressId { get; set; }
/*  public int LoginUsername { get; set; }
  public Byte[] LoginPassword { get; set; }*/
}

﻿namespace lori.backend.Web.ApiModels;

public class OrderDTO
{
  public int Id { get; set; }
  public string Status { get; set; } = null!;

  public string Priority { get; set; } = null!;

  public DateTime EarliestDelivery { get; set; }
  public DateTime LatestDelivery { get; set; }

  public string ReceiptType { get; set; } = null!;
  public int CustomerId { get; set; }
}

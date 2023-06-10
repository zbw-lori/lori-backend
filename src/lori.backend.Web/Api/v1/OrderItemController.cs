using lori.backend.Infrastructure.Data;
using lori.backend.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace lori.backend.Web.Api.v1;

public class OrderItemController : BaseApiController
{
  private readonly LoriDbContext _context;

  public OrderItemController(LoriDbContext context)
  {
    _context = context;
  }

  // GET: /orderItem
  [HttpGet]
  [SwaggerOperation(
    Summary = "Gets a list of all OrderItems",
    OperationId = "OrderItem.GetAllOrderItems")
  ]
  public async Task<ActionResult<IEnumerable<OrderItem>>> GetAllOrderItems()
  {
    return await _context.OrderItems.ToListAsync();
  }

  // GET: /orderItem/1
  [HttpGet("{id}")]
  [SwaggerOperation(
    Summary = "Gets a list of all Items in specified Order",
    OperationId = "OrderItem.GetOrderItems")
  ]
  public async Task<ActionResult<IEnumerable<OrderItem>>> GetOrderItems(int id)
  {
    var order = await _context.Orders.FindAsync(id);
    if (order == null)
    {
      return NotFound("Invalid OrderId!");
    }

    return await _context.OrderItems.Where(orderItem => orderItem.OrderId == id).ToListAsync();
  }

  // PUT: /orderItem/1
  [HttpPut]
  [SwaggerOperation(
    Summary = "Update OrderItem",
    OperationId = "OrderItem.PutOrderItem")
  ]
  public async Task<IActionResult> PutOrderItem(OrderItem orderItem)
  {
    var item = await _context.OrderItems.FindAsync(orderItem.OrderId, orderItem.ItemId);
    if (item == null)
    {
      return NotFound();
    }
    item.Quantity = orderItem.Quantity;

    await _context.SaveChangesAsync();
    return NoContent();
  }

  // POST: /orderItem/1
  [HttpPost("{id}")]
  [SwaggerOperation(
    Summary = "Add Item to specified Order",
    OperationId = "OrderItem.AddOrderItem")
  ]
  public async Task<ActionResult<OrderItem>> AddOrderItem(int id, OrderItem orderItem)
  {
    var order = await _context.Orders.FindAsync(id);
    if (order == null)
    {
      return NotFound("Invalid OrderId!");
    }
    orderItem.OrderId = order.Id;

    var item = await _context.Items.FindAsync(orderItem.ItemId);
    if (item == null)
    {
      return NotFound("Invalid ItemId!");
    }

    _context.OrderItems.Add(orderItem);
    await _context.SaveChangesAsync();
    return CreatedAtAction(
           nameof(GetOrderItems),
                new { id = orderItem.OrderId },
                     orderItem);
  }

  // DELETE: /order
  [HttpDelete]
  [SwaggerOperation(
    Summary = "Delete OrderItem",
    OperationId = "OrderItem.DeleteOrderItem")
  ]
  public async Task<IActionResult> DeleteOrder(OrderItem orderItem)
  {
    var item = await _context.OrderItems.FindAsync(orderItem.OrderId, orderItem.ItemId);
    if (item == null)
    {
      return NotFound("Invalid OrderId and ItemId!");
    }
    _context.OrderItems.Remove(item);
    await _context.SaveChangesAsync();
    return NoContent();
  }
}

using lori.backend.Infrastructure.Data;
using lori.backend.Core.Models;
using lori.backend.Web.ApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace lori.backend.Web.Api.v1;

public class OrderController : BaseApiController
{
  private readonly LoriDbContext _context;

  public OrderController(LoriDbContext context)
  {
    _context = context;
  }

  // GET: /order
  [HttpGet]
  [SwaggerOperation(
    Summary = "Gets a list of all Orders",
    OperationId = "Order.GetOrders")
  ]
  public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
  {
    await _context.Customers.LoadAsync();
    _context.Orders.Include(o => o.Customer);
    return await _context.Orders.Select(x => OrderToDTO(x)).ToListAsync();
  }

  // GET: /order/1
  [HttpGet("{id}")]
  [SwaggerOperation(
    Summary = "Gets details of the specified Order",
    OperationId = "Order.GetOrder")
  ]
  public async Task<ActionResult<OrderDTO>> GetOrder(int id)
  {
    var Order = await _context.Orders.FindAsync(id);

    if (Order == null)
    {
      return NotFound();
    }

    return OrderToDTO(Order);
  }

  // PUT: /order/1
  [HttpPut("{id}")]
  [SwaggerOperation(
    Summary = "Update a specified Order",
    OperationId = "Order.PutOrder")
  ]
  public async Task<IActionResult> PutOrder(int id, OrderDTO OrderDTO)
  {
    if (id != OrderDTO.Id)
    {
      return BadRequest();
    }
    var Order = await _context.Orders.FindAsync(id);
    if (Order == null)
    {
      return NotFound();
    }
    Order.Status = OrderDTO.Status;
    Order.Priority = OrderDTO.Priority;
    Order.Created = OrderDTO.Created;
    Order.ReceiptType = OrderDTO.ReceiptType;
    Order.CustomerId = OrderDTO.CustomerId;
    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException) when (!OrderExists(id))
    {
      return NotFound();
    }
    return NoContent();
  }

  // POST: /order
  [HttpPost]
  [SwaggerOperation(
    Summary = "Create a new Order",
    OperationId = "Order.PostOrder")
  ]
  public async Task<ActionResult<OrderDTO>> PostOrder(OrderDTO OrderDTO)
  {
    var Order = new Order
    {
      Status = OrderDTO.Status,
      Priority = OrderDTO.Priority,
      Created = OrderDTO.Created,
      ReceiptType = OrderDTO.ReceiptType,
      CustomerId = OrderDTO.CustomerId
  };

    var customer = await _context.Customers.FindAsync(OrderDTO.CustomerId);
    if (customer == null)
    {
      return NotFound();
    }
    Order.CustomerId = customer.Id;

    _context.Orders.Add(Order);
    await _context.SaveChangesAsync();
    return CreatedAtAction(
           nameof(GetOrder),
                new { id = Order.Id },
                     OrderToDTO(Order));
  }

  // DELETE: /order/1
  [HttpDelete("{id}")]
  [SwaggerOperation(
    Summary = "Delete specified Order",
    OperationId = "Order.DeleteOrder")
  ]
  public async Task<IActionResult> DeleteOrder(int id)
  {
    var Order = await _context.Orders.FindAsync(id);
    if (Order == null)
    {
      return NotFound();
    }
    _context.Orders.Remove(Order);
    await _context.SaveChangesAsync();
    return NoContent();
  }

  private bool OrderExists(int id)
  {
    return _context.Orders.Any(e => e.Id == id);
  }

  private static OrderDTO OrderToDTO(Order Order) =>
    new OrderDTO
    {
      Id = Order.Id,
      Status = Order.Status,
      Priority = Order.Priority,
      Created = Order.Created,
      ReceiptType = Order.ReceiptType,
      CustomerId = Order.CustomerId
    };
}

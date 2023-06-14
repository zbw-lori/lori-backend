using lori.backend.Infrastructure.Data;
using lori.backend.Core.Models;
using lori.backend.Web.ApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using lori.backend.Infrastructure.Services;

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
    Priority priority;
    if (!Enum.TryParse<Priority>(OrderDTO.Priority, true, out priority))
    {
      var prios = String.Join(", ", Enum.GetNames<Priority>());
      return BadRequest($"Invalid priority! Use one of: {prios}");
    }
    Order.Priority = priority;
    Order.EarliestDelivery = OrderDTO.EarliestDelivery;
    Order.LatestDelivery = OrderDTO.LatestDelivery;
    ReceiptType receiptType;
    if (!Enum.TryParse<ReceiptType>(OrderDTO.ReceiptType, true, out receiptType))
    {
      var types = String.Join(", ", Enum.GetNames<ReceiptType>());
      return BadRequest($"Invalid receipt type! Use one of: {types}");
    }
    Order.ReceiptType = receiptType;
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
    ReceiptType receiptType;
    if (!Enum.TryParse<ReceiptType>(OrderDTO.ReceiptType, true, out receiptType))
    {
      var types = String.Join(", ", Enum.GetNames<ReceiptType>());
      return BadRequest($"Invalid receipt type! Use one of: {types}");
    }

    Priority priority;
    if (!Enum.TryParse<Priority>(OrderDTO.Priority, true, out priority))
    {
      var prios = String.Join(", ", Enum.GetNames<Priority>());
      return BadRequest($"Invalid priority! Use one of: {prios}");
    }

    var Order = new Order
    {
      Status = OrderDTO.Status,
      Priority = priority,
      EarliestDelivery = OrderDTO.EarliestDelivery,
      LatestDelivery = OrderDTO.LatestDelivery,
      ReceiptType = receiptType,
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

  // POST: /order/prioritize
  [HttpPost]
  [SwaggerOperation(
    Summary = "Send orders to prioritize",
    OperationId = "Order.PostOrderToPrio")
  ]
  public ActionResult<OrderDTO> PostOrderToPrio(List<Order> OrderDTOList)
  {
    var orderService = new OrderService();

    var prioretizedOrders =  orderService.CreateOrderPrio(OrderDTOList);

    return CreatedAtAction(
           nameof(PostOrderToPrio),
                prioretizedOrders);
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
      Priority = Order.Priority.ToString(),
      EarliestDelivery = Order.EarliestDelivery,
      LatestDelivery = Order.LatestDelivery,
      ReceiptType = Order.ReceiptType.ToString(),
      CustomerId = Order.CustomerId
    };
}

using lori.backend.Infrastructure.Data;
using lori.backend.Core.Models;
using lori.backend.Web.ApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace lori.backend.Web.Api.v1;

public class RouteController : BaseApiController
{
  private readonly LoriDbContext _context;

  public RouteController(LoriDbContext context)
  {
    _context = context;
  }

  // GET: /route
  [HttpGet]
  [SwaggerOperation(
    Summary = "Gets a list of all Routes",
    OperationId = "Route.GetRoutes")
  ]
  public async Task<ActionResult<IEnumerable<RouteDTO>>> GetRoutes()
  {
    await _context.Robots.LoadAsync();
    await _context.Orders.LoadAsync();
    _context.Routes.Include(r => r.Robot);
    _context.Routes.Include(r => r.Order);
    return await _context.Routes.Select(x => RouteToDTO(x)).ToListAsync();
  }

  // GET: /route/1
  [HttpGet("{id}")]
  [SwaggerOperation(
    Summary = "Gets details of the specified Route",
    OperationId = "Route.GetRoute")
  ]
  public async Task<ActionResult<RouteDTO>> GetRoute(int id)
  {
    var Route = await _context.Routes.FindAsync(id);

    if (Route == null)
    {
      return NotFound();
    }

    return RouteToDTO(Route);
  }

  // PUT: /route/1
  [HttpPut("{id}")]
  [SwaggerOperation(
    Summary = "Update a specified Route",
    OperationId = "Route.PutRoute")
  ]
  public async Task<IActionResult> PutRoute(int id, RouteDTO RouteDTO)
  {
    if (id != RouteDTO.Id)
    {
      return BadRequest();
    }
    var Route = await _context.Routes.FindAsync(id);
    if (Route == null)
    {
      return NotFound();
    }
    Route.RobotId = RouteDTO.RobotId;
    Route.OrderId = RouteDTO.OrderId;
    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException) when (!RouteExists(id))
    {
      return NotFound();
    }
    return NoContent();
  }

  // POST: /route
  [HttpPost]
  [SwaggerOperation(
    Summary = "Create a new Route",
    OperationId = "Route.PostRoute")
  ]
  public async Task<ActionResult<RouteDTO>> PostRoute(RouteDTO RouteDTO)
  {
    var Route = new Core.Models.Route
    {
      RobotId = RouteDTO.RobotId,
      OrderId = RouteDTO.OrderId,
    };

    var robot = await _context.Robots.FindAsync(RouteDTO.RobotId);
    var order = await _context.Orders.FindAsync(RouteDTO.OrderId);

    if (order == null || robot == null)
    {
      return NotFound();
    }
    Route.RobotId = robot.Id;
    Route.OrderId = order.Id;

    _context.Routes.Add(Route);
    await _context.SaveChangesAsync();
    return CreatedAtAction(
           nameof(GetRoute),
                new { id = Route.Id },
                     RouteToDTO(Route));
  }

  // DELETE: /route/1
  [HttpDelete("{id}")]
  [SwaggerOperation(
    Summary = "Delete specified Route",
    OperationId = "Route.DeleteRoute")
  ]
  public async Task<IActionResult> DeleteRoute(int id)
  {
    var Route = await _context.Routes.FindAsync(id);
    if (Route == null)
    {
      return NotFound();
    }
    _context.Routes.Remove(Route);
    await _context.SaveChangesAsync();
    return NoContent();
  }

  private bool RouteExists(int id)
  {
    return _context.Routes.Any(e => e.Id == id);
  }

  private static RouteDTO RouteToDTO(Core.Models.Route Route) =>
    new RouteDTO
    {
      Id = Route.Id,
      RobotId = Route.RobotId,
      OrderId = Route.OrderId
    };
}

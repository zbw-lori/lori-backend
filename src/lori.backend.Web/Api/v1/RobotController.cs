using lori.backend.Infrastructure.Data;
using lori.backend.Infrastructure.Models;
using lori.backend.Web.ApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lori.backend.Web.Api.v1;
[Route ("api/v1/[controller]")]
[ApiController]
public class RobotController : ControllerBase
{
  private readonly LoriDbContext _context;

  public RobotController(LoriDbContext context)
  {
    _context = context;
  }

  // GET: api/v1/robot
  [HttpGet]
  public async Task<ActionResult<IEnumerable<RobotDTO>>> GetRobots()
  {
    return await _context.Robots
      .Select(x => RobotToDTO(x))
      .ToListAsync();
  }

  // GET: api/v1/robot/1
  // <snippet_GetByID>
  [HttpGet("{id}")]
  public async Task<ActionResult<RobotDTO>> GetRobot(int id)
  {
    var robot = await _context.Robots.FindAsync(id);

    if (robot == null)
    {
      return NotFound();
    }

    return RobotToDTO(robot);
  }

  // PUT: api/v1/robot/1
  // <snippet_Update>

  private static RobotDTO RobotToDTO(Robot robot) =>
    new RobotDTO
    {
      Id = robot.Id,
      Name = robot.Name,
      Description = robot.Description,
      Model = robot.Model
    };
}

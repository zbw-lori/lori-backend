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
  [HttpPut("{id}")]
  public async Task<IActionResult> PutRobot(int id, RobotDTO robotDTO)
  {
    if (id != robotDTO.Id)
    {
      return BadRequest();
    }
    var robot = await _context.Robots.FindAsync(id);
    if (robot == null)
    {
      return NotFound();
    }
    robot.Name = robotDTO.Name;
    robot.Description = robotDTO.Description;
    robot.Model = robotDTO.Model;
    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException) when (!RobotExists(id))
    {
      return NotFound();
    }
    return NoContent();
  }

  // POST: api/v1/robot
  [HttpPost]
  public async Task<ActionResult<RobotDTO>> PostRobot(RobotDTO robotDTO)
  {
    var robot = new Robot
    {
      Name = robotDTO.Name,
      Description = robotDTO.Description,
      Model = robotDTO.Model
    };
    _context.Robots.Add(robot);
    await _context.SaveChangesAsync();
    return CreatedAtAction(
           nameof(GetRobot),
                new { id = robot.Id },
                     RobotToDTO(robot));
  }

  // DELETE: api/v1/robot/1
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteRobot(int id)
  {
    var robot = await _context.Robots.FindAsync(id);
    if (robot == null)
    {
      return NotFound();
    }
    _context.Robots.Remove(robot);
    await _context.SaveChangesAsync();
    return NoContent();
  }

  private bool RobotExists(int id)
  {
    return _context.Robots.Any(e => e.Id == id);
  }

  private static RobotDTO RobotToDTO(Robot robot) =>
    new RobotDTO
    {
      Id = robot.Id,
      Name = robot.Name,
      Description = robot.Description,
      Model = robot.Model
    };
}

using lori.backend.Core.Models;
using lori.backend.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace lori.backend.Web.Api.v1;

public class RoleController : BaseApiController
{
  private readonly LoriDbContext _context;

  public RoleController(LoriDbContext context)
  {
    _context = context;
  }

  // GET: /role
  [HttpGet]
  [SwaggerOperation(
    Summary = "Gets a list of all roles",
    OperationId = "Role.GetRoles")
  ]
  public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
  {
    return await _context.Roles.ToListAsync();
  }

  // GET: /role/1
  [HttpGet("{id}")]
  [SwaggerOperation(
    Summary = "Gets details of the specified role",
    OperationId = "Role.GetRole")
  ]
  public async Task<ActionResult<Role>> GetRole(int id)
  {
    var role = await _context.Roles.FindAsync(id);

    if (role == null)
    {
      return NotFound();
    }

    return role;
  }

  // PUT: /role/1
  [HttpPut("{id}")]
  [SwaggerOperation(
    Summary = "Update a specified role",
    OperationId = "Role.PutRole")
  ]
  public async Task<IActionResult> PutRole(int id, Role newRole)
  {
    var role = await _context.Roles.FindAsync(id);
    if (role == null)
    {
      return NotFound();
    }
    role.Name = newRole.Name;
    role.Description = newRole.Description;

    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException) when (!RoleExists(id))
    {
      return NotFound();
    }
    return NoContent();
  }

  // POST: /role
  [HttpPost]
  [SwaggerOperation(
    Summary = "Create a new role",
    OperationId = "Role.PostRole")
  ]
  public async Task<ActionResult<Role>> PostRole(Role newRole)
  {
    var role = new Role
    {
      Name = newRole.Name,
      Description = newRole.Description,
    };
    _context.Roles.Add(role);
    await _context.SaveChangesAsync();
    return CreatedAtAction(
           nameof(GetRole),
                new
                {
                  id = role.Id
                },
                     role);
  }

  // DELETE: /role/1
  [HttpDelete("{id}")]
  [SwaggerOperation(
    Summary = "Delete specified role",
    OperationId = "Role.DeleteRole")
  ]
  public async Task<IActionResult> DeleteRole(int id)
  {
    var role = await _context.Roles.FindAsync(id);
    if (role == null)
    {
      return NotFound();
    }
    _context.Roles.Remove(role);
    await _context.SaveChangesAsync();
    return NoContent();
  }

  private bool RoleExists(int id)
  {
    return _context.Roles.Any(e => e.Id == id);
  }
}

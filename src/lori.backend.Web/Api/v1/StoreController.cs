using lori.backend.Infrastructure.Data;
using lori.backend.Core.Models;
using lori.backend.Web.ApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace lori.backend.Web.Api.v1;

public class StoreController : BaseApiController
{
  private readonly LoriDbContext _context;

  public StoreController(LoriDbContext context)
  {
    _context = context;
  }

  // GET: /store
  [HttpGet]
  [SwaggerOperation(
    Summary = "Gets a list of all Stores",
    OperationId = "Store.GetStores")
  ]
  public async Task<ActionResult<IEnumerable<StoreDTO>>> GetStores()
  {
    return await _context.Stores
      .Select(x => StoreToDTO(x))
      .ToListAsync();
  }

  // GET: /store/1
  [HttpGet("{id}")]
  [SwaggerOperation(
    Summary = "Gets details of the specified Store",
    OperationId = "Store.GetStore")
  ]
  public async Task<ActionResult<StoreDTO>> GetStore(int id)
  {
    var Store = await _context.Stores.FindAsync(id);

    if (Store == null)
    {
      return NotFound();
    }

    return StoreToDTO(Store);
  }

  // PUT: /store/1
  [HttpPut("{id}")]
  [SwaggerOperation(
    Summary = "Update a specified Store",
    OperationId = "Store.PutStore")
  ]
  public async Task<IActionResult> PutStore(int id, StoreDTO StoreDTO)
  {
    if (id != StoreDTO.Id)
    {
      return BadRequest();
    }
    var Store = await _context.Stores.FindAsync(id);
    if (Store == null)
    {
      return NotFound();
    }
    Store.Name = StoreDTO.Name;
    Store.Quantity = StoreDTO.Quantity;
    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException) when (!StoreExists(id))
    {
      return NotFound();
    }
    return NoContent();
  }

  // POST: /store
  [HttpPost]
  [SwaggerOperation(
    Summary = "Create a new Store",
    OperationId = "Store.PostStore")
  ]
  public async Task<ActionResult<StoreDTO>> PostStore(StoreDTO StoreDTO)
  {
    var Store = new Store
    {
      Name = StoreDTO.Name,
      Quantity = StoreDTO.Quantity,
    };
    _context.Stores.Add(Store);
    await _context.SaveChangesAsync();
    return CreatedAtAction(
           nameof(GetStore),
                new { id = Store.Id },
                     StoreToDTO(Store));
  }

  // DELETE: /store/1
  [HttpDelete("{id}")]
  [SwaggerOperation(
    Summary = "Delete specified Store",
    OperationId = "Store.DeleteStore")
  ]
  public async Task<IActionResult> DeleteStore(int id)
  {
    var Store = await _context.Stores.FindAsync(id);
    if (Store == null)
    {
      return NotFound();
    }
    _context.Stores.Remove(Store);
    await _context.SaveChangesAsync();
    return NoContent();
  }

  private bool StoreExists(int id)
  {
    return _context.Stores.Any(e => e.Id == id);
  }

  private static StoreDTO StoreToDTO(Store Store) =>
    new StoreDTO
    {
      Id = Store.Id,
      Name = Store.Name,
      Quantity = Store.Quantity
    };
}

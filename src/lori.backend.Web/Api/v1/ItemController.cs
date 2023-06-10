using lori.backend.Infrastructure.Data;
using lori.backend.Core.Models;
using lori.backend.Web.ApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace lori.backend.Web.Api.v1;

public class ItemController : BaseApiController
{
  private readonly LoriDbContext _context;

  public ItemController(LoriDbContext context)
  {
    _context = context;
  }

  // GET: /item
  [HttpGet]
  [SwaggerOperation(
    Summary = "Gets a list of all Items",
    OperationId = "Item.GetItems")
  ]
  public async Task<ActionResult<IEnumerable<ItemDTO>>> GetItems()
  {
    await _context.Stores.LoadAsync();
    _context.Items.Include(i => i.Store);
    return await _context.Items.Select(x => ItemToDto(x)).ToListAsync();
  }

  // GET: /item/1
  [HttpGet("{id}")]
  [SwaggerOperation(
    Summary = "Gets details of the specified Item",
    OperationId = "Item.GetItem")
  ]
  public async Task<ActionResult<ItemDTO>> GetItem(int id)
  {
    var item = await _context.Items.FindAsync(id);

    if (item == null)
    {
      return NotFound();
    }

    return ItemToDto(item);
  }

  // PUT: /item/1
  [HttpPut("{id}")]
  [SwaggerOperation(
    Summary = "Update a specified Item",
    OperationId = "Item.PutItem")
  ]
  public async Task<IActionResult> PutItem(int id, ItemDTO ItemDTO)
  {
    if (id != ItemDTO.Id)
    {
      return BadRequest();
    }
    var item = await _context.Items.FindAsync(id);
    if (item == null)
    {
      return NotFound();
    }
    item.Name = ItemDTO.Name;
    item.Price = ItemDTO.Price;
    item.StockQuantity = ItemDTO.StockQuantity;
    item.StoreId = ItemDTO.StoreId;
    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException) when (!ItemExists(id))
    {
      return NotFound();
    }
    return NoContent();
  }

  // POST: /item
  [HttpPost]
  [SwaggerOperation(
    Summary = "Create a new Item",
    OperationId = "Item.PostItem")
  ]
  public async Task<ActionResult<ItemDTO>> PostItem(ItemDTO ItemDTO)
  {
    var item = new Item
    {
      Name = ItemDTO.Name,
      Price = ItemDTO.Price,
      StockQuantity = ItemDTO.StockQuantity,
      StoreId = ItemDTO.StoreId,
    };

    var store = await _context.Stores.FindAsync(ItemDTO.StoreId);
    if (store == null)
    {
      return NotFound();
    }
    item.StoreId = store.Id;

    _context.Items.Add(item);
    await _context.SaveChangesAsync();
    return CreatedAtAction(
           nameof(GetItem),
                new { id = item.Id },
                     ItemToDto(item));
  }

  // DELETE: /item/1
  [HttpDelete("{id}")]
  [SwaggerOperation(
    Summary = "Delete specified Item",
    OperationId = "Item.DeleteItem")
  ]
  public async Task<IActionResult> DeleteItem(int id)
  {
    var item = await _context.Items.FindAsync(id);
    if (item == null)
    {
      return NotFound();
    }
    _context.Items.Remove(item);
    await _context.SaveChangesAsync();
    return NoContent();
  }

  private bool ItemExists(int id)
  {
    return _context.Items.Any(e => e.Id == id);
  }

  private static ItemDTO ItemToDto(Item item) =>
    new ItemDTO
    {
      Id = item.Id,
      Name = item.Name,
      Price = item.Price,
      StockQuantity = item.StockQuantity,
      StoreId = item.StoreId
    };
}

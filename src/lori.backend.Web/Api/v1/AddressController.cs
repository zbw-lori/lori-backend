using lori.backend.Infrastructure.Data;
using lori.backend.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace lori.backend.Web.Api.v1;

public class AddressController : BaseApiController
{
  private readonly LoriDbContext _context;

  public AddressController(LoriDbContext context)
  {
    _context = context;
  }

  // GET: /addresses
  [HttpGet]
  [SwaggerOperation(
    Summary = "Gets a list of all addresses",
    OperationId = "Address.GetAddresses")
  ]
  public async Task<ActionResult<IEnumerable<Address>>> GetAddresses()
  {
    return await _context.Addresses.ToListAsync();
  }

  // GET: /addresses/1
  [HttpGet("{id}")]
  [SwaggerOperation(
    Summary = "Gets details of the specified address",
    OperationId = "Address.GetAddress")
  ]
  public async Task<ActionResult<Address>> GetAddress(int id)
  {
    var address = await _context.Addresses.FindAsync(id);

    if (address == null)
    {
      return NotFound();
    }

    return address;
  }

  // PUT: /addresses/1
  [HttpPut("{id}")]
  [SwaggerOperation(
    Summary = "Update a specified address",
    OperationId = "Address.PutAddress")
  ]
  public async Task<IActionResult> PutAddress(int id, Address newAddress)
  {
    var address = await _context.Addresses.FindAsync(id);
    if (address == null)
    {
      return NotFound();
    }
    address.Street = newAddress.Street;
    address.StreetNumber = newAddress.StreetNumber;
    address.City = newAddress.City;
    address.CityCode = newAddress.CityCode;
    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException) when (!AddressExists(id))
    {
      return NotFound();
    }
    return NoContent();
  }

  // POST: /addresses
  [HttpPost]
  [SwaggerOperation(
    Summary = "Create a new address",
    OperationId = "Address.PostAddress")
  ]
  public async Task<ActionResult<Address>> PostAddress(Address newAddress)
  {
    var address = new Address
    {
      Street = newAddress.Street,
      StreetNumber = newAddress.StreetNumber,
      City = newAddress.City,
      CityCode = newAddress.CityCode,
    };
    _context.Addresses.Add(address);
    await _context.SaveChangesAsync();
    return CreatedAtAction(
           nameof(GetAddress),
                new
                {
                  id = address.Id
                },
                     address);
  }

  // DELETE: /addresses/1
  [HttpDelete("{id}")]
  [SwaggerOperation(
    Summary = "Delete specified address",
    OperationId = "Address.DeleteAddress")
  ]
  public async Task<IActionResult> DeleteAddress(int id)
  {
    var address = await _context.Addresses.FindAsync(id);
    if (address == null)
    {
      return NotFound();
    }
    _context.Addresses.Remove(address);
    await _context.SaveChangesAsync();
    return NoContent();
  }

  private bool AddressExists(int id)
  {
    return _context.Addresses.Any(e => e.Id == id);
  }
}


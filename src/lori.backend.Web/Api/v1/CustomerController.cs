using lori.backend.Infrastructure.Data;
using lori.backend.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace lori.backend.Web.Api.v1;

public class CustomerController : BaseApiController
{
  private readonly LoriDbContext _context;

  public CustomerController(LoriDbContext context)
  {
    _context = context;
  }

  // GET: /customers
  [HttpGet]
  [SwaggerOperation(
    Summary = "Gets a list of all customers",
    OperationId = "Customer.GetCustomers")
  ]
  public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer()
  {
    await _context.Addresses.LoadAsync();
    _context.Customers.Include(c => c.Address);
    return await _context.Customers.ToListAsync();
  }

  // GET: /customers/1
  [HttpGet("{id}")]
  [SwaggerOperation(
    Summary = "Gets details of the specified customer",
    OperationId = "Customer.GetCustomer")
  ]
  public async Task<ActionResult<Customer>> GetCustomer(int id)
  {
    var customer = await _context.Customers.FindAsync(id);

    if (customer == null)
    {
      return NotFound();
    }
    var address = await _context.Addresses.FindAsync(customer.AddressId);
    customer.Address = address;

    return customer;
  }

  // PUT: /customers/1
  [HttpPut("{id}")]
  [SwaggerOperation(
    Summary = "Update a specified customer",
    OperationId = "Customer.PutCustomer")
  ]
  public async Task<IActionResult> PutCustomer(int id, Customer newCustomer)
  {
    var customer = await _context.Customers.FindAsync(id);
    if (customer == null)
    {
      return NotFound();
    }
    customer.Forename = newCustomer.Forename;
    customer.Surename = newCustomer.Surename;
    customer.Email = newCustomer.Email;
    customer.Phone = newCustomer.Phone;
    customer.AddressId = newCustomer.AddressId;
    try
    {
      await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException) when (!CustomerExists(id))
    {
      return NotFound();
    }
    return NoContent();
  }

  // POST: /customers
  [HttpPost]
  [SwaggerOperation(
    Summary = "Create a new customer",
    OperationId = "Customer.PostCustomer")
  ]
  public async Task<ActionResult<Customer>> PostCustomer(Customer newCustomer)
  {
    var customer = new Customer
    {
      Forename = newCustomer.Forename,
      Surename = newCustomer.Surename,
      Email = newCustomer.Email,
      Phone = newCustomer.Phone,
      AddressId = newCustomer.AddressId,
    };
    var address = await _context.Addresses.FindAsync(customer.AddressId);
    customer.Address = address;
    _context.Customers.Add(customer);
    await _context.SaveChangesAsync();
    return CreatedAtAction(
           nameof(GetCustomer),
                new
                {
                  id = customer.Id
                },
                     customer);
  }

  // DELETE: /customers/1
  [HttpDelete("{id}")]
  [SwaggerOperation(
    Summary = "Delete specified customer",
    OperationId = "Customer.DeleteCustomer")
  ]
  public async Task<IActionResult> DeleteCustomer(int id)
  {
    var customer = await _context.Customers.FindAsync(id);
    if (customer == null)
    {
      return NotFound();
    }
    _context.Customers.Remove(customer);
    await _context.SaveChangesAsync();
    return NoContent();
  }

  private bool CustomerExists(int id)
  {
    return _context.Customers.Any(e => e.Id == id);
  }
}

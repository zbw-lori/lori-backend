using lori.backend.Infrastructure.Data;
using lori.backend.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using lori.backend.Web.ApiModels;

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
  public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomer()
  {
    await _context.Addresses.LoadAsync();
    _context.Customers.Include(c => c.Address);
    await _context.Logins.LoadAsync();
    _context.Customers.Include(c => c.Login);
    return await _context.Customers.Select(x => CustomerToDTO(x)).ToListAsync();
  }

  // GET: /customers/1
  [HttpGet("{id}")]
  [SwaggerOperation(
    Summary = "Gets details of the specified customer",
    OperationId = "Customer.GetCustomer")
  ]
  public async Task<ActionResult<CustomerDTO>> GetCustomer(int id)
  {
    var customer = await _context.Customers.FindAsync(id);

    if (customer == null)
    {
      return NotFound();
    }
    var address = await _context.Addresses.FindAsync(customer.AddressId);
    customer.Address = address;

    return CustomerToDTO(customer);
  }

  // PUT: /customers/1
  [HttpPut("{id}")]
  [SwaggerOperation(
    Summary = "Update a specified customer",
    OperationId = "Customer.PutCustomer")
  ]
  public async Task<IActionResult> PutCustomer(int id, CustomerDTO customerDto)
  {
    var customer = await _context.Customers.FindAsync(id);
    if (customer == null)
    {
      return NotFound();
    }
    customer.Forename = customerDto.Forename;
    customer.Surename = customerDto.Surename;
    customer.Email = customerDto.Email;
    customer.Phone = customerDto.Phone;
    customer.AddressId = customerDto.AddressId;
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
  public async Task<ActionResult<CustomerDTO>> PostCustomer(CustomerDTO customerDto)
  {
    var customer = new Customer
    {
      Forename = customerDto.Forename,
      Surename = customerDto.Surename,
      Email = customerDto.Email,
      Phone = customerDto.Phone,
      AddressId = customerDto.AddressId,
      Username = customerDto.Username
    };
    var address = await _context.Addresses.FindAsync(customer.AddressId);
    customer.Address = address;
    _context.Customers.Add(customer);
    await _context.SaveChangesAsync();
    return CreatedAtAction(
            nameof(GetCustomer),
            new { id = customer.Id },
            CustomerToDTO(customer));
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

  private static CustomerDTO CustomerToDTO(Customer customer) =>
    new CustomerDTO
    {
      Id = customer.Id,
      Forename = customer.Forename,
      Surename = customer.Surename,
      Email = customer.Email,
      Phone = customer.Phone,
      AddressId = customer.AddressId,
      Username = customer.Username
    };
}

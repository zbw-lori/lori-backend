using System.Text;
using Ardalis.HttpClientTestExtensions;
using lori.backend.Core.Models;
using Newtonsoft.Json;
using Xunit;

namespace lori.backend.FunctionalTests.ControllerApis;

public class ApiCustomerController : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private HttpClient _client;

  public ApiCustomerController(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsAllSeedCustomers()
  {
    var result = await _client.GetAndDeserializeAsync<IEnumerable<Customer>>("/api/v1/customer");

    Assert.Equal(2, result.Count());
  }

  [Fact]
  public async Task ReturnsSeedCustomerGivenId1()
  {
    var expected = new Customer
    {
      Forename = "Max",
      Surename = "Muster",
      Email = "max.muster@test.ch",
      Phone = "202-555-0102",
      AddressId = 1,
    };

    var result = await _client.GetAndDeserializeAsync<Customer>("/api/v1/customer/1");

    Assert.Equal(expected.Forename, result.Forename);
    Assert.Equal(expected.Surename, result.Surename);
    Assert.Equal(expected.Email, result.Email);
    Assert.Equal(expected.Phone, result.Phone);
    Assert.Equal(expected.AddressId, result.AddressId);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenId0()
  {
    await _client.GetAndEnsureNotFoundAsync("/api/v1/customer/0");
  }

  [Fact]
  public async Task ReturnsSuccessCreateCustomer()
  {
    var newCustomer = new Customer
    {
      Forename = "Musi",
      Surename = "Mala",
      Email = "musi@mala.com",
      Phone = "123-456-789",
      AddressId = 2,
    };
    var content = new StringContent(JsonConvert.SerializeObject(newCustomer), Encoding.UTF8, "application/json");

    var result = await _client.PostAsync("/api/v1/customer", content);

    result.EnsureSuccessStatusCode();
  }

  [Fact]
  public async Task ReturnsSuccessUpdateCustomer()
  {
    var newCustomer = new Customer
    {
      Forename = "Maxine",
      Surename = "Parker",
      Email = "peter.parker@spider.com",
      Phone = "478-488-8628",
      AddressId = 2,
    };
    var content = new StringContent(JsonConvert.SerializeObject(newCustomer), Encoding.UTF8, "application/json");

    var result = await _client.PutAsync("/api/v1/customer/2", content);

    result.EnsureSuccessStatusCode();
    var customer = await _client.GetAndDeserializeAsync<Customer>("/api/v1/customer/2");
    Assert.Equal(newCustomer.Forename, customer.Forename);
  }

  [Fact]
  public async Task ReturnsSuccessRemoveCustomer()
  {
    await ReturnsSuccessCreateCustomer();
    await _client.DeleteAndEnsureNoContentAsync("/api/v1/customer/3");
  }
}

using System.Text;
using Ardalis.HttpClientTestExtensions;
using lori.backend.Infrastructure.Models;
using Newtonsoft.Json;
using Xunit;

namespace lori.backend.FunctionalTests.ControllerApis;

[Collection("Sequential")]
public class ApiAddressesController : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly HttpClient _client;

  public ApiAddressesController(CustomWebApplicationFactory<Program> factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsAllSeedAddresses()
  {
    var result = await _client.GetAndDeserializeAsync<IEnumerable<Address>>("/api/v1/address");

    Assert.Equal(2, result.Count());
  }

  [Fact]
  public async Task ReturnsSeedAddressGivenId1()
  {
    var expected = new Address
    {
      Street = "Musterstrasse",
      StreetNumber = 1,
      City = "St.Gallen",
      CityCode = 9000
    };

    var result = await _client.GetAndDeserializeAsync<Address>("/api/v1/address/1");

    Assert.Equal(expected.Street, result.Street);
    Assert.Equal(expected.StreetNumber, result.StreetNumber);
    Assert.Equal(expected.City, result.City);
    Assert.Equal(expected.CityCode, result.CityCode);
  }

  [Fact]
  public async Task ReturnsNotFoundGivenId0()
  {
    await _client.GetAndEnsureNotFoundAsync("/api/v1/address/0");
  }

  [Fact]
  public async Task ReturnsSuccessCreateAddress()
  {
    var newAddress = new Address
    {
      Street = "Teststrasse",
      StreetNumber = 12,
      City = "TestStadt",
      CityCode = 1234
    };
    var content = new StringContent(JsonConvert.SerializeObject(newAddress), Encoding.UTF8, "application/json");

    var result = await _client.PostAsync("/api/v1/address", content);

    result.EnsureSuccessStatusCode();
  }

  public async Task ReturnsSuccessUpdateAddress()
  {
    var newAddress = new Address
    {
      Street = "Teststrasse",
      StreetNumber = 33,
      City = "TestStadt",
      CityCode = 1234
    };
    var content = new StringContent(JsonConvert.SerializeObject(newAddress), Encoding.UTF8, "application/json");

    var result = await _client.PatchAsync("/api/v1/address", content);

    result.EnsureSuccessStatusCode();
    var address = await _client.GetAndDeserializeAsync<Address>("/api/v1/address/3");
    Assert.Equal(newAddress.Street, address.Street);
    Assert.Equal(newAddress.StreetNumber, address.StreetNumber);
    Assert.Equal(newAddress.City, address.City);
    Assert.Equal(newAddress.CityCode, address.CityCode);
  }

  public async Task ReturnsSuccessRemoveAddress()
  {
    var result = await _client.DeleteAndEnsureNoContentAsync("/api/v1/address/3");
  }
}

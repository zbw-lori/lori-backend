using Ardalis.HttpClientTestExtensions;
using lori.backend.Infrastructure.Models;
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
  public async Task ReturnsSeedProjectGivenId1()
  {
    var result = await _client.GetAndDeserializeAsync<IEnumerable<Address>>("/api/v1/address");

    Assert.Equal(2, result.Count());
  }

  [Fact]
  public async Task ReturnsNotFoundGivenId0()
  {
    await _client.GetAndEnsureNotFoundAsync("/api/v1/address/0");
  }
}

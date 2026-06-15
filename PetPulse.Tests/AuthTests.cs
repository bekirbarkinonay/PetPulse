using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

public class AuthTests : IClassFixture<WebApplicationFactory<PetPulse.API.Program>>
{
    private readonly WebApplicationFactory<PetPulse.API.Program> _factory;
    public AuthTests(WebApplicationFactory<PetPulse.API.Program> factory) => _factory = factory;

    [Fact]
    public async Task GetUsers_Returns_Unauthorized_Without_Role()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/api/Auth/users?role=Guest");
        Assert.False(response.IsSuccessStatusCode);
    }
}
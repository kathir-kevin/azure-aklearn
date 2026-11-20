using Microsoft.AspNetCore.Mvc.Testing;

namespace LearnApi.Tests;

public class BlobNamingTests
{
    [Theory]
    [InlineData("Screenshot 2026-11-04 165604.png", "screenshot-2026-11-04-165604.png")]
    [InlineData("My Photo (1).JPG", "my-photo-1.jpg")]
    [InlineData("###.png", "file.png")]
    public void Sanitize_ReturnsUrlSafeName(string input, string expected)
    {
        Assert.Equal(expected, BlobNaming.Sanitize(input));
    }
}

public class RootEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public RootEndpointTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task Root_ReturnsRunningMessage()
    {
        var client = _factory.CreateClient();
        var text = await client.GetStringAsync("/");
        Assert.Equal("LearnApi is running", text);
    }
}
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace MinimalApi.Test;

public class UnitTest
{
    [Fact]
    public async Task TestHelloEndpoint()
    {
        await using var application = new WebApplicationFactory<Program>();
        using var client = application.CreateClient();

        var response = await client.GetStringAsync("/hello");

        Assert.Equal("Hello World!", response);
    }
}
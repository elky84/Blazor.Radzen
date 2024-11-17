using MinimalApi.Endpoint.Api;

namespace MinimalApi.Endpoint;

public static class AccountEndpoint
{
    public static void Map(RouteGroupBuilder api)
    {
        var routeGroup = api.MapGroup("Account")
            .WithTags(["Account"]);

        api.MapPost("/GuestSignIn", SignIn.Handle);
    }
}
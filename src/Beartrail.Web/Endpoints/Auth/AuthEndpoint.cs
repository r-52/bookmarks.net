using System.Security.Claims;
namespace Beartrail.Web.Endpoints.Auth;

public static class AuthEndpoint
{
    public static RouteGroupBuilder MapAuth(this RouteGroupBuilder group)
    {
        group.MapPost("/login", async ([FromBody] LoginCommand command, [FromServices] ISender sender) =>
        {
            var result = await sender.Send(command);
            return Results.Ok(result);
        }).AllowAnonymous();
        group.MapGet("/", (ClaimsPrincipal user) =>
        {
            return Results.Ok();
        }).RequireAuthorization();
        return group;
    }

}

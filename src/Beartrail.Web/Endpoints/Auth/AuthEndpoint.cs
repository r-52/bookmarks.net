using System.Security.Claims;

namespace Beartrail.Web.Endpoints.Auth;

public static class AuthEndpoint
{
    public static RouteGroupBuilder MapAuth(this RouteGroupBuilder group)
    {
        group.MapPost("/login", async ([FromServices] IJwtTokenGeneratorService tokenGenerator, [FromBody] LogInUserDataTransferObject logInUserDataTransfer) =>
        {
            var token = await tokenGenerator.GenerateTokenAsync(logInUserDataTransfer);
            if (string.IsNullOrEmpty(token))
            {
                return Results.BadRequest();
            }
            return Results.Ok(new
            {
                Token = token
            });
        }).AllowAnonymous();
        group.MapGet("/", (ClaimsPrincipal user) =>
        {
            return Results.Ok();
        }).RequireAuthorization();
        return group;
    }

}

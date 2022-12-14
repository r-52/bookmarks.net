namespace Beartrail.Application.Interfaces.Jwt;

public interface IJwtTokenGeneratorService
{
    public Task<string> GenerateTokenAsync(string email);
}

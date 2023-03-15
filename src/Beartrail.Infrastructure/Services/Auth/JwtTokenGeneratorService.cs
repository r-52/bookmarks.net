namespace Beartrail.Infrastructure.Services;

public class JwtTokenGeneratorService : IJwtTokenGeneratorService
{
    private readonly IOptions<JwtSettings> _settings;
    private readonly ILogger<JwtTokenGeneratorService> _logger;

    public JwtTokenGeneratorService(IOptions<JwtSettings> settings, ILogger<JwtTokenGeneratorService> logger)
    {
        _settings = settings;
        _logger = logger;
    }

    public async Task<string> GenerateTokenAsync(string email)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Value.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer: _settings.Value.Issuers.FirstOrDefault(),
            audience: _settings.Value.Audiences.FirstOrDefault(),
            claims: new List<Claim>
            {
                new Claim(ClaimTypes.Email.ToString(), email),
                new Claim(ClaimTypes.Name.ToString(), email),
            },
            notBefore: null,
            expires: DateTime.Now.AddHours(_settings.Value.ValidInHours),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

namespace Beartrail.Infrastructure.Services;

public class JwtTokenGeneratorService : IJwtTokenGeneratorService
{
    private readonly IOptions<JwtSettings> _settings;
    private readonly ILogger<JwtTokenGeneratorService> _logger;
    private readonly UserManager<BearUser> _userManager;

    public JwtTokenGeneratorService(IOptions<JwtSettings> settings, ILogger<JwtTokenGeneratorService> logger, UserManager<BearUser> userManager)
    {
        _userManager = userManager;
        _settings = settings;
        _logger = logger;
    }

    public async Task<string> GenerateTokenAsync(LogInUserDataTransferObject logInUserData)
    {
        var bearUser = await _userManager.FindByEmailAsync(logInUserData.Email);
        if (bearUser is null)
        {
            _logger.LogInformation($"[{this.ToString()}] User not found {logInUserData.Email}");
            return "";
        }
        await _userManager.CheckPasswordAsync(bearUser, logInUserData.Password);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Value.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roles = (await _userManager.GetRolesAsync(bearUser)) ?? new List<string>();
        var claimRoles = new List<Claim>();
        foreach (var role in roles)
        {
            _logger.LogInformation($"[{this.ToString()}] User found {logInUserData.Email} - Adding role {role}"); claimRoles.Add(
                new Claim(ClaimTypes.Role, role)
            );
        }
        var token = new JwtSecurityToken(issuer: _settings.Value.Issuer,
            audience: _settings.Value.Audience,
            claims: new List<Claim>
            {
                new Claim(ClaimTypes.Email.ToString(), bearUser!.NormalizedEmail),
                new Claim(ClaimTypes.Name.ToString(), bearUser!.UserName),
            }.Concat(claimRoles).ToList(),
            notBefore: null,
            expires: DateTime.Now.AddHours(_settings.Value.ValidInHours),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

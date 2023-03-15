using Beartrail.Infrastracture.Identity;

namespace Beartrail.Infrastructure.Database;

public class ApplicationDataContextSeeder : IApplicationDataContextSeeder
{
    private readonly ILogger<ApplicationDataContextSeeder> _logger;
    private readonly ApplicationDataContext _context;
    private readonly UserManager<BearUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDataContextSeeder(ILogger<ApplicationDataContextSeeder> logger, ApplicationDataContext context, UserManager<BearUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;

    }

    public async Task SeedAsync()
    {
        _logger.LogInformation($"[{this.ToString()}] Trying to apply migrations...");
        await _context.Database.MigrateAsync();
        _logger.LogInformation($"[{this.ToString()}] Done with migrations...");

        _logger.LogInformation($"[{this.ToString()}] Trying to seed...");
        _logger.LogInformation($"[{this.ToString()}] Starting with roles");
        await this.CreateRoles(new() {
            "User",
            "HR-Agent",
            "Supervisor",
            "Administrator",
        });
        _logger.LogInformation($"[{this.ToString()}] Done with roles");
        _logger.LogInformation($"[{this.ToString()}] Creating users");
        List<ApplicationContextUserSeederDataTransferObject> users = new() {
            new ApplicationContextUserSeederDataTransferObject{
                Password = Environment.GetEnvironmentVariable("BEAR__DEFAULT_USER_PASSWD"),
                RoleName = "Administrator",
                UserName = Environment.GetEnvironmentVariable("BEAR__DEFAULT_USER_EMAIL"),
            }
        };
        await CreateUser(users);
        _logger.LogInformation($"[{this.ToString()}] Done with users");

        await _context.SaveChangesAsync();
        _logger.LogInformation($"[{this.ToString()}] Done with seeding");
    }

    private async Task CreateUser(List<ApplicationContextUserSeederDataTransferObject> applicationContextUsers)
    {
        foreach (var user in applicationContextUsers)
        {
            var roleName = user.RoleName;
            var role = await _roleManager.FindByNameAsync(roleName);
            var maybeUser = await _userManager.FindByEmailAsync(user.UserName);
            if (maybeUser is null)
            {
                BearUser x = new()
                {
                    Email = user.UserName,
                    UserName = user.UserName,
                };
                await _userManager.CreateAsync(x);

                var passwordHashResult = await _userManager.AddPasswordAsync(x, user.Password);
            }
        }
    }

    private async Task CreateRoles(List<string> names)
    {
        foreach (var name in names)
        {
            var role = new BearRole(name);
            if (_roleManager.Roles.All(r => r.Name != role.Name))
            {
                _logger.LogInformation($"[{this.ToString()}] Creating role {name}");
                await _roleManager.CreateAsync(role);
                _logger.LogInformation($"[{this.ToString()}] Created role {name}");
            }
        }
    }
}

internal class ApplicationContextUserSeederDataTransferObject
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string RoleName { get; set; } = string.Empty;
}

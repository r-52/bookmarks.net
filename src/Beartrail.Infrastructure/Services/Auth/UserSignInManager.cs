namespace Beartrail.Infrastructure.Services;

public class UserSignInManager : IUserSignInManager
{
    private readonly UserManager<BearUser> _userManager;

    private readonly ILogger<UserSignInManager> _logger;

    public UserSignInManager(UserManager<BearUser> userManager, ILogger<UserSignInManager> logger)
    {
        _logger = logger;
        _userManager = userManager;

    }

    public async Task<SignInResultDataTransferObject> SignInAsync(LogInUserDataTransferObject logInUserData)
    {
        var bearUser = await _userManager.FindByEmailAsync(logInUserData.Email);
        if (bearUser is null)
        {
            _logger.LogInformation($"[{this.ToString()}] User not found {logInUserData.Email}");
            return new()
            {
                Email = null,
                IsSuccess = false,
            };
        }
        var isMatched = await _userManager.CheckPasswordAsync(bearUser, logInUserData.Password);
        return new()
        {
            Email = bearUser.NormalizedEmail,
            IsSuccess = isMatched,
        };
    }
}

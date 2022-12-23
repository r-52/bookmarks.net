namespace Beartrail.Application.Interfaces.Auth;

public interface IUserSignInManager
{
    public Task<SignInResultDataTransferObject> SignInAsync(LogInUserDataTransferObject logInUserData);
}

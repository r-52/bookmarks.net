namespace Beartrail.Application.Interfaces.Auth;

public interface ICurrentUserManager
{
    public Task<Guid> GetIdentityUserIdAsync();
}

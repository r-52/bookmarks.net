namespace Beartrail.Application.Interfaces.Database;

public interface IApplicationDataContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new());
}

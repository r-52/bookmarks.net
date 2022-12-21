namespace Beartrail.Infrastructure.Database;

public class ApplicationDataContext : IdentityDbContext<BearUser>, IApplicationDataContext
{
    public ApplicationDataContext(
                DbContextOptions<ApplicationDataContext> options
    ) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}

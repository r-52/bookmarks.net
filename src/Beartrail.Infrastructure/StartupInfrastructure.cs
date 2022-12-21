namespace Beartrail.Infrastructure;

public static class StartupInfrastructure
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDataContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("Default"),
                builder => builder.MigrationsAssembly(typeof(ApplicationDataContext).Assembly.FullName)));
        services.AddScoped<IApplicationDataContext>(provider => provider.GetRequiredService<ApplicationDataContext>());


        services
            .AddIdentityCore<BearUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDataContext>();

        return services;
    }
}

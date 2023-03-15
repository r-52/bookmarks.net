var builder = WebApplication.CreateBuilder(args);

builder.AddCustomConfig();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{

    var jwtSettings = builder.Configuration
                        .GetSection(JwtSettings.OptionsName)
                        .Get<JwtSettings>();
    options.IncludeErrorDetails = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuers = jwtSettings!.Issuers,
        ValidAudiences = jwtSettings!.Audiences,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings!.Key))
    };
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var envParser = scope.ServiceProvider.GetRequiredService<IEnvFileParser>();
    envParser.ParseDotEnvFile(System.IO.Directory.GetCurrentDirectory());

    var seeder = scope.ServiceProvider.GetRequiredService<IApplicationDataContextSeeder>();
    await seeder.SeedAsync();
}


app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");
app.MapGroup("/auth").MapAuth();

app.Run();

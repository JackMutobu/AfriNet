using AfriNetLocalApi.Configuration;
using AfriNetLocalApi.Constants;
using AfriNetLocalApi.Recharges.Services;
using AfriNetLocalApi.Services.Accounts;
using AfriNetLocalApi.Services.Auth;
using AfriNetLocalApi.Services.Bundles;
using AfriNetLocalApi.Services.Users;
using AfriNetRouterLib;
using AfriNetRouterLib.Interfaces;
using FastEndpoints.Security;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder();

builder.Services.AddDbContext<AfriNetLocalDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("LocalDatabase");
    options.UseSqlite(connectionString);
    options.EnableDetailedErrors(true);
});

builder.Services.AddFastEndpoints();
builder.Services.AddJWTBearerAuth(AuthKeys.JWTKey);
builder.Services.SwaggerDocument(config => config.AutoTagPathSegmentIndex = 2);
builder.Services.AddSingleton(p => new AppSettings()
{
    RouterIP = builder.Configuration.GetValue<string>("AppSettings:Router:IP")!,
    RouterPassword = builder.Configuration.GetValue<string>("AppSettings:Router:Password")!,
    RouterUsername = builder.Configuration.GetValue<string>("AppSettings:Router:Username")
});
builder.Services.AddSingleton<IRouterService>(p =>
{
    var appSettings = p.GetService<AppSettings>();
    return new TpLinkArcherC50Service($"http://{appSettings!.RouterIP}", appSettings.RouterPassword);
});

builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<IBundleService, BundleService>();
builder.Services.AddTransient<IRechargeService, RechargeService>();

var app = builder.Build();

using var serviceScope = app.Services.CreateAsyncScope();
var db = serviceScope.ServiceProvider.GetRequiredService<AfriNetLocalDbContext>();
if (db is not null)
{
    try
    {
        if (db.Database.GetPendingMigrations().Any())
            db.Database.Migrate();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();
app.UseSwaggerGen();
app.Run();
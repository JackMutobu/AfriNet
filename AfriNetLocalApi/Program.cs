using AfriNetLocalApi.Configuration;
using AfriNetLocalApi.Constants;
using AfriNetLocalApi.Services.Auth;
using AfriNetRouterLib;
using AfriNetRouterLib.Interfaces;
using FastEndpoints.Security;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder();
builder.Services.AddFastEndpoints();
builder.Services.AddJWTBearerAuth(AuthKeys.JWTKey);
builder.Services.SwaggerDocument();
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

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();
app.UseSwaggerGen();
app.Run();
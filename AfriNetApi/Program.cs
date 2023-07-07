using FastEndpoints.Swagger;


var builder = WebApplication.CreateBuilder();
builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();
builder.Services.AddHttpClient("tplink",client =>
{
    client.BaseAddress = new Uri($"http://192.168.1.78/");
    client.Timeout = TimeSpan.FromSeconds(5);
});

var app = builder.Build();
app.UseAuthorization();
app.UseFastEndpoints();
app.UseSwaggerGen();
app.Run();
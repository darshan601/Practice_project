using JWTLibrary;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using OcelotApiGateway.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder1 =>
    {
        builder1.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});

builder.Services.AddCustomJwtAuth();

var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors();

app.UseMiddleware<AttachSignatureToRequest>();
app.UseOcelot().Wait();

app.Run();
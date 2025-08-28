using Microsoft.EntityFrameworkCore;
using OrderMicroservice.Data;
using OrderMicroservice.Helper;

var builder = WebApplication.CreateBuilder(args);

var AllowedOriginSetting = "AllowedOrigin";
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Rabbit Mq consumer
// builder.Services.AddSingleton<RabbitMqService>(provider =>
// {
//     return RabbitMqService.CreateAsync().GetAwaiter().GetResult();
// });
// builder.Services.AddHostedService<RabbitMqBackgroundService>();

builder.Services.AddDbContext<OrderDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("OrderConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors(builder1 =>
    {
        builder1.WithOrigins(AllowedOriginSetting)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
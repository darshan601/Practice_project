using EFCore.Data;
using EFCore.Entity;
using EFCore.Helper;
using EFCore.Repo;
using EFCore.Services;
using JWTLibrary;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.DependencyInjection;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

var AllowedOriginSetting = "AllowedOrigin";

// initializing efcore - giving path to the db
builder.Services.AddDbContext<MovieDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("MovieConnection"))
        // .UseLazyLoadingProxies()
    );


// Repos
builder.Services.AddScoped<IRepository<Movie>, MovieRepository>();
builder.Services.AddScoped<IRepository<Genre>, GenreRepository>();
builder.Services.AddScoped<MovieRepository, MovieRepository>();
// Service
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IMovieService, MovieService>();
// builder.Services.AddScoped<Notification, Notification>();
// AutoMapper
builder.Services.AddAutoMapper(typeof(ApplicationMapper));
// JWT
builder.Services.AddCustomJwtAuth();
// Caching
builder.Services.AddMemoryCache();

// Redis
// builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
// builder.Services.AddHttpClient();

// RabbitMQ - initializing Service.
// builder.Services.AddSingleton<NotificationService>(provider =>
//     {
//         return NotificationService.CreateAsync().GetAwaiter().GetResult();
//     }
// );

builder.Services.AddSharedServices(builder.Configuration,builder.Configuration["MySerilog:FileName"]!);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServiceResiliencePipelines(builder.Configuration);


var app = builder.Build();

app.UseCors(builder1 =>
{
    builder1.WithOrigins(AllowedOriginSetting)
        .AllowAnyHeader()
        .AllowAnyMethod();
});


app.UseSwagger();
app.UseSwaggerUI();
app.UseSharedPolicies();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
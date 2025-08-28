using Microsoft.EntityFrameworkCore;
using OrderMicroservice.Entities;

namespace OrderMicroservice.Data;

public class OrderDbContext(DbContextOptions<OrderDbContext> options):DbContext(options)
{
    DbSet<Order> Orders { get; set; }
    
    DbSet<OrderMovie> OrderMovies { get; set; }
    
    DbSet<OrderUser> OrderUsers { get; set; }
}
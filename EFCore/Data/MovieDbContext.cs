using EFCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Data;

public class MovieDbContext(DbContextOptions<MovieDbContext> options):DbContext(options)
{
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Genre> Genres { get; set; }
    // public DbSet<MovieGenre> MovieGenres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>()
            .Property(m => m.MovieName)
            .HasMaxLength(100);

        // modelBuilder.Entity<MovieGenre>(x =>
        // {
        //     x.HasKey(mg => new { mg.MovieId, mg.GenreId });
        //
        //     x.HasOne(mg => mg.Movie)
        //         .WithMany(m => m.MovieGenres)
        //         .HasForeignKey(mg => mg.MovieId);
        //
        //     x.HasOne(mg => mg.Genre)
        //         .WithMany(g => g.MovieGenres)
        //         .HasForeignKey(mg => mg.GenreId);
        // });

        modelBuilder.Entity<Movie>()
            .HasMany(m => m.Genres)
            .WithMany(g => g.Movies)
            .UsingEntity(j => j.ToTable("MovieGenres"));

    base.OnModelCreating(modelBuilder);
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseLazyLoadingProxies();
    //     base.OnConfiguring(optionsBuilder);
    // }
}
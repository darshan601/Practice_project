using EFCore.Data;
using EFCore.Entity;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Repo;

public class MovieRepository(MovieDbContext dbContext):BaseRepository<Movie>(dbContext)
{
    public async Task<IEnumerable<Movie>> GetAllMoviesAsync()
    {
        return await dbContext.Movies.Include(m => m.Genres).ToListAsync();
    }
}
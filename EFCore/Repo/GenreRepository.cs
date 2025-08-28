using EFCore.Data;
using EFCore.Entity;

namespace EFCore.Repo;

public class GenreRepository(MovieDbContext movieDbContext):BaseRepository<Genre>(movieDbContext)
{
    
}
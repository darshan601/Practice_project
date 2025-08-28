using EFCore.Entity;
using EFCore.Entity.RequestModels;
using EFCore.Entity.ResponseModel;

namespace EFCore.Services;

public interface IGenreService
{
    Task<IEnumerable<GenreResponse>> GetAllGenresAsync();

    Task<GenreResponse> GetGenreByIdAsync(int id);

    Task<Response> AddGenreAsync(CreateGenreRequest request);

    Task<Response> UpdateGenreAsync(UpdateGenreRequest genreRequest);
}
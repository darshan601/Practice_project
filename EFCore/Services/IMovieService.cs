using System.Collections;
using EFCore.Entity;
using EFCore.Entity.RequestModels;
using EFCore.Entity.ResponseModel;

namespace EFCore.Services;

public interface IMovieService
{
    Task<IEnumerable<MovieResponse>> GetMoviesAsync();
    
    Task<MovieResponse> GetMovieByIdAsync(int id);

    Task<Response> AddMovieAsync(CreateMovieRequest request);

    Task<Response> UpdateMovieAsync(CreateMovieRequest request, int id);
    
    Task<Response> DeleteMovieAsync(int id);

    // Task SendToQueueAsync(string messageType, object payload);
}
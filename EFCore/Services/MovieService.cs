using System.Collections;
using System.Text.Json;
using AutoMapper;
using EFCore.Data;
using EFCore.Entity;
using EFCore.Entity.RequestModels;
using EFCore.Entity.ResponseModel;
using EFCore.Helper;
using EFCore.Repo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Polly.Registry;
using StackExchange.Redis;

namespace EFCore.Services;

public class MovieService(MovieRepository repository,IRepository<Genre> genreRepository,
    MovieDbContext context ,IMapper mapper, IMemoryCache cache, ResiliencePipelineProvider<string> resiliencePipeline):IMovieService
{
    // IConnectionMultiplexer muxer
    // private readonly IDatabase redis=muxer.GetDatabase();
    
    public async Task<IEnumerable<MovieResponse>> GetMoviesAsync()
    {
        var output = cache.Get<IEnumerable<MovieResponse>>(nameof(GetMoviesAsync));
        
        if (output is not null && output.Any())
            return output;

        // var cachedData = await redis.StringGetAsync(nameof(GetMoviesAsync));

        // if (cachedData.HasValue)
        // {
        //     return JsonSerializer.Deserialize<IEnumerable<MovieResponse>> (cachedData!);
        // }
        
        
        var list = await repository.GetAllMoviesAsync();

        if (list is null) return null!;

        var responseList = mapper.Map<IEnumerable<MovieResponse>>(list);
        // await redis.StringSetAsync(nameof(GetMoviesAsync), JsonSerializer.Serialize(responseList),
        //     TimeSpan.FromMinutes(2));
        
        cache.Set(nameof(GetMoviesAsync), responseList, TimeSpan.FromMinutes(2));

        return responseList;
    }

    public async Task<MovieResponse> GetMovieByIdAsync(int id)
    {
        
        var movie = await repository.GetByIdAsync(id);
        
        if(movie is null) return null!;

        var movieResponse = mapper.Map<MovieResponse>(movie);
        
        return movieResponse;
    }

    public async Task<Response> AddMovieAsync(CreateMovieRequest request)
    {
        var movie = mapper.Map<Movie>(request);

        // var genres = (ICollection<Genre>)await genreRepository.GetMultipleByAsync(g => request.GenreIds.Contains(g.Id));

        if (request.GenreIds is null || !request.GenreIds.Any())
            return new Response { Flag = false, Message = "Not Genres Found" };
            
        movie.Genres = request.GenreIds.Select(id =>
        {
            var g = new Genre { Id = id };
            context.Entry(g).State = EntityState.Unchanged;
            return g;
        }).ToList();

        // foreach (var genre in movie.Genres)
        // {
        //     context.Entry(genre).State = EntityState.Unchanged;
        // }

        var retryPipeline = resiliencePipeline.GetPipeline("api-pipeline");

        await retryPipeline.ExecuteAsync(async token => await repository.AddAsync(movie));
        // await repository.AddAsync(movie);
        // await SendToQueueAsync("add-movie",request);
        

        return new Response { Flag = true };

    }

    public async Task<Response> UpdateMovieAsync(CreateMovieRequest request, int id)
    {
        var existingMovie = await repository.GetByIdAsync(id);
        
        if (existingMovie is null) return new Response{Flag = false, Message = "Movie Not Found"};

        var movie = mapper.Map<Movie>(request);
        
        movie.Genres = request.GenreIds.Select(id =>
        {
            var g = new Genre { Id = id };
            context.Entry(g).State = EntityState.Unchanged;
            return g;
        }).ToList();
        
        movie.Id = id;

        await repository.UpdateAsync(movie, id);
        
        // await SendToQueueAsync("update-movie",request);

        return new Response { Flag = true };
    }

    public async Task<Response> DeleteMovieAsync(int id)
    {
        var existingMovie = await repository.GetByIdAsync(id);
        
        if (existingMovie is null) return new Response{Flag = false, Message = "Movie Not Found"};
        
        await repository.DeleteAsync(id);
        
        // await SendToQueueAsync("delete-movie",id);
        
        return new Response { Flag = true };
    }

    // public async Task SendToQueueAsync(string messageType, object payload)
    // {
    //     await notificationService.PublishAsync(messageType, payload);
    // }
}
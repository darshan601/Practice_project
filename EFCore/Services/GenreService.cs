using AutoMapper;
using EFCore.Entity;
using EFCore.Entity.RequestModels;
using EFCore.Entity.ResponseModel;
using EFCore.Helper;
using EFCore.Repo;
using Microsoft.Extensions.Caching.Memory;

namespace EFCore.Services;

public class GenreService(IRepository<Genre> genreRepository, IMapper mapper, IMemoryCache cache):IGenreService
{
    
    public async Task<IEnumerable<GenreResponse>> GetAllGenresAsync()
    {
        // Get cache
        var output = cache.Get<IEnumerable<GenreResponse>>(nameof(GetAllGenresAsync));

        if (output is not null && output.Any())
            return output;
        
        var list = await genreRepository.GetAllAsync();

        if (list is null) return null!;
        
        var genreResponseList = mapper.Map<IEnumerable<GenreResponse>>(list);
        // Set cache
        cache.Set(nameof(GetAllGenresAsync), genreResponseList, TimeSpan.FromMinutes(2));
        
        return genreResponseList;
    }

    public async Task<GenreResponse> GetGenreByIdAsync(int id)
    {
        var genre = await genreRepository.GetByIdAsync(id);
        
        if(genre is null) return null!;

        var genreResponse = mapper.Map<GenreResponse>(genre);

        return genreResponse;
    }

    public async Task<Response> AddGenreAsync(CreateGenreRequest genreRequest)
    {
        var existingGenre = await genreRepository.GetByAsync(g => g.Name == genreRequest.Name);
        
        if (existingGenre is not null)
            return new Response{Flag = false, Message ="Genre already exists"};

        var genre = mapper.Map<Genre>(genreRequest);

        await genreRepository.AddAsync(genre);
        
        return new Response{Flag = true};
    }

    public async Task<Response> UpdateGenreAsync(UpdateGenreRequest genreRequest)
    {
        var existingGenre = await genreRepository.GetByIdAsync(genreRequest.Id);

        if (existingGenre is null)
        {
            return new Response{Flag = false, Message = "Genre not found"};
        }

        var genre = mapper.Map<Genre>(genreRequest);

        await genreRepository.UpdateAsync(genre, genreRequest.Id);

        return new Response{Flag = true};
    }
}
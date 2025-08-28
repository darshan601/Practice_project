using EFCore.Entity;
using EFCore.Entity.RequestModels;
using EFCore.Repo;
using EFCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFCore.Controllers;

[ApiController]
[Route("/api/genre")]
public class GenreController(IGenreService service):ControllerBase
{

    [HttpGet("genres")]
    public async Task<IActionResult> GetGenres()
    {
        var genres = await service.GetAllGenresAsync();

        return genres is not null ? Ok(genres): BadRequest("Failed to get genres");
    }

    [HttpPost("add-genre")]
    public async Task<IActionResult> AddGenre( CreateGenreRequest request)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var response = await service.AddGenreAsync(request);
        
        return response.Flag ? Ok("Added Successfully") : BadRequest(response.Message);
    }

    [HttpGet("genres/{id}")]
    [Authorize]
    public async Task<IActionResult> GetGenreById(int id)
    {
        var genre = await service.GetGenreByIdAsync(id);
        
        return genre is not null ? Ok(genre) : BadRequest("Failed to get genre");
    }

    [HttpPut("genres")]
    public async Task<IActionResult> UpdateGenre([FromBody] UpdateGenreRequest request)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);

        var response = await service.UpdateGenreAsync(request);

        return response.Flag ? Ok("Updated Successfully") : BadRequest(response.Message);


    }
    
    
    
}
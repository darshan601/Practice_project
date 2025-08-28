using EFCore.Entity;
using EFCore.Entity.RequestModels;
using EFCore.Helper;
using EFCore.Repo;
using EFCore.Services;
using Microsoft.AspNetCore.Mvc;

namespace EFCore.Controllers;

[ApiController]
[Route("/api/movie")]
public class MovieController(IMovieService movieService):ControllerBase
{
    
    [HttpGet("get-all")]
    public async Task<IActionResult> GetMovies()
    {
        var movies = await movieService.GetMoviesAsync();
        
        return Ok(movies);
    }

    [HttpPost("add-movie")]
    public async Task<IActionResult> AddMovie([FromBody]CreateMovieRequest movieRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var res = await movieService.AddMovieAsync(movieRequest);
        
        return res.Flag ? Ok("Inserted Successfully") : BadRequest(res.Message);
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> UpdateMovie([FromBody] CreateMovieRequest movieRequest, int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var res = await movieService.UpdateMovieAsync(movieRequest, id);
        
        return res.Flag ? Ok("Updated Successfully") : BadRequest(res.Message);
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var res = await movieService.DeleteMovieAsync(id);
        
        return res.Flag ? Ok("Deleted Successfully") : BadRequest(res.Message);
    }


    [HttpPost("addToQueue")]
    public async Task<IActionResult> AddToQueue([FromBody] string message)
    {
        // await movieService.SendToQueueAsync("Random-Message", message);

        return Ok("Done");
    }
    
    
}
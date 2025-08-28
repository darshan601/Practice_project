using System.ComponentModel.DataAnnotations;
using Authentication.Entity;
using Authentication.Entity.Request;
using Authentication.Service;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers;


[ApiController]
[Route("/api/authentication")]
public class AuthenticationController:ControllerBase
{
    private readonly AuthService service;
    
    public AuthenticationController(AuthService service)
    {
        this.service = service;
    }
    
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);

        var flag=await service.Register(user);

        return flag ? Ok("Registered successfully") : BadRequest("Failed to register user");
        ;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await service.Login(loginRequest);
        
        return result is not null ? Ok(result) : BadRequest("Failed to login");

    }
    
}
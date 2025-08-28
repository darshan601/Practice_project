using Microsoft.AspNetCore.Mvc;
using OrderMicroservice.Helper;

namespace OrderMicroservice.Controllers;

[ApiController]
[Route("/api/order")]
public class OrderController:ControllerBase
{
    
    
    public OrderController()
    {
        
    }
    
    
    [HttpGet("get-message")]
    public async Task<IActionResult> GetMessage()
    {
        return Ok();
    }
}
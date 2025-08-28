using Microsoft.AspNetCore.Mvc;
using TaskMicroservice.Entities;
using TaskMicroservice.Repositories;

namespace TaskMicroservice.Controllers;

[ApiController]
[Route("/api/tasks")]
public class TaskController(IRepository<Tasks> tasksRepository):ControllerBase
{

    [HttpGet("getAll")]
    public IActionResult Get()
    {
        var tasks=tasksRepository.GetAll();
        return Ok(tasks);
    }

    [HttpPost("add")]
    public IActionResult AddTasks([FromForm]TasksRequest tasksRequest)
    {
        Console.WriteLine($"Adding Task: {tasksRequest.Title}");

        var task = new Tasks()
        {
            Id = new int(),
            Title = tasksRequest.Title,
            Description = tasksRequest.Description,
            IsCompleted = tasksRequest.IsCompleted,
        };
        
        tasksRepository.Add(task);

        return Ok("Task Added Successfully");
    }
    
}
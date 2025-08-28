using System.ComponentModel.DataAnnotations;

namespace TaskMicroservice.Entities;

public class TasksRequest
{
    [Required] public string Title { get; set; }
    
    [Required]
    public string Description { get; set; }

    public bool IsCompleted { get; set; } = false;

}
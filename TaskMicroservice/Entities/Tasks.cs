using System.ComponentModel.DataAnnotations;

namespace TaskMicroservice.Entities;

public class Tasks
{
    [Required]
    public int Id { get; set; }

    [Required] public string Title { get; set; } = new string("");
    
    [Required]
    public string Description { get; set; } = new string("");

    public bool IsCompleted { get; set; } = false;
}
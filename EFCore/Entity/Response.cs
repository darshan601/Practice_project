using System.ComponentModel.DataAnnotations;

namespace EFCore.Entity;

public class Response
{
    [Required]
    public bool Flag { get; set; }

    [Required]
    public string Message { get; set; }
}
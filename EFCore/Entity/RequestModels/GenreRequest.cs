using System.ComponentModel.DataAnnotations;

namespace EFCore.Entity.RequestModels;

public class CreateGenreRequest
{
    [Required]
    [StringLength(15)]
    public string Name { get; set; }
}
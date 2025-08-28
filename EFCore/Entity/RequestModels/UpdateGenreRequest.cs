using System.ComponentModel.DataAnnotations;

namespace EFCore.Entity.RequestModels;

public class UpdateGenreRequest
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace EFCore.Entity.RequestModels;

public class CreateMovieRequest
{
    [Required]
    [StringLength(50)]
    public string MovieName { get; set; }
    
    public string Director { get; set; }
    
    public DateTime ReleaseDate { get; set; }

    [Required]
    public List<int> GenreIds { get; set; }
}
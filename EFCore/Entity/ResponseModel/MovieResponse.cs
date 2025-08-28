using System.ComponentModel.DataAnnotations;

namespace EFCore.Entity.ResponseModel;

public class MovieResponse
{
    [Required]
    public int Id { get; set; }

    [Required]
    public string MovieName { get; set; }

    public string Director { get; set; }

    public DateTime ReleaseDate { get; set; }
    
    [Required]
    public List<GenreResponse> Genres { get; set; } = new();
}
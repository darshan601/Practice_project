using System.ComponentModel.DataAnnotations;

namespace EFCore.Entity;

public class Genre
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    // public virtual ICollection<MovieGenre> MovieGenres { get; set; }
    public virtual ICollection<Movie> Movies { get; set; }
}
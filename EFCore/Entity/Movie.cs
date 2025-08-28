using System.ComponentModel.DataAnnotations;

namespace EFCore.Entity;

public class Movie
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string MovieName { get; set; }
    
    public string Director { get; set; }
    
    public DateTime ReleaseDate { get; set; }

    // public virtual ICollection<MovieGenre> MovieGenres { get; set; }
    
    public virtual ICollection<Genre> Genres { get; set; }
}
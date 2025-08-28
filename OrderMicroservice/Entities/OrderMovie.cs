using System.Text.Json;

namespace OrderMicroservice.Entities;

public class OrderMovie
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime ReleaseDate { get; set; }
    
    public string Director { get; set; }
    
    public string GenreJson { get; set; }

    public List<string> Genres => JsonSerializer.Deserialize<List<string>>(GenreJson);
}
namespace MovieApi.Domain.Entities;

public class Movie : Entity
{
    public string Title { get; set; }
    
    public string Summary { get; set; }
    
    public bool InTheaters { get; set; }
    
    public DateTime ReleaseDate { get; set; }
    
    public DateTime OffTheatersDate { get; set; }
}
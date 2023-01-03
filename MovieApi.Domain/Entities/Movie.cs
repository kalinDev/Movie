using MovieApi.Domain.Enums;

namespace MovieApi.Domain.Entities;

public class Movie : Entity
{
    public string Title { get; set; }
    
    public string Summary { get; set; }
    
    public DateTime ReleaseDate { get; set; }
    
    public DateTime OffTheatersDate { get; set; }
    
    public string PosterUri { get; set; }
    
    public Room Room { get; set; }
    
    public TimeSpan Duration { get; set; }
}